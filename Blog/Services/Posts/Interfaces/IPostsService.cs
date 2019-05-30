using Blog.Models;
using Blog.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Dtos;

namespace Blog.Services.Posts.Interfaces
{
    public interface IPostsService
    {
        PostsViewModel GetPosts(SortParametersDto sortParameters, string search);
        PostShowViewModel GetPost(int postId);
        PostsViewModel GetCurrentUserPosts(string currentUserId, SortParametersDto sortParameters, string search);
    }
}
