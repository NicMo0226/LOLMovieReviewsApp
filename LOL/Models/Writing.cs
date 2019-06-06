using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LOL.Models
{
    [Table("WRITER")]
    public class Writing
    {
        [Column("w_id")]
        public int WritingId { get; set; }

        [Required]
        [Display(Name = "Writer Selection")]
        [Column("p_id")]
        public int PersonId { get; set; }

        [Required]
        [Display(Name = "Film Selection")]
        [Column("film_id")]
        public int FilmId { get; set; }
      
      
    }
}