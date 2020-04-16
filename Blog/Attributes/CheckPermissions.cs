using System;
using System.Linq;
using System.Web.Mvc;
using Blog.Data.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace Blog.Attributes
{
    /// <summary>
    /// Check permissions to edit post.
    /// </summary>
    /// <seealso cref="FilterAttribute" />
    /// <seealso cref="IAuthorizationFilter" />
    internal class CheckPermissionsToEditForPosts : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Called when [authorization].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(filterContext.RouteData.Values["id"].ToString().IsNullOrWhiteSpace())
                filterContext.Result = new RedirectResult("~/Posts/Index/");

            var id = int.Parse(filterContext.RouteData.Values["id"].ToString());
            BlogContext db = new BlogContext();
            var postToEdit = db.Posts.FirstOrDefault(post => post.Id.Equals(id));

            var userId = filterContext.HttpContext.User.Identity.GetUserId();

            if (postToEdit != null && !postToEdit.Author.Equals(userId))
                filterContext.Result = new RedirectResult("~/Posts/Show/" + id);
        }
    }

    /// <summary>
    /// Check permissions to edit comments.
    /// </summary>
    /// <seealso cref="FilterAttribute" />
    /// <seealso cref="IAuthorizationFilter" />
    class CheckPermissionsToEditForComments : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.RouteData.Values["id"] == null || filterContext.RouteData.Values["id"].ToString().IsNullOrWhiteSpace())
                filterContext.Result = new RedirectResult("~/Posts/Index/");

            var id = int.Parse(filterContext.RouteData.Values["id"].ToString());
            var db = new BlogContext();
            var commentToEdit = db.Comments.FirstOrDefault(comment => comment.Id.Equals(id));

            var userId = filterContext.HttpContext.User.Identity.GetUserId();

            if (commentToEdit != null && !commentToEdit.Author.Equals(userId))
                filterContext.Result = new RedirectResult("~/Posts/Show/" + commentToEdit.PostID);
        }
    }
}
