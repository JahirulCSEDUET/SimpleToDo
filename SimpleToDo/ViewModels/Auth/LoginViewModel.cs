using System.ComponentModel.DataAnnotations;

namespace SimpleToDo.Web.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email is Required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password is required.")]
        public string Password { get; set; }
    }
}
