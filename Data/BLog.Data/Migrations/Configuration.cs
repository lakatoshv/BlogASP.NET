using System.Data.Entity.Migrations;

namespace BLog.Data.Migrations
{
    /// <summary>
    /// Db Configuration.
    /// </summary>
    /// <seealso cref="DbMigrationsConfiguration{BlogContext}" />
    internal sealed class Configuration : DbMigrationsConfiguration<BlogContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Запускается после обновления до последней миграции для обеспечения возможности обновления начального значения данных.
        /// </summary>
        /// <param name="context">Контекст, используемый для обновления начального значения данных.</param>
        protected override void Seed(BlogContext context)
        {
            //context.Users.Add(new ApplicationUser
            //{
            //    FirstName = "Vitalii",
            //    LastName = "Lakatosh",
            //    Email = "some.pochta1@gmail.com",
            //    PasswordHash = hasher.HashPassword(null, "Test_123"),

            //    Profile = new Profile
            //    {
            //        ProfileImage = "https://habrastorage.org/webt/od/ie/1k/odie1kfvgxwkus8qxtmateofkek.png",
            //    },

            //    Posts = new List<Post>
            //    {
            //        new Post
            //        {
            //            Title = "Building the orbits of celestial bodies using Python",
            //            Description = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial).",
            //            Content = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial). Using this assumption, let us consider the motion of one and the same point in two different reference frames K and K ', of which the second moves relative to the first with an arbitrary speed",
            //            CreatedAt = DateTime.Now,
            //            ImageUrl = "https://habrastorage.org/webt/od/ie/1k/odie1kfvgxwkus8qxtmateofkek.png",
            //            Tags = "Development under Windows, Mathematics, Astronomy, Algorithms, Python",

            //            Comments = new List<Comment>
            //            {
            //                new Comment
            //                {
            //                    AuthorId = "02167117-1222-4afd-96f9-807bdb7a132b",
            //                    CommentBody = "Comment 1",
            //                },
            //                new Comment
            //                {
            //                    AuthorId = "02167117-1222-4afd-96f9-807bdb7a132b",
            //                    CommentBody = "Comment 2",
            //                }
            //            },

            //            PostTags = new List<Tag>
            //            {
            //                new Tag
            //                {
            //                    Title = "Development under Windows",
            //                },
            //                new Tag
            //                {
            //                    Title = "Mathematics",
            //                },
            //                new Tag
            //                {
            //                    Title = "Astronomy",
            //                },
            //                new Tag
            //                {
            //                    Title = "Algorithms",
            //                },
            //            },
            //        },

            //        new Post
            //        {
            //            Title = "Building the orbits of celestial bodies using Python",
            //            Description = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial).",
            //            Content = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial). Using this assumption, let us consider the motion of one and the same point in two different reference frames K and K ', of which the second moves relative to the first with an arbitrary speed",
            //            CreatedAt = DateTime.Now,
            //            ImageUrl = "https://habrastorage.org/webt/od/ie/1k/odie1kfvgxwkus8qxtmateofkek.png",
            //            Tags = "Development under Windows, Mathematics, Astronomy, Algorithms, Python",

            //            Comments = new List<Comment>
            //            {
            //                new Comment
            //                {
            //                    AuthorId = "02167117-1222-4afd-96f9-807bdb7a132b",
            //                    CommentBody = "Comment 1",
            //                },
            //                new Comment
            //                {
            //                    AuthorId = "02167117-1222-4afd-96f9-807bdb7a132b",
            //                    CommentBody = "Comment 2",
            //                }
            //            },

            //            PostTags = new List<Tag>
            //            {
            //                new Tag
            //                {
            //                    Title = "Development under Windows",
            //                },
            //                new Tag
            //                {
            //                    Title = "Mathematics",
            //                },
            //                new Tag
            //                {
            //                    Title = "Astronomy",
            //                },
            //                new Tag
            //                {
            //                    Title = "Algorithms",
            //                },
            //            },
            //        },
            //    }
            //});

            //context.Roles.Add(new IdentityRole("User"));
            //context.Roles.Add(new IdentityRole("Moderator"));
            //context.Roles.Add(new IdentityRole("Admin"));

            //base.Seed(context);
        }
    }
}
