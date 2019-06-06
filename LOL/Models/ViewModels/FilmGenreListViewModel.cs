using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOL.Models.ViewModels
{
    public class FilmGenreListViewModel
    {
        //the Writing model represented here
        public FilmGenre FilmgenreCredit;

        //here we have the linked Person to access their data
        public Genre Genre;

        //here we have the linked film to acces its data
        public Film Film;
    }
}