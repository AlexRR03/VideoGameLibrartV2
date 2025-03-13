using Microsoft.AspNetCore.Mvc;
using ProyectoJuegos.Filters;
using ProyectoJuegos.Models;
using ProyectoJuegos.Repositories;

namespace ProyectoJuegos.Controllers
{
    public class UsersController : Controller
    {
        private Repository repo;
        public UsersController(Repository repo)
        {
            this.repo = repo;
        }
        [AuthorizeUsers]
        public async Task <IActionResult> Profile()
        {
            List<UserVideoGameModel> userVideoGames = await this.repo.GetVideoGamesByUserAsync() ;
            ViewData["VIDEOGAMESNUMBER"] = userVideoGames.Count();
            var mostPlayed = userVideoGames
                    .OrderByDescending(x => x.PlayTimeHours)
                    .FirstOrDefault();
            ViewData["MOSTPLAYED"] = mostPlayed.Name;
            return View(userVideoGames);
        }
    }
}
