using System.ComponentModel.DataAnnotations;

namespace SimpleToDo.Web.ViewModels.Project
{
    public class ProjectCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
