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
    public class DirectingsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Directings
        public ActionResult Index()
        {
            //create a list for the view model to link Film and Person
            List<DirectingListViewModel> DirectingList =
                 new List<DirectingListViewModel>();

            //seperate list for the directing credits to get the keys
            List<Directing> directingCredits;

            //populate the Directings list by selecting all records
            //from the db context
            directingCredits = db.Directings.ToList();

            //loop through each record t get the forwign keys
            //then populate the view model with the relevant 
            //Film / Person
            foreach (Directing d in directingCredits)
            {
                //match the ID between DIrecting and FIlm - store the single record in 'Film'
                Film film = db.Films.Where(x => x.FilmId == d.FilmId).Single();

                //match the ID between Directing and Film - -store the single record in 'Director'
                Person director = db.Persons.Where(x => x.PersonId == d.PersonId).Single();

                //new ActingListViewModel object then add to the list
                DirectingListViewModel toAdd = new DirectingListViewModel();

                toAdd.DirectingCredit = d; //get the directing credit recored
                toAdd.Film = film;   //get the film record
                toAdd.Director = director; //get the person record  (ad director)

                //add to the DirectingList (list of ViewModel objects)
                DirectingList.Add(toAdd);
            }
            //send the DIrectingListViewModel list to the view for display
            return View(DirectingList);
        }

        // GET: Directings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directing directing = db.Directings.Find(id);
            if (directing == null)
            {
                return HttpNotFound();
            }
            return View(directing);
        }

        // GET: Directings/Create
        public ActionResult Create(int FilmId = 0, int PersonId = 0)
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
            var personsQuery = from p in db.Persons
                               orderby p.PersonSname
                               select new
                               {
                                   Name = p.PersonFname + " " + p.PersonSname,
                                   p.PersonId

                               };
            //if no id set
            if (PersonId == 0)
                //construct full films dropdown list without preselection 
                //do so from the query results and display the NAme (combined above)
                //store in FilmId in the viewBag
                ViewBag.PersonId = new SelectList(personsQuery, "PersonId", "Name", null);
            else
                ViewBag.PersonId = new SelectList(personsQuery, "PersonId", "Name", PersonId);

            //generte the view                                      
            return View();
        }

        // POST: Directings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DirectingId,PersonId,FilmId")] Directing directing)
        {
            if (ModelState.IsValid)
            {
                db.Directings.Add(directing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(directing);
        }

        // GET: Directings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directing directing = db.Directings.Find(id);
            if (directing == null)
            {
                return HttpNotFound();
            }
            //code to generate dropdowns 
            //FILMs --------------------------------------------------------------------------------------------
            //from the Films model Dbset
            //Select all columns from the databse 
            //orderby the film title 
            var filmQuery = from m in db.Films
                            orderby m.FilmTitle
                            select m;
            //construct full films dropdown list preselected with the foregn key
            //do so from the query results and display the FilmTiltle
            //store in FilmID in the ViewBag
            ViewBag.FilmId = new SelectList(filmQuery, "FilmId", "FilmTitle", directing.FilmId);

            //PERSONS-------------------------------------------------------------------------------------------
            //from the Persons Model DbSet
            //select the fname and sname as a new field called Name
            //and the peron id - order by sname
            var personsQuery = from p in db.Persons
                               orderby p.PersonSname
                               select new
                               {
                                   Name = p.PersonFname + " " + p.PersonSname,
                                   p.PersonId

                               };
            //construct full films dropdown list preseklected with the foregn key
            //do so from the query results and display the Name (combined above) 
            //store in FilmId in the ViewBag
            ViewBag.PersonId = new SelectList(personsQuery, "PersonId", "Name", directing.PersonId);

            return View(directing);
        }
          
        // POST: Directings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DirectingId,PersonId,FilmId")] Directing directing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(directing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(directing);
        }

        // GET: Directings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directing directing = db.Directings.Find(id);
            if (directing == null)
            {
                return HttpNotFound();
            }
            return View(directing);
        }
        // POST: Directings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Directing directing = db.Directings.Find(id);
            db.Directings.Remove(directing);
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
