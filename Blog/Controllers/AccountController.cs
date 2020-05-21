using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLog.Data;
using Blog.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Blog.Models;
using Blog.Services.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Controllers
{
    [Authorize]
    public class AccountController : Controller
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
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        public AccountController()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
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

        // GET: /Account/Login        
        /// <summary>
        /// Logins the specified return URL.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login        
        /// <summary>
        /// Logins the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Login failures do not block account.
            // Replace account errors with password entry errors shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
                case SignInStatus.Failure:
                    ModelState.AddModelError("", @"Неудачная попытка входа.");
                    return View(model);
                default:
                    ModelState.AddModelError("", @"Неудачная попытка входа.");
                    return View(model);
            }
        }

        // GET: /Account/VerifyCode        
        /// <summary>
        /// Verifies the code.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="rememberMe">if set to <c>true</c> [remember me].</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require user pre-login with username and password or external login.
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/VerifyCode        
        /// <summary>
        /// Verifies the code.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The code below protects against attacks by a selection method aimed at two-factor codes. 
            // If the user enters the wrong codes for the specified time, his account will be blocked for a specified period. 
            // Account lockout options can be configured in IdentityConfig.
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                    ModelState.AddModelError("", @"Неправильный код.");
                    return View(model);
                case SignInStatus.RequiresVerification:
                    return View(model);
                default:
                    ModelState.AddModelError("", @"Неправильный код.");
                    return View(model);
            }
        }

        // GET: /Account/Register        
        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register        
        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var store = new UserStore<ApplicationUser>(new BlogContext());
                var userManager = new UserManager<ApplicationUser>(store);
                var createdUser = userManager.FindByEmailAsync(model.Email).Result;
                var profile = new Profile {ApplicationUserId = createdUser.Id };
                var db = new BlogContext();
                db.Profiles.Add(profile);
                db.SaveChanges();
                await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                // Send an email with this link.
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "Подтверждение учетной записи", "Подтвердите вашу учетную запись, щелкнув <a href=\"" + callbackUrl + "\">здесь</a>");

                return RedirectToAction("Index", "Home");
            }
            AddErrors(result);

            // The appearance of this message indicates an error; redisplay of form
            return View(model);
        }

        // GET: /Account/ConfirmEmail        
        /// <summary>
        /// Confirms the email.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        // GET: /Account/ForgotPassword        
        /// <summary>
        /// Forgot the password.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword        
        /// <summary>
        /// Forgot the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                // Не показывать, что пользователь не существует или не подтвержден
                return View("ForgotPasswordConfirmation");
            }

            // Send an email with this link
            // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
            // await UserManager.SendEmailAsync(user.Id, "Сброс пароля", "Сбросьте ваш пароль, щелкнув <a href=\"" + callbackUrl + "\">здесь</a>");
            // return RedirectToAction("ForgotPasswordConfirmation", "Account");

            // The appearance of this message indicates an error; redisplay of form
            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation        
        /// <summary>
        /// Forgot the password confirmation.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword        
        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword        
        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Do not show that the user does not exist.
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation        
        /// <summary>
        /// Resets the password confirmation.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // POST: /Account/ExternalLogin        
        /// <summary>
        /// Externals the login.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Redirect request to an external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/SendCode        
        /// <summary>
        /// Sends the code.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="rememberMe">if set to <c>true</c> [remember me].</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/SendCode        
        /// <summary>
        /// Sends the code.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Create and submit a token
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        }

        // GET: /Account/ExternalLoginCallback        
        /// <summary>
        /// Externals the login callback.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Perform user login through this external login provider, if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                    ModelState.AddModelError("", @"Неправильный код.");
                    return RedirectToAction("Index", "Posts");
                default:
                    // If the user does not have an account, then he is invited to create it
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        // POST: /Account/ExternalLoginConfirmation        
        /// <summary>
        /// Externals the login confirmation.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Retrieving user information from an external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // POST: /Account/LogOut        
        /// <summary>
        /// Logs the out.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/ExternalLoginFailure        
        /// <summary>
        /// Externals the login failure.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        /// <summary>
        /// Releases unmanaged and, if indicated, managed resources.
        /// </summary>
        /// <param name="disposing">True to free all resources (managed and unmanaged); false to free only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Вспомогательные приложения
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
        /// Redirects to local.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Challenge result.
        /// </summary>
        /// <seealso cref="HttpUnauthorizedResult" />
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ChallengeResult"/> class.
            /// </summary>
            /// <param name="provider">The provider.</param>
            /// <param name="redirectUri">The redirect URI.</param>
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ChallengeResult"/> class.
            /// </summary>
            /// <param name="provider">The provider.</param>
            /// <param name="redirectUri">The redirect URI.</param>
            /// <param name="userId">The user identifier.</param>
            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            /// <summary>
            /// Gets or sets the login provider.
            /// </summary>
            /// <value>
            /// The login provider.
            /// </value>
            public string LoginProvider { get; set; }

            /// <summary>
            /// Gets or sets the redirect URI.
            /// </summary>
            /// <value>
            /// The redirect URI.
            /// </value>
            public string RedirectUri { get; set; }

            /// <summary>
            /// Gets or sets the user identifier.
            /// </summary>
            /// <value>
            /// The user identifier.
            /// </value>
            public string UserId { get; set; }

            /// <summary>
            /// Allows processing of the result of the execution of an action method by a user type inherited from the class <see>
            ///     <cref>T:System.Web.Mvc.ActionResult</cref>
            /// </see>
            /// .
            /// </summary>
            /// <param name="context">The context in which the result is executed. Context information includes information about the controller, HTTP-content, request context, and route data.</param>
            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}