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
    public class ReviewsController : Controller
    {
        private DBContext db = new DBContext();
        // GET: Reviews
        public ActionResult Index()
        {
            //list of film reviews model objects to link
            //reviews with related films
            List<FilmReviewViewModel> FilmReviewList =
                new List<FilmReviewViewModel>();
            //list of review objects to cycle through and map id's 
            List<Review> Reviews;
            //populate the list with the review records from the database
            Reviews = db.Reviews.ToList();

            //loop through each review in the list of ata (each row
            foreach (Review r in Reviews)
            {

                //select the film record where the ids amtch
                Film film = db.Films.Where(x => x.FilmId == r.FilmId).Single();

                //create a new film review view model object to add
                FilmReviewViewModel toAdd = new FilmReviewViewModel();
                //set the review record and film record from the
                //ones matched in the loop


                toAdd.Review = r;
                toAdd.Film = film;
                FilmReviewList.Add(toAdd);
            }
            return View(FilmReviewList);
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            //find the related film
            Film film = db.Films.Where(x => x.FilmId == review.FilmId).Single();
            //create a new view model objetc and assign thee reciview and film details
            FilmReviewViewModel FilmReview = new FilmReviewViewModel();
            FilmReview.Review = review;
            FilmReview.Film = film;

            //send the view model to the view
            return View(FilmReview);

        }


          
        // GET: Reviews/Create
        public ActionResult Create(int id = 0)
        {
            //if no id sent, redirect to review index
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            //otherwise, select the film the id matches
            Film film = db.Films.Where(x => x.FilmId == id).Single();

            //then populate these values in the viewbag
            ViewBag.FilmId = id;
            ViewBag.FilmTitle = film.FilmTitle;

            //return View();

            return View("Create","_ModalLayout");
        }


        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReviewId,FilmId,ReviewUname,ReviewContent,ReviewRating")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(review);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            //if no id sent, redirect to review index
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }

            //otherwise, select the film the id matches
            Film film = db.Films.Where(x => x.FilmId == review.FilmId).Single();

            //then populate these values in the viewbag
            ViewBag.FilmId = film.FilmId;
            ViewBag.FilmTitle = film.FilmTitle;

            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReviewId,FilmId,ReviewUname,ReviewContent,ReviewRating")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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
