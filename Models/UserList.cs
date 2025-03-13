using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoJuegos.Models
{
    [Table("UserList")]
    public class UserList
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("UserId")]
        public int UserId { get; set; }
        [Column("Name")]
        public string Name { get; set; }

        [Column("Description")]
        public string Description { get; set; }

    }
}
