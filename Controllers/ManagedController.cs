using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProyectoJuegos.Models;
using ProyectoJuegos.Repositories;

namespace ProyectoJuegos.Controllers
{
    public class ManagedController : Controller
    {
        private Repository repo;
        public ManagedController(Repository repo)
        {
            this.repo = repo;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Login(string email, string password)
        {
            User user = await this.repo.LoginUserAsync(email, password);
            if(user != null)
            {
                // Create the claims
                // Crear la identidad
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                // Agregar los claims paso a paso
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

                // Crear el principal
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Profile", "Users");


            }
            else
            {
                ViewData["MESSAGGE"] = "Email or password are incorrect";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
