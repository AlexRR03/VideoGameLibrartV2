using Microsoft.EntityFrameworkCore;
using ProyectoJuegos.Data;
using ProyectoJuegos.Models;

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

    }
}
