﻿using System.Web.Mvc;
using Blog.Models;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Posts");
        }
        
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            Message message = new Message();
            return View(message);
        }
    }
}