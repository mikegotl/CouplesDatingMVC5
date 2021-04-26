using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class TagTypesController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: TagTypes
        public ActionResult Index()
        {
            return View(db.TagType.ToList());
        }

        // GET: TagTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagType tagType = db.TagType.Find(id);
            if (tagType == null)
            {
                return HttpNotFound();
            }
            return View(tagType);
        }

        // GET: TagTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tagTypeID,name")] TagType tagType)
        {
            if (ModelState.IsValid)
            {
                db.TagType.Add(tagType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tagType);
        }

        // GET: TagTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagType tagType = db.TagType.Find(id);
            if (tagType == null)
            {
                return HttpNotFound();
            }
            return View(tagType);
        }

        // POST: TagTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tagTypeID,name")] TagType tagType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tagType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tagType);
        }

        // GET: TagTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagType tagType = db.TagType.Find(id);
            if (tagType == null)
            {
                return HttpNotFound();
            }
            return View(tagType);
        }

        // POST: TagTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TagType tagType = db.TagType.Find(id);
            db.TagType.Remove(tagType);
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