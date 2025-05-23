﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoJuegos.Models
{
    [Table("UserVideoGame")]
    public class UserVideoGame
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("VideoGameId")]
        public int VideoGameId { get; set; }


        [Column("PlayTimeHours")]
        public int PlayTimeHours { get; set; }


        [Column("Status")]
        public string Status { get; set; }
    }
}
