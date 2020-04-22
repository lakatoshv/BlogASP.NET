using System.Web.Mvc;
using Blog.Data.Models;
using Blog.Data.Models.Repository;
using Blog.Data.Models.Repository.Interfaces;
using Blog.Services;
using Blog.Services.Interfaces;
using Blog.Services.Posts;
using Blog.Services.Posts.Interfaces;
using Unity;
using Unity.Mvc5;

namespace Blog
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();


            container.RegisterType<ICommentsService, CommentsService>();
            container.RegisterType<IMessagesService, MessagesService>();
            container.RegisterType<IPostsService, PostsService>();
            container.RegisterType<IProfilesService, ProfilesService>();
            container.RegisterType<ITagsService, TagsService>();


            container.RegisterType<IRepository<Comment>, Repository<Comment>>();
            container.RegisterType<IRepository<Message>, Repository<Message>>();
            container.RegisterType<IRepository<Post>, Repository<Post>>();
            container.RegisterType<IRepository<Profile>, Repository<Profile>>();
            container.RegisterType<IRepository<Tag>, Repository<Tag>>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}