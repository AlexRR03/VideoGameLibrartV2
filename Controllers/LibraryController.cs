using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProyectoJuegos.Helpers;
using ProyectoJuegos.Models;
using ProyectoJuegos.Repositories;

namespace ProyectoJuegos.Controllers
{
    public class LibraryController:Controller
    {
        private Repository repo;

        public LibraryController(Repository repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> AddGame(int idVideoGame)
        {
           VideoGame videoGame = await this.repo.FindVideoGameAsync(idVideoGame);
            List<string> gameStatus = HelperListStatus.GetGameStatusList();
            ViewData["STATUS"] = gameStatus;
            return View(videoGame);

        }
        [HttpPost]
        public async Task<IActionResult> AddGame(UserVideoGame userVideoGame)
        {
             await this.repo.AddGameToLibraryAsync(userVideoGame.VideoGameId, userVideoGame.PlayTimeHours, userVideoGame.Status);
            return RedirectToAction("Profile","Users");
        }
    }
}
