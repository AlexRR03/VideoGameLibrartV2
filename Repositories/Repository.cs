using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoJuegos.Data;
using ProyectoJuegos.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProyectoJuegos.Repositories
{
    public class Repository
    {
        private ProjectGamesContext gamesContext;

        public Repository(ProjectGamesContext gamesContext)
        {
            this.gamesContext = gamesContext;
        }
        
        //Metodo para obtener la lista de VideoJuegos que se utilizara en el dashboard
        public async Task<List<VideoGame>> GetVideoGamesAsync()
        {
            var query = from data in this.gamesContext.VideoGames select data;
            return await query.ToListAsync();
        }

        public async Task<VideoGame>FindVideoGameAsync(int id)
        {
            var query = from data in this.gamesContext.VideoGames where data.Id == id select data;
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetPlatformsGameAsync(string name)
        {
            string sql = "EXEC SP_GetPlatformsByGame @GameName";

            var platforms = await this.gamesContext.Database
                .SqlQueryRaw<string>(sql, new SqlParameter("@GameName", name))
                .ToListAsync();

            return platforms;
        }
        public async Task<List<VideoGame>> VideoGameSearch(string? name, string? genre, int? year,string? developer)
        {
            IQueryable<VideoGame> query = this.gamesContext.VideoGames;
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

    }
}
