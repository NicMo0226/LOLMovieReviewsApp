using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOL.Models.ViewModels
{
    public class WritingListViewModel
    {
        //the Writing model represented here
        public Writing WritingCredit;

        //here we have the linked Person to access their data
        public Person Writer;

        //here we have the linked film to acces its data
        public Film Film;
        
    }
}