using AutoFixture.Xunit2;
using AutoMapper;
using LibraryManagement.Application.Dto.Requests;
using LibraryManagement.Application.Dto.Responses;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LibraryManagement.Tests
{
    public class ServiceTests
    {
        [Theory, AutoData]
        public async void UserServices_AddUsersInformationAsyncMethod_WorksCorreclty(AddInformationRequest request, User user, Person person, Address address)
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var configurationMock = new Mock<IConfiguration>();
            var mapperMock = new Mock<IMapper>();
            IUserService sut = new UserService(userRepositoryMock.Object, configurationMock.Object, mapperMock.Object);

            userRepositoryMock.Setup(x => x.FindUserByIdAsync(user.Id)).ReturnsAsync(user);

            mapperMock.Setup(x => x.Map<Person>(request)).Returns(person);
            mapperMock.Setup(x => x.Map<Address>(request)).Returns(address);
            mapperMock.Setup(x => x.Map(request, user)).Returns(user);

            //Act
            var testResponse = await sut.AddUsersInformationAsync(request, user.Id);

            //Assert
            userRepositoryMock.Verify(x => x.EditUsersPersonInformationAsync(person, user.Id), Times.Once);
            userRepositoryMock.Verify(x => x.EditUsersAdressInformationAsync(address, user.Id), Times.Once);
        }

        [Theory, AutoData]
        public async void UserServices_UpdatePersonAsyncMethod_WorksCorreclty(User user, Person person)
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            IUserService sut = new UserService(userRepositoryMock.Object, null, null);
            userRepositoryMock.Setup(x => x.UpdatePersonAsync(user.Id, "FirstName", person.FirstName))
        .ReturnsAsync(person.FirstName);
            userRepositoryMock.Setup(x => x.UpdatePersonAsync(user.Id, "LastName", person.LastName))
        .ReturnsAsync(person.LastName);
            userRepositoryMock.Setup(x => x.UpdatePersonAsync(user.Id, "PersonalCode", person.PersonalCode))
        .ReturnsAsync(person.PersonalCode);
            userRepositoryMock.Setup(x => x.UpdatePersonAsync(user.Id, "PhoneNumber", person.PhoneNumber))
        .ReturnsAsync(person.PhoneNumber);
            userRepositoryMock.Setup(x => x.UpdatePersonAsync(user.Id, "Email", person.Email))
        .ReturnsAsync(person.Email);

            //Act
            var resultFirstName = await sut.UpdatePersonAsync(user.Id, "FirstName", person.FirstName);
            var resultLastName = await sut.UpdatePersonAsync(user.Id, "LastName", person.LastName);
            var resultPersonCode = await sut.UpdatePersonAsync(user.Id, "PersonalCode", person.PersonalCode);
            var resultPhoneNumber = await sut.UpdatePersonAsync(user.Id, "PhoneNumber", person.PhoneNumber);
            var resultEmail = await sut.UpdatePersonAsync(user.Id, "Email", person.Email);

            //Assert
            Assert.Equal(person.FirstName, resultFirstName);
            Assert.Equal(person.LastName, resultLastName);
            Assert.Equal(person.PersonalCode, resultPersonCode);
            Assert.Equal(person.PhoneNumber, resultPhoneNumber);
            Assert.Equal(person.Email, resultEmail);
        }

        [Theory, AutoData]
        public async void UserServices_UpdateAdressAsyncMethod_WorksCorreclty(User user, Address address)
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            IUserService sut = new UserService(userRepositoryMock.Object, null, null);
            userRepositoryMock.Setup(x => x.UpdateAddressAsync(user.Id, "City", address.City))
        .ReturnsAsync(address.City);
            userRepositoryMock.Setup(x => x.UpdateAddressAsync(user.Id, "Street", address.Street))
        .ReturnsAsync(address.Street);
            userRepositoryMock.Setup(x => x.UpdateAddressAsync(user.Id, "HouseNumber", address.HouseNumber))
        .ReturnsAsync(address.HouseNumber);
            userRepositoryMock.Setup(x => x.UpdateAddressAsync(user.Id, "FlatNumber", address.FlatNumber))
        .ReturnsAsync(address.FlatNumber);

            //Act
            var resultCity = await sut.UpdateAddressAsync(user.Id, "City", address.City);
            var resultStreet = await sut.UpdateAddressAsync(user.Id, "Street", address.Street);
            var resultHouseNumber = await sut.UpdateAddressAsync(user.Id, "HouseNumber", address.HouseNumber);
            var resultFlatNumber = await sut.UpdateAddressAsync(user.Id, "FlatNumber", address.FlatNumber);

            //Assert
            Assert.Equal(address.City, resultCity);
            Assert.Equal(address.Street, resultStreet);
            Assert.Equal(address.HouseNumber, resultHouseNumber);
            Assert.Equal(address.FlatNumber, resultFlatNumber);
        }

        [Theory, AutoData]
        public async void UserServices_CheckPersonnalCodeExistAsyncMethod_ReturnsCorrectBool(User user)
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var sut = new UserService(userRepositoryMock.Object, null, null);
            userRepositoryMock.Setup(x => x.FindUserByPersonalCodeAsync(user.Person.PersonalCode)).ReturnsAsync(user);
            var invalidPersonalCode = "123456789";

            //Act
            var personalCodeExist = await sut.CheckPersonnalCodeExistAsync(user.Id, user.Person.PersonalCode);
            var personalCodeDoesntExist = await sut.CheckPersonnalCodeExistAsync(user.Id, invalidPersonalCode);

            //Assert
            Assert.True(personalCodeExist);
            Assert.False(personalCodeDoesntExist);
        }

        [Theory, AutoData]
        public async Task UserServices_GetUserInformationAsyncMethod_ReturnCorrectUser(User user, UserInformationResponse response)
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var sut = new UserService(userRepositoryMock.Object, null, mapperMock.Object);
            userRepositoryMock.Setup(x => x.FindUserByIdAsync(user.Id)).ReturnsAsync(user);

            mapperMock.Setup(x => x.Map<UserInformationResponse>(user)).Returns(response);
            mapperMock.Setup(x => x.Map(user, response)).Returns(response);

            //Act
            var actualUser = await sut.GetUserInformationAsync(user.Id);

            //Assert
            Assert.Equal(response.Username, actualUser.Username);
            Assert.Equal(response.Person.Id, actualUser.Person.Id);
            Assert.Equal(response.Person.Address.Id, actualUser.Person.Address.Id);
        }

        [Theory, AutoData]
        public async Task UserServices_DeleteUserByIdAsyncMethod_RemovesUserCorrectly(User user)
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var sut = new UserService(userRepositoryMock.Object, null, null);
            userRepositoryMock.Setup(x => x.FindUserByIdAsync(user.Id)).ReturnsAsync(user);

            //Act
            var result = await sut.DeleteUserByIdAsync(user.Id);

            //Assert
            userRepositoryMock.Verify(x => x.DeleteUserByIdAsync(user.Id), Times.Once);
            Assert.True(result);
        }
    }
}
