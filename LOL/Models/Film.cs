using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LOL.Models
{
    [Table("FILM")]
    public class Film
    {
        [Column("film_id")]
        public int FilmId { get; set; }

        [Required]
        [Column("film_name")]
        [Display(Name = "Film Title")]
        public string FilmTitle { get; set; }

        [Required]
        [Column("film_sequel")]
        [Display(Name = "Sequel")]
        public string FilmSequel { get; internal set; }

        [Column("film_trailer")]
        [Display(Name = "Film Trailer")]
        public string FilmTrailer { get; internal set; }

        [Required]
        [Column("film_bbfc")]
        [Display(Name = "Age Guidence")]
        public string FilmBBFC { get; internal set; }

        [Required]
        [Column("film_desc")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string FilmDesc { get; internal set; }

        [Column("film_date")]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime? FilmReleaseDate { get; internal set; }

        [Column("film_mainimg")]
        [Display(Name = "Image")]
        public string FilmImage { get; internal set; }

        public string FilmDescTrimmed
        {

            //get only; updates are to FilmDesc
            get
            {

                //if the length of the desc is greater than 100 characters
                if ((FilmDesc.Length) > 100)
                    //get a substring of the first 100 characters followed by ellipses
                    return FilmDesc.Substring(0, 100) + ".....";
                else
                    //otherwise return the full description
                    return FilmDesc;

            }
        }

       
    }
}
