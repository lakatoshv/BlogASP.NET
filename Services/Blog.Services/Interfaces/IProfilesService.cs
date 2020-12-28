using System.Threading.Tasks;
using Blog.Data.Models;
using Blog.Services.Core.Dtos.Users;
using Blog.Services.GeneralService.Interfaces;

namespace Blog.Services.Interfaces
{
    /// <summary>
    /// Profiles service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{T}" />
    public interface IProfilesService : IGeneralService<Profile>
    {
        /// <summary>
        /// Gets the profile by user identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task.</returns>
        Task<ProfileDto> GetProfileByUserId(int id, string userId);

        /// <summary>
        /// Gets the profile with posts by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task.</returns>
        Task<ProfileDto> GetProfileWithPostsByUserId(string userId);

        /// <summary>
        /// Gets the profile with posts by identifier.
        /// </summary>
        /// <param name="id">The profile identifier.</param>
        /// <returns>Task.</returns>
        Task<ProfileDto> GetProfileWithPostsById(int id);

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