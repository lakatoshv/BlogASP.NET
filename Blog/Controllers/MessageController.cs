using System.Data;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Data.Models;
using Microsoft.AspNet.Identity;
using Blog.Services.Interfaces;

namespace Blog.Controllers
{
    /// <summary>
    /// Message controller.
    /// </summary>
    public class MessageController : Controller
    {
        /// <summary>
        /// Messages service.
        /// </summary>
        private readonly IMessagesService _messagesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageController"/> class.
        /// </summary>
        /// <param name="messagesService">The posts service.</param>
        public MessageController(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        // GET: Message
        /// <summary>
        /// Indexes the specified search.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View(await _messagesService.GetAllAsync());
        }

        // GET: Message/Details/5
        /// <summary>
        /// Shows the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }
            return View(await _messagesService.FindAsync(id.Value));
        }

        // GET: Message/Create
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Message/Create
        /// <summary>
        /// Creates the specified post model.
        /// </summary>
        /// <param name="message">The post model.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Message message)
        {
            //post.CreatedAt = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return View();
            }

            //postModel.CreatedAt = DateTime.Now;
            message.SenderId = User.Identity.GetUserId();

            await _messagesService.InsertAsync(message);
            return RedirectToAction("index");
        }

        /// <summary>
        /// Send message.
        /// </summary>
        /// <param name="messageModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendMessage(Message messageModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                messageModel.SenderId = User.Identity.GetUserId();
            }
            else if(string.IsNullOrWhiteSpace(messageModel.SenderEmail) && string.IsNullOrWhiteSpace(messageModel.SenderName))
                return RedirectToAction("Contact", "Home");

            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Contact", "Home");
                }

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

            var postModel = await _messagesService.FindAsync(id.Value);

            return View(postModel);
        }

        // POST: Message/Edit/5
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="message">The edited post.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, Message message)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            try
            {
                var messageModel = await _messagesService.FindAsync(id.Value);

                message.SenderId = messageModel.SenderId;
                await _messagesService.UpdateAsync(message);

                return RedirectToAction("Show/" + id, "Posts");
            }
            catch (DataException /* dex */)
            {
                return RedirectToAction("Index", "Posts");
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
        }

        // GET: Message/Delete/5
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            return View();
        }

        // POST: Message/Delete/5
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _messagesService.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
