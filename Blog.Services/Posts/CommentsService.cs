using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.HelperClasses;
using Blog.Data.Models;
using Blog.Data.Models.Repository.Interfaces;
using Blog.Services.Dtos;
using Blog.Services.Dtos.Posts;
using Blog.Services.Interfaces;
using Blog.Services.Posts.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Services.Posts
{
    /// <summary>
    /// Comments service.
    /// </summary>
    /// <seealso cref="GeneralService{Comment}" />
    /// <seealso cref="ICommentsService" />
    public class CommentsService : GeneralService<Comment>, ICommentsService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository<Comment> _repository;

        /// <summary>
        /// The profiles service
        /// </summary>
        private readonly IProfilesService _profilesService;

        /// <summary>
        /// The posts service
        /// </summary>
        private readonly IPostsService _postsService;

        /// <summary>
        /// The tags service
        /// </summary>
        private readonly ITagsService _tagsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="profilesService">The profiles service.</param>
        /// <param name="postsService">The posts service.</param>
        public CommentsService(
            IRepository<Comment> repository,
            IProfilesService profilesService,
            IPostsService postsService,
            ITagsService tagsService)
            : base(repository)
        {
            _repository = repository;
            _profilesService = profilesService;
            _postsService = postsService;
            _tagsService = tagsService;
        }

        /// <inheritdoc/>
        public async Task<CommentsDto> GetAllComments()
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);

            var commentsDto = new CommentsDto()
            {
                Comments = new List<CommentDto>()
            };

            var comments = await _repository.GetAllAsync();
            comments.ToList().ForEach(async comment => {
                var commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                var comm = new CommentDto()
                {
                    Profile = await _profilesService.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(comment.Author))
                };

                if (commentAuthor != null)
                {
                    comment.Author = commentAuthor.UserName;
                }

                comm.Comment = comment;

                commentsDto.Comments.Add(comm);
            });
            return commentsDto;
        }

        /// <inheritdoc/>
        public async Task<IList<Comment>> GetCommentsForPost(int postId)
        {
            return await Repository.Table.Where(comment => comment.PostID.Equals(postId)).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<PostShowDto> GetCommentsWithPost(int postId)
        {
            var postModel = new PostShowDto()
            {
                Post = await _postsService.FirstOrDefaultAsync(post => post.Id.Equals(postId))
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

            postModel.Comments = await GetCommentsByPostId(postId, author);


            return postModel;
        }

        /// <inheritdoc/>
        public async Task<CommentsDto> GetCommentsByPostId(int postId, string author)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);

            var commentsViewModel = new CommentsDto()
            {
                Comments = new List<CommentDto>()
            };
            var comments = Where(comment => comment.PostID.Equals(postId));
            await comments.ForEachAsync(comment => {
                var commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                var comm = new CommentDto()
                {
                    Profile = _profilesService.FirstOrDefault(pr => pr.ApplicationUser.Equals(comment.Author))
                };

                if (commentAuthor != null)
                    comment.Author = commentAuthor.UserName;

                comm.Comment = comment;

                commentsViewModel.Comments.Add(comm);
            });
            return commentsViewModel;
        }

        /// <inheritdoc/>
        public async Task<CommentsDto> GetPagedCommentsByPostId(int postId, string author, SortParametersDto sortParameters)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);

            var commentsDto = new CommentsDto()
            {
                Comments = new List<CommentDto>()
            };
            var comments = await Where(comment => comment.PostID.Equals(postId)).ToListAsync();
            comments.ForEach(comment => {
                var commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                var comm = new CommentDto
                {
                    Profile = _profilesService.FirstOrDefault(pr => pr.ApplicationUser.Equals(comment.Author))
                };

                if (commentAuthor != null)
                {
                    comment.Author = commentAuthor.UserName;
                }

                comm.Comment = comment;

                commentsDto.Comments.Add(comm);
            });
            var count = commentsDto.Comments.Count;
            commentsDto.Comments = commentsDto.Comments.Skip((sortParameters.CurrentPage - 1) * sortParameters.PageSize).Take(sortParameters.PageSize).ToList();
            commentsDto.PageInfo = new PageInfo { PageNumber = sortParameters.CurrentPage, PageSize = sortParameters.PageSize, TotalItems = count };
            return commentsDto;
        }

        /// <inheritdoc/>
        public async Task<CommentWithPostsDto> GetCommentModelWithPosts(string search)
        {
            var postModel = new CommentWithPostsDto()
            {
                Posts = await _postsService.ToListAsync()
            };

            return postModel;
        }

        /// <inheritdoc/>
        public async Task<CommentWithPostDto> GetPostWithCommentModel(string search, int commentId)
        {
            var comment = await FirstOrDefaultAsync(comm => comm.Id.Equals(commentId));
            if (comment == null)
            {
                return null;
            }

            var postModel = new CommentWithPostDto()
            {
                Post = new PostDto() { Post = await _postsService.FirstOrDefaultAsync(post => post.Id.Equals(comment.PostID)) },
                Comment = new CommentDto() { Comment = comment }
            };

            if (postModel.Post != null)
            {
                postModel.Post.Profile =
                    await _profilesService.FirstOrDefaultAsync(profile => profile.ApplicationUser.Equals(postModel.Comment.Comment.Author));
            }

            if (postModel.Comment != null)
            {
                postModel.Comment.Profile = await _profilesService.FirstOrDefaultAsync(profile =>
                    profile.ApplicationUser.Equals(postModel.Comment.Comment.Author));
            }

            return postModel;
        }

        /// <inheritdoc/>
        public async Task<CommentWithPostsDto> GetPostsWithCommentModel(string search)
        {
            var postModel = new CommentWithPostsDto()
            {
                Posts = await _postsService.ToListAsync()
            };

            return postModel;
        }

        /// <inheritdoc/>
        public async Task DeletePostComments(int id)
        {
            await DeleteAsync(await GetCommentsForPost(id));
        }
    }
}