using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Data.Models;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;
using Blog.Services.Interfaces;
using Blog.Services.Posts.Interfaces;
using ClosedXML.Excel;

namespace Blog.Services
{
    /// <summary>
    /// Upload from file service.
    /// </summary>
    /// <seealso cref="IUploadFromFileService" />
    public class UploadFromFileService : IUploadFromFileService
    {
        /// <summary>
        /// The posts service.
        /// </summary>
        private readonly IPostsService _postsService;

        /// <summary>
        /// The comments service.
        /// </summary>
        private readonly ICommentsService _commentsService;

        /// <summary>
        /// The tags service.
        /// </summary>
        private readonly ITagsService _tagsService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadFromFileService"/> class.
        /// </summary>
        /// <param name="postsService">The posts service.</param>
        /// <param name="commentsService"></param>
        /// <param name="tagsService"></param>
        public UploadFromFileService(
            IPostsService postsService, 
            ICommentsService commentsService, 
            ITagsService tagsService)
        {
            _postsService = postsService;
            _commentsService = commentsService;
            _tagsService = tagsService;

            var config = new MapperConfiguration(cfg => { cfg.CreateMap<PostDto, Post>(); });
            _mapper = config.CreateMapper();
        }

        /// <inheritdoc cref="IUploadFromFileService"/>
        public async Task<ResultDto> UploadPostsFromExcel(Stream inputStream, string currentUserId)
        {
            var resultDto = new ResultDto();

            var postsToCreate = new List<Post>();
            var postsToEdit = new List<Post>();
            var postsToDelete = new List<Post>();

            var posts = await _postsService.GetAllAsync();

            var workSheet = WorkSheetUsedRows(inputStream, "Posts");
            if (!workSheet.Success)
            {
                resultDto.ExceptionMessage = workSheet.ExceptionMessage;

                return resultDto;
            }

            foreach (var row in workSheet.Rows)
            {
                var item = new PostDto(row);

                var post = posts.FirstOrDefault(x => x.Title.ToLower().Equals(item.Title));

                if (row.Cell(7).Value.ToString().TrimStart(' ').TrimEnd(' ').ToLower().Equals("edit") && post != null)
                {
                    post.Description = item.Description;
                    post.Content = item.Content;
                    post.ImageUrl = item.ImageUrl;
                    post.Status = item.Status;

                    post.PostTags = item.PostTags;
                    postsToEdit.Add(post);
                }
                else if (row.Cell(7).Value.ToString().TrimStart(' ').TrimEnd(' ').ToLower().Equals("edit") &&
                         post != null)
                {
                    await _commentsService.DeletePostComments(post.Id);
                    await _tagsService.DeletePostTags(post.Id);
                    postsToDelete.Add(post);
                }
                else
                {
                    var newPost = _mapper.Map<PostDto, Post>(item);
                    newPost.AuthorId = currentUserId;
                    postsToCreate.Add(newPost);
                }
            }

            if (postsToCreate.Count > 0)
            {
                await _postsService.InsertAsync(postsToCreate);
            }

            if (postsToEdit.Count > 0)
            {
                await _postsService.UpdateAsync(postsToEdit);
            }

            if (postsToDelete.Count > 0)
            {
                await _postsService.DeleteAsync(postsToDelete);
            }

            resultDto.Success = true;

            return resultDto;
        }

        /// <summary>
        /// Works the sheet used rows.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="workSheetName">Name of the work sheet.</param>
        /// <returns>WorkSheetDto.</returns>
        private static WorkSheetDto WorkSheetUsedRows(Stream inputStream, string workSheetName)
        {
            var workSheetDto = new WorkSheetDto();
            XLWorkbook workbook;
            try
            {
                workbook = new XLWorkbook(inputStream);
            }
            catch (Exception e)
            {
                workSheetDto.ExceptionMessage = $"Check your file. {e.Message}";

                return workSheetDto;
            }

            IXLWorksheet worksheet;
            try
            {
                worksheet = workbook.Worksheet(workSheetName);
            }
            catch (Exception e)
            {
                workSheetDto.ExceptionMessage = $"Sheet not found. {e.Message}";
                
                return workSheetDto;
            }

            worksheet.FirstRow().Delete();

            workSheetDto.Rows = worksheet.RowsUsed();
            workSheetDto.Success = true;

            return workSheetDto;
        }
    }
}