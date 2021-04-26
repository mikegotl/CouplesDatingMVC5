using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class tblMediaTypesController : Controller
    {
        private Couples_Entities db = new Couples_Entities();

        // GET: tblMediaTypes
        public ActionResult Index()
        {
            return View(db.tblMediaType.ToList());
        }

        // GET: tblMediaTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMediaType tblMediaType = db.tblMediaType.Find(id);
            if (tblMediaType == null)
            {
                return HttpNotFound();
            }
            return View(tblMediaType);
        }

        // GET: tblMediaTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tblMediaTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mediaTypeID,name,description")] tblMediaType tblMediaType)
        {
            if (ModelState.IsValid)
            {
                db.tblMediaType.Add(tblMediaType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblMediaType);
        }

        // GET: tblMediaTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMediaType tblMediaType = db.tblMediaType.Find(id);
            if (tblMediaType == null)
            {
                return HttpNotFound();
            }
            return View(tblMediaType);
        }

        // POST: tblMediaTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mediaTypeID,name,description")] tblMediaType tblMediaType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblMediaType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblMediaType);
        }

        // GET: tblMediaTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMediaType tblMediaType = db.tblMediaType.Find(id);
            if (tblMediaType == null)
            {
                return HttpNotFound();
            }
            return View(tblMediaType);
        }

        // POST: tblMediaTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblMediaType tblMediaType = db.tblMediaType.Find(id);
            db.tblMediaType.Remove(tblMediaType);
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