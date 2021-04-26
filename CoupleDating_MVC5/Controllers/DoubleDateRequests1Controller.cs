using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class DoubleDateRequests1Controller : Controller
    {
        private Couples_Entities db = new Couples_Entities();

        // GET: DoubleDateRequests1
        public async Task<ActionResult> Index()
        {
            var doubleDateRequest = db.DoubleDateRequest.Include(d => d.tblMember).Include(d => d.tblMember1);
            return View(await doubleDateRequest.ToListAsync());
        }

        // GET: DoubleDateRequests1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoubleDateRequest doubleDateRequest = await db.DoubleDateRequest.FindAsync(id);
            if (doubleDateRequest == null)
            {
                return HttpNotFound();
            }
            return View(doubleDateRequest);
        }

        // GET: DoubleDateRequests1/Create
        public ActionResult Create()
        {
            ViewBag.requestingMemberID = new SelectList(db.tblMember, "memberId", "FirstName1");
            ViewBag.requestToMemberID = new SelectList(db.tblMember, "memberId", "FirstName1");
            return View();
        }

        // POST: DoubleDateRequests1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "doubleDateRequestID,requestDate,accepted,acceptedDate,requestingMemberID,requestToMemberID,memo,requestedDateMeetTime,requestingMemberReview,requestedMemberReview,requestingMemberRating,requestedMemberRating")] DoubleDateRequest doubleDateRequest)
        {
            if (ModelState.IsValid)
            {
                db.DoubleDateRequest.Add(doubleDateRequest);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.requestingMemberID = new SelectList(db.tblMember, "memberId", "FirstName1", doubleDateRequest.requestingMemberID);
            ViewBag.requestToMemberID = new SelectList(db.tblMember, "memberId", "FirstName1", doubleDateRequest.requestToMemberID);
            return View(doubleDateRequest);
        }

        // GET: DoubleDateRequests1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoubleDateRequest doubleDateRequest = await db.DoubleDateRequest.FindAsync(id);
            if (doubleDateRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.requestingMemberID = new SelectList(db.tblMember, "memberId", "FirstName1", doubleDateRequest.requestingMemberID);
            ViewBag.requestToMemberID = new SelectList(db.tblMember, "memberId", "FirstName1", doubleDateRequest.requestToMemberID);
            return View(doubleDateRequest);
        }

        // POST: DoubleDateRequests1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "doubleDateRequestID,requestDate,accepted,acceptedDate,requestingMemberID,requestToMemberID,memo,requestedDateMeetTime,requestingMemberReview,requestedMemberReview,requestingMemberRating,requestedMemberRating")] DoubleDateRequest doubleDateRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doubleDateRequest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.requestingMemberID = new SelectList(db.tblMember, "memberId", "FirstName1", doubleDateRequest.requestingMemberID);
            ViewBag.requestToMemberID = new SelectList(db.tblMember, "memberId", "FirstName1", doubleDateRequest.requestToMemberID);
            return View(doubleDateRequest);
        }

        // GET: DoubleDateRequests1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoubleDateRequest doubleDateRequest = await db.DoubleDateRequest.FindAsync(id);
            if (doubleDateRequest == null)
            {
                return HttpNotFound();
            }
            return View(doubleDateRequest);
        }

        // POST: DoubleDateRequests1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DoubleDateRequest doubleDateRequest = await db.DoubleDateRequest.FindAsync(id);
            db.DoubleDateRequest.Remove(doubleDateRequest);
            await db.SaveChangesAsync();
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
