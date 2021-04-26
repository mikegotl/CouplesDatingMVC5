using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class DoubleDateDetailsController : Controller
    {
        private Couples_Entities db = new Couples_Entities();

        // GET: DoubleDateDetails
        public ActionResult Index()
        {
            var doubleDateDetails = db.DoubleDateDetails.Include(d => d.DoubleDateRequest).Include(d => d.Place);
            return View(doubleDateDetails.ToList());
        }

        // GET: DoubleDateDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoubleDateDetails doubleDateDetails = db.DoubleDateDetails.Find(id);
            if (doubleDateDetails == null)
            {
                return HttpNotFound();
            }
            return View(doubleDateDetails);
        }

        // GET: DoubleDateDetails/Create
        public ActionResult Create()
        {
            ViewBag.doubleDateRequestID = new SelectList(db.DoubleDateRequest, "doubleDateRequestID", "memo");
            ViewBag.placeID = new SelectList(db.Place, "PlaceID", "Name");
            return View();
        }

        // POST: DoubleDateDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "doubleDateDetailsID,doubleDateRequestID,placeSequenceNbr,placeID,meetTime,externalPlaceName,externalPlaceAddress,memo,createDate")] DoubleDateDetails doubleDateDetails)
        {
            if (ModelState.IsValid)
            {
                db.DoubleDateDetails.Add(doubleDateDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.doubleDateRequestID = new SelectList(db.DoubleDateRequest, "doubleDateRequestID", "memo", doubleDateDetails.doubleDateRequestID);
            ViewBag.placeID = new SelectList(db.Place, "PlaceID", "Name", doubleDateDetails.placeID);
            return View(doubleDateDetails);
        }

        // GET: DoubleDateDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoubleDateDetails doubleDateDetails = db.DoubleDateDetails.Find(id);
            if (doubleDateDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.doubleDateRequestID = new SelectList(db.DoubleDateRequest, "doubleDateRequestID", "memo", doubleDateDetails.doubleDateRequestID);
            ViewBag.placeID = new SelectList(db.Place, "PlaceID", "Name", doubleDateDetails.placeID);
            return View(doubleDateDetails);
        }

        // POST: DoubleDateDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "doubleDateDetailsID,doubleDateRequestID,placeSequenceNbr,placeID,meetTime,externalPlaceName,externalPlaceAddress,memo,createDate")] DoubleDateDetails doubleDateDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doubleDateDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.doubleDateRequestID = new SelectList(db.DoubleDateRequest, "doubleDateRequestID", "memo", doubleDateDetails.doubleDateRequestID);
            ViewBag.placeID = new SelectList(db.Place, "PlaceID", "Name", doubleDateDetails.placeID);
            return View(doubleDateDetails);
        }

        // GET: DoubleDateDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoubleDateDetails doubleDateDetails = db.DoubleDateDetails.Find(id);
            if (doubleDateDetails == null)
            {
                return HttpNotFound();
            }
            return View(doubleDateDetails);
        }

        // POST: DoubleDateDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoubleDateDetails doubleDateDetails = db.DoubleDateDetails.Find(id);
            db.DoubleDateDetails.Remove(doubleDateDetails);
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