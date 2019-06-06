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
    public class PersonsController : Controller
    {
        private DBContext db = new DBContext();

  

//to be accessed via AJAX - autocomp[lete jQuery UI plugin
        public ActionResult Search(string term)
        {
            //select all the films in the db
            //and get the id and titel only 
            //id and label used for autocomplete functionality
            var persons = from p in db.Persons
                          select new
                          {
                              id = p.PersonId,
                              label = p.PersonFname + " " + p.PersonSname

                          };

            //now check the searchstring given for any matches in title
            persons = persons.Where(p => p.label.Contains(term));

            //convert to and return the JSON for the search UI
            return Json(persons, JsonRequestBehavior.AllowGet);
        }

        // GET: persons
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
            var persons = from p in db.Persons
                          select p;

            //check if the search string is not empty
            if (!String.IsNullOrEmpty(searchString))
            {
                //if we have a search term then select where the title contains it
                //analogous to LIKE %term% in SQL
                persons = persons.Where(p => "label".Contains(searchString));
            }
            //check the sortOrder param
            switch (sortOrder)
            {
                case "name_desc":
                    //order by title descending
                    persons = persons.OrderByDescending(p => p.PersonSname);
                    break;
                default:
                    //order by title ascending
                    persons = persons.OrderBy(p => p.PersonSname);
                    break;
            }
            //how many record per page (could also be a param....)
            int pageCount = 8;
            //if page is null set 1 otherwise keep page value
            int pageNumber = (page ?? 1);

            //send the updated films list to view
            return View(persons.ToPagedList(pageNumber, pageCount));
        }
        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonId,PersonFname,PersonSname," +
            "DateOfBirth,PersonDesc,PersonImage")] Person person,
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
                        person.PersonImage = "~/Content/Images/" +
                            Path.GetFileName(upload.FileName);
                    }
                    else
                    {
                        //construct a message that can be displayed in tech view
                        ViewBag.Message = "Not valid image Format";
                    }
                }
                //add the person to the database and save
                db.Persons.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonId,PersonFname,PersonSname,DateOfBirth," +
            "PersonDesc,PersonImage")] Person person,
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
                        person.PersonImage = "~/Content/Images/" +
                            Path.GetFileName(upload.FileName);
                    }
                    else
                    {
                        //construct a message that can be displayed in tech view
                        ViewBag.Message = "Not valid image format";
                    }
                }
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
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
