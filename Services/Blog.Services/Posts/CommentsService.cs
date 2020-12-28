using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.HelperClasses;
using Blog.Data.Models;
using BLog.Data.Repository.Interfaces;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;
using Blog.Services.Posts.Interfaces;
using Blog.Services.GeneralService;

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
        /// Initializes a new instance of the <see cref="CommentsService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public CommentsService(
            IRepository<Comment> repository)
            : base(repository)
        {
        }

        /// <inheritdoc cref="ICommentsService"/>
        public async Task<CommentsDto> GetAllComments() =>
            new CommentsDto()
            {
                Comments = await Table
                    .Include(x => x.Author)
                    .Include(x => x.Author.Profile)
                    .ToListAsync()
            };

        /// <inheritdoc cref="ICommentsService"/>
        public async Task<CommentsDto> GetCommentsForPost(int? postId, string authorId,
            SortParametersDto sortParameters)
        {
            var comments = Table;
            if (postId != null)
            {
                comments = comments.Where(comment => comment.PostId == postId);
            }

            if (!string.IsNullOrWhiteSpace(authorId))
            {
                comments = comments.Where(comment => comment.AuthorId.Equals(authorId));
            }

            var commentsDto = new CommentsDto()
            {
                Comments = await comments.Include(x => x.Author).ToListAsync(),
            };

            var count = commentsDto.Comments.Count;

            if (sortParameters == null) return commentsDto;

            commentsDto.Comments = commentsDto.Comments.Skip((sortParameters.CurrentPage - 1) * sortParameters.PageSize).Take(sortParameters.PageSize).ToList();
            commentsDto.PageInfo = new PageInfo { PageNumber = sortParameters.CurrentPage, PageSize = sortParameters.PageSize, TotalItems = count };
            
            return commentsDto;
        }

        /// <inheritdoc cref="ICommentsService"/>
        public async Task<Comment> GetComment(int id) =>
            await Where(x => x.Id == id)
                .Include(x => x.Post)
                .Include(x => x.Author)
                .Include(x => x.Author.Profile)
                .FirstOrDefaultAsync();

        /// <inheritdoc cref="ICommentsService"/>
        public async Task DeletePostComments(int id) =>
            await DeleteAsync(await Where(x => x.PostId == id).ToListAsync());
    }
}