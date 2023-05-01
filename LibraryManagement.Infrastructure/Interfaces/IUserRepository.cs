using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="user">The user object to add.</param>
        Task AddUserAsync(User user);

        /// <summary>
        /// Finds a user by their username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The user object, or null if not found.</returns>
        Task<User> FindUserByUsernameAsync(string username);

        /// <summary>
        /// Finds a user by their personal code.
        /// </summary>
        /// <param name="personalCode">The personal code to search for.</param>
        /// <returns>The user object, or null if not found.</returns>
        Task<User> FindUserByPersonalCodeAsync(string personalCode);

        /// <summary>
        /// Finds a user by their ID.
        /// </summary>
        /// <param name="id">The ID to search for.</param>
        /// <returns>The user object, or null if not found.</returns>
        Task<User> FindUserByIdAsync(Guid id);

        /// <summary>
        /// Finds the profile image for a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The profile image for the user, or null if not found.</returns>
        Task<ProfileImage> FindImageByUserIdAsync(Guid id);

        /// <summary>
        /// Edits a user's personal information.
        /// </summary>
        /// <param name="person">The updated person object.</param>
        /// <param name="userId">The ID of the user to update.</param>
        Task EditUsersPersonInformationAsync(Person person, Guid userId);

        /// <summary>
        /// Edits a user's address information.
        /// </summary>
        /// <param name="address">The updated address object.</param>
        /// <param name="userId">The ID of the user to update.</param>
        Task EditUsersAdressInformationAsync(Address address, Guid userId);

        /// <summary>
        /// Updates a property of a user's person object.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="property">The name of the property to update.</param>
        /// <param name="value">The new value for the property.</param>
        /// <returns>A message indicating whether the update was successful.</returns>
        Task<string> UpdatePersonAsync(Guid userId, string property, string value);

        /// <summary>
        /// Updates a property of a user's address object.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="property">The name of the property to update.</param>
        /// <param name="value">The new value for the property.</param>
        /// <returns>A message indicating whether the update was successful.</returns>
        Task<string> UpdateAddressAsync(Guid userId, string property, string value);

        /// <summary>
        /// Updates a user's profile image.
        /// </summary>
        /// <param name="newImage">The new profile image to save.</param>
        /// <param name="id">The ID of the user to update.</param>
        /// <returns>The updated profile image.</returns>
        Task<ProfileImage> UpdateUsersImageAsync(ProfileImage newImage, Guid id);

        /// <summary>
        /// Deletes a user from the repository by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        Task DeleteUserByIdAsync(Guid id);
    }
}
