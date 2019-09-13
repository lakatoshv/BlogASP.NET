using System.Collections.Generic;
using Blog.Core.Dtos;
using Blog.Models;
using Blog.ViewModels.Posts;

namespace Blog.Services.Posts.Interfaces
{
    interface ICommentsService
    {
        IList<Comment> GetCommentsForPost(int postId);
        CommentsViewModel GetPagedCommentsByPostId(int postId, string author, SortParametersDto sortParameters);
    }
}
