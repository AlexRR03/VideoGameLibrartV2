using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProyectoJuegos.Helpers;
using ProyectoJuegos.Models;
using ProyectoJuegos.Repositories;
using ProyectoJuegos.Services;

namespace ProyectoJuegos.Controllers
{
    public class LibraryController:Controller
    {
        private ApiService service;

        public LibraryController(ApiService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> AddGame(int idVideoGame)
        {
            VideoGame videoGame = await this.service.FindVideoGameAsync(idVideoGame);
            List<string> gameStatus = HelperListStatus.GetGameStatusList();
            ViewData["STATUS"] = gameStatus;
            return View(videoGame);

        }
        [HttpPost]
        public async Task<IActionResult> AddGame(UserVideoGame userVideoGame)
        {
            try
            {
                
                await this.service.AddGameToLibraryAsync(userVideoGame.VideoGameId, userVideoGame.PlayTimeHours, userVideoGame.Status);

                
                return RedirectToAction("Profile", "Users");
            }
            catch (Exception ex)
            {
                
                ViewBag.ErrorMessage = ex.Message;
                return View();  // Retorna a la vista actual si hubo un error
            }
        }

        //public async Task<IActionResult> EditGame(int idVideoGame)
        //{
        //    UserVideoGame userVideoGame = await this.repo.FindVideoGameLibraryAsync(idVideoGame);
        //    List<string> gameStatus = HelperListStatus.GetGameStatusList();
        //    ViewData["STATUS"] = gameStatus;
        //    return View(userVideoGame);
        //}
        //[HttpPost]
        //public async Task<IActionResult> EditGame(UserVideoGame userVideoGame)
        //{
        //    await this.repo.UpdateVideoGameLibrary(userVideoGame.VideoGameId, userVideoGame.PlayTimeHours, userVideoGame.Status);
        //    return RedirectToAction("Profile", "Users");
        //}


        //public async Task<IActionResult> DeleteGame(int id)
        //{
        //    await this.repo.DeleteVideoGameLibraryAsync(id);
        //    return RedirectToAction("Profile", "Users");
        //}
    }
}
