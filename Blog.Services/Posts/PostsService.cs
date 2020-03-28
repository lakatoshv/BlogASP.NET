using System;
using Blog.Data.Models;
using Blog.Services.Posts.Interfaces;
using Blog.Services.Dtos.Posts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Blog.Services.Dtos;
using Blog.Core.Enums;
using Blog.Core.HelperClasses;

namespace Blog.Services.Posts
{
    /// <summary>
    /// Posts service.
    /// </summary>
    public class PostsService : IPostsService
    {
        /// <summary>
        /// Comments service.
        /// </summary>
        private readonly CommentsService _commentsService;

        /// <summary>
        /// Blog context
        /// </summary>
        private BlogContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsService"/> class.
        /// </summary>
        public PostsService()
        {
            _commentsService = new CommentsService();
            _dbContext = new BlogContext();
        }

        public async Task<PostsDto> GetPosts(string search, bool onlyWithComments = false)
        {
            IList<PostDto> postModel = new List<PostDto>();

            IQueryable<Post> postsQueryable = _dbContext.Posts;
            if (!string.IsNullOrWhiteSpace(search))
            {
                postsQueryable = postsQueryable.Where(post => post.Title.Equals(search));
            }

            var posts = await postsQueryable.ToListAsync();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(async item => {
                PostDto post = new PostDto()
                {
                    Profile = await _dbContext.Profiles.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(item.Author)),
                    CommentsCount = _dbContext.Comments.Count(comment => comment.PostID.Equals(item.Id))
                };
                ApplicationUser user = await userManager.FindByIdAsync(item.Author);
                if (user != null)
                {
                    item.Author = user.UserName;
                }


                post.Post = item;

                var tags = await _dbContext.Tags.Where(tag => tag.PostId.Equals(item.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    post.Post.PostTags.Add(tag);
                }

                post.CommentsCount = _dbContext.Comments.Count(comment => comment.PostID.Equals(item.Id));
                postModel.Add(post);
            });

            if (onlyWithComments)
            {
                postModel = postModel.Where(x => x.CommentsCount > 0).ToList();
            }
            return new PostsDto() { Posts = postModel };
        }

        /// <inheritdoc/>
        public async Task<PostShowDto> GetPost(int postId)
        {
            PostShowDto postModel = new PostShowDto()
            {
                Post = await _dbContext.Posts.FirstOrDefaultAsync(post => post.Id.Equals(postId))
            };
            postModel.Profile = await _dbContext.Profiles.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(postModel.Post.Author));
            if (postModel.Post == null) return null;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var author = postModel.Post.Author;
            ApplicationUser user = await userManager.FindByIdAsync(author);
            if (user != null)
            {
                postModel.Post.Author = user.UserName;
            }

            var tags = await _dbContext.Tags.Where(tag => tag.PostId.Equals(postId)).ToListAsync();
            foreach (var tag in tags)
            {
                postModel.Post.PostTags.Add(tag);
            }
            
            return postModel;
        }

        public async Task<PostShowDto> GetPostModel(int postId)
        {
            PostShowDto postModel = new PostShowDto()
            {
                Post = await _dbContext.Posts.FirstOrDefaultAsync(post => post.Id.Equals(postId))
            };
            postModel.Profile = await _dbContext.Profiles.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(postModel.Post.Author));
            if (postModel.Post == null) return null;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var author = postModel.Post.Author;
            ApplicationUser user = await userManager.FindByIdAsync(author);
            if (user != null)
            {
                postModel.Post.Author = user.UserName;
            }

            var tags = _dbContext.Tags.Where(tag => tag.PostId.Equals(postId)).ToList();
            foreach (var tag in tags)
            {
                postModel.Post.PostTags.Add(tag);
            }
            
