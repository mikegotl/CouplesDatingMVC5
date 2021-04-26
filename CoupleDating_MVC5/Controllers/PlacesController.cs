using CoupleDating_MVC5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Controllers
{
    public class PlacesController : Controller
    {
        private DBEntities db = new DBEntities();
        private Config config = new Config();

        // GET: Places
        public ActionResult Index()
        {
            return View(db.Place.ToList());
        }

        // GET: Places/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Place place = db.Place.Find(id);
        //    if (place == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(place);
        //}

        public ActionResult Details(string id)
        {
            if (config.UID == null)
            {
                //user is not logged in
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                GPlace gPlace = new GPlace();
                GoogleAPIs gapi = new GoogleAPIs();

                //hydrate gPlace
                gPlace = gapi.GetPlaceDetails(id);

                Place place = new Place();
                place.GPlace_Id = gPlace.place_id;
                place.Name = gPlace.name;
                place.Address = gPlace.formattedAddress;
                place.Description = gPlace.website_url;

                //photos
                foreach (GPlacePhotoUrls gPlPhotoUrl in gPlace.photoUrls)
                {
                    PlaceMedia pMedia = new PlaceMedia();

                    pMedia.path = gPlPhotoUrl.photoUrl.ToString();

                    place.PlaceMedia.Add(pMedia);
                }

                return View(place);
            }
        }

        // GET: Places/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Places/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlaceID,Name,Description,Blurb,City,State,Zip,Address,ContactNumber")] Place place)
        {
            if (ModelState.IsValid)
            {
                db.Place.Add(place);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(place);
        }

        // GET: Places/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Place.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // POST: Places/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlaceID,Name,Description,Blurb,City,State,Zip,Address,ContactNumber")] Place place)
        {
            if (ModelState.IsValid)
            {
                db.Entry(place).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(place);
        }

        // GET: Places/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Place.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // POST: Places/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Place place = db.Place.Find(id);
            db.Place.Remove(place);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        [OutputCache(Duration = 86400, VaryByParam = "city;photoCat")]
        public PartialViewResult ListPlaces(string city, string photoCat)
        {
            GoogleAPIs gapi = new GoogleAPIs();
            string location = city;
            string strQuery;
            int limit = 20;

            if (String.IsNullOrEmpty(photoCat))
            {
                strQuery = "nightlife+in+" + location;
            }
            else
            {
                strQuery = photoCat + "+in+" + location;
                ViewBag.photoCat = photoCat;
            }

            //Fresh load from google places api
            //List<GPlace> places = await gapi.GetPlaces(strQuery, limit);
            //ViewBag.places = places;

            //ViewBag.location = location;
            //ViewBag.lastUpdated = DateTime.Now.ToString("T");

            return PartialView("_TopPlacesByCityCategory");
        }

        [HttpGet]
        public List<GPlace> GetGPlaces(string city)
        {
            GoogleAPIs gapi = new GoogleAPIs();
            string location = city;
            string strQuery = "nightlife+in+" + location;
            int limit = 20;

            List<GPlace> places = gapi.GetPlaceList(strQuery, limit);
            return places;
        }

        [HttpGet]
        public JsonResult GetGPlacesJson(string city, string query, int limit = 20)
        {
            GoogleAPIs gapi = new GoogleAPIs();
            string location = "";
            if (city == null || city == "")
            {
                location = config.LoggedInMember.City;
            }
            else
            {
                location = city;
            }
            string strQuery = query + location;
            List<GPlace> places = gapi.GetPlaceList(strQuery, limit);
            return Json(places, JsonRequestBehavior.AllowGet);
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