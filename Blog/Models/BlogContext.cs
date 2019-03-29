using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class BlogContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Profile> Profiles { get; set; }
    }
    public class BlogDbInitializer : DropCreateDatabaseAlways<BlogContext>
    {
        protected override void Seed(BlogContext context)
        {
            context.Posts.Add(new Post {
                Id = 0,
                Title = "Building the orbits of celestial bodies using Python",
                Description = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial).",
                Content = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial). Using this assumption, let us consider the motion of one and the same point in two different reference frames K and K ', of which the second moves relative to the first with an arbitrary speed",
                Author = "Vital L",
                Imgurl = "https://habrastorage.org/webt/od/ie/1k/odie1kfvgxwkus8qxtmateofkek.png",
                Tags = "Development under Windows, Mathematics, Astronomy, Algorithms, Python",
            });
            context.Posts.Add(new Post
            {
                Id = 1,
                Title = "Building the orbits of celestial bodies using Python",
                Description = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial).",
                Content = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial). Using this assumption, let us consider the motion of one and the same point in two different reference frames K and K ', of which the second moves relative to the first with an arbitrary speed",
                Author = "Vital L",
                Imgurl = "https://habrastorage.org/webt/od/ie/1k/odie1kfvgxwkus8qxtmateofkek.png",
                Tags = "Development under Windows, Mathematics, Astronomy, Algorithms, Python",
            });
            context.Posts.Add(new Post
            {
                Id = 2,
                Title = "Building the orbits of celestial bodies using Python",
                Description = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial).",
                Content = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial). Using this assumption, let us consider the motion of one and the same point in two different reference frames K and K ', of which the second moves relative to the first with an arbitrary speed",
                Author = "Vital L",
                Imgurl = "https://habrastorage.org/webt/od/ie/1k/odie1kfvgxwkus8qxtmateofkek.png",
                Tags = "Development under Windows, Mathematics, Astronomy, Algorithms, Python",
            });
            context.Posts.Add(new Post
            {
                Id = 3,
                Title = "Building the orbits of celestial bodies using Python",
                Description = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial).",
                Content = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial). Using this assumption, let us consider the motion of one and the same point in two different reference frames K and K ', of which the second moves relative to the first with an arbitrary speed",
                Author = "Vital L",
                Imgurl = "https://habrastorage.org/webt/od/ie/1k/odie1kfvgxwkus8qxtmateofkek.png",
                Tags = "Development under Windows, Mathematics, Astronomy, Algorithms, Python",
            });
            context.Posts.Add(new Post
            {
                Id = 4,
                Title = "Building the orbits of celestial bodies using Python",
                Description = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial).",
                Content = "To find the trajectories of relative motions in classical mechanics, we use the assumption of absolute time in all reference frames (both inertial and non-inertial). Using this assumption, let us consider the motion of one and the same point in two different reference frames K and K ', of which the second moves relative to the first with an arbitrary speed",
                Author = "Vital L",
                Imgurl = "https://habrastorage.org/webt/od/ie/1k/odie1kfvgxwkus8qxtmateofkek.png",
                Tags = "Development under Windows, Mathematics, Astronomy, Algorithms, Python",
            });
            base.Seed(context);
        }
    }
}