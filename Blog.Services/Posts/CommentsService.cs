using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.HelperClasses;
using Blog.Data.Models;
using Blog.Services.Dtos;
using Blog.Services.Dtos.Posts;
using Blog.Services.Posts.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Services.Posts
{
    /// <summary>
    /// Comments service.
    /// </summary>
    public class CommentsService : ICommentsService
    {
        /// <summary>
        /// Blog context.
        /// </summary>
        private readonly BlogContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsService"/> class.
        /// </summary>
        public CommentsService()
        {
            _dbContext = new BlogContext();
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

            var comments = await _dbContext.Comments.ToListAsync();
            comments.ForEach(async comment => {
                var commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                var comm = new CommentDto()
                {
                    Profile = await _dbContext.Profiles.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(comment.Author))
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
            return await _dbContext.Comments.Where(comment => comment.PostID.Equals(postId)).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<PostShowDto> GetCommentsWithPost(int postId)
        {
            var postModel = new PostShowDto()
            {
                Post = await _dbContext.Posts.FirstOrDefaultAsync(post => post.Id.Equals(postId))
            };
            postModel.Profile = await _dbContext.Profiles.FirstOrDefaultAsync(pr => pr.ApplicationUser.Equals(postModel.Post.Author));
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

            var tags = await _dbContext.Tags.Where(tag => tag.PostId.Equals(postId)).ToListAsync();
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
            var comments = _dbContext.Comments.Where(comment => comment.PostID.Equals(postId)).AsQueryable();
            await comments.ForEachAsync(comment => {
                var commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                var comm = new CommentDto()
                {
                    Profile = _dbContext.Profiles.FirstOrDefault(pr => pr.ApplicationUser.Equals(comment.Author))
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
            var comments = await _dbContext.Comments.Where(comment => comment.PostID.Equals(postId)).ToListAsync();
            comments.ForEach(comment => {
                var commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                var comm = new CommentDto
                {
                    Profile = _dbContext.Profiles.FirstOrDefault(pr => pr.ApplicationUser.Equals(comment.Author))
                };

                if (commentAuthor != null)
                    comment.Author = commentAuthor.UserName;

                comm.Comment = comment;

                commentsDto.Comments.Add(comm);
            });
            var count = commentsDto.Comments.Count;
            commentsDto.Comments = commentsDto.Comments.Skip((sortParameters.CurrentPage - 1) * sortParameters.PageSize).Take(sortParameters.PageSize).ToList();
            commentsDto.PageInfo = new PageInfo { PageNumber = sortParameters.CurrentPage, PageSize = sortParameters.PageSize, TotalItems = count };
            return commentsDto;
        }

        /// <inheritdoc/>
        public async Task<Comment> GetComment(int id)
        {
            return await _dbContext.Comments.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task<CommentWithPostsDto> GetCommentModelWithPosts(string search)
        {
            var postModel = new CommentWithPostsDto()
            {
                Posts = await _dbContext.Posts.ToListAsync()
            };

            return postModel;
        }

        /// <inheritdoc/>
        public async Task<CommentWithPostDto> GetPostWithCommentModel(string search, int commentId)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(comm => comm.Id.Equals(commentId));
            if (comment == null)
            {
                return null;
            }

            var postModel = new CommentWithPostDto()
            {
                Post = new PostDto() { Post = await _dbContext.Posts.FirstOrDefaultAsync(post => post.Id.Equals(comment.PostID)) },
                Comment = new CommentDto() { Comment = comment }
            };

            if (postModel.Post != null)
            {
                postModel.Post.Profile =
                    await _dbContext.Profiles.FirstOrDefaultAsync(profile => profile.ApplicationUser.Equals(postModel.Comment.Comment.Author));
            }

            if (postModel.Comment != null)
            {
                postModel.Comment.Profile = await _dbContext.Profiles.FirstOrDefaultAsync(profile =>
                    profile.ApplicationUser.Equals(postModel.Comment.Comment.Author));
            }

            return postModel;
        }

        /// <inheritdoc/>
        public async Task<CommentWithPostsDto> GetPostsWithCommentModel(string search)
        {
            var postModel = new CommentWithPostsDto()
            {
                Posts = await _dbContext.Posts.ToListAsync()
            };

            return postModel;
        }

        /// <inheritdoc/>
        public async Task Update(Comment comment)
        {
            _dbContext.Entry(comment).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task CreateComment(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteComment(int id)
        {
            var commentForDelete = await GetComment(id);

            if (commentForDelete != null)
            {
                _dbContext.Comments.Remove(commentForDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public async Task DeletePostComments(int id)
        {
            _dbContext.Comments.RemoveRange(await GetCommentsForPost(id));
            await _dbContext.SaveChangesAsync();
        }
    }
}