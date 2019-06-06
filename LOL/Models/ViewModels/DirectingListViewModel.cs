using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOL.Models.ViewModels
{
    public class DirectingListViewModel
    {
        //the Directing model represented here
        public Directing DirectingCredit;

        //here we have the linked person to access their data
        public Person Director;

        //here we have the linked film to access its data
        public Film Film;
    }
}