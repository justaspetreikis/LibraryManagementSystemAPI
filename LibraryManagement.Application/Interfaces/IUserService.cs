using LibraryManagement.Application.Dto.Requests;
using LibraryManagement.Application.Dto.Response;
using LibraryManagement.Application.Dto.Responses;

namespace LibraryManagement.Application.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Signs up a new user and returns a response with username and user ID.
        /// </summary>
        /// <param name="request">The sign-up request object containing username, password and profile image.</param>
        /// <returns>A response object containing username and user ID.</returns>
        Task<UserSignUpResponse> SignUpAsync(SignUpRequest request);

        /// <summary>
        /// Logs in a user with the provided username and password.
        /// </summary>
        /// <param name="username">The username of the user to log in.</param>
        /// <param name="password">The password of the user to log in.</param>
        /// <returns>A JWT token as a string on successful login, otherwise throws exception.</returns>
        Task<string> LoginAsync(string username, string password);

        /// <summary>
        /// Adds a user's personal and address information to their profile.
        /// </summary>
        /// <param name="information">The information to add to the user's profile.</param>
        /// <param name="userId">The ID of the user to add information for.</param>
        /// <returns>A response object indicating whether the information was successfully added.</returns>
        Task<AddInformationResponse> AddUsersInformationAsync(AddInformationRequest information, Guid userId);

        /// <summary>
        /// Updates a user's personal information.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="property">The name of the property to update.</param>
        /// <param name="value">The new value for the property.</param>
        /// <returns>A string indicating whether the update was successful.</returns>
        Task<string> UpdatePersonAsync(Guid userId, string property, string value);

        /// <summary>
        /// Updates a user's address information.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="property">The name of the property to update.</param>
        /// <param name="value">The new value for the property.</param>
        /// <returns>A string indicating whether the update was successful.</returns>
        Task<string> UpdateAddressAsync(Guid userId, string property, string value);

        /// <summary>
        /// Checks if a personal code exists for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <param name="personalCode">The personal code to check.</param>
        /// <returns>True if the personal code exists, otherwise false.</returns>
        Task<bool> CheckPersonnalCodeExistAsync(Guid userId, string personalCode);

        /// <summary>
        /// Gets a user's information by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to get information for.</param>
        /// <returns>A response object containing the user's information.</returns>
        Task<UserInformationResponse> GetUserInformationAsync(Guid userId);

        /// <summary>
        /// Deletes a user from the repository by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>True if the user was successfully deleted, otherwise false.</returns>
        Task<bool> DeleteUserByIdAsync(Guid id);

        /// <summary>
        /// Updates the profile image of the user with the specified user ID with the provided new image.
        /// </summary>
        /// <param name="userId">The ID of the user whose profile image is to be updated.</param>
        /// <param name="newImage">The new profile image to be set for the user.</param>
        /// <returns>An object containing id, name, conntent type and bytes of the updated profile image.</returns>
        Task<UpdateImageResponse> UpdateImageAsync(Guid userId, UpdateProfileImageRequest newImage);
    }
}
