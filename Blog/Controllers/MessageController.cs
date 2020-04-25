using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Data.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Blog.Services.Interfaces;

namespace Blog.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessagesService _messagesService;

        public MessageController(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        // GET: Message        
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View();
        }

        // GET: Message/SendedRequests
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult SendedRequests()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "Posts");
            }

            var userId = User.Identity.GetUserId();
            var messages = _messagesService.GetAll(x => x.ApplicationUser == userId);
            return View(messages);
        }

        // GET: Message/Details/5        
        /// <summary>
        /// Details the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public async Task<ActionResult> Details(int? id)
        {
            if (!User.Identity.IsAuthenticated || id == null)
            {
                RedirectToAction("Index", "Posts");
            }

            return View(await _messagesService.FindAsync(id.Value));
        }

        // GET: Message/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "Posts");
            }

            return View();
        }

        // POST: Message/Create        
        /// <summary>
        /// Creates the specified collection.
        /// </summary>
        /// <param name="messageModel"></param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Message messageModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                messageModel.ApplicationUser = User.Identity.GetUserId();
            }
            else if (messageModel.Email.IsNullOrWhiteSpace() && messageModel.Name.IsNullOrWhiteSpace())
            {
                return RedirectToAction("Contact", "Home");
            }

            if (!ModelState.IsValid)
            {
                return null;
            }

            try
            {

                await _messagesService.InsertAsync(messageModel);
                return RedirectToAction("SendedRequests", "Message");
            }
            catch
            {
                return RedirectToAction("Contact", "Home");
            }
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="messageModel">The message model.</param>
        /// <returns>ActionResult.</returns>
        public async Task<ActionResult> SendMessage(Message messageModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                messageModel.ApplicationUser = User.Identity.GetUserId();
            }
            else if(messageModel.Email.IsNullOrWhiteSpace() && messageModel.Name.IsNullOrWhiteSpace())
            {
                return RedirectToAction("Contact", "Home");
            }

            if (!ModelState.IsValid)
            {
                return null;
            }

            try
            {

                await _messagesService.InsertAsync(messageModel);
                return RedirectToAction("Contact", "Home");
            }
            catch
            {
                return RedirectToAction("Contact", "Home");
            }
        }

        // GET: Message/Edit/5        
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Message/Edit/5        
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>ActionResult.</returns>
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
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Message/Delete/5        
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>ActionResult.</returns>
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
