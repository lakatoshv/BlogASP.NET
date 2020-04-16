﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Data.Models;
using Blog.Services.Dtos;
using Blog.Services.Dtos.Posts;

namespace Blog.Services.Posts.Interfaces
{
    /// <summary>
    /// Comments service interface.
    /// </summary>
    interface ICommentsService
    {
        /// <summary>
        /// Async get list of all posts.
        /// </summary>
        /// <returns>CommentsDto.</returns>
        Task<CommentsDto> GetAllComments();

        /// <summary>
        /// Async get all post comments by postId.
        /// </summary>
        /// <param name="postId">postId.</param>
        /// <returns>IList.</returns>
        Task<IList<Comment>> GetCommentsForPost(int postId);

        /// <summary>
        /// Async get comments with post.
        /// </summary>
        /// <param name="postId">postId.</param>
        /// <returns>PostShowDto.</returns>
        Task<PostShowDto> GetCommentsWithPost(int postId);

        /// <summary>
        /// Async get comments by post id.
        /// </summary>
        /// <param name="postId">postId.</param>
        /// <param name="author">author.</param>
        /// <returns>CommentsDto.</returns>
        Task<CommentsDto> GetCommentsByPostId(int postId, string author);

        /// <summary>
        /// Async get paged post comments by postId.
        /// </summary>
        /// <param name="postId">postId.</param>
        /// <param name="author">author.</param>
        /// <param name="sortParameters">sortParameters.</param>
        /// <returns>CommentsDto.</returns>
        Task<CommentsDto> GetPagedCommentsByPostId(int postId, string author, SortParametersDto sortParameters);

        /// <summary>
        /// Async get comment by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Comment> GetComment(int id);

        /// <summary>
        /// Async get comment model with posts list.
        /// </summary>
        /// <param name="search">search.</param>
        /// <returns>CommentWithPostsDto.</returns>
        Task<CommentWithPostsDto> GetCommentModelWithPosts(string search);

        /// <summary>
        /// Async get post with comment model.
        /// </summary>
        /// <param name="search">search.</param>
        /// <param name="commentId"></param>
        /// <returns>CommentWithPostDto.</returns>
        Task<CommentWithPostDto> GetPostWithCommentModel(string search, int commentId);

        /// <summary>
        /// Async create comment.
        /// </summary>
        /// <param name="comment">comment.</param>
        Task CreateComment(Comment comment);

        /// <summary>
        /// Async get posts with comment model.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<CommentWithPostsDto> GetPostsWithCommentModel(string search);

        /// <summary>
        /// Updates the specified comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <returns>Task.</returns>
        Task Update(Comment comment);

        /// <summary>
        /// Async delete comment by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteComment(int id);

        /// <summary>
        /// Async delete post comments.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeletePostComments(int id);
    }
}