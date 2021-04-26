using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class MemberMediasController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: MemberMedias
        public ActionResult Index()
        {
            var MemberMedia = db.MemberMedia.Include(t => t.MediaType).Include(t => t.Member);
            return View(MemberMedia.ToList());
        }

        // GET: MemberMedias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberMedia MemberMedia = db.MemberMedia.Find(id);
            if (MemberMedia == null)
            {
                return HttpNotFound();
            }
            return View(MemberMedia);
        }

        // GET: MemberMedias/Create
        public ActionResult Create()
        {
            ViewBag.mediaTypeID = new SelectList(db.MediaType, "mediaTypeID", "name");
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1");
            return View();
        }

        // POST: MemberMedias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mediaID,ID,mediaTypeID,path")] MemberMedia MemberMedia)
        {
            if (ModelState.IsValid)
            {
                db.MemberMedia.Add(MemberMedia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.mediaTypeID = new SelectList(db.MediaType, "mediaTypeID", "name", MemberMedia.mediaTypeID);
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", MemberMedia.ID);
            return View(MemberMedia);
        }

        // GET: MemberMedias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberMedia MemberMedia = db.MemberMedia.Find(id);
            if (MemberMedia == null)
            {
                return HttpNotFound();
            }
            ViewBag.mediaTypeID = new SelectList(db.MediaType, "mediaTypeID", "name", MemberMedia.mediaTypeID);
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", MemberMedia.ID);
            return View(MemberMedia);
        }

        // POST: MemberMedias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mediaID,ID,mediaTypeID,path")] MemberMedia MemberMedia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(MemberMedia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.mediaTypeID = new SelectList(db.MediaType, "mediaTypeID", "name", MemberMedia.mediaTypeID);
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", MemberMedia.ID);
            return View(MemberMedia);
        }

        // GET: MemberMedias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberMedia MemberMedia = db.MemberMedia.Find(id);
            if (MemberMedia == null)
            {
                return HttpNotFound();
            }
            return View(MemberMedia);
        }

        // POST: MemberMedias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MemberMedia MemberMedia = db.MemberMedia.Find(id);
            db.MemberMedia.Remove(MemberMedia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteMyProfPic(int id)
        {
            MemberMedia mm = db.MemberMedia.Find(id);
            db.MemberMedia.Remove(mm);
            db.SaveChanges();

            return RedirectToAction("Edit", "Members");
        }

        public ActionResult AssignPrimary(int id)
        {
            MemberMedia mm = db.MemberMedia.Find(id);
            mm.primary = true;

            long ID = (long)mm.ID;

            List<MemberMedia> mms = db.MemberMedia.Where(m => m.ID == ID && m.ID != id).ToList<MemberMedia>();

            foreach (MemberMedia i in mms)
            {
                i.primary = false;
            }

            db.SaveChanges();
            return RedirectToAction("Edit", "Members");
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