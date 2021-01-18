using System.Data.Entity;
using Blog.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BLog.Data
{
    /// <summary>
    /// Blog context.
    /// </summary>
    /// <seealso cref="IdentityDbContext{ApplicationUser}" />
    public class BlogContext : IdentityDbContext<ApplicationUser>
    {
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the profiles.
        /// </summary>
        /// <value>
        /// The profiles.
        /// </value>
        public DbSet<Profile> Profiles { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public DbSet<Message> Messages { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogContext"/> class.
        /// </summary>
        public BlogContext()
            : base("blogaspnet", throwIfV1Schema: false)
        {
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>BlogContext.</returns>
        public static BlogContext Create() =>
            new BlogContext();

        /// <summary>
        /// On model creating.
        /// </summary>
        /// <param name="builder">builder.</param>
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            /*ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }*/

            // builder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            // builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            builder.Entity<Profile>()
                .HasRequired(p => p.ApplicationUser)
                .WithOptional(t => t.Profile)
                .WillCascadeOnDelete(false);

            builder.Entity<ApplicationUser>()
                .HasMany(p => p.Posts)
                .WithRequired(t => t.Author)
                .HasForeignKey(s => s.AuthorId)
                .WillCascadeOnDelete(false);

            builder.Entity<ApplicationUser>()
                .HasMany(p => p.Comments)
                .WithRequired(t => t.Author)
                .HasForeignKey(s => s.AuthorId)
                .WillCascadeOnDelete(false);

            builder.Entity<ApplicationUser>()
                .HasMany(p => p.Messages)
                .WithOptional(t => t.Sender)
                .HasForeignKey(s => s.SenderId)
                .WillCascadeOnDelete(false);

            builder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithRequired(t => t.Post)
                .HasForeignKey(s => s.PostId)
                .WillCascadeOnDelete(false);

            builder.Entity<Post>()
                .HasMany(p => p.PostTags)
                .WithRequired(t => t.Post)
                .HasForeignKey(s => s.PostId)
                .WillCascadeOnDelete(false);
        }
    }
}