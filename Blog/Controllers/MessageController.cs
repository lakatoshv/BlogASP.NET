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

        // GET: Message/Details/5        
        /// <summary>
        /// Detailses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Message/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Message/Create        
        /// <summary>
        /// Creates the specified collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>ActionResult.</returns>
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
