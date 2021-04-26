using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class PlaceMediasController : Controller
    {
        private Couples_Entities db = new Couples_Entities();

        // GET: PlaceMedias
        public ActionResult Index()
        {
            var placeMedia = db.PlaceMedia.Include(p => p.Place).Include(p => p.tblMediaType);
            return View(placeMedia.ToList());
        }

        // GET: PlaceMedias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceMedia placeMedia = db.PlaceMedia.Find(id);
            if (placeMedia == null)
            {
                return HttpNotFound();
            }
            return View(placeMedia);
        }

        // GET: PlaceMedias/Create
        public ActionResult Create()
        {
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name");
            ViewBag.MediaTypeID = new SelectList(db.tblMediaType, "mediaTypeID", "name");
            return View();
        }

        // POST: PlaceMedias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlaceMediaID,MediaTypeID,Name,Description,PlaceID,path")] PlaceMedia placeMedia)
        {
            if (ModelState.IsValid)
            {
                db.PlaceMedia.Add(placeMedia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", placeMedia.PlaceID);
            ViewBag.MediaTypeID = new SelectList(db.tblMediaType, "mediaTypeID", "name", placeMedia.MediaTypeID);
            return View(placeMedia);
        }

        // GET: PlaceMedias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceMedia placeMedia = db.PlaceMedia.Find(id);
            if (placeMedia == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", placeMedia.PlaceID);
            ViewBag.MediaTypeID = new SelectList(db.tblMediaType, "mediaTypeID", "name", placeMedia.MediaTypeID);
            return View(placeMedia);
        }

        // POST: PlaceMedias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlaceMediaID,MediaTypeID,Name,Description,PlaceID,path")] PlaceMedia placeMedia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(placeMedia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", placeMedia.PlaceID);
            ViewBag.MediaTypeID = new SelectList(db.tblMediaType, "mediaTypeID", "name", placeMedia.MediaTypeID);
            return View(placeMedia);
        }

        // GET: PlaceMedias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceMedia placeMedia = db.PlaceMedia.Find(id);
            if (placeMedia == null)
            {
                return HttpNotFound();
            }
            return View(placeMedia);
        }

        // POST: PlaceMedias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlaceMedia placeMedia = db.PlaceMedia.Find(id);
            db.PlaceMedia.Remove(placeMedia);
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