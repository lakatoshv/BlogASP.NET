using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Blog.Models
{
    /// <summary>
    /// Index view model.
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// Gets or sets hasPassword.
        /// </summary>
        public bool HasPassword { get; set; }

        /// <summary>
        /// Gets or sets logins.
        /// </summary>
        public IList<UserLoginInfo> Logins { get; set; }

        /// <summary>
        /// Gets or sets phoneNumber.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets twoFactor.
        /// </summary>
        public bool TwoFactor { get; set; }

        /// <summary>
        /// Gets or sets browserRemembered.
        /// </summary>
        public bool BrowserRemembered { get; set; }
    }

    /// <summary>
    /// Manage logins view model.
    /// </summary>
    public class ManageLoginsViewModel
    {
        /// <summary>
        /// Gets or sets currentLogins.
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        /// Gets or sets otherLogins.
        /// </summary>
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    /// <summary>
    /// Factor view model.
    /// </summary>
    public class FactorViewModel
    {
        /// <summary>
        /// Gets or sets purpose.
        /// </summary>
        public string Purpose { get; set; }
    }

    /// <summary>
    /// Set password view model.
    /// </summary>
    public class SetPasswordViewModel
    {
        /// <summary>
        /// Gets or sets newPassword.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать символов не менее: {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets confirmPassword.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Change password view model.
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Gets or sets oldPassword.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets newPassword.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать символов не менее: {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets confirmPassword.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Add phone number view model.
    /// </summary>
    public class AddPhoneNumberViewModel
    {
        /// <summary>
        /// Gets or sets number.
        /// </summary>
        [Required]
        [Phone]
        [Display(Name = "Номер телефона")]
        public string Number { get; set; }
    }

    /// <summary>
    /// Verify phone number view model.
    /// </summary>
    public class VerifyPhoneNumberViewModel
    {
        /// <summary>
        /// Gets or sets code.
        /// </summary>
        [Required]
        [Display(Name = "Код")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets phoneNumber.
        /// </summary>
        [Required]
        [Phone]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
    }

    /// <summary>
    /// Configure two factor view model.
    /// </summary>
    public class ConfigureTwoFactorViewModel
    {
        /// <summary>
        /// Gets or sets selectedProvider.
        /// </summary>
        public string SelectedProvider { get; set; }

        /// <summary>
        /// Gets or sets providers.
        /// </summary>
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}