using System.Threading.Tasks;
using Blog.Data.Models;
using Blog.Services.Dtos.Users;

namespace Blog.Services.Interfaces
{
    /// <summary>
    /// Profiles service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{Profile}" />
    public interface IProfilesService : IGeneralService<Profile>
    {
        /// <summary>
        /// Gets the current user profile by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="currentUserId">The current user identifier.</param>
        /// <returns>Task.</returns>
        Task<ProfileDto> GetCurrentUserProfileById(int id, string currentUserId);

        /// <summary>
        /// Gets the current user profile with posts.
        /// </summary>
        /// <param name="currentUserId">The current user identifier.</param>
        /// <returns>Task.</returns>
        Task<ProfileDto> GetCurrentUserProfileWithPosts(string currentUserId);

        /// <summary>
        /// Gets the profile with posts.
        /// </summary>
        /// <param name="profileId">The user identifier.</param>
        /// <returns>Task.</returns>
        Task<ProfileDto> GetProfileWithPosts(int profileId);

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="email">The email.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="profileId">The profile identifier.</param>
        /// <param name="profile">The profile.</param>
        /// <returns>Task.</returns>
        Task UpdateProfile(string userId, string email, string phoneNumber, int profileId, Profile profile);
    }
}