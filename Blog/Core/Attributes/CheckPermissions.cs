using System;
using System.Linq;
using System.Web.Mvc;
using Blog.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace Blog.Core.Attributes
{
    class CheckPermissionsToEditForPosts : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(filterContext.RouteData.Values["id"].ToString().IsNullOrWhiteSpace())
                filterContext.Result = new RedirectResult("~/Posts/Index/");

            int id = Int32.Parse(filterContext.RouteData.Values["id"].ToString());
            BlogContext db = new BlogContext();
            var postToEdit = db.Posts.FirstOrDefault(post => post.Id.Equals(id));

            var userId = filterContext.HttpContext.User.Identity.GetUserId();

            if (postToEdit != null && !postToEdit.Author.Equals(userId))
                filterContext.Result = new RedirectResult("~/Posts/Show/" + id);
        }
    }

    class CheckPermissionsToEditForComments : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.RouteData.Values["id"].ToString().IsNullOrWhiteSpace())
                filterContext.Result = new RedirectResult("~/Posts/Index/");

            int id = Int32.Parse(filterContext.RouteData.Values["id"].ToString());
            BlogContext db = new BlogContext();
            var commentToEdit = db.Comments.FirstOrDefault(comment => comment.Id.Equals(id));

            var userId = filterContext.HttpContext.User.Identity.GetUserId();

            if (commentToEdit != null && !commentToEdit.Author.Equals(userId))
                filterContext.Result = new RedirectResult("~/Posts/Show/" + commentToEdit.PostID);
        }
    }
}
