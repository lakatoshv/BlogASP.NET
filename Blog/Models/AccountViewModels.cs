using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    /// <summary>
    /// External login confirmation view model.
    /// </summary>
    public class ExternalLoginConfirmationViewModel
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }
    }

    /// <summary>
    /// External login list view model.
    /// </summary>
    public class ExternalLoginListViewModel
    {
        /// <summary>
        /// Gets or sets returnUrl.
        /// </summary>
        public string ReturnUrl { get; set; }
    }

    /// <summary>
    /// Send code view model.
    /// </summary>
    public class SendCodeViewModel
    {
        /// <summary>
        /// Gets or sets selectedProvider.
        /// </summary>
        public string SelectedProvider { get; set; }

        /// <summary>
        /// Gets or sets providers.
        /// </summary>
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }

        /// <summary>
        /// Gets or sets returnUrl.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets rememberMe.
        /// </summary>
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Verify code view model.
    /// </summary>
    public class VerifyCodeViewModel
    {
        /// <summary>
        /// Gets or sets provider.
        /// </summary>
        [Required]
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets code.
        /// </summary>
        [Required]
        [Display(Name = "Код")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets returnUrl.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets rememberBrowser.
        /// </summary>
        [Display(Name = "Запомнить браузер?")]
        public bool RememberBrowser { get; set; }

        /// <summary>
        /// Gets or sets rememberMe.
        /// </summary>
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Forgot view model.
    /// </summary>
    public class ForgotViewModel
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }
    }

    /// <summary>
    /// Login view model.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required]
        [Display(Name = "Адрес электронной почты")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets rememberMe.
        /// </summary>
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Register view model.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets firstName.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets lastName.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [Display(Name = "Фамілія")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets confirmPassword.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Reset password view model.
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets confirmPassword.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets code.
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// Forgot password view model.
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Почта")]
        public string Email { get; set; }
    }
}
