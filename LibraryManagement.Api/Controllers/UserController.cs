using LibraryManagement.Application.Dto.Requests;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagement.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> UserSignUpAsync([FromForm] SignUpRequest userInformation)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newUser = await _userService.SignUpAsync(userInformation);
                return Ok($"User: {newUser.Username}, was created succesfuly");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var token = await _userService.LoginAsync(loginRequest.Username, loginRequest.Password);

                if (token == null)
                {
                    return BadRequest("Invalid username or password");
                }

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpPost("addInformation")]
        [Authorize]
        public async Task<IActionResult> AddUsersInformationAsync([FromBody] AddInformationRequest information)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {  
                var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = Guid.Parse(userClaim.Value);          

                var editedUser = await _userService.AddUsersInformationAsync(information, userId);

                return Ok(editedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsersInformationAsync()
        {
            try
            {
                var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = Guid.Parse(userClaim.Value);

                var usersInformation = await _userService.GetUserInformationAsync(userId);
                return Ok(usersInformation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("image")]
        [Authorize]
        public async Task<IActionResult> UpdateUsersImageAsync([FromForm] UpdateProfileImageRequest image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userClaim.Value);

            try
            {
                var newImage = await _userService.UpdateImageAsync(userId, image);
                return Ok($"Profile photo updateed with new image: {newImage.Name}");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("firstName")]
        [Authorize]
        public async Task<IActionResult> UpdateUsersFirstNameAsync([FromBody] UpdateFirstNameRequest firstName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = Guid.Parse(userClaim.Value);

                var updatedUsersFirstName = await _userService.UpdatePersonAsync(userId, "FirstName", firstName.FirstName);
                return Ok($"Users name changed to {updatedUsersFirstName}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("lastName")]
        [Authorize]
        public async Task<IActionResult> UpdateUsersLastNameAsync([FromBody] UpdateLastNameRequest lastName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = Guid.Parse(userClaim.Value);

                var updatedUsersLastName = await _userService.UpdatePersonAsync(userId, "LastName", lastName.LastName);
                return Ok($"Users last name changed to {updatedUsersLastName}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("phoneNumber")]
        [Authorize]
        public async Task<IActionResult> UpdateUsersPhoneNumberAsync([FromBody] UpdatePhoneNumberRequest phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = Guid.Parse(userClaim.Value);

                var updatedUsersPhoneNumber = await _userService.UpdatePersonAsync(userId, "PhoneNumber",phoneNumber.PhoneNumber);
                return Ok($"Users phone number changed to {updatedUsersPhoneNumber}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("email")]
        [Authorize]
        public async Task<IActionResult> UpdateUsersEmail([FromBody] UpdateEmailRequest email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = Guid.Parse(userClaim.Value);

                var updatedUsersEmail = await _userService.UpdatePersonAsync(userId, "Email", email.Email);
                return Ok($"Users email changed to {updatedUsersEmail}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("personalCode")]
        [Authorize]
        public async Task<IActionResult> UpdatePersonalCodeAsync([FromBody] UpdatePersonalCodeRequest personalCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userClaim.Value);

            if (await _userService.CheckPersonnalCodeExistAsync(userId, personalCode.PersonalCode))
            {
                throw new Exception("User with this personal code already exist");
            }
            try
            {
                var updatedPersonalCode = await _userService.UpdatePersonAsync(userId, "PersonalCode", personalCode.PersonalCode);
                return Ok($"Users personal code changed to {updatedPersonalCode}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("city")]
        [Authorize]
        public async Task<IActionResult> UpdateCity([FromBody] UpdateCityRequest city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userClaim.Value);

            try
            {
                var updatedCity = await _userService.UpdateAddressAsync(userId, "City", city.City);
                return Ok($"Users city changed to {updatedCity}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("street")]
        [Authorize]
        public async Task<IActionResult> UpdateStreet([FromBody] UpdateStreetRequest street)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userClaim.Value);

            try
            {
                var updatedStreet = await _userService.UpdateAddressAsync(userId, "Street", street.Street);
                return Ok($"Users street changed to {updatedStreet}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("houseNumber")]
        [Authorize]
        public async Task<IActionResult> UpdateHouseNumber([FromBody] UpdateHouseNumberRequest houseNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userClaim.Value);

            try
            {
                var updatedHouseNumber = await _userService.UpdateAddressAsync(userId, "HouseNumber", houseNumber.HouseNumber);
                return Ok($"Users house number changed to {updatedHouseNumber}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("flatNumber")]
        [Authorize]
        public async Task<IActionResult> UpdateFlatNumber([FromBody] UpdateFlatNumberRequest flatNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userClaim.Value);

            try
            {
                var updatedFlatNumber = await _userService.UpdateAddressAsync(userId, "FlatNumber", flatNumber.FlatNumber);
                return Ok($"Users flat number changed to {updatedFlatNumber}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            var userClaim = User.FindFirst(ClaimTypes.Role);
            var userRole = userClaim.Value;

            if(userRole != "Admin")
            {
                return BadRequest("Only user with Admin role can delete users");
            }
            try
            {
                var isUserDeleted = await _userService.DeleteUserByIdAsync(userId);
                return Ok($"User was found and deleted: {isUserDeleted}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
