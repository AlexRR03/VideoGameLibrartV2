namespace ProyectoJuegos.Models
{
    public class CreateUserListViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> SelectedGamesId { get; set; }
        public List<VideoGame> AvailableVideoGames { get; set; }
    }
}
