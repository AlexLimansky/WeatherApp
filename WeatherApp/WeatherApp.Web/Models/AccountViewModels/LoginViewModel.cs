using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Web.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "The Email field is required.")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
