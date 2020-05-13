using System.Data.Entity;
using System.Threading.Tasks;
using BLog.Data;
using Blog.Data.Models;
using BLog.Data.Repository.Interfaces;
using Blog.Services.Core.Dtos.Users;
using Blog.Services.Interfaces;
using Blog.Services.GeneralService;
using Blog.Services.Posts.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Services
{
    /// <summary>
    /// Profiles service.
    /// </summary>
    /// <seealso cref="IProfilesService" />
    public class ProfilesService : GeneralService<Profile>, IProfilesService
    {

        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// The posts service.
        /// </summary>
        private readonly IPostsService _postsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilesService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="postsService">The posts service.</param>
        public ProfilesService(IRepository<Profile> repository,
            IPostsService postsService)
            : base(repository)
        {

            var store = new UserStore<ApplicationUser>(new BlogContext());
            _userManager = new UserManager<ApplicationUser>(store);

            _postsService = postsService;
        }

        /// <inheritdoc/>
        public async Task<ProfileDto> GetProfileByUserId(int id, string userId)
        {
            var profile = new ProfileDto { Profile = await Where(pr => pr.Id.Equals(id))
                .Include(x => x.ApplicationUser).FirstOrDefaultAsync() };
            if (profile.Profile == null || !profile.Profile.ApplicationUserId.Equals(userId))
            {
                return null;
            }
            return profile;
        }

        /// <inheritdoc/>
        public async Task<ProfileDto> GetProfileWithPostsByUserId(string userId) =>
            new ProfileDto
            {
                Profile = await Where(pr => pr.ApplicationUserId.Equals(userId))
                    .Include(x => x.ApplicationUser).FirstOrDefaultAsync(),
                Posts = await _postsService.Where(post => post.AuthorId.Equals(userId)).ToListAsync()
            };

        /// <inheritdoc/>
        public async Task<ProfileDto> GetProfileWithPostsById(int id)
        {
            var profile = new ProfileDto
            {
                Profile = await Where(pr => pr.Id.Equals(id))
                    .Include(x => x.ApplicationUser).FirstOrDefaultAsync()
            };
            if (profile.Profile != null && string.IsNullOrWhiteSpace(profile.Profile.ApplicationUserId))
            {
                return null;
            }

            profile.Posts = await _postsService
                .Where(post => post.Author.Equals(profile.Profile.ApplicationUser)).ToListAsync();

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

            profile.ApplicationUserId = userId;
            profile.Id = profileId;

            await UpdateAsync(profile);
        }
    }
}