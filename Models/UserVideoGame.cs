using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProyectoJuegos.Enums;

namespace ProyectoJuegos.Models
{
    [Table("UserVideoGame"]
    public class UserVideoGame
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Column("VideoGameId")]
        public int VideoGameId { get; set; }

        public VideoGame VideoGame { get; set; }

        [Column("Rating")]
        public int Rating { get; set; }
        [Column("PlayTimeHours")]
        public int PlayTimeHours { get; set; }

        [Column("Status")]
        public GameStatus Status { get; set; }


    }
}
