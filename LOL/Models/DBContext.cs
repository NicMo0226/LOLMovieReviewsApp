using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LOL.Models
{
    public class DBContext : DbContext
    {
        public DbSet<Film> Films { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Acting> Actings { get; set; }

        public DbSet<Directing> Directings { get; set; }

        public DbSet<Writing> Writings { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<FilmGenre> FilmGenres { get; set; }

        public DbSet<Review> Reviews { get; set; }

       

    }
}