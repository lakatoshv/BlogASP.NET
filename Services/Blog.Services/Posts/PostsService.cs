using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Enums;
using Blog.Core.HelperClasses;
using Blog.Data.Models;
using BLog.Data.Repository.Interfaces;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;
using Blog.Services.Posts.Interfaces;
using Blog.Services.GeneralService;
using System.Web.Mvc;

namespace Blog.Services.Posts
{
    /// <summary>
    /// Posts service.
    /// </summary>
    public class PostsService : GeneralService<Post>, IPostsService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostsService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public PostsService(
            IRepository<Post> repository)
            : base(repository)
        {
        }

        /// <inheritdoc cref="IPostsService"/>
        public async Task<PostsDto> GetPosts(SortParametersDto sortParameters, string search, bool onlyWithComments = false)
        {
            var postsQueryable = GetAll();
            if (!string.IsNullOrWhiteSpace(search))
            {
                postsQueryable = postsQueryable.Where(post => post.Title.Equals(search));
            }

            var postModel = await postsQueryable
                .Include(x => x.Author)
                .Include(x => x.Author.Profile)
                .Include(x => x.Comments)
                .Include(x => x.PostTags)
                .ToListAsync();

            if (onlyWithComments)
            {
                postModel = postModel.Where(x => x.Comments.Count > 0).ToList();
            }

            var postsDto = new PostsDto() { Posts = postModel };
            if (sortParameters == null)
            {
                return postsDto;
            }

            postsDto.Posts = SortPosts(postsDto.Posts.AsQueryable(), sortParameters).ToList();

            if (sortParameters.DisplayType != null && !sortParameters.DisplayType.Equals("grid"))
            {
                postsDto.Posts = postModel
                    .Skip((sortParameters.CurrentPage - 1) * sortParameters.PageSize)
                    .Take(sortParameters.PageSize).ToList();
            }

            postsDto.PageInfo = new PageInfo { PageNumber = sortParameters.CurrentPage, PageSize = sortParameters.PageSize, TotalItems = postModel.Count };
            
            return postsDto;
        }

        /// <inheritdoc cref="IPostsService"/>
        public async Task<PostsDto> GetUserPosts(string userId, SortParametersDto sortParameters, string search)
        {
            var postsEnumerable = Table;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                postsEnumerable = postsEnumerable.Where(post => post.AuthorId.Equals(userId));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                postsEnumerable = postsEnumerable.Where(post => post.Title.Equals(search));
            }

            postsEnumerable = postsEnumerable
                .Include(x => x.Author)
                .Include(x => x.Author.Profile)
                .Include(x => x.PostTags)
                .Include(x => x.Comments);

            IList<Post> postModel = sortParameters != null 
                ? await SortPosts(postsEnumerable, sortParameters).ToListAsync()
                : await postsEnumerable.ToListAsync();

            var postsViewModel = new PostsDto();

            if (sortParameters == null) return postsViewModel;

            if (!sortParameters.DisplayType.Equals("grid"))
            {
                postsViewModel.Posts = postModel
                    .Skip((sortParameters.CurrentPage - 1) * sortParameters.PageSize)
                    .Take(sortParameters.PageSize).ToList();
            }
            else
            {
                postsViewModel.Posts = postModel;
            }

            postsViewModel.PageInfo = new PageInfo { PageNumber = sortParameters.CurrentPage, PageSize = sortParameters.PageSize, TotalItems = postModel.Count };
            return postsViewModel;
        }

        /// <inheritdoc cref="IPostsService"/>
        public async Task<PostShowDto> GetPost(int postId, SortParametersDto sortParameters)
        {
            var postModel = new PostShowDto()
            {
                Post = await Where(post => post.Id == postId)
                    .Include(x => x.Author)
                    .Include(x => x.Author.Profile)
                    .Include(x => x.PostTags)
                    .FirstOrDefaultAsync()
            };

            if (sortParameters == null || postModel.Post == null) return postModel;

            var count = postModel.Post.Comments.Count;

            postModel.Comments.Comments = postModel.Post.Comments.Skip((sortParameters.CurrentPage - 1) * sortParameters.PageSize).Take(sortParameters.PageSize).ToList();
            postModel.Comments.PageInfo = new PageInfo { PageNumber = sortParameters.CurrentPage, PageSize = sortParameters.PageSize, TotalItems = count };
            postModel.Post.Comments = null;

            return postModel;
        }

        /// <inheritdoc cref="IPostsService"/>
        public async Task CreatePost(Post postModel)
        {
            if (!string.IsNullOrWhiteSpace(postModel.Tags))
            {
                IList<Tag> tags = new List<Tag>();
                for (var index = 0;
                    index < postModel.Tags.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
                    index++)
                {
                    var tag = postModel.Tags.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)[index];
                    var tagForAdd = new Tag()
                    {
                        Title = tag,
                        PostId = postModel.Id
                    };
                    tags.Add(tagForAdd);
                }

                postModel.PostTags = tags;
            }

            await InsertAsync(postModel);
        }

        /// <inheritdoc cref="IPostsService"/>
        public async Task ChangePostStatus(int id, Status status)
        {
            var post = await FindAsync(id);
            post.Status = status;
            await UpdateAsync(post);
        }

        /// <inheritdoc cref="IPostsService"/>
        public SelectList GetPostsSelectList(int? postId) =>
            postId.HasValue 
                ? Repository.GetTableSelectList("Id", "Title", postId.Value) 
                : Repository.GetTableSelectList("Id", "Title", null);

        /// <summary>
        /// Sort posts by sort parameters.
        /// </summary>
        /// <param name="posts">posts.</param>
        /// <param name="sortParameters">sortParameters.</param>
        /// <returns>IOrderedEnumerable.</returns>
        private static IOrderedQueryable<Post> SortPosts(IQueryable<Post> posts, SortParametersDto sortParameters)
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