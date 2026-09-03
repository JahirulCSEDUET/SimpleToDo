using Microsoft.AspNetCore.Mvc;

namespace SimpleToDo.Web.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
