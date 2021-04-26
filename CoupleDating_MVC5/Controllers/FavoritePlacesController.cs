using CoupleDating_MVC5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Controllers
{
    [RoutePrefix("FavoritePlaces")]
    public class FavoritePlacesController : Controller
    {
        private DBEntities db = new DBEntities();
        private Config config = new Config();

        // GET: FavoritePlaces
        [Authorize]
        //[OutputCache(CacheProfile = "Client1Hour")]
        public ActionResult MyFavoritePlaces()
        {
            List<FavoritePlaces> myfavoritePlaces = db.FavoritePlaces.Where(f => f.ID == config.LoggedInMember.ID).ToList<FavoritePlaces>();
            return GetFavGPlaces(myfavoritePlaces);
        }

        private ActionResult GetFavGPlaces(List<FavoritePlaces> myfavoritePlaces)
        {
            GoogleAPIs gapi = new GoogleAPIs();
            List<GPlace> gPlaces = new List<GPlace>();
            bool favsAreEqual = true;

            //if (Session["myfavoritePlaces"] != null)
            //{
            //    List<FavoritePlaces> sMyFavoritePlaces = (List<FavoritePlaces>)Session["myfavoritePlaces"];

            //    if (sMyFavoritePlaces != null)
            //    {
            //        foreach (FavoritePlaces f_db in myfavoritePlaces)
            //        {
            //            var found = sMyFavoritePlaces.Where(m => m.GPlace_Id == f_db.GPlace_Id);
            //            if (found == null)
            //            {
            //                favsAreEqual = false;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    favsAreEqual = false;
            //}

            favsAreEqual = false;

            if (Session["gPlaces"] != null && Session["filteredModel"] != null && favsAreEqual)
            {
                //return data to view
                ViewBag.gPlaces = (List<GPlace>)Session["gPlaces"];
                return View((List<FavoritePlaces>)Session["filteredModel"]);
            }
            else
            {
                //for each Favorite Place, get Place info from Google API and add to gPlaces list.
                foreach (FavoritePlaces fp in myfavoritePlaces)
                {
                    GPlace gp = new GPlace();
                    if (fp.GPlace_Id != null && fp.GPlace_Id != "0")
                    {
                        gp = gapi.GetPlaceDetails(fp.GPlace_Id);
                        gPlaces.Add(gp);
                    }
                }

                //match gPlaces from API call returns to FavoritePlaces data from db
                List<FavoritePlaces> filteredModel = new List<FavoritePlaces>();
                foreach (GPlace gp in gPlaces)
                {
                    if (myfavoritePlaces.Where(g => g.GPlace_Id == gp.place_id).Count() > 0)
                    {
                        filteredModel.Add(myfavoritePlaces.Where(g => g.GPlace_Id == gp.place_id).FirstOrDefault());
                    }
                }

                //store session objects
                //Session["gPlaces"] = gPlaces;
                //Session["filteredModel"] = filteredModel;
                //Session["myfavoritePlaces"] = myfavoritePlaces;

                //return data to view
                ViewBag.gPlaces = gPlaces;
                return View(filteredModel);
            }
        }

        public ActionResult Index()
        {
            //***this action is not used***
            var favoritePlaces = db.FavoritePlaces.Include(f => f.Place).Include(f => f.Member);
            return View(favoritePlaces.ToList());
        }

        // GET: FavoritePlaces/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FavoritePlaces favoritePlaces = db.FavoritePlaces.Find(id);
            if (favoritePlaces == null)
            {
                return HttpNotFound();
            }
            return View(favoritePlaces);
        }

        // GET: FavoritePlaces/Create
        public ActionResult Create()
        {
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name");
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1");
            return View();
        }

        // POST: FavoritePlaces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FavoritePlaceID,PlaceID,ID,CreateDate,Enabled")] FavoritePlaces favoritePlaces)
        {
            if (ModelState.IsValid)
            {
                db.FavoritePlaces.Add(favoritePlaces);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", favoritePlaces.PlaceID);
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", favoritePlaces.ID);
            return View(favoritePlaces);
        }

        public ActionResult Add(string PlaceID)
        {
            FavoritePlaces fp = new FavoritePlaces();
            //fp.PlaceID = PlaceID;
            fp.ID = (int)config.LoggedInMember.ID;
            fp.DateCreated= DateTime.Now;
            fp.Enabled = true;
            fp.GPlace_Id = PlaceID;

            db.FavoritePlaces.Add(fp);
            db.SaveChanges();

            return RedirectToAction("Details", "Places", new { id = PlaceID });
        }

        //public ActionResult Remove(int PlaceID, long ID)
        //{
        //    FavoritePlaces fpRemove = db.FavoritePlaces.Where(f => f.ID == ID && f.PlaceID == PlaceID).FirstOrDefault();

        //    if (fpRemove != null)
        //    {
        //        db.FavoritePlaces.Remove(fpRemove);
        //        db.SaveChanges();
        //    }

        //    return RedirectToAction("Details", "Places", new { id = (long)PlaceID });
        //}

        public ActionResult Remove(string PlaceID, long ID)
        {
            FavoritePlaces fpRemove = db.FavoritePlaces.Where(f => f.ID == ID && f.GPlace_Id == PlaceID).FirstOrDefault();

            if (fpRemove != null)
            {
                db.FavoritePlaces.Remove(fpRemove);
                db.SaveChanges();
            }

            return RedirectToAction("Details", "Places", new { id = PlaceID });
        }

        // GET: FavoritePlaces/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FavoritePlaces favoritePlaces = db.FavoritePlaces.Find(id);
            if (favoritePlaces == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", favoritePlaces.PlaceID);
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", favoritePlaces.ID);
            return View(favoritePlaces);
        }

        // POST: FavoritePlaces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FavoritePlaceID,PlaceID,ID,CreateDate,Enabled")] FavoritePlaces favoritePlaces)
        {
            if (ModelState.IsValid)
            {
                db.Entry(favoritePlaces).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PlaceID = new SelectList(db.Place, "PlaceID", "Name", favoritePlaces.PlaceID);
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", favoritePlaces.ID);
            return View(favoritePlaces);
        }

        // GET: FavoritePlaces/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FavoritePlaces favoritePlaces = db.FavoritePlaces.Find(id);
            if (favoritePlaces == null)
            {
                return HttpNotFound();
            }
            return View(favoritePlaces);
        }

        // POST: FavoritePlaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FavoritePlaces favoritePlaces = db.FavoritePlaces.Find(id);
            db.FavoritePlaces.Remove(favoritePlaces);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteMyFav(int id)
        {
            FavoritePlaces favoritePlaces = db.FavoritePlaces.Find(id);

            if (favoritePlaces != null)
            {
                db.FavoritePlaces.Remove(favoritePlaces);
                db.SaveChanges();
            }

            return RedirectToAction("MyFavoritePlaces");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public class FavoritePlaceMedia
        {
            public string GPlace_Id;
            public string path;
        }
    }
}