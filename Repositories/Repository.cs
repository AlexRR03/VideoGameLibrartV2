using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoJuegos.Data;
using ProyectoJuegos.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProyectoJuegos.Repositories
{
    public class Repository
    {
        private ProjectGamesContext context;

        public Repository(ProjectGamesContext context)
        {
            this.context = context;
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



















        #endregion
    }
}
