using CoupleDating_MVC5.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Controllers
{
    public class FavoritesController : Controller
    {
        private long? _routeID;

        public long? RouteID
        {
            get
            {
                if (_routeID == null || _routeID == 0)
                {
                    _routeID = (long)Convert.ToInt16(Url.RequestContext.RouteData.Values["id"]);
                }

                return _routeID;
            }
            set { _routeID = value; }
        }

        private DBEntities db = new DBEntities();
        private Config config = new Config();

        [Authorize]
        public ActionResult MyFavoriteCouples()
        {
            var myfavs = db.Favorites.Where(f => f.ID == config.LoggedInMember.ID);

            return View(myfavs.ToList());
        }

        // GET: Favorites

        [Authorize]
        public ActionResult Index()
        {
            var favorites = db.Favorites.Include(f => f.Member).Include(f => f.Member1);
            return View(favorites.ToList());
        }

        // GET: Favorites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favorites favorites = db.Favorites.Find(id);
            if (favorites == null)
            {
                return HttpNotFound();
            }
            return View(favorites);
        }

        // GET: Favorites/Create
        public ActionResult Create()
        {
            ViewBag.favoriteID = new SelectList(db.Member, "ID", "FirstName1");
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1");
            return View();
        }

        // POST: Favorites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "favoriteID,favoriteID,ID,DateCreated,enabled")] Favorites favorites)
        {
            if (ModelState.IsValid)
            {
                db.Favorites.Add(favorites);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.favoriteID = new SelectList(db.Member, "ID", "FirstName1", favorites.ID);
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", favorites.ID);
            return View(favorites);
        }

        // GET: Favorites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favorites favorites = db.Favorites.Find(id);
            if (favorites == null)
            {
                return HttpNotFound();
            }
            ViewBag.favoriteID = new SelectList(db.Member, "ID", "FirstName1", favorites.ID);
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", favorites.ID);
            return View(favorites);
        }

        // POST: Favorites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "favoriteID,favoriteID,ID,DateCreated,enabled")] Favorites favorites)
        {
            if (ModelState.IsValid)
            {
                db.Entry(favorites).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.favoriteID = new SelectList(db.Member, "ID", "FirstName1", favorites.ID);
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", favorites.ID);
            return View(favorites);
        }

        // GET: Favorites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favorites favorites = db.Favorites.Find(id);
            if (favorites == null)
            {
                return HttpNotFound();
            }
            return View(favorites);
        }

        // POST: Favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Favorites favorites = db.Favorites.Find(id);
            db.Favorites.Remove(favorites);
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

        public ActionResult Add(int favoriteMemberID)
        {
            Favorites f = new Favorites();
            f.FavoriteMemberID = favoriteMemberID;
            f.DateCreated = DateTime.Now;
            f.MemberID = (int)config.LoggedInMember.ID;
            f.Enabled = true;

            db.Favorites.Add(f);
            db.SaveChanges();

            return RedirectToAction("Details", "Members", new { id = (long)favoriteMemberID });
        }

        public ActionResult Remove(int favoriteMemberID)
        {
            Favorites foundF = db.Favorites.Where(f => f.FavoriteMemberID == favoriteMemberID && f.MemberID == config.LoggedInMember.ID).FirstOrDefault();

            if (foundF != null)
            {
                db.Favorites.Remove(foundF);
                db.SaveChanges();
            }

            return RedirectToAction("Details", "Members", new { id = (long)favoriteMemberID });
        }

        public ActionResult DeleteMyFav(int id)
        {
            Favorites f = db.Favorites.Where(m => m.ID == id).FirstOrDefault();
            db.Favorites.Remove(f);
            db.SaveChanges();
            return RedirectToAction("MyFavoriteCouples");
        }

        //public ActionResult AddFavorite()
        //{
        //    Favorites f = db.Favorites.Where(m=>m.favoriteID == (long)RouteID && m.ID == (long)Config.LoggedInMember.ID).FirstOrDefault();

        //    return PartialView("_Favorite", f);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddFavorite([Bind(Include = "favoriteID,favoriteID,ID,DateCreated,enabled")] Favorites favorites)
        //{
        //    Favorites existingF = new Favorites();
        //    existingF = db.Favorites.Where(m => m.favoriteID == (long)RouteID && m.ID == (long)Config.LoggedInMember.ID).FirstOrDefault();

        //    if (existingF != null)
        //    {
        //        //exists - disable if enabled and vice versa
        //        if (existingF.enabled == true)
        //        {
        //            existingF.enabled = false;
        //        }
        //        else {
        //            existingF.enabled = true;
        //        }
        //        db.SaveChanges();
        //        return RedirectToAction("Details", "Members", new { id = (long)RouteID });
        //    }
        //    else {
        //        //does not exist - add
        //        Favorites f = new Favorites();
        //        f.ID = (long)Config.LoggedInMember.ID;
        //        f.favoriteID = (long)RouteID;
        //        f.DateCreated = DateTime.Now;
        //        f.enabled = true;

        //        db.Favorites.Add(f);
        //        db.SaveChanges();
        //        return RedirectToAction("Details", "Members", new { id = (long)RouteID });
        //    }
        //}
    }
}