using System.ComponentModel.DataAnnotations;

namespace SimpleToDo.Web.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Full Name is required.")]
        public string FullName {  get; set; }
        [Required(ErrorMessage ="Email Is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password is required.")]
        public string Password { get; set; }
    }
}
