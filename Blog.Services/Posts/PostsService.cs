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
using Blog.Data.Models.Repository.Interfaces;
using Blog.Services.Interfaces;

namespace Blog.Services.Posts
{
    /// <summary>
    /// Posts service.
    /// </summary>
    public class PostsService : GeneralService<Post>, IPostsService
    {
        /// <summary>
        /// The comments service
        /// </summary>
        private readonly ICommentsService _commentsService;

        /// <summary>
        /// The profiles service
        /// </summary>
        private readonly IProfilesService _profilesService;

        /// <summary>
        /// The tags service
        /// </summary>
        private readonly ITagsService _tagsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="commentsService">The comments service.</param>
        /// <param name="profilesService">The profiles service.</param>
        /// <param name="tagsService">The tags service.</param>
        public PostsService(
            IRepository<Post> repository,
            ICommentsService commentsService,
            IProfilesService profilesService,
            ITagsService tagsService) 
            : base(repository)
        {
            _commentsService = commentsService;
            _profilesService = profilesService;
            _tagsService = tagsService;
        }

        /// <inheritdoc/>
        public async Task<PostsDto> GetPosts(string search, bool onlyWithComments = false)
        {
            IList<PostDto> postModel = new List<PostDto>();

            IQueryable<Post> postsQueryable = GetAll();
            if (!string.IsNullOrWhiteSpace(search))
            {
                postsQueryable = postsQueryable.Where(post => post.Title.Equals(search));
            }

            var posts = await postsQueryable.ToListAsync();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(item => {
                var post = new PostDto()
                {
                    Profile = _profilesService.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(item.Author)).Result,
                    CommentsCount = _commentsService.Count(comment => comment.PostID.Equals(item.Id))
                };
                var user =  userManager.FindByIdAsync(item.Author).Result;
                if (user != null)
                {
                    item.Author = user.UserName;
                }


                post.Post = item;

                var tags = _tagsService.Where(tag => tag.PostId.Equals(item.Id)).ToListAsync().Result;
                foreach (var tag in tags)
                {
                    post.Post.PostTags.Add(tag);
                }

                post.CommentsCount = _commentsService.Count(comment => comment.PostID.Equals(item.Id));
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
            var postModel = new PostShowDto()
            {
                Post = await FirstOrDefaultAsync(post => post.Id.Equals(postId))
            };
            postModel.Profile = await _profilesService.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(postModel.Post.Author));
            if (postModel.Post == null)
            {
                return null;
            }

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var author = postModel.Post.Author;
            var user = await userManager.FindByIdAsync(author);
            if (user != null)
            {
                postModel.Post.Author = user.UserName;
            }

            var tags = await _tagsService.Where(tag => tag.PostId.Equals(postId)).ToListAsync();
            foreach (var tag in tags)
            {
                postModel.Post.PostTags.Add(tag);
            }
            
            return postModel;
        }

        /// <inheritdoc/>
        public async Task<PostShowDto> GetPostModel(int postId)
        {
            var postModel = new PostShowDto()
            {
                Post = await FirstOrDefaultAsync(post => post.Id.Equals(postId))
            };
            postModel.Profile = await _profilesService.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(postModel.Post.Author));
            if (postModel.Post == null)
            {
                return null;
            }

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var author = postModel.Post.Author;
            var user = await userManager.FindByIdAsync(author);
            if (user != null)
            {
                postModel.Post.Author = user.UserName;
            }

            var tags = _tagsService.Where(tag => tag.PostId.Equals(postId)).ToList();
            foreach (var tag in tags)
            {
                postModel.Post.PostTags.Add(tag);
            }
            
            return postModel;
        }

        /// <inheritdoc/>
        public async Task<PostShowDto> GetPostWithComments(int postId, SortParametersDto sortParameters)
        {
            var postModel = new PostShowDto()
            {
                Post = await FirstOrDefaultAsync(post => post.Id.Equals(postId))
            };
            postModel.Profile = await _profilesService.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(postModel.Post.Author));
            if (postModel.Post == null)
            {
                return null;
            }

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var author = postModel.Post.Author;
            var user = await userManager.FindByIdAsync(author);
            if (user != null)
            {
                postModel.Post.Author = user.UserName;
            }

            var tags = await _tagsService.Where(tag => tag.PostId.Equals(postId)).ToListAsync();
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

            var postsEnumerable = Where(post => post.Author.Equals(userId));
            if (!string.IsNullOrWhiteSpace(search))
            {
                postsEnumerable = postsEnumerable.Where(post => post.Title.Equals(search));
            }

