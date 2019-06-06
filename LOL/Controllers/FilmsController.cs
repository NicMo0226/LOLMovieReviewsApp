using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LOL.Models;
using LOL.Models.ViewModels;
using PagedList;

namespace LOL.Controllers
{
    public class FilmsController : Controller
    {
        private DBContext db = new DBContext();

        //to be accessed via AJAX - autocomp[lete jQuery UI plugin
        public ActionResult Search(string term)
        {
            //select all the films in the db
            //and get the id and titel only 
            //id and label used for autocomplete functionality
            var films = from f in db.Films
                        select new
                        {
                            id = f.FilmId,
                            label = f.FilmTitle
                        };
            //now check the searchstring given for any matches in title
            films = films.Where(f => f.label.Contains(term));

            //convert to and return the JSON for the search UI
            return Json(films, JsonRequestBehavior.AllowGet);
        }

        // GET: Films
        public ActionResult Index(string sortOrder, string searchString,
            string currentFilter, int? page)
        {
            //for the viewBag to keep a note of current sort order
            ViewBag.CurrentSort = sortOrder;

            //add a new value to the viewbag to retain current sort order
            //check if the SortOrder param is empty - if so we''ll se the next choice 
            //to title_desc (order by tile descending) otherwise empty 
            //string lets us construct a toggle link for the alternative
            ViewBag.TitleSortParm =
                String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";

            //if there is a search string
            if (searchString != null)
            {
                //set page 1
                page = 1;
            }
            else
            {
                //if no search string, set to the current filter
                searchString = currentFilter;
            }
            //the current filter is nwow in the search string - note kept in view 
            ViewBag.CurrentFilter = searchString;


            //Select all the films in the db
            var films = from f in db.Films
                        select f;

            //check if the search string is not empty
            if (!String.IsNullOrEmpty(searchString))
            {
                //if we have a search term then select where the title contains it
                //analogous to LIKE %term% in SQL
                films = films.Where(f => f.FilmTitle.Contains(searchString));
            }
            //check the sortOrder param
            switch (sortOrder)
            {
                case "title_desc":
                    //order by title descending
                    films = films.OrderByDescending(f => f.FilmTitle);
                    break;
                default:
                    //order by title ascending
                    films = films.OrderBy(f => f.FilmTitle);
                    break;
            }
            //how many record per page (could also be a param....)
            int pageCount = 6;
            //if page is null set 1 otherwise keep page value
            int pageNumber = (page ?? 1);

            //send the updated films list to view
            return View(films.ToPagedList(pageNumber, pageCount ));
        }


        // GET: Films/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
           



            //new view model object
            FilmPageViewModel filmPage = new FilmPageViewModel();

            //get the current film and assign to view model
            filmPage.Film = film;
            //populate the Reviews list for the view model by matching all reviews
            //where the film id matches
            filmPage.Reviews = db.Reviews.Where(x => x.FilmId == film.FilmId).ToList();


            //for ACTORS, fiorst we need to getr all realted records in the join table
            IList<Acting> actorLinks = db.Actings.Where(x => x.FilmId == film.FilmId).ToList();

            //then we'll construct a list of the person records to match 
            IList<Person> actors = new List<Person>();
            //here we loop through the acting records in the join table
            foreach (Acting a in actorLinks)
            {
                //and add to the list of actors the matching person record for each
                actors.Add(db.Persons.Where(x => x.PersonId == a.PersonId).Single());
            }
            //once populated, we can assign the list of people as directors to the view model
            filmPage.Actors = actors;

            //for DIRECTORS, fiorst we need to getr all realted records in the join table
            IList<Directing> directorLinks = db.Directings.Where(x => x.FilmId == film.FilmId).ToList();
            //then we'll construct a list of the person records to match 
            IList<Person> directors = new List<Person>();
            //here we loop through the acting records in the join table
            foreach (Directing d in directorLinks)
            {
                //and add to the list of actors the matching person record for each
                directors.Add(db.Persons.Where(x => x.PersonId == d.PersonId).Single());
            }
            //once populated, we can assign the list of people as directors to the view model
            filmPage.Directors = directors;

