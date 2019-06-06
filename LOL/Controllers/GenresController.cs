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
    public class GenresController : Controller
    {

        private DBContext db = new DBContext();
        public ActionResult Index()
        {
            //list of film reviews model objects to link
            //reviews with related films
            List<FilmGenreListViewModel> FilmgenreList =
                new List<FilmGenreListViewModel>();
        //list of review objects to cycle through and map id's 
        List<FilmGenre> filmgenreCredits;
            //populate the list with the review records from the database

            filmgenreCredits = db.FilmGenres.ToList();

            //loop through each review in the list of ata (each row
            foreach (FilmGenre g in filmgenreCredits)
            {

                //select the film record where the ids amtch
                Film film = db.Films.Where(x => x.FilmId == g.FilmId).Single();
                //select the film record where the ids amtch
                Genre genre = db.Genres.Where(x => x.GenreId == g.GenreId).Single();

                //create a new film review view model object to add
                FilmGenreListViewModel toAdd = new FilmGenreListViewModel();
        //set the review record and film record from the
        //ones matched in the loop


        toAdd.FilmgenreCredit = g;
                toAdd.Film = film;
                toAdd.Genre = genre;        //get the person recor (as actor)

                //add to the ActingList (lisyt of ViewModel objects)
                FilmgenreList.Add(toAdd);
            }
            //send the DIrectingListViewModel list to the view for display
            return View(FilmgenreList);
        }

        // GET: Genres/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // GET: Genres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GenreId,GenreName")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                db.Genres.Add(genre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(genre);
        }

        // GET: Genres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GenreId,GenreName")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genre);
        }

        // GET: Genres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Genre genre = db.Genres.Find(id);
            db.Genres.Remove(genre);
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
