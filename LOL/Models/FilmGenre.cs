using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LOL.Models
{
    [Table("FILM_GENRE")]
    public class FilmGenre
    {
        [Column("film_genre_id")]
        public int FilmGenreId { get; set; }

        [Required]
        [Display(Name = "Genre Selection")]
        [Column("genre_id")]
        public int GenreId { get; set; }

        [Required]
        [Display(Name = "Film Selection")]
        [Column("film_id")]
        public int FilmId { get; set; }
    }
}