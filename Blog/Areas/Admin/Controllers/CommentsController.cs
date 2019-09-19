using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Blog.Areas.Admin.Services.Posts;
using Blog.Models;

namespace Blog.Areas.Admin.Controllers
{
    public class CommentsController : Controller
    {
        private readonly CommentsService _commentsService = new CommentsService();

        // GET: Admin/Comments
        public ActionResult Index()
        {
            return View(_commentsService.GetAllComments());
        }
    }
}
