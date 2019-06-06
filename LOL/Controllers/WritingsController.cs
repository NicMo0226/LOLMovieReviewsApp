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
    public class WritingsController : Controller
    {
        private DBContext db = new DBContext();
        // GET: Writings
        public ActionResult Index()
        { 
        //create a list for the view model to link Film and Person
            List<WritingListViewModel> WritingList =
                 new List<WritingListViewModel>();

            //seperate list for the directing credits to get the keys
            List<Writing> writingCredits;

            //populate the Directings list by selecting all records
            //from the db context
            writingCredits = db.Writings.ToList();

            //loop through each record t get the forwign keys
            //then populate the view model with the relevant 
            //Film / Person
            foreach (Writing w in writingCredits)
            {
                //match the ID between DIrecting and FIlm - store the single record in 'Film'
                Film film = db.Films.Where(x => x.FilmId == w.FilmId).Single();

                //match the ID between Directing and Film - -store the single record in 'Director'
                Person writer = db.Persons.Where(x => x.PersonId == w.PersonId).Single();

                //new ActingListViewModel object then add to the list
                WritingListViewModel toAdd = new WritingListViewModel();

                toAdd.WritingCredit = w; //get the directing credit recored
                toAdd.Film = film;   //get the film record
                toAdd.Writer = writer; //get the person record  (ad director)

                //add to the DirectingList (list of ViewModel objects)
                WritingList.Add(toAdd);
            }
            //send the DIrectingListViewModel list to the view for display
            return View(WritingList);
        }

        // GET: Writings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Writing writing = db.Writings.Find(id);
            if (writing == null)
            {
                return HttpNotFound();
            }
            return View(writing);
        }

        // GET: Writings/Create
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

        // POST: Writings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WritingId,PersonId,FilmId")] Writing writing)
        {
            if (ModelState.IsValid)
            {
                db.Writings.Add(writing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(writing);
        }

        // GET: Writings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Writing writing = db.Writings.Find(id);
            if (writing == null)
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
        ViewBag.FilmId = new SelectList(filmQuery, "FilmId", "FilmTitle", writing.FilmId);

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
        ViewBag.PersonId = new SelectList(personsQuery, "PersonId", "Name", writing.PersonId);

            return View(writing);
    }

    // POST: Actings/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "ActingId,PersonId,FilmId")] Writing writing)
    {
        if (ModelState.IsValid)
        {
            db.Entry(writing).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(writing);

    }



    // GET: Writings/Delete/5
    public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Writing writing = db.Writings.Find(id);
            if (writing == null)
            {
                return HttpNotFound();
            }
            return View(writing);
        }
        // POST: Writings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Writing writing = db.Writings.Find(id);
            db.Writings.Remove(writing);
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
