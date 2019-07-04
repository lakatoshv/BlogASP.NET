using System.Web.Mvc;
using Blog.Areas.Admin.Services.Posts;
using Blog.Models;

namespace Blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PostsController : Controller
    {
        private readonly PostsService _postsService = new PostsService();
        private readonly BlogContext _db = new BlogContext();
        // GET: Admin/Posts
        [HttpGet]
        public ActionResult Index(string search)
        {
            var posts = _postsService.GetPosts(search);
            return View(posts);
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