            var posts = await SortPosts(postsEnumerable, sortParameters).ToListAsync();

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(async item => {
                var post = new PostDto()
                {
                    Profile = await _profilesService.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(item.Author))
                };

                var user = await userManager.FindByIdAsync(item.Author);
                if (user != null)
                {
                    item.Author = user.UserName;
                }


                post.Post = item;
                post.Post.PostTags = new List<Tag>();

                var tags = await _tagsService.Where(tag => tag.PostId.Equals(item.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    post.Post.PostTags.Add(tag);
                }

                post.CommentsCount = _commentsService.Count(comment => comment.PostID.Equals(item.Id));
                postModel.Add(post);
            });
            var postsViewModel = new PostsDto();
            if (!sortParameters.DisplayType.Equals("grid"))
            {
                postsViewModel.Posts = postModel.Skip((sortParameters.CurrentPage - 1) * sortParameters.PageSize)
                    .Take(sortParameters.PageSize).ToList();
            }
            else
            {
                postsViewModel.Posts = postModel;
            }

            postsViewModel.PageInfo = new PageInfo { PageNumber = sortParameters.CurrentPage, PageSize = sortParameters.PageSize, TotalItems = postModel.Count };
            return postsViewModel;
        }

        /// <inheritdoc/>
        public async Task<PostsDto> GetUserPosts(string userId, string search)
        {
            IList<PostDto> postModel = new List<PostDto>();

            var postsQueryable = Where(post => post.Author.Equals(userId));
            if (!string.IsNullOrWhiteSpace(search))
                postsQueryable = postsQueryable.Where(post => post.Title.Equals(search));

            var posts = await postsQueryable.ToListAsync();

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(async item => {
                var post = new PostDto()
                {
                    Profile = await _profilesService.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(item.Author))
                };

                var user = await userManager.FindByIdAsync(item.Author);
                if (user != null)
                {
                    item.Author = user.UserName;
                }


                post.Post = item;
                post.Post.PostTags = new List<Tag>();

                var tags = await _tagsService.Where(tag => tag.PostId.Equals(item.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    post.Post.PostTags.Add(tag);
                }

                post.CommentsCount = _commentsService.Count(comment => comment.PostID.Equals(item.Id));
                postModel.Add(post);
            });

            return new PostsDto() { Posts = postModel };
        }

        /// <inheritdoc/>
        public async Task<PostsDto> GetPosts(SortParametersDto sortParameters, string search)
        {
            IList<PostDto> postModel = new List<PostDto>();

            var postsEnumerable = Where(post => post.Status == Status.Approved);
            if (!string.IsNullOrWhiteSpace(search))
            {
                postsEnumerable = postsEnumerable.Where(post => post.Title.Equals(search));
            }

            var posts = await SortPosts(postsEnumerable, sortParameters).ToListAsync();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(item =>
            {
                var post = new PostDto()
                {
                    Profile = _profilesService.FirstOrDefault(pr => pr.ApplicationUser.Equals(item.Author))
                };

                var user = userManager.FindById(item.Author);
                if (user != null)
                {
                    item.Author = user.UserName;
                }

                post.Post = item;

                var tags = _tagsService.Where(tag => tag.PostId.Equals(item.Id)).ToList();
                foreach (var tag in tags)
                {
                    post.Post.PostTags.Add(tag);
                }

                post.CommentsCount = _commentsService.Count(comment => comment.PostID.Equals(item.Id));
                postModel.Add(post);
            });
            var postsViewModel = new PostsDto()
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
            
            Insert(postModel);

            if (!string.IsNullOrWhiteSpace(postModel.Tags))
            {
                IList<Tag> tags = new List<Tag>();
                for (var index = 0;
                    index < postModel.Tags.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries).Length;
                    index++)
                {
                    var tag = postModel.Tags.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries)[index];
                    var tagForAdd = new Tag()
                    {
                        Title = tag,
                        PostId = postModel.Id
                    };
                    tags.Add(tagForAdd);
                }

                await _tagsService.InsertAsync(tags);
            }
            await _tagsService.Where(tag => tag.PostId.Equals(postModel.Id)).ForEachAsync(
                t =>
                {
                    postModel.PostTags.Add(t);
                }

            );
            await UpdateAsync(postModel);
        }

        /// <inheritdoc/>
        public async Task ChangePostStatus(int id, Status status)
        {
            var post = await FindAsync(id);
            post.Status = status;
            await UpdateAsync(post);
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