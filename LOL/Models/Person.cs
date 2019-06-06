using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LOL.Models
{
    [Table("PERSON")]
    public class Person
    {
        [Column("p_id")]
        public int PersonId { get; set; }

        [Required]
        [Column("p_fname")]
        [Display(Name = "First Name")]
        public string PersonFname { get; set; }

        [Required]
        [Column("p_sname")]
        [Display(Name = "Surname")]
        public string PersonSname { get; set; }

        [Column("p_dob")]
        [Display(Name = "Born")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Column("p_bio")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Bio")]
        public string PersonDesc { get; set; }
     
        [Column("p_mainimg")]
        [Display(Name = "Image")]
        public string PersonImage { get; set; }

        //trimmed / sample copy of the description for the listings
        public string PersonDescTrimmed
        {

            //get only; updates are to FilmDesc
            get
            {

                //if the length of the desc is greater than 100 characters
                if ((PersonDesc.Length) > 100)
                    //get a substring of the first 100 characters followed by ellipses
                    return PersonDesc.Substring(0, 100) + ".....";
                else
                    //otherwise return the full description
                    return PersonDesc;

            }
        }
    }
}