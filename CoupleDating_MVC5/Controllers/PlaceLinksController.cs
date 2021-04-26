using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class PlaceLinksController : Controller
    {
        private Couples_Entities db = new Couples_Entities();

        // GET: PlaceLinks
        public ActionResult Index()
        {
            var placeLinks = db.PlaceLinks.Include(p => p.PlaceLinkType).Include(p => p.Place);
            return View(placeLinks.ToList());
        }

        // GET: PlaceLinks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceLinks placeLinks = db.PlaceLinks.Find(id);
            if (placeLinks == null)
            {
                return HttpNotFound();
            }
            return View(placeLinks);
        }

        // GET: PlaceLinks/Create
        public ActionResult Create()
        {
            ViewBag.PlaceLinkTypeID = new SelectList(db.PlaceLinkType, "PlaceLinkTypeID", "Description");
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name");
            return View();
        }

        // POST: PlaceLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlaceLinkID,PlaceLinkTypeID,LinkUrl,PlaceID")] PlaceLinks placeLinks)
        {
            if (ModelState.IsValid)
            {
                db.PlaceLinks.Add(placeLinks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlaceLinkTypeID = new SelectList(db.PlaceLinkType, "PlaceLinkTypeID", "Description", placeLinks.PlaceLinkTypeID);
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", placeLinks.PlaceID);
            return View(placeLinks);
        }

        // GET: PlaceLinks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceLinks placeLinks = db.PlaceLinks.Find(id);
            if (placeLinks == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlaceLinkTypeID = new SelectList(db.PlaceLinkType, "PlaceLinkTypeID", "Description", placeLinks.PlaceLinkTypeID);
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", placeLinks.PlaceID);
            return View(placeLinks);
        }

        // POST: PlaceLinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlaceLinkID,PlaceLinkTypeID,LinkUrl,PlaceID")] PlaceLinks placeLinks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(placeLinks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PlaceLinkTypeID = new SelectList(db.PlaceLinkType, "PlaceLinkTypeID", "Description", placeLinks.PlaceLinkTypeID);
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", placeLinks.PlaceID);
            return View(placeLinks);
        }

        // GET: PlaceLinks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceLinks placeLinks = db.PlaceLinks.Find(id);
            if (placeLinks == null)
            {
                return HttpNotFound();
            }
            return View(placeLinks);
        }

        // POST: PlaceLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlaceLinks placeLinks = db.PlaceLinks.Find(id);
            db.PlaceLinks.Remove(placeLinks);
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