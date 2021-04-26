using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class PlaceTagsController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: PlaceTags
        public ActionResult Index()
        {
            var placeTags = db.PlaceTags.Include(p => p.Tag).Include(p => p.Place);
            return View(placeTags.ToList());
        }

        // GET: PlaceTags/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceTags placeTags = db.PlaceTags.Find(id);
            if (placeTags == null)
            {
                return HttpNotFound();
            }
            return View(placeTags);
        }

        // GET: PlaceTags/Create
        public ActionResult Create()
        {
            ViewBag.TagID = new SelectList(db.Tag, "tagID", "name");
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name");
            return View();
        }

        // POST: PlaceTags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlaceTagID,PlaceID,TagID")] PlaceTags placeTags)
        {
            if (ModelState.IsValid)
            {
                db.PlaceTags.Add(placeTags);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TagID = new SelectList(db.Tag, "tagID", "name", placeTags.TagID);
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", placeTags.PlaceID);
            return View(placeTags);
        }

        // GET: PlaceTags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceTags placeTags = db.PlaceTags.Find(id);
            if (placeTags == null)
            {
                return HttpNotFound();
            }
            ViewBag.TagID = new SelectList(db.Tag, "tagID", "name", placeTags.TagID);
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", placeTags.PlaceID);
            return View(placeTags);
        }

        // POST: PlaceTags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlaceTagID,PlaceID,TagID")] PlaceTags placeTags)
        {
            if (ModelState.IsValid)
            {
                db.Entry(placeTags).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TagID = new SelectList(db.Tag, "tagID", "name", placeTags.TagID);
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", placeTags.PlaceID);
            return View(placeTags);
        }

        // GET: PlaceTags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceTags placeTags = db.PlaceTags.Find(id);
            if (placeTags == null)
            {
                return HttpNotFound();
            }
            return View(placeTags);
        }

        // POST: PlaceTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlaceTags placeTags = db.PlaceTags.Find(id);
            db.PlaceTags.Remove(placeTags);
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