using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
