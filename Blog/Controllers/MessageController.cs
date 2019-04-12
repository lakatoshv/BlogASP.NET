using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace Blog.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Index()
        {
            return View();
        }

        // GET: Message/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Message/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Message/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult SendMessage(Message messageModel)
        {
            BlogContext db = new BlogContext();
            if (User.Identity.IsAuthenticated)
            {
                messageModel.ApplicationUser = User.Identity.GetUserId();
            }
            else if(messageModel.Email.IsNullOrWhiteSpace() && messageModel.Name.IsNullOrWhiteSpace())
                return RedirectToAction("Contact", "Home");

            try
            {
                if (ModelState.IsValid)
                {
                    var result = db.Messages.Add(messageModel);
                    db.SaveChanges();
                    return RedirectToAction("Contact", "Home");
                }
                return null;
            }
            catch
            {
                return RedirectToAction("Contact", "Home");
            }
        }

        // GET: Message/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Message/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Message/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Message/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
