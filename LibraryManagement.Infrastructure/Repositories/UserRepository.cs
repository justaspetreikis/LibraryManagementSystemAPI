using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Database;
using LibraryManagement.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly LibraryManagementDbContext _dbContext;
        public UserRepository(LibraryManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);

            try
            {
                await SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error adding user to database", ex);
            }
        }

        public async Task<User> FindUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> FindUserByPersonalCodeAsync(string personalCode)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Person.PersonalCode == personalCode);
        }

        public async Task<User> FindUserByIdAsync(Guid id)
        {
            return await _dbContext.Users
        .Include(u => u.Person)
            .ThenInclude(p => p.ProfileImage)
        .Include(u => u.Person)
            .ThenInclude(p => p.Address)
        .FirstOrDefaultAsync(u => u.Id == id); ;
        }

        public async Task<ProfileImage> FindImageByUserIdAsync(Guid id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user.Person.ProfileImage;
        }

        public async Task EditUsersPersonInformationAsync(Person person, Guid userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var existingPerson = await _dbContext.Persons.FindAsync(user.PersonId);

            existingPerson.FirstName = person.FirstName;
            existingPerson.LastName = person.LastName;
            existingPerson.PhoneNumber = person.PhoneNumber;
            existingPerson.Email = person.Email;
            existingPerson.PersonalCode = person.PersonalCode;

            try
            {
                await SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error adding person in database", ex);
            }
        }

        public async Task EditUsersAdressInformationAsync(Address address, Guid userId)
        {
            var user = await _dbContext.Users.Include(u => u.Person)
            .ThenInclude(p => p.Address).FirstOrDefaultAsync(u => u.Id == userId);
            var existingAddress = await _dbContext.Addresses.FindAsync(user.Person.AddressId);

            existingAddress.Street = address.Street;
            existingAddress.City = address.City;
            existingAddress.HouseNumber = address.HouseNumber;
            existingAddress.FlatNumber = address.FlatNumber;

            try
            {
                await SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error adding adress in database", ex);
            }
        }

        public async Task<string> UpdatePersonAsync(Guid userId, string property, string value)
        {
            var user = await _dbContext.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Id == userId);

            var propertyInfo = user.Person.GetType().GetProperty(property);

            if (propertyInfo == null)
            {
                throw new Exception($"Invalid property specified: {property}");
            }

            var convertedValue = Convert.ChangeType(value, propertyInfo.PropertyType);
            propertyInfo.SetValue(user.Person, convertedValue);

            try
            {
                await SaveChangesAsync();
                return value;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error updating {property} in database", ex);
            }
        }

        public async Task<string> UpdateAddressAsync(Guid userId, string property, string value)
        {
            var user = await _dbContext.Users.Include(u => u.Person).ThenInclude(p => p.Address).FirstOrDefaultAsync(u => u.Id == userId);

            var propertyInfo = user.Person.Address.GetType().GetProperty(property);

            if (propertyInfo == null)
            {
                throw new Exception($"Invalid property specified: {property}");
            }

            var convertedValue = Convert.ChangeType(value, propertyInfo.PropertyType);
            propertyInfo.SetValue(user.Person.Address, convertedValue);

            try
            {
                await SaveChangesAsync();
                return value;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error updating {property} in database", ex);
            }
        }

        public async Task<ProfileImage> UpdateUsersImageAsync(ProfileImage newImage, Guid id)
        {
            var user = await FindUserByIdAsync(id);
            var userImage = user.Person.ProfileImage;

            userImage.Name = newImage.Name;
            userImage.ImageBytes = newImage.ImageBytes;
            userImage.ContentType = newImage.ContentType;

            try
            {
                await SaveChangesAsync();
                return newImage;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error updating image in database", ex);
            }
        }

        public async Task DeleteUserByIdAsync(Guid id)
        {
            var user = await FindUserByIdAsync(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.Persons.Remove(user.Person);
                _dbContext.Addresses.Remove(user.Person.Address);
                _dbContext.Images.Remove(user.Person.ProfileImage);
                try
                {
                    await SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception($"Error deleting user with id: {id}", ex);
                }
            }
        }

        private async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
