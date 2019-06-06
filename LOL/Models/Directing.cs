using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LOL.Models
{
    [Table("DIRECTOR")]
    public class Directing
    {
        [Column("d_id")]
        public int DirectingId { get; set; }

        [Required]
        [Display(Name = "Director Selection")]
        [Column("p_id")]
        public int PersonId { get; set; }

        [Required]
        [Display(Name = "Film Selection")]
        [Column("film_id")]
        public int FilmId { get; set; }
    }
}