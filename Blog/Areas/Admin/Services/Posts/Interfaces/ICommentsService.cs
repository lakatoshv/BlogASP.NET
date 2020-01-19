using System.Collections.Generic;
using Blog.Core.Dtos;
using Blog.Areas.Admin.ViewModels.Posts;
using Blog.Models;

namespace Blog.Areas.Admin.Services.Posts.Interfaces
{
    interface ICommentsService
    {
        CommentWithPostsViewModel GetPostsWithCommentModel(string search);
        CommentsViewModel GetAllComments();
        IList<Comment> GetCommentsForPost(int postId);
        PostShowViewModel GetCommentsWithPost(int postId);
        CommentsViewModel GetPagedCommentsByPostId(int postId, string author, SortParametersDto sortParameters);
    }
}
