﻿using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Models
{
    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            BlogContext db = new BlogContext();
            Profile profile = db.Profiles.Where(pr => pr.ApplicationUser.Equals(this.Id)).FirstOrDefault();
            if (profile != null)
                userIdentity.AddClaims(new[]
                {
                    new Claim("FirstName", !profile.FirstName.IsNullOrWhiteSpace() ? profile.FirstName : ""),
                    new Claim("LastName", !profile.LastName.IsNullOrWhiteSpace() ? profile.LastName : ""),
                    new Claim("ProfileImg", !profile.ProfileImg.IsNullOrWhiteSpace() ? profile.ProfileImg : "")
                });

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("blogaspnet", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}