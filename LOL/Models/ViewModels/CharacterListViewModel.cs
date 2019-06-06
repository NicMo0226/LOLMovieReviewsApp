using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOL.Models.ViewModels
{
    public class CharacterListViewModel
    {
        //the Character model represented here
        public Character CharacterCredit;

        //here we have the linked Person to access their data
        public Person Character;

        //here we have the linked film to acces its data
        public Film Film;

      

    }
}