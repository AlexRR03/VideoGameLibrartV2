using Microsoft.AspNetCore.Mvc;
using ProyectoJuegos.Models;
using ProyectoJuegos.Repositories;

namespace ProyectoJuegos.Controllers
{
    public class DashboardController : Controller
    {
        private Repository repo;

        public DashboardController(Repository repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<VideoGame> videoGames = await this.repo.GetVideoGamesAsync();
            return View(videoGames);
        }
        public async Task<IActionResult> Details (int id)
        {
            VideoGame videoGame = await this.repo.FindVideoGameAsync(id);
            return View(videoGame);
        }
    }
}
