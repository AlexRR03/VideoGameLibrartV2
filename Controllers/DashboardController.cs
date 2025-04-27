using Microsoft.AspNetCore.Mvc;
using ProyectoJuegos.Models;
using ProyectoJuegos.Repositories;
using ProyectoJuegos.Services;

namespace ProyectoJuegos.Controllers
{
    public class DashboardController : Controller
    {
        private ApiService service;

        public DashboardController(ApiService service)
        {
            this.service = service;
        }


        public async Task<IActionResult> Index(string? name, string? genre, int? year, string? developer)
        {
            List<VideoGame> videoGames;

            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(genre) || year.HasValue || !string.IsNullOrEmpty(developer))
            {
                // If there are filters, execute the search
                videoGames = await this.service.VideoGameSearchAsync(name, genre, year, developer);
            }
            else
            {
                // If no filters, load all video games
                videoGames = await this.service.GetVideoGamesAsync();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_VideoGamesList", videoGames);
            }

            return View(videoGames);
        }

        public async Task<IActionResult> Details (int id)
        {
            VideoGame videoGame = await this.service.FindVideoGameAsync(id);
            if (videoGame == null)
            {
                return RedirectToAction("Error", "Shared");
            }
            List<string> platforms = await this.service.GetPlatformsGameAsync(videoGame.Name);
            var viewModel = new VideoGameDetailsViewModel
            {
                VideoGame = videoGame,
                Platforms = platforms
            };
            return View(viewModel);
        }
    }
}
