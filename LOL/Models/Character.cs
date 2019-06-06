using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LOL.Models
{
    [Table("CHARACTER")]
    public class Character
    {
        [Column("c_id")]
        public int CharacterId { get; set; }

        [Required]
        [Display(Name = "Character Name")]
        [Column("c_fname")]
        public string CharacterFname { get; set; }

        [Column("c_sname")]
        public string CharacterSname { get; set; }

     

        [Column("c_date")]
        [Display(Name = "Date Appeared")]
        [DataType(DataType.Date)]
        public DateTime? CharacterDate { get; set; }

    
        [Display(Name = "Quotes")]
        [Column("c_quote")]
        [DataType(DataType.MultilineText)]
              public string CharacterQuote { get; set; }
    
        public string CharacterQuoteTrimmed
        {

            //get only; updates are to FilmDesc
            get
            {

                //if the length of the desc is greater than 100 characters
                if ((CharacterQuote.Length) > 100)
                    //get a substring of the first 100 characters followed by ellipses
                    return CharacterQuote.Substring(0, 100) + ".....";
                else
                    //otherwise return the full description
                    return CharacterQuote;

            }
        }
    }
}
