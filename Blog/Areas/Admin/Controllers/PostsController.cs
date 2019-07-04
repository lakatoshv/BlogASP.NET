using System.Web.Mvc;
using Blog.Models;

namespace Blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PostsController : Controller
    {
        // GET: Admin/Posts
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Posts/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Posts/Create
        [HttpPost]
        public ActionResult Create(Post postModel)
        {
            return View();
        }

        // GET: Admin/Posts/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            return View();
        }

        // POST: Admin/Posts/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, Post editedPost)
        {
            return View();
        }

        // GET: Admin/Posts/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost]
        public ActionResult Delete()
        {
            return View();
        }
    }
}