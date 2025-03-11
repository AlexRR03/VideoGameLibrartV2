using Microsoft.EntityFrameworkCore;
using ProyectoJuegos.Models;

namespace ProyectoJuegos.Data
{
    public class ProjectGamesContext: DbContext
    {
        public ProjectGamesContext(DbContextOptions<ProjectGamesContext> options) : base(options) { }
    
        public DbSet<VideoGame> VideoGames { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
