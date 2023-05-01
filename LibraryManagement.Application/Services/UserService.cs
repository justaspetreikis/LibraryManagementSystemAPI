using AutoMapper;
using LibraryManagement.Application.Dto.Requests;
using LibraryManagement.Application.Dto.Response;
using LibraryManagement.Application.Dto.Responses;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagement.Application.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<UserSignUpResponse> SignUpAsync(SignUpRequest request)
        {
            var usernameExist = await _userRepository.FindUserByUsernameAsync(request.Username);
            if (usernameExist != null)
            {
                throw new ArgumentException("Username already exist");
            }

            var newUser = CreateUser(request);
            var image = await CreateImageAsync(request.Image);

            var person = new Person() { Id = Guid.NewGuid()};
            var adress = new Address() { Id = Guid.NewGuid() };
            var role = "User";

            newUser.Role = role;
            newUser.Person = person;
            newUser.Person.Address = adress;
            newUser.PersonId = person.Id;
            newUser.Person.AddressId = adress.Id;
            newUser.Person.ProfileImageId = image.Id; 
            newUser.Person.ProfileImage = image;

            await _userRepository.AddUserAsync(newUser);

            return _mapper.Map<UserSignUpResponse>(newUser); ;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userRepository.FindUserByUsernameAsync(username);

            if (user == null)
            {
                throw new ArgumentException("Invalid username or password.");
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new ArgumentException("Invalid username or password.");
            }

            var token = GetJwtToken(user.Id, user.Role);

            return token;
        }

        public async Task<AddInformationResponse> AddUsersInformationAsync(AddInformationRequest information, Guid userId)
        {
            var personalCodeExist = await _userRepository.FindUserByPersonalCodeAsync(information.PersonalCode);
            if (personalCodeExist != null)
            {
                throw new ArgumentException("User with personal code already exist");
            }

            var person = _mapper.Map<Person>(information);
            var address = _mapper.Map<Address>(information);

            await _userRepository.EditUsersPersonInformationAsync(person, userId);
            await _userRepository.EditUsersAdressInformationAsync(address, userId);

            var editedUser = await _userRepository.FindUserByIdAsync(userId);

            return (_mapper.Map<AddInformationResponse>(editedUser));
        }

        public async Task<string> UpdatePersonAsync(Guid userId, string property, string value)
        {
            return (await _userRepository.UpdatePersonAsync(userId, property, value));
        }

        public async Task<string> UpdateAddressAsync(Guid userId, string property, string value)
        {
            return (await _userRepository.UpdateAddressAsync(userId, property, value));
        }

        public async Task<UpdateImageResponse> UpdateImageAsync(Guid userId, UpdateProfileImageRequest newImage)
        {
            var fileToImage = await CreateImageAsync(newImage.Image);
            var updatedImage = await _userRepository.UpdateUsersImageAsync(fileToImage, userId);

            return _mapper.Map<UpdateImageResponse>(updatedImage);
        }

        public async Task<bool> CheckPersonnalCodeExistAsync(Guid userId, string peronalCode)
        {
            var personalCodeExist = await _userRepository.FindUserByPersonalCodeAsync(peronalCode);
            return (personalCodeExist == null) ? false : true;
        }

        public async Task<UserInformationResponse> GetUserInformationAsync(Guid userId)
        {
            var user = await _userRepository.FindUserByIdAsync(userId);
            return (_mapper.Map<UserInformationResponse>(user));
        }

        public async Task<bool> DeleteUserByIdAsync(Guid id)
        {
            var userToDelete = await _userRepository.FindUserByIdAsync(id);

            if(userToDelete == null)
            {
                return false;
            }

            await _userRepository.DeleteUserByIdAsync(id);
            return true;
        }

        private User CreateUser(SignUpRequest signUpInformation)
        {
            CreatePasswordHash(signUpInformation.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = _mapper.Map<User>(signUpInformation);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return user;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }

        private string GetJwtToken(Guid userId, string role)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }

        private static async Task<ProfileImage> CreateImageAsync(IFormFile imageFile)
        {
            using var memoryStream = new MemoryStream();
            await imageFile.CopyToAsync(memoryStream);

            var originalImage = Image.FromStream(memoryStream);
            var resizedImage = ResizeImage(originalImage, 200, 200);

            using var resizedStream = new MemoryStream();
            resizedImage.Save(resizedStream, originalImage.RawFormat);

            var image = new ProfileImage
            {
                Id = Guid.NewGuid(),
                Name = imageFile.FileName,
                ImageBytes = resizedStream.ToArray(),
                ContentType = imageFile.ContentType
            };

            return image;
        }

        private static Image ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using var graphics = Graphics.FromImage(destImage);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var wrapMode = new ImageAttributes();
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);

            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

            return destImage;
        }
    }
}
