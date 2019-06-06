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
using PagedList;

namespace LOL.Controllers
{
    public class CharactersController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Characters
        //to be accessed via AJAX - autocomp[lete jQuery UI plugin
        public ActionResult Search(string term)
        {
            //select all the films in the db
            //and get the id and titel only 
            //id and label used for autocomplete functionality
            var characters = from c in db.Characters
                          select new
                          {
                              id = c.CharacterId,
                              label = c.CharacterFname + " " + c.CharacterSname

                          };

            //now check the searchstring given for any matches in title
            characters = characters.Where(p => p.label.Contains(term));

            //convert to and return the JSON for the search UI
            return Json(characters, JsonRequestBehavior.AllowGet);
        }

        // GET: characters
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
                String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

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
            var characters = from c in db.Characters
                             select c;

            //check if the search string is not empty
            if (!String.IsNullOrEmpty(searchString))
            {
                //if we have a search term then select where the title contains it
                //analogous to LIKE %term% in SQL
                characters = characters.Where(c => "label".Contains(searchString));
            }
            //check the sortOrder param
            switch (sortOrder)
            {
                case "name_desc":
                    //order by title descending
                    characters = characters.OrderByDescending(c => c.CharacterSname);
                    break;
                default:
                    //order by title ascending
                    characters = characters.OrderBy(c => c.CharacterSname);
                    break;
            }
            //how many record per page (could also be a param....)
            int pageCount = 8;
            //if page is null set 1 otherwise keep page value
            int pageNumber = (page ?? 1);

            //send the updated films list to view
            return View(characters.ToPagedList(pageNumber, pageCount));
        }
        // GET: Characters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // GET: Characters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Characters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CharacterId,CharacterFname,CharacterSname,CharacterImage," +
            "CharacterDate,CharacterQuote")] Character character)
        {
            if (ModelState.IsValid)
            {
               
                //add the person to the database and save
                db.Characters.Add(character);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(character);
        }

        // GET: Characters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // POST: Characters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CharacterId,CharacterFname,CharacterSname," +
            "CharacterImage,CharacterDate,CharacterQuote")] Character character)
        {
            if (ModelState.IsValid)
            {
               db.Entry(character).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(character);
        }

        // GET: Characters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // POST: Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Character character = db.Characters.Find(id);
            db.Characters.Remove(character);
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
