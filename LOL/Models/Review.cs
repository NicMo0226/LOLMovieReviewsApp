using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LOL.Models
{
    [Table("REVIEW")]
    public class Review
    {
        [Column("review_id")]
        public int ReviewId { get; set; }

        [Required]
        [Column("film_id")]
        [Display(Name = "Film")]
        public int FilmId { get; set; }

        [Required]
        [Column("review_uname")]
        [Display(Name = "Reviewer: ")]
        public string ReviewUname { get; set; }

        [Required]
        [Column("review_desc")]
        [Display(Name = "Review")]
        [DataType(DataType.MultilineText)]
        public string ReviewContent { get; set; } 

        [Required]
        [Column("review_star")]
        [Display(Name = "Rating")]
        public int ReviewRating { get; set; }
       
        public string ReviewContentTrimmed
        {

            //get only; updates are to FilmDesc
            get
            {

                //if the length of the desc is greater than 100 characters
                if ((ReviewContent.Length) > 100)
                    //get a substring of the first 100 characters followed by ellipses
                    return ReviewContent.Substring(0, 100) + ".....";
                else
                    //otherwise return the full description
                    return ReviewContent;

            }
        }
    }
}
