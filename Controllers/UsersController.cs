using Microsoft.AspNetCore.Mvc;
using ProyectoJuegos.Filters;

namespace ProyectoJuegos.Controllers
{
    public class UsersController : Controller
    {
        [AuthorizeUsers]
        public IActionResult Profile()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return View(claims);
        }
    }
}
