using Microsoft.AspNetCore.Mvc;
using ProyectoJuegos.Filters;

namespace ProyectoJuegos.Controllers
{
    public class UsersController : Controller
    {
        [AuthorizeUsers]
        public IActionResult Profile()
        {
            
            return View();
        }
    }
}
