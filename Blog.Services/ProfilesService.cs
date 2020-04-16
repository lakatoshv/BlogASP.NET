using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Blog.Data.Models;
using Blog.Services.Dtos.Users;
using Blog.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Services
{
    /// <summary>
    /// Profiles service.
    /// </summary>
    /// <seealso cref="IProfilesService" />
    public class ProfilesService : IProfilesService
    {
        /// <summary>
        /// Blog context
        /// </summary>
        private readonly BlogContext _dbContext;

        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilesService"/> class.
        /// </summary>
        public ProfilesService()
        {
            _dbContext = new BlogContext();

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            _userManager = new UserManager<ApplicationUser>(store);
        }

        /// <inheritdoc/>
        public async Task<ProfileDto> GetCurrentUserProfileById(int id, string currentUserId)
        {
            var profile = new ProfileDto {ProfileData = await _dbContext.Profiles.FirstOrDefaultAsync(pr => pr.Id.Equals(id))};
            if (profile.ProfileData == null || !profile.ProfileData.ApplicationUser.Equals(currentUserId))
            {
                return null;
            }

            profile.UserData = await _userManager.FindByIdAsync(currentUserId);
            return profile;
        }

        /// <inheritdoc/>
        public async Task<ProfileDto> GetCurrentUserProfileWithPosts(string currentUserId)
        {
            
            return new ProfileDto
            {
                UserData = await _userManager.FindByIdAsync(currentUserId),
                ProfileData = await _dbContext.Profiles.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(currentUserId)),
                Posts = await _dbContext.Posts.Where(post => post.Author.Equals(currentUserId)).ToListAsync()
            };
        }

        /// <inheritdoc/>
        public async Task<ProfileDto> GetProfileWithPosts(int profileId)
        {
            var profile = new ProfileDto
            {
                ProfileData = await _dbContext.Profiles.FirstOrDefaultAsync(pr => pr.Id.Equals(profileId))
            };
            if (profile.ProfileData != null && string.IsNullOrWhiteSpace(profile.ProfileData.ApplicationUser))
            {
                return null;
            }

            profile.Posts = await _dbContext.Posts.Where(post => post.Author.Equals(profile.ProfileData.ApplicationUser)).ToListAsync();
            if (profile.ProfileData != null)
            {
                profile.UserData = await _userManager.FindByIdAsync(profile.ProfileData.ApplicationUser);
            }

            return profile;
        }

        // Simplify with automapper.
        /// <inheritdoc/>
        public async Task UpdateProfile(string userId, string email, string phoneNumber, int profileId, Profile profile)
        {

            var userModel = await _userManager.FindByIdAsync(userId);
            //if (!model.ProfileData.ApplicationUser.Equals(userId)) return RedirectToAction("Index", "Posts");
            userModel.Email = email;
            userModel.PhoneNumber = phoneNumber;
            await _userManager.UpdateAsync(userModel);

            profile.ApplicationUser = userId;
            profile.Id = profileId;

            _dbContext.Entry(profile).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}