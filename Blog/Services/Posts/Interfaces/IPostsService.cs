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
        IList<PostViewModel> GetPosts(SortParametersDto sortParameters);
        PostShowViewModel GetPost(int postId);
        IList<PostViewModel> GetCurrentUserPosts(string currentUserId, SortParametersDto sortParameters);
    }
}
