using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using ProyectoJuegos.Data;
using ProyectoJuegos.Helpers;
using ProyectoJuegos.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ProyectoJuegos.Repositories
{
    public class Repository
    {
        #region Procedures
//        CREATE PROCEDURE SP_GetGamesByUser
//@userId INT
//AS
//BEGIN
//    SELECT
//        vg.Id AS VideoGameId,
//        vg.[Name],
//        vg.Genre,
//        vg.Developer,
//		vg.ImageName,
//        uvg.PlayTimeHours,
//		uvg.[Status]
//    FROM VideoGame vg
//    INNER JOIN UserVideoGame uvg ON vg.Id = uvg.VideoGameId
//    WHERE uvg.UserId = @userId;
//        END;
//GO
        #endregion

        private ProjectGamesContext context;
        private IHttpContextAccessor contextAccessor;

        public Repository(ProjectGamesContext context, IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.contextAccessor = contextAccessor;
        }

        #region Videojuegos

        //Metodo para obtener la lista de VideoJuegos que se utilizara en el dashboard
        public async Task<List<VideoGame>> GetVideoGamesAsync()
        {
            var query = from data in this.context.VideoGames select data;
            return await query.ToListAsync();
        }

        //Metodo para obtener un VideoJuego por su Id
        public async Task<VideoGame>FindVideoGameAsync(int id)
        {
            var query = from data in this.context.VideoGames where data.Id == id select data;
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetPlatformsGameAsync(string name)
        {
            string sql = "EXEC SP_GetPlatformsByGame @GameName";

            var platforms = await this.context.Database
                .SqlQueryRaw<string>(sql, new SqlParameter("@GameName", name))
                .ToListAsync();

            return platforms;
        }
        //Metodo para la busqueda de VideoJuegos
        public async Task<List<VideoGame>> VideoGameSearch(string? name, string? genre, int? year,string? developer)
        {
            IQueryable<VideoGame> query = this.context.VideoGames;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(v => EF.Functions.Like(v.Name, $"%{name}%"));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(v => EF.Functions.Like(v.Genre, $"%{genre}%"));
            }

            if (year.HasValue)
            {
                query = query.Where(v => v.ReleaseYear == year);
            }

            if (!string.IsNullOrEmpty(developer))
            {
                query = query.Where(v => EF.Functions.Like(v.Developer, $"%{developer}%"));
            }
            return await query.ToListAsync();
        }
        #endregion

        #region Perfil

        public async Task<User> LoginUserAsync(string email, string password)
        {
            var consulta = from data in this.context.Users
                           where data.Email == email
                           select data;
            User user = await consulta.FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            else
            {
                string salt = user.Salt;
                byte[] temp = HelperCriptography.EncryptPass(password, salt);
                byte[] passHas = user.PasswordHash;
                bool response = HelperCriptography.ComparePass(temp, passHas);
                if(response)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<UserVideoGame> FindVideoGameLibraryAsync(int id)
        {
            string dato = this.contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userId = int.Parse(dato);
            return await this.context.UserVideoGames
        .Where(uvg => uvg.Id == id && uvg.UserId == userId)
        .FirstOrDefaultAsync();
        }

        public async Task<List<UserVideoGameModel>>GetVideoGamesByUserAsync()
        {
            string dato = this.contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userId = int.Parse(dato);
            var result = await context.Database.SqlQueryRaw<UserVideoGameModel>("SP_GetGamesByUser @UserId", new SqlParameter("@UserId", userId)).ToListAsync();
            return result;


        }
        #endregion

        #region VideoGames
        public async Task AddGameToLibraryAsync(int idVideoGame, int playtimeHours,string status)
        {
            string dato = this.contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int idUser = int.Parse(dato);

            UserVideoGame userVideoGame = new UserVideoGame();
            userVideoGame.UserId = idUser;
            userVideoGame.VideoGameId =idVideoGame ;
            userVideoGame.PlayTimeHours = playtimeHours;
            userVideoGame.Status = status;

            await this.context.UserVideoGames.AddAsync(userVideoGame);
            await this.context.SaveChangesAsync();
        }

        public async Task  UpdateVideoGameLibrary(int idVideoGame, int playtimeHours, string status)
        {
            string dato = this.contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int idUser = int.Parse(dato);
            UserVideoGame userVideoGame = await this.FindVideoGameLibraryAsync(idVideoGame);
            if (userVideoGame != null)
            {
                userVideoGame.PlayTimeHours = playtimeHours;
                userVideoGame.Status = status;
                await this.context.SaveChangesAsync();
            }

        }

        public async Task DeleteVideoGameLibraryAsync(int id)
        {
            UserVideoGame userVideoGame = await this.FindVideoGameLibraryAsync(id);
            if (userVideoGame != null)
            {
                this.context.UserVideoGames.Remove(userVideoGame);
                await this.context.SaveChangesAsync();
            }
        }




        #endregion

        #region Users
        private async Task<int> GetMaxIdUser()
        {
            if (this.context.Users.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Users.MaxAsync(u => u.Id) + 1;
            }
        }

        public async Task RegisterUserAsync(string username, string email, string password)
        {
            User user = new User();
            user.Id = await this.GetMaxIdUser();
            user.Username = username;
            user.Email = email;
            user.Password = password;
            user.Salt = HelperCriptography.GenerateSalt();
            user.PasswordHash = HelperCriptography.EncryptPass(password,user.Salt);
            await this.context.Users.AddAsync(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<UserList> CreateUserListAsync(string name, string description)
        {
            string dato = this.contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userId = int.Parse(dato);
            UserList userList = new UserList();
            userList.UserId = userId;
            userList.Name = name;
            userList.Description = description;
            this.context.UserList.Add(userList);
            await this.context.SaveChangesAsync();
            return userList;

        }


            #endregion

        }
}
