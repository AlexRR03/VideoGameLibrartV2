using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ProyectoJuegos.Models;
using ProyectoJuegos.Repositories;
using ProyectoJuegos.Services;

namespace ProyectoJuegos.Controllers
{
    public class ManagedController : Controller
    {
        private ApiService service;
        public ManagedController(ApiService service)
        {
            this.service = service;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string token = await this.service.GetTokenAsync(model.Email, model.Password);
            if(token == null)
            {
                ViewData["MESSAGGE"] = "User or Password Incorrect";
            }
            else
            {
                HttpContext.Session.SetString("TOKEN", token);
                string cleanToken = token.Trim('"');
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.NameIdentifier, ClaimTypes.Name);
                identity.AddClaim(new Claim(ClaimTypes.Name, model.Email));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, model.Password));
                identity.AddClaim(new Claim("TOKEN", cleanToken));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddHours(1) });
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
