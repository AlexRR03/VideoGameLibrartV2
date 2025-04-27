using Microsoft.AspNetCore.Mvc;
using ProyectoJuegos.Filters;
using ProyectoJuegos.Models;
using ProyectoJuegos.Repositories;
using ProyectoJuegos.Services;

namespace ProyectoJuegos.Controllers
{
    public class UsersController : Controller
    {
        private ApiService service;
        public UsersController(ApiService service)
        {
            this.service = service;
        }
        [AuthorizeUsers]
        public async Task<IActionResult> Profile()
        {
            List<UserVideoGameModel> listUserVideoGames = await this.service.GetVideoGamesByUserAsync();
            if (listUserVideoGames == null || listUserVideoGames.Count <= 0)
            {
                ViewData["VIDEOGAMESNUMBER"] = 0;
                ViewData["MOSTPLAYED"] = "No games played yet";
            }
            else
            {
                ViewData["VIDEOGAMESNUMBER"] = listUserVideoGames.Count;
                var mostPlayed = listUserVideoGames
                        .OrderByDescending(x => x.PlayTimeHours)
                        .FirstOrDefault();
                ViewData["MOSTPLAYED"] = mostPlayed.Name;
            }
            return View(listUserVideoGames);
        }

        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Register(string username, string email, string password)
        //{
        //    await this.repo.RegisterUserAsync(username, email, password);
        //    return RedirectToAction("Index", "Dashboard");
        //}

        //public IActionResult CreateList()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> CreateList(UserList userList)
        //{
        //    await this.repo.CreateUserListAsync(userList.Name, userList.Description);
        //    return RedirectToAction("Index", "Dashboard");
        //}
        //public async Task<IActionResult> GetUserList()
        //{
        //    List<UserList> userLists = await this.repo.GetUserListsAsync();
        //    return View(userLists);
        //}

        //public async Task<IActionResult> AddVideoGamesList()
        //{
        //    List<UserList> nameUserList = await this.repo.GetUserListsAsync();
        //    ViewData["NameUserList"] = nameUserList;
        //    return View(nameUserList);
        //}


    }
}
