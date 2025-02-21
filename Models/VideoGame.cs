using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoJuegos.Models
{
    [Table("VideoGame")]
    public class VideoGame
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Genre")]
        public string Genre { get; set; }

        [Column("Developer")]
        public string Developer { get; set; }

        [Column("ReleaseYear")]
        public int ReleaseYear { get; set; }

        [Column("ImageName")]
        public string ImageName { get; set; }
    }
}
