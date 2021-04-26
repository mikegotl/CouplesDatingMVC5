using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class PlaceLinkTypesController : Controller
    {
        private Couples_Entities db = new Couples_Entities();

        // GET: PlaceLinkTypes
        public ActionResult Index()
        {
            return View(db.PlaceLinkType.ToList());
        }

        // GET: PlaceLinkTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceLinkType placeLinkType = db.PlaceLinkType.Find(id);
            if (placeLinkType == null)
            {
                return HttpNotFound();
            }
            return View(placeLinkType);
        }

        // GET: PlaceLinkTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlaceLinkTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlaceLinkTypeID,Description")] PlaceLinkType placeLinkType)
        {
            if (ModelState.IsValid)
            {
                db.PlaceLinkType.Add(placeLinkType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(placeLinkType);
        }

        // GET: PlaceLinkTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceLinkType placeLinkType = db.PlaceLinkType.Find(id);
            if (placeLinkType == null)
            {
                return HttpNotFound();
            }
            return View(placeLinkType);
        }

        // POST: PlaceLinkTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlaceLinkTypeID,Description")] PlaceLinkType placeLinkType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(placeLinkType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(placeLinkType);
        }

        // GET: PlaceLinkTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceLinkType placeLinkType = db.PlaceLinkType.Find(id);
            if (placeLinkType == null)
            {
                return HttpNotFound();
            }
            return View(placeLinkType);
        }

        // POST: PlaceLinkTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlaceLinkType placeLinkType = db.PlaceLinkType.Find(id);
            db.PlaceLinkType.Remove(placeLinkType);
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