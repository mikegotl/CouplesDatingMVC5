using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class PlaceTypesController : Controller
    {
        private Couples_Entities db = new Couples_Entities();

        // GET: PlaceTypes
        public ActionResult Index()
        {
            return View(db.PlaceType.ToList());
        }

        // GET: PlaceTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceType placeType = db.PlaceType.Find(id);
            if (placeType == null)
            {
                return HttpNotFound();
            }
            return View(placeType);
        }

        // GET: PlaceTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlaceTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlaceTypeID,Description")] PlaceType placeType)
        {
            if (ModelState.IsValid)
            {
                db.PlaceType.Add(placeType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(placeType);
        }

        // GET: PlaceTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceType placeType = db.PlaceType.Find(id);
            if (placeType == null)
            {
                return HttpNotFound();
            }
            return View(placeType);
        }

        // POST: PlaceTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlaceTypeID,Description")] PlaceType placeType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(placeType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(placeType);
        }

        // GET: PlaceTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceType placeType = db.PlaceType.Find(id);
            if (placeType == null)
            {
                return HttpNotFound();
            }
            return View(placeType);
        }

        // POST: PlaceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlaceType placeType = db.PlaceType.Find(id);
            db.PlaceType.Remove(placeType);
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