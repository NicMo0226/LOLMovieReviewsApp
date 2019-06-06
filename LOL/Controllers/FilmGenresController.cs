using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LOL.Models;
using LOL.Models.ViewModels;

namespace LOL.Controllers
{
    public class FilmGenresController : Controller
    {
        private DBContext db = new DBContext();

        // GET: FilmGenres
        public ActionResult Index()
        {
            //create a list for the view model to link Film and Person
            List<FilmGenreListViewModel> FilmGenreList =
                 new List<FilmGenreListViewModel>();

            //seperate list for the directing credits to get the keys
            List<FilmGenre> filmgenreCredits;

            //populate the Directings list by selecting all records
            //from the db context
            filmgenreCredits = db.FilmGenres.ToList();

            //loop through each record t get the forwign keys
            //then populate the view model with the relevant 
            //Film / Person
            foreach (FilmGenre g in filmgenreCredits)
            {
                //match the ID between DIrecting and FIlm - store the single record in 'Film'
                Film film = db.Films.Where(x => x.FilmId == g.FilmId).Single();

                //match the ID between Directing and Film - -store the single record in 'Director'
                Genre type = db.Genres.Where(x => x.GenreId == g.GenreId).Single();

                //new ActingListViewModel object then add to the list
                FilmGenreListViewModel toAdd = new FilmGenreListViewModel();

                toAdd.FilmgenreCredit = g; //get the directing credit recored
                toAdd.Film = film;   //get the film record
                toAdd.Genre = type; //get the person record  (ad director)

                //add to the DirectingList (list of ViewModel objects)
                FilmGenreList.Add(toAdd);
            }
            //send the DIrectingListViewModel list to the view for display
            return View(FilmGenreList);
        }

        // GET: FilmGenres/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilmGenre filmGenre = db.FilmGenres.Find(id);
            if (filmGenre == null)
            {
                return HttpNotFound();
            }
            return View(filmGenre);
        }

        // GET: FilmGenres/Create
        public ActionResult Create(int FilmId = 0, int GenreId = 0)
        {
            //FILMS------------------------------------------------------------
            //from the Films model Dbset
            //select all columns from the database
            //order by the film title
            var filmQuery = from m in db.Films
                            orderby m.FilmTitle
                            select m;
            //if no id set
            if (FilmId == 0)
                //construct full dropdown list without preselection 
                //do so from the query results and display tyhe FilmTitle
                //store in FIlmID in the viweBag
                ViewBag.FilmId = new SelectList(filmQuery, "FilmId", "FilmTitle", null);
            else//consruct as above but with the FilmID preselected 
                ViewBag.FilmId = new SelectList(filmQuery, "FilmId", "FilmTitle", FilmId);


            //PERSONS----------------------------------------------------------------------
            //from the Persons model DbSet
            //select the fname and sname as a new field called Name
            //and the person id - order by the sname
            var genresQuery = from g in db.Genres
                              orderby g.GenreName
                              select new
                              {
                                  Name = g.GenreName,
                                  g.GenreId

                              };
            //if no id set
            if (GenreId == 0)
                //construct full films dropdown list without preselection 
                //do so from the query results and display the NAme (combined above)
                //store in FilmId in the viewBag
                ViewBag.GenreId = new SelectList(genresQuery, "GenreId", "Name", null);
            else
                ViewBag.GenreId = new SelectList(genresQuery, "GenreId", "Name", GenreId);

            //generte the view                                      
            return View();
        }

        // POST: FilmGenres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FilmGenreId,GenreId,FilmId")] FilmGenre filmGenre)
        {
            if (ModelState.IsValid)
            {
                db.FilmGenres.Add(filmGenre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(filmGenre);
        }

        // GET: FilmGenres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilmGenre filmGenre = db.FilmGenres.Find(id);
            if (filmGenre == null)
            {
                return HttpNotFound();
            }
            return View(filmGenre);
        }

        // POST: FilmGenres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FilmGenreId,GenreId,FilmId")] FilmGenre filmGenre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filmGenre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(filmGenre);
        }

        // GET: FilmGenres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilmGenre filmGenre = db.FilmGenres.Find(id);
            if (filmGenre == null)
            {
                return HttpNotFound();
            }
            return View(filmGenre);
        }

        // POST: FilmGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FilmGenre filmGenre = db.FilmGenres.Find(id);
            db.FilmGenres.Remove(filmGenre);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
