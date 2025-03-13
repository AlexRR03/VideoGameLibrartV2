using System.Security.Claims;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using ProyectoJuegos.Data;
using ProyectoJuegos.Models;


namespace ProyectoJuegos.Repositories
{
    public class Repository
    {
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
            User user = await this.context.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
            return user;
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
        
        


        #endregion

    }
}
