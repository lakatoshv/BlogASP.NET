using System.Web;
using System.Web.Mvc;
using Blog.Controllers;
using BLog.Data;
using Blog.Data.Models;
using Blog.Services.Interfaces;
using Blog.Services.Posts;
using Blog.Services.Posts.Interfaces;
using BLog.Data.Repository;
using BLog.Data.Repository.Interfaces;
using Blog.Services;
using Blog.Services.Identity;
using Blog.Services.Identity.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace Blog
{
    /// <summary>
    /// Unity configuration.
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Register components.
        /// </summary>
        [System.Obsolete]
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<ICommentsService, CommentsService>();
            container.RegisterType<IMessagesService, MessagesService>();
            container.RegisterType<IPostsService, PostsService>();
            container.RegisterType<IProfilesService, ProfilesService>();
            container.RegisterType<ITagsService, TagsService>();
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();

            // Identity
            container.RegisterType<ApplicationSignInManager>();
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<ISmsService, SmsService>();


            container.RegisterType<HttpContextBase>(
                new InjectionFactory(_ => new HttpContextWrapper(HttpContext.Current)));
            container.RegisterType<IOwinContext>(new InjectionFactory(c => c.Resolve<HttpContextBase>().GetOwinContext()));
            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(c => c.Resolve<IOwinContext>().Authentication));

            container.RegisterType<IdentityDbContext<ApplicationUser>, BlogContext>();
            container.RegisterType<UserManager<ApplicationUser>>();
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();

            container.RegisterType<AccountController>(
                new InjectionConstructor());

            // Entities
            container.RegisterType<IRepository<Comment>, Repository<Comment>>();
            container.RegisterType<IRepository<Message>, Repository<Message>>();
            container.RegisterType<IRepository<Post>, Repository<Post>>();
            container.RegisterType<IRepository<Profile>, Repository<Profile>>();
            container.RegisterType<IRepository<Tag>, Repository<Tag>>();
            container.RegisterType<IRepository<ApplicationUser>, Repository<ApplicationUser>>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}