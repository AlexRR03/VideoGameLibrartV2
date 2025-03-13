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
        public async Task<IActionResult> Profile()
        {
            List<UserVideoGameModel> userVideoGames = await this.repo.GetVideoGamesByUserAsync();
            if (userVideoGames.Count <= 0)
            {
                ViewData["VIDEOGAMESNUMBER"] = 0;
                ViewData["MOSTPLAYED"] = "No games played yet";
            }
            else
            {
                ViewData["VIDEOGAMESNUMBER"] = userVideoGames.Count();
                var mostPlayed = userVideoGames
                        .OrderByDescending(x => x.PlayTimeHours)
                        .FirstOrDefault();
                ViewData["MOSTPLAYED"] = mostPlayed.Name;
            }
            return View(userVideoGames);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            await this.repo.RegisterUserAsync(username, email, password);
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult CreateList()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateList(UserList userList)
        {
            await this.repo.CreateUserListAsync(userList.Name, userList.Description);
            return RedirectToAction("Index", "Dashboard");
        }
        public async Task<IActionResult> GetUserList()
        {
            List<UserList> userLists = await this.repo.GetUserListsAsync();
            return View(userLists);
        }

        public async Task<IActionResult> AddVideoGamesList()
        {
            List<UserList> nameUserList = await this.repo.GetUserListsAsync();
            ViewData["NameUserList"] = nameUserList;
            return View(nameUserList);
        }


    }
}
