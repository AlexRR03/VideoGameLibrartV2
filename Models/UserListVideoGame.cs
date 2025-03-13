using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoJuegos.Models
{
    [Table("UserListVideoGame")]
    public class UserListVideoGame
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("UserListId")]
        public int UserListId { get; set; }
        [Column("VideoGameId")]
        public int VideoGameId { get; set; }

        public UserList UserList { get; set; }
        public VideoGame VideoGame { get; set; }
    }
}