            //for WRITERS, fiorst we need to getr all realted records in the join table
            IList<Writing> writerLinks = db.Writings.Where(x => x.FilmId == film.FilmId).ToList();
            //then we'll construct a list of the person records to match 
            IList<Person> writers = new List<Person>();
            //here we loop through the acting records in the join table
            foreach (Writing w in writerLinks)
            {
                //and add to the list of actors the matching person record for each
                writers.Add(db.Persons.Where(x => x.PersonId == w.PersonId).Single());
            }
            //once populated, we can assign the list of people as directors to the view model
            filmPage.Writers = writers;
            //here we will use a LinQ query to get the average review score for reviews 
            //related to the this film -  the additional ? symbols are if there is null result
            //if so we set to 0
            //get the current film and assign to view model

            
           


            var avg = db.Reviews.Where(x => x.FilmId == film.FilmId)
            .Average(x => (double?)x.ReviewRating) ?? 0;

            ViewBag.AverageReview = avg.ToString("0.00");

            //float test = 9.3456433f;

            //ViewBag.AverageReview = test.ToString("0.00");

            return View(filmPage);            
        }





        // GET: Films/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FilmId,FilmTitle,FilmSequel,FilmBBFC," +
            "FilmDesc,FilmReleaseDate,FilmTrailer,FilmImage")] Film film,
            HttpPostedFileBase upload)
        {
            //if we have valid data in the form
            if (ModelState.IsValid)
            {
                //check to see if the file has been uploaded
                if (upload != null && upload.ContentLength > 0)
                {
                    //check to see if valid MIME type (jpg / png or gif image)
                    if (upload.ContentType == "image/jpeg" ||
                        upload.ContentType == "image/jpg" ||
                        upload.ContentType == "image/gif" ||
                        upload.ContentType == "image/png")
                    {
                        //construct A PATH TO put the file in an Images subfolder in Content
                        string path = Path.Combine(Server.MapPath("~/Content/Images/"),
                            Path.GetFileName(upload.FileName));

                        //save the file to that path location
                        upload.SaveAs(path);

                        //store the relative path tro the image in the database
                        film.FilmImage = "~/Content/Images/" +
                            Path.GetFileName(upload.FileName);
                    }
                    else
                    {
                        //construct a message that can be displayed in tech view
                        ViewBag.Message = "Not valid image Format";
                    }
                }
                //add the film to the database and save
                db.Films.Add(film);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(film);
        }

        // GET: Films/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FilmId,FilmTitle,FilmSequel,FilmBBFC," +
            "FilmDesc,FilmReleaseDate,FilmTrailer,FilmImage")] Film film,
            HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                //check to see if the file has been uploaded
                if (upload != null && upload.ContentLength > 0)
                {
                    //check to see if valid MIME type (jpg / png or gif image)
                    if (upload.ContentType == "image/jpeg" ||
                        upload.ContentType == "image/jpg" ||
                        upload.ContentType == "image/gif" ||
                        upload.ContentType == "image/png")
                    {
                        //construct A PATH TO put the file in an Images subfolder in Content
                        string path = Path.Combine(Server.MapPath("~/Content/Images/"),
                            Path.GetFileName(upload.FileName));

                        //save the file to that path location
                        upload.SaveAs(path);

                        //store the relative path to the image in the database
                        film.FilmImage = "~/Content/Images/" +
                            Path.GetFileName(upload.FileName);
                    }
                    else
                    {
                        //construct a message that can be displayed in tech view
                        ViewBag.Message = "Not valid image format";
                    }
                }

                        db.Entry(film).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(film);
        }

        // GET: Films/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Film film = db.Films.Find(id);
            db.Films.Remove(film);
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
