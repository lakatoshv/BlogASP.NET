using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Blog.Core.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Blog.Models;
using Blog.Services.Identity;

namespace Blog.Controllers
{
    /// <summary>
    /// Manage controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize]
    public class ManageController : Controller
    {
        /// <summary>
        /// The sign in manager.
        /// </summary>
        private ApplicationSignInManager _signInManager;

        /// <summary>
        /// The user manager.
        /// </summary>
        private ApplicationUserManager _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageController"/> class.
        /// </summary>
        public ManageController()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        /// <summary>
        /// Gets the sign in manager.
        /// </summary>
        /// <value>
        /// The sign in manager.
        /// </value>
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        /// <summary>
        /// Gets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: /Manage/Index        
        /// <summary>
        /// Indexes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        [HttpGet]
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль изменен."
                : message == ManageMessageId.SetPasswordSuccess ? "Пароль задан."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Настроен поставщик двухфакторной проверки подлинности."
                : message == ManageMessageId.Error ? "Произошла ошибка."
                : message == ManageMessageId.AddPhoneSuccess ? "Ваш номер телефона добавлен."
                : message == ManageMessageId.RemovePhoneSuccess ? "Ваш номер телефона удален."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };

            return View(model);
        }

        // POST: /Manage/RemoveLogin        
        /// <summary>
        /// Removes the login.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }

            return RedirectToAction("ManageLogins", new { Message = message });
        }

        // GET: /Manage/AddPhoneNumber        
        /// <summary>
        /// Adds the phone number.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult AddPhoneNumber() => View();

        // POST: /Manage/AddPhoneNumber        
        /// <summary>
        /// Adds the phone number.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Create and send a token.
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService == null)
            {
                return RedirectToAction("VerifyPhoneNumber", new {PhoneNumber = model.Number});
            }

            var message = new IdentityMessage
            {
                Destination = model.Number,
                Body = "Ваш код безопасности: " + code
            };
            await UserManager.SmsService.SendAsync(message);
            
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        // POST: /Manage/EnableTwoFactorAuthentication        
        /// <summary>
        /// Enables the two factor authentication.
        /// </summary>
        /// <returns>Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("Index", "Manage");
        }

        // POST: /Manage/DisableTwoFactorAuthentication        
        /// <summary>
        /// Disables the two factor authentication.
        /// </summary>
        /// <returns>Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("Index", "Manage");
        }

        // GET: /Manage/VerifyPhoneNumber        
        /// <summary>
        /// Verifies the phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns>Task.</returns>
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Sending SMS through SMS Provider to Verify Phone Number.
            
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        // POST: /Manage/VerifyPhoneNumber        
        /// <summary>
        /// Verifies the phone number.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (!result.Succeeded)
            {
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            //ModelState.AddModelError("", "Не удалось проверить телефон");
        }

        // POST: /Manage/RemovePhoneNumber        
        /// <summary>
        /// Removes the phone number.
        /// </summary>
        /// <returns>Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        // GET: /Manage/ChangePassword        
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ChangePassword() => View();

        // POST: /Manage/ChangePassword        
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }

            AddErrors(result);

            return View(model);
        }

        // GET: /Manage/SetPassword        
        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult SetPassword() => View();

        // POST: /Manage/SetPassword        
        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
            }

            AddErrors(result);

            return View(model);
        }

        // GET: /Manage/ManageLogins        
        /// <summary>
        /// Manages the logins.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        [HttpGet]
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "Внешнее имя входа удалено."
                : message == ManageMessageId.Error ? "Произошла ошибка."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }

            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        // POST: /Manage/LinkLogin        
        /// <summary>
        /// Links the login.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider) =>
            new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());

        // GET: /Manage/LinkLoginCallback        
        /// <summary>
        /// Links the login callback.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }

            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        /// <summary>
        /// Releases unmanaged and, if indicated, managed resources.
        /// </summary>
        /// <param name="disposing">True to free all resources (managed and unmanaged); false to free only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Supporting Applications
        // Used to protect against XSRF attacks when adding external logins        
        /// <summary>
        /// The XSRF key.
        /// </summary>
        private const string XsrfKey = "XsrfId";

        /// <summary>
        /// Gets the authentication manager.
        /// </summary>
        /// <value>
        /// The authentication manager.
        /// </value>
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        /// <summary>
        /// Adds the errors.
        /// </summary>
        /// <param name="result">The result.</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// Determines whether this instance has password.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has password; otherwise, <c>false</c>.
        /// </returns>
        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return user?.PasswordHash != null;
        }

        /*
        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }
        */
        #endregion
    }
}