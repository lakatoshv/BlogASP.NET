using System.Linq;
using System.Web.Mvc;
using BLog.Data;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace Blog.Core.Attributes
{
    /// <summary>
    /// Check permissions to edit for posts.
    /// </summary>
    /// <seealso cref="FilterAttribute" />
    /// <seealso cref="IAuthorizationFilter" />
    internal class CheckPermissionsToEditForPosts : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(filterContext.RouteData.Values["id"].ToString().IsNullOrWhiteSpace())
                filterContext.Result = new RedirectResult("~/Posts/Index/");

            var id = int.Parse(filterContext.RouteData.Values["id"].ToString());
            var db = new BlogContext();
            var postToEdit = db.Posts.FirstOrDefault(post => post.Id.Equals(id));

            if (filterContext.HttpContext.User == null)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
            }
            else
            {
                var userId = filterContext.HttpContext.User?.Identity.GetUserId();
                if (postToEdit != null && !postToEdit.AuthorId.Equals(userId))
                    filterContext.Result = new RedirectResult("~/Posts/Show/" + id);
            }
        }
    }

    internal class CheckPermissionsToEditForComments : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.RouteData.Values["id"].ToString().IsNullOrWhiteSpace())
                filterContext.Result = new RedirectResult("~/Posts/Index/");

            var id = int.Parse(filterContext.RouteData.Values["id"].ToString());
            var db = new BlogContext();
            var commentToEdit = db.Comments.FirstOrDefault(comment => comment.Id.Equals(id));

            if (filterContext.HttpContext.User == null)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
            }
            else
            {
                var userId = filterContext.HttpContext.User?.Identity.GetUserId();
                if (commentToEdit != null && !commentToEdit.AuthorId.Equals(userId))
                    filterContext.Result = new RedirectResult("~/Posts/Show/" + commentToEdit.PostId);
            }
        }
    }
}