            return postModel;
        }

        /// <inheritdoc/>
        public async Task<PostShowDto> GetPostWithComments(int postId, SortParametersDto sortParameters)
        {
            PostShowDto postModel = new PostShowDto()
            {
                Post = await _dbContext.Posts.FirstOrDefaultAsync(post => post.Id.Equals(postId))
            };
            postModel.Profile = await _dbContext.Profiles.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(postModel.Post.Author));
            if (postModel.Post == null) return null;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var author = postModel.Post.Author;
            ApplicationUser user = await userManager.FindByIdAsync(author);
            if (user != null)
            {
                postModel.Post.Author = user.UserName;
            }

            var tags = await _dbContext.Tags.Where(tag => tag.PostId.Equals(postId)).ToListAsync();
            foreach (var tag in tags)
            {
                postModel.Post.PostTags.Add(tag);
            }

            postModel.Comments = await _commentsService.GetPagedCommentsByPostId(postId, author, sortParameters);


            return postModel;
        }

        /// <inheritdoc/>
        public async Task<PostsDto> GetUserPosts(string userId, SortParametersDto sortParameters, string search)
        {
            IList<PostDto> postModel = new List<PostDto>();

            IQueryable<Post> postsEnumerable = _dbContext.Posts.Where(post => post.Author.Equals(userId));
            if (!string.IsNullOrWhiteSpace(search))
                postsEnumerable = postsEnumerable.Where(post => post.Title.Equals(search));

            var posts = await SortPosts(postsEnumerable, sortParameters).ToListAsync();

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(async item => {
                PostDto post = new PostDto()
                {
                    Profile = await _dbContext.Profiles.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(item.Author))
                };

                ApplicationUser user = await userManager.FindByIdAsync(item.Author);
                if (user != null)
                {
                    item.Author = user.UserName;
                }


                post.Post = item;
                post.Post.PostTags = new List<Tag>();

                var tags = await _dbContext.Tags.Where(tag => tag.PostId.Equals(item.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    post.Post.PostTags.Add(tag);
                }

                post.CommentsCount = _dbContext.Comments.Count(comment => comment.PostID.Equals(item.Id));
                postModel.Add(post);
            });
            PostsDto postsViewModel = new PostsDto();
            if (!sortParameters.DisplayType.Equals("grid"))
                postsViewModel.Posts = postModel.Skip((sortParameters.CurrentPage - 1) * sortParameters.PageSize)
                    .Take(sortParameters.PageSize).ToList();
            else
                postsViewModel.Posts = postModel;
            postsViewModel.PageInfo = new PageInfo { PageNumber = sortParameters.CurrentPage, PageSize = sortParameters.PageSize, TotalItems = postModel.Count };
            return postsViewModel;
        }

        /// <inheritdoc/>
        public async Task<PostsDto> GetUserPosts(string userId, string search)
        {
            IList<PostDto> postModel = new List<PostDto>();

            IQueryable<Post> postsQueryable = _dbContext.Posts.Where(post => post.Author.Equals(userId));
            if (!string.IsNullOrWhiteSpace(search))
                postsQueryable = postsQueryable.Where(post => post.Title.Equals(search));

            var posts = await postsQueryable.ToListAsync();

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(async item => {
                PostDto post = new PostDto()
                {
                    Profile = await _dbContext.Profiles.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(item.Author))
                };

                ApplicationUser user = await userManager.FindByIdAsync(item.Author);
                if (user != null)
                {
                    item.Author = user.UserName;
                }


                post.Post = item;
                post.Post.PostTags = new List<Tag>();

                var tags = await _dbContext.Tags.Where(tag => tag.PostId.Equals(item.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    post.Post.PostTags.Add(tag);
                }

                post.CommentsCount = _dbContext.Comments.Count(comment => comment.PostID.Equals(item.Id));
                postModel.Add(post);
            });

            return new PostsDto() { Posts = postModel };
        }

        /// <inheritdoc/>
        public async Task<PostsDto> GetPosts(SortParametersDto sortParameters, string search)
        {
            IList<PostDto> postModel = new List<PostDto>();

            IQueryable<Post> postsEnumerable = _dbContext.Posts.Where(post => post.Status == Status.Approved);
            if (!string.IsNullOrWhiteSpace(search))
                postsEnumerable = postsEnumerable.Where(post => post.Title.Equals(search));
            List<Post> posts = await SortPosts(postsEnumerable, sortParameters).ToListAsync();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(async item =>
            {
                PostDto post = new PostDto()
                {
                    Profile = await _dbContext.Profiles.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(item.Author))
                };

                ApplicationUser user = await userManager.FindByIdAsync(item.Author);
                if (user != null)
                    item.Author = user.UserName;


                post.Post = item;

                var tags = await _dbContext.Tags.Where(tag => tag.PostId.Equals(item.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    post.Post.PostTags.Add(tag);
                }

                post.CommentsCount = _dbContext.Comments.Count(comment => comment.PostID.Equals(item.Id));
                postModel.Add(post);
            });
            PostsDto postsViewModel = new PostsDto()
            {
                Posts = postModel.Skip((sortParameters.CurrentPage - 1) * sortParameters.PageSize)
                    .Take(sortParameters.PageSize).ToList(),
                PageInfo = new PageInfo
                {
                    PageNumber = sortParameters.CurrentPage,
                    PageSize = sortParameters.PageSize,
                    TotalItems = postModel.Count
                }
            };

            return postsViewModel;
        }

        /// <inheritdoc/>
        public async Task CreatePost(Post postModel)
        {
            
            _dbContext.Posts.Add(postModel);
            await _dbContext.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(postModel.Tags))
            {
                String[] tags = postModel.Tags.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var tag in tags)
                {
                    var tagForAdd = new Tag()
                    {
                        Title = tag,
                        PostId = postModel.Id
                    };
                    _dbContext.Tags.Add(tagForAdd);

                }
            }
            _dbContext.SaveChanges();
            await _dbContext.Tags.Where(tag => tag.PostId.Equals(postModel.Id)).ForEachAsync(
                t =>
                {
                    postModel.PostTags.Add(t);
                }

            );
            _dbContext.Entry(postModel).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public async Task EditPost(int id, Post post)
        {
            var postModel = await GetPostModel(id);

            post.Author = postModel.Post.Author;
            post.Likes = postModel.Post.Likes;
            post.Dislikes = postModel.Post.Dislikes;
            post.Seen = postModel.Post.Seen;
            post.CreatedAt = DateTime.Now;
            _dbContext.Entry(post).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task ChangePostStatus(int id, Status status)
        {
            var post = await GetPost(id);
            post.Status = status;
            _dbContext.Entry(post).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeletePost(int id)
        {
            Post postForDelete = await _dbContext.Posts.FirstOrDefaultAsync(post => post.Id.Equals(id));

            if (postForDelete != null)
            {
                await _commentsService.DeletePostComments(id);

                _dbContext.Posts.Remove(postForDelete);
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Sort posts by sort parameters.
        /// </summary>
        /// <param name="posts">posts.</param>
        /// <param name="sortParameters">sortParameters.</param>
        /// <returns>IOrderedEnumerable.</returns>
        private IOrderedQueryable<Post> SortPosts(IQueryable<Post> posts, SortParametersDto sortParameters)
        {
            if (sortParameters.SortBy.Equals("CreatedAt") && sortParameters.OrderBy.Equals("asc"))
                return posts.OrderBy(x => x.CreatedAt);
            if (sortParameters.SortBy.Equals("CreatedAt") && sortParameters.OrderBy.Equals("desc"))
                return posts.OrderByDescending(x => x.CreatedAt);

            if (sortParameters.SortBy.Equals("Title") && sortParameters.OrderBy.Equals("asc"))
                return posts.OrderBy(x => x.Title);
            if (sortParameters.SortBy.Equals("Title") && sortParameters.OrderBy.Equals("desc"))
                return posts.OrderByDescending(x => x.Title);

            if (sortParameters.SortBy.Equals("Author") && sortParameters.OrderBy.Equals("asc"))
                return posts.OrderBy(x => x.Author);
            if (sortParameters.SortBy.Equals("Author") && sortParameters.OrderBy.Equals("desc"))
                return posts.OrderByDescending(x => x.Author);

            if (sortParameters.SortBy.Equals("Likes") && sortParameters.OrderBy.Equals("asc"))
                return posts.OrderBy(x => x.Likes);
            if (sortParameters.SortBy.Equals("Likes") && sortParameters.OrderBy.Equals("desc"))
                return posts.OrderByDescending(x => x.Likes);

            if (sortParameters.SortBy.Equals("Dislikes") && sortParameters.OrderBy.Equals("asc"))
                return posts.OrderBy(x => x.Dislikes);
            if (sortParameters.SortBy.Equals("Dislikes") && sortParameters.OrderBy.Equals("desc"))
                return posts.OrderByDescending(x => x.Dislikes);

            else
                return posts.OrderBy(x => x.Id);
            /*
            Expression<Func<Post, object>> sortExpression;
            switch (sortParameters.SortBy)
            {
                case :
                    sortExpression = (x => x.CreatedAt);
                    break;
                case "Title":
                    sortExpression = (x => x.Title);
                    break;
                case "Author":
                    sortExpression = (x => x.Author);
                    break;
                case "Likes":
                    sortExpression = (x => x.Likes);
                    break;
                case "Dislikes":
                    sortExpression = (x => x.Dislikes);
                    break;
                default:
                    sortExpression = (x => x.Id);
                    break;
            }
            */
        }
    }
}