﻿using Blog.Core.Dtos;
using Blog.ViewModels.Posts;

namespace Blog.Services.Posts.Interfaces
{
    interface ICommentsService
    {
        CommentsViewModel GetPagedCommentsByPostId(int postId, string author, SortParametersDto sortParameters);
    }
}
