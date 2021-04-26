using CoupleDating_MVC5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Controllers
{
    public class PlacesAsyncController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: PlacesAsync
        public async Task<ActionResult> Index()
        {
            return View(await db.Place.ToListAsync());
        }

        // GET: PlacesAsync/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = await db.Place.FindAsync(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // GET: PlacesAsync/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlacesAsync/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PlaceID,Name,Description,Blurb,City,State,Zip,Address,ContactNumber,GPlace_Id")] Place place)
        {
            if (ModelState.IsValid)
            {
                db.Place.Add(place);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(place);
        }

        // GET: PlacesAsync/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = await db.Place.FindAsync(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // POST: PlacesAsync/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PlaceID,Name,Description,Blurb,City,State,Zip,Address,ContactNumber,GPlace_Id")] Place place)
        {
            if (ModelState.IsValid)
            {
                db.Entry(place).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(place);
        }

        // GET: PlacesAsync/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = await db.Place.FindAsync(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // POST: PlacesAsync/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Place place = await db.Place.FindAsync(id);
            db.Place.Remove(place);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "city;photoCat")]
        public async Task<PartialViewResult> ListPlaces(string city, string photoCat)
        {
            GoogleAPIs gapi = new GoogleAPIs();

            string location;

            if (string.IsNullOrEmpty(city))
            {
                location = "Orlando";
            }
            else
            {
                location = city;
            }

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
            List<GPlace> places = await gapi.GetPlaces(strQuery, limit);
            ViewBag.places = places;

            ViewBag.location = location;
            ViewBag.lastUpdated = DateTime.Now.ToString("T");

            return PartialView("_AsyncTopPlaces");
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