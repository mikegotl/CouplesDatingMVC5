using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class DoubleDateMediaUploadsController : Controller
    {
        private Couples_Entities db = new Couples_Entities();

        // GET: DoubleDateMediaUploads
        public ActionResult Index()
        {
            var doubleDateMediaUploads = db.DoubleDateMediaUploads.Include(d => d.DoubleDateRequest).Include(d => d.Place).Include(d => d.tblMember);
            return View(doubleDateMediaUploads.ToList());
        }

        // GET: DoubleDateMediaUploads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoubleDateMediaUploads doubleDateMediaUploads = db.DoubleDateMediaUploads.Find(id);
            if (doubleDateMediaUploads == null)
            {
                return HttpNotFound();
            }
            return View(doubleDateMediaUploads);
        }

        // GET: DoubleDateMediaUploads/Create
        public ActionResult Create()
        {
            ViewBag.doubleDateRequestID = new SelectList(db.DoubleDateRequest, "doubleDateRequestID", "memo");
            ViewBag.placeID = new SelectList(db.Place, "PlaceID", "Name");
            ViewBag.memberID = new SelectList(db.tblMember, "memberId", "FirstName1");
            return View();
        }

        // POST: DoubleDateMediaUploads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ddMediaUploadID,Name,Memo,Path,doubleDateRequestID,placeID,memberID")] DoubleDateMediaUploads doubleDateMediaUploads)
        {
            if (ModelState.IsValid)
            {
                db.DoubleDateMediaUploads.Add(doubleDateMediaUploads);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.doubleDateRequestID = new SelectList(db.DoubleDateRequest, "doubleDateRequestID", "memo", doubleDateMediaUploads.doubleDateRequestID);
            ViewBag.placeID = new SelectList(db.Place, "PlaceID", "Name", doubleDateMediaUploads.placeID);
            ViewBag.memberID = new SelectList(db.tblMember, "memberId", "FirstName1", doubleDateMediaUploads.memberID);
            return View(doubleDateMediaUploads);
        }

        // GET: DoubleDateMediaUploads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoubleDateMediaUploads doubleDateMediaUploads = db.DoubleDateMediaUploads.Find(id);
            if (doubleDateMediaUploads == null)
            {
                return HttpNotFound();
            }
            ViewBag.doubleDateRequestID = new SelectList(db.DoubleDateRequest, "doubleDateRequestID", "memo", doubleDateMediaUploads.doubleDateRequestID);
            ViewBag.placeID = new SelectList(db.Place, "PlaceID", "Name", doubleDateMediaUploads.placeID);
            ViewBag.memberID = new SelectList(db.tblMember, "memberId", "FirstName1", doubleDateMediaUploads.memberID);
            return View(doubleDateMediaUploads);
        }

        // POST: DoubleDateMediaUploads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ddMediaUploadID,Name,Memo,Path,doubleDateRequestID,placeID,memberID")] DoubleDateMediaUploads doubleDateMediaUploads)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doubleDateMediaUploads).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.doubleDateRequestID = new SelectList(db.DoubleDateRequest, "doubleDateRequestID", "memo", doubleDateMediaUploads.doubleDateRequestID);
            ViewBag.placeID = new SelectList(db.Place, "PlaceID", "Name", doubleDateMediaUploads.placeID);
            ViewBag.memberID = new SelectList(db.tblMember, "memberId", "FirstName1", doubleDateMediaUploads.memberID);
            return View(doubleDateMediaUploads);
        }

        // GET: DoubleDateMediaUploads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoubleDateMediaUploads doubleDateMediaUploads = db.DoubleDateMediaUploads.Find(id);
            if (doubleDateMediaUploads == null)
            {
                return HttpNotFound();
            }
            return View(doubleDateMediaUploads);
        }

        // POST: DoubleDateMediaUploads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoubleDateMediaUploads doubleDateMediaUploads = db.DoubleDateMediaUploads.Find(id);
            db.DoubleDateMediaUploads.Remove(doubleDateMediaUploads);
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