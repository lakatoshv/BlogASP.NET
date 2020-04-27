using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Data.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Blog.Services.Interfaces;

namespace Blog.Controllers
{
    /// <summary>
    /// Message controller.
    /// </summary>
    /// <seealso cref="Controller" />
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
        [Authorize]
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
        [Authorize]
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
        [HttpGet]
        [Authorize]
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
        [Authorize]
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
        [HttpPost]
        [Authorize]
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
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            return View(await _messagesService.FindAsync(id.Value));
        }

        // POST: Message/Edit/5        
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Edit(Message editedPost)
        {
            if (!User.Identity.IsAuthenticated || editedPost.ApplicationUser != User.Identity.GetUserId())
            {
                return RedirectToAction("Index", "Posts");
            }

            try
            {
                await _messagesService.UpdateAsync(editedPost);
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
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            var message = await _messagesService.FindAsync(id.Value);

            if (!User.Identity.IsAuthenticated || message.ApplicationUser != User.Identity.GetUserId())
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }

        // POST: Message/Delete/5        
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var message = await _messagesService.FindAsync(id);

            if (!User.Identity.IsAuthenticated || message.ApplicationUser != User.Identity.GetUserId())
            {
                return RedirectToAction("Index", "Posts");
            }

            try
            {
                await _messagesService.DeleteAsync(message);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
