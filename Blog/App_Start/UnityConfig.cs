using System.Web.Mvc;
using Blog.Data.Models;
using Blog.Services.Interfaces;
using Blog.Services.Posts;
using Blog.Services.Posts.Interfaces;
using BLog.Data.Repository;
using BLog.Data.Repository.Interfaces;
using Blog.Services;
using Blog.Services.Identity;
using Blog.Services.Identity.Interfaces;
using Unity;
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
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<ICommentsService, CommentsService>();
            container.RegisterType<IMessagesService, MessagesService>();
            container.RegisterType<IPostsService, PostsService>();
            container.RegisterType<IProfilesService, ProfilesService>();
            container.RegisterType<ITagsService, TagsService>();

            // Identity
            container.RegisterType<ApplicationSignInManager>();
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<ISmsService, SmsService>();

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