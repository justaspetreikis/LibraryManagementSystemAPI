using AutoFixture.Xunit2;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Database;
using LibraryManagement.Infrastructure.Interfaces;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Tests
{
    public class RepositoryTests
    {
        [Theory, AutoData]
        public async Task Repository_AddUserAsyncMethod_AddsUserCorrectly(User user)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<LibraryManagementDbContext>().UseInMemoryDatabase(databaseName: "mydb").Options;
            var testDbContext = new LibraryManagementDbContext(options);
            IUserRepository testRepository = new UserRepository(testDbContext);

            //Act
            await testRepository.AddUserAsync(user);
            var actualUser = await testRepository.FindUserByIdAsync(user.Id);

            //Assert
            Assert.Equal(user, actualUser);
            Assert.NotNull(actualUser);
            Assert.Equal(user.Username, actualUser.Username);
            Assert.Equal(user.PasswordHash, actualUser.PasswordHash);
            Assert.Equal(user.PasswordSalt, actualUser.PasswordSalt);
            Assert.Equal(user.Id, actualUser.Id);
        }

        [Theory, AutoData]
        public async Task Repository_FindUserByIdAsyncMethod_ReturnCorrectUser(User user)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<LibraryManagementDbContext>().UseInMemoryDatabase(databaseName: "mydb").Options;
            var testDbContext = new LibraryManagementDbContext(options);
            IUserRepository testRepository = new UserRepository(testDbContext);
            testDbContext.Users.AddAsync(user);
            testDbContext.SaveChangesAsync();

            //Act
            var actualUser = await testRepository.FindUserByIdAsync(user.Id);

            //Assert
            Assert.Equal(user, actualUser);
        }

        [Theory, AutoData]
        public async Task Repository_FindUserByUsernameAsyncMethod_ReturnCorrectUser(User user)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<LibraryManagementDbContext>().UseInMemoryDatabase(databaseName: "mydb").Options;
            var testDbContext = new LibraryManagementDbContext(options);
            IUserRepository testRepository = new UserRepository(testDbContext);
            testDbContext.Users.AddAsync(user);
            testDbContext.SaveChangesAsync();

            //Act
            var actualUser = await testRepository.FindUserByUsernameAsync(user.Username);

            //Assert
            Assert.Equal(user, actualUser);
        }

        [Theory, AutoData]
        public async Task Repository_FindUserByPersonalCodeAsyncMethod_ReturnCorrectUser(User user)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<LibraryManagementDbContext>().UseInMemoryDatabase(databaseName: "mydb").Options;
            var testDbContext = new LibraryManagementDbContext(options);
            IUserRepository testRepository = new UserRepository(testDbContext);
            await testDbContext.Users.AddAsync(user);
            await testDbContext.SaveChangesAsync();

            //Act
            var actualUser = await testRepository.FindUserByPersonalCodeAsync(user.Person.PersonalCode);

            //Assert
            Assert.Equal(user, actualUser);
        }

        [Theory, AutoData]
        public async Task Repository_FindImageByUserIdAsyncMethod_ReturnCorrectImage(User user)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<LibraryManagementDbContext>().UseInMemoryDatabase(databaseName: "mydb").Options;
            var testDbContext = new LibraryManagementDbContext(options);
            IUserRepository testRepository = new UserRepository(testDbContext);
            await testRepository.AddUserAsync(user);

            //Act
            var actualImage = await testRepository.FindImageByUserIdAsync(user.Id);

            //Assert
            Assert.Equal(user.Person.ProfileImage, actualImage);
        }

        [Theory, AutoData]
        public async Task Repository_EditUsersPersonInformationAsync_AddsPersonInformationCorrectly(Person person, User user)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<LibraryManagementDbContext>().UseInMemoryDatabase(databaseName: "mydb").Options;
            var testDbContext = new LibraryManagementDbContext(options);
            IUserRepository testRepository = new UserRepository(testDbContext);
            await testDbContext.Users.AddAsync(user);
            await testDbContext.SaveChangesAsync();

            //Act
            await testRepository.EditUsersPersonInformationAsync(person, user.Id);
            var actualUser = await testRepository.FindUserByIdAsync(user.Id);
            var actualPerson = actualUser.Person;

            //Assert
            Assert.Equal(person.FirstName, actualPerson.FirstName);
            Assert.Equal(person.LastName, actualPerson.LastName);
            Assert.Equal(person.Email, actualPerson.Email);
            Assert.Equal(person.PersonalCode, actualPerson.PersonalCode);
            Assert.Equal(person.PhoneNumber, actualPerson.PhoneNumber);
        }

        [Theory, AutoData]
        public async Task Repository_EditUsersAddressInformationAsync_AddsAddressInformationCorrectly(Address address, User user)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<LibraryManagementDbContext>().UseInMemoryDatabase(databaseName: "mydb").Options;
            var testDbContext = new LibraryManagementDbContext(options);
            IUserRepository testRepository = new UserRepository(testDbContext);
            await testDbContext.Users.AddAsync(user);
            await testDbContext.SaveChangesAsync();

            //Act
            await testRepository.EditUsersAdressInformationAsync(address, user.Id);
            var actualUser = await testRepository.FindUserByIdAsync(user.Id);
            var actualAdress = actualUser.Person.Address;

            //Assert
            Assert.Equal(address.City, actualAdress.City);
            Assert.Equal(address.Street, actualAdress.Street);
            Assert.Equal(address.HouseNumber, actualAdress.HouseNumber);
            Assert.Equal(address.FlatNumber, actualAdress.FlatNumber);
        }

        [Theory, AutoData]
        public async Task Repository_UpdatePersonAsync_UpdatesUsersPersonCorrectly(User user)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<LibraryManagementDbContext>().UseInMemoryDatabase(databaseName: "mydb").Options;
            var testDbContext = new LibraryManagementDbContext(options);
            IUserRepository testRepository = new UserRepository(testDbContext);
            await testDbContext.Users.AddAsync(user);
            await testDbContext.SaveChangesAsync();

            //Act
            string firstName = "Tom";
            string lastName = "Craig";
            string personalCode = "33333333333";
            string phoneNumber = "+370000000";
            string email = "email@email.lt";
            var actualFirstName = await testRepository.UpdatePersonAsync(user.Id, "FirstName", firstName);
            var actualLastName = await testRepository.UpdatePersonAsync(user.Id, "LastName", lastName);
            var actualPersonalCode = await testRepository.UpdatePersonAsync(user.Id, "PersonalCode", personalCode);
            var actualPhoneNumber = await testRepository.UpdatePersonAsync(user.Id, "PhoneNumber", phoneNumber);
            var actualEmail = await testRepository.UpdatePersonAsync(user.Id, "Email", email);

            //Assert
            Assert.Equal(firstName, actualFirstName);
            Assert.Equal(lastName, actualLastName);
            Assert.Equal(personalCode, actualPersonalCode);
            Assert.Equal(phoneNumber, actualPhoneNumber);
            Assert.Equal(email, actualEmail);
        }

        [Theory, AutoData]
        public async Task Repository_UpdateAdressAsync_UpdatesUsersAddressCorrectly(User user)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<LibraryManagementDbContext>().UseInMemoryDatabase(databaseName: "mydb").Options;
            var testDbContext = new LibraryManagementDbContext(options);
            IUserRepository testRepository = new UserRepository(testDbContext);
            await testDbContext.Users.AddAsync(user);
            await testDbContext.SaveChangesAsync();

            //Act
            string city = "Prienai";
            string street = "Gatve";
            string houseNumber = "10";
            string flatNumber = "6";
            var actualCity = await testRepository.UpdateAddressAsync(user.Id, "City", city);
            var actualStreet = await testRepository.UpdateAddressAsync(user.Id, "Street", street);
            var actualHouseNumber = await testRepository.UpdateAddressAsync(user.Id, "HouseNumber", houseNumber);
            var actualFlatNumber = await testRepository.UpdateAddressAsync(user.Id, "FlatNumber", flatNumber);

            //Assert
            Assert.Equal(city, actualCity);
            Assert.Equal(street, actualStreet);
            Assert.Equal(houseNumber, actualHouseNumber);
            Assert.Equal(flatNumber, actualFlatNumber);
        }

        [Theory, AutoData]
        public async Task Repository_DeleteUserByIdAsync_RemovesUserCorectly(User user)
        {
            //Arrange
            var option = new DbContextOptionsBuilder<LibraryManagementDbContext>().UseInMemoryDatabase(databaseName: "testDB").Options;
            var testDbContext = new LibraryManagementDbContext(option);
            IUserRepository testRepository = new UserRepository(testDbContext);
            await testRepository.AddUserAsync(user);

            //Act
            await testRepository.DeleteUserByIdAsync(user.Id);
            var actualUser = await testRepository.FindUserByIdAsync(user.Id);

            //Asserr
            Assert.Null(actualUser);
        }
    }
}
