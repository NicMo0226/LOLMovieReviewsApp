using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LOL.Models
{
    [Table("GENRE")]
    public class Genre
    {
        [Column("genre_id")]
        public int GenreId { get; set; }

        [Required]
        [Column("genre_name")]
        [Display(Name ="Genre Type")]
        public string GenreName { get; set; }
    }
}