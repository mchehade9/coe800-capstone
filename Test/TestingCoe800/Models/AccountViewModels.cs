using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TestingCoe800.Models
{

  

    public class ExternalLoginConfirmationViewModel
    {
        private static UsersDBEntities db = new UsersDBEntities();
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]

        [Display(Name = "UserRoles")]

        public string UserRoles { get; set; } = "Guest";



        [Display(Name = "FirstName")]

        public string FirstName { get; set; }


        [Display(Name = "LastName")]

        public string LastName { get; set; }
        [Required]
        [Display(Name = "UserRoleFK")]

        public string UserRolesFk { get; set; } = db.AspNetRoles.SingleOrDefault(r => r.Name == "Guest").ToString();


    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }

    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }



        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }


        /*[Display(Name = "UserRolesFK")]

        public string UserRolesFk { get; set; }*/
    }

    public class CreateNewUser
    {
        private static UsersDBEntities db = new UsersDBEntities();

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }


    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    [Required]
    [Display(Name = "FirstName")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "LastName")]
    public string LastName { get; set; }



    [Display(Name = "UserRoleFK")]

    public string UserRoleFK { get; set; }



    [Display(Name = "UserRoles")]

    public string UserRoles { get; set; }
}
    

    public class RegisterViewModel
    {
        private static UsersDBEntities db = new UsersDBEntities();

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

       

        [Display(Name = "UserRoleFK")]

        public string UserRoleFK { get; set; } 



        [Display(Name = "UserRoles")]

        public string UserRoles { get; set; } 
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

}
