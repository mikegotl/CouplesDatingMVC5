using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class PlaceTypes1Controller : Controller
    {
        private Couples_Entities db = new Couples_Entities();

        // GET: PlaceTypes1
        public ActionResult Index()
        {
            var placeTypes = db.PlaceTypes.Include(p => p.Place).Include(p => p.PlaceType);
            return View(placeTypes.ToList());
        }

        // GET: PlaceTypes1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceTypes placeTypes = db.PlaceTypes.Find(id);
            if (placeTypes == null)
            {
                return HttpNotFound();
            }
            return View(placeTypes);
        }

        // GET: PlaceTypes1/Create
        public ActionResult Create()
        {
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name");
            ViewBag.PlaceTypeID = new SelectList(db.PlaceType, "PlaceTypeID", "Description");
            return View();
        }

        // POST: PlaceTypes1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlaceTypesID,PlaceID,PlaceTypeID")] PlaceTypes placeTypes)
        {
            if (ModelState.IsValid)
            {
                db.PlaceTypes.Add(placeTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", placeTypes.PlaceID);
            ViewBag.PlaceTypeID = new SelectList(db.PlaceType, "PlaceTypeID", "Description", placeTypes.PlaceTypeID);
            return View(placeTypes);
        }

        // GET: PlaceTypes1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceTypes placeTypes = db.PlaceTypes.Find(id);
            if (placeTypes == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", placeTypes.PlaceID);
            ViewBag.PlaceTypeID = new SelectList(db.PlaceType, "PlaceTypeID", "Description", placeTypes.PlaceTypeID);
            return View(placeTypes);
        }

        // POST: PlaceTypes1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlaceTypesID,PlaceID,PlaceTypeID")] PlaceTypes placeTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(placeTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", placeTypes.PlaceID);
            ViewBag.PlaceTypeID = new SelectList(db.PlaceType, "PlaceTypeID", "Description", placeTypes.PlaceTypeID);
            return View(placeTypes);
        }

        // GET: PlaceTypes1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceTypes placeTypes = db.PlaceTypes.Find(id);
            if (placeTypes == null)
            {
                return HttpNotFound();
            }
            return View(placeTypes);
        }

        // POST: PlaceTypes1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlaceTypes placeTypes = db.PlaceTypes.Find(id);
            db.PlaceTypes.Remove(placeTypes);
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