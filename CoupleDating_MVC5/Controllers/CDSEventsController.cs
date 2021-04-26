using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class CDSEventsController : Controller
    {
        private Couples_Entities db = new Couples_Entities();

        // GET: CDSEvents
        public ActionResult Index()
        {
            var cDSEvents = db.CDSEvents.Include(c => c.DoubleDateRequest).Include(c => c.EventStatu).Include(c => c.EventType).Include(c => c.Group);
            return View(cDSEvents.ToList());
        }

        // GET: CDSEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CDSEvent cDSEvent = db.CDSEvents.Find(id);
            if (cDSEvent == null)
            {
                return HttpNotFound();
            }
            return View(cDSEvent);
        }

        // GET: CDSEvents/Create
        public ActionResult Create()
        {
            ViewBag.doubleDateRequestID = new SelectList(db.DoubleDateRequest, "doubleDateRequestID", "memo");
            ViewBag.eventStatusID = new SelectList(db.EventStatus, "EventStatusID", "Description");
            ViewBag.eventTypeID = new SelectList(db.EventType, "eventTypeID", "description");
            ViewBag.groupID = new SelectList(db.Group, "groupID", "name");
            return View();
        }

        // POST: CDSEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "eventID,title,createdBy,dateStart,dateEnd,eventTypeID,groupID,createdOn,modifiedOn,modifiedBy,doubleDateRequestID,description,eventStatusID")] CDSEvent cDSEvent)
        {
            if (ModelState.IsValid)
            {
                db.CDSEvents.Add(cDSEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.doubleDateRequestID = new SelectList(db.DoubleDateRequest, "doubleDateRequestID", "memo", cDSEvent.doubleDateRequestID);
            ViewBag.eventStatusID = new SelectList(db.EventStatus, "EventStatusID", "Description", cDSEvent.eventStatusID);
            ViewBag.eventTypeID = new SelectList(db.EventType, "eventTypeID", "description", cDSEvent.eventTypeID);
            ViewBag.groupID = new SelectList(db.Group, "groupID", "name", cDSEvent.groupID);
            return View(cDSEvent);
        }

        // GET: CDSEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CDSEvent cDSEvent = db.CDSEvents.Find(id);
            if (cDSEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.doubleDateRequestID = new SelectList(db.DoubleDateRequest, "doubleDateRequestID", "memo", cDSEvent.doubleDateRequestID);
            ViewBag.eventStatusID = new SelectList(db.EventStatus, "EventStatusID", "Description", cDSEvent.eventStatusID);
            ViewBag.eventTypeID = new SelectList(db.EventType, "eventTypeID", "description", cDSEvent.eventTypeID);
            ViewBag.groupID = new SelectList(db.Group, "groupID", "name", cDSEvent.groupID);
            return View(cDSEvent);
        }

        // POST: CDSEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "eventID,title,createdBy,dateStart,dateEnd,eventTypeID,groupID,createdOn,modifiedOn,modifiedBy,doubleDateRequestID,description,eventStatusID")] CDSEvent cDSEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cDSEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.doubleDateRequestID = new SelectList(db.DoubleDateRequest, "doubleDateRequestID", "memo", cDSEvent.doubleDateRequestID);
            ViewBag.eventStatusID = new SelectList(db.EventStatus, "EventStatusID", "Description", cDSEvent.eventStatusID);
            ViewBag.eventTypeID = new SelectList(db.EventType, "eventTypeID", "description", cDSEvent.eventTypeID);
            ViewBag.groupID = new SelectList(db.Group, "groupID", "name", cDSEvent.groupID);
            return View(cDSEvent);
        }

        // GET: CDSEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CDSEvent cDSEvent = db.CDSEvents.Find(id);
            if (cDSEvent == null)
            {
                return HttpNotFound();
            }
            return View(cDSEvent);
        }

        // POST: CDSEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CDSEvent cDSEvent = db.CDSEvents.Find(id);
            db.CDSEvents.Remove(cDSEvent);
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
