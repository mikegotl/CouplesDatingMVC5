using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class tblQuestionAnswersController : Controller
    {
        private Couples_Entities db = new Couples_Entities();

        // GET: tblQuestionAnswers
        public ActionResult Index()
        {
            var tblQuestionAnswers = db.tblQuestionAnswers.Include(t => t.tblMember).Include(t => t.tblQuestionChoices);
            ViewBag.questionChoiceID = new SelectList(db.tblQuestionChoices, "questionChoiceID", "questionChoiceFull");

            ViewBag.questionChoices = db.tblQuestionChoices.Include(t => t.tblQuestions);

            return View(tblQuestionAnswers.ToList());
        }

        // GET: tblQuestionAnswers/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblQuestionAnswers tblQuestionAnswers = db.tblQuestionAnswers.Find(id);
            if (tblQuestionAnswers == null)
            {
                return HttpNotFound();
            }
            return View(tblQuestionAnswers);
        }

        // GET: tblQuestionAnswers/Create
        public ActionResult Create()
        {
            ViewBag.memberID = new SelectList(db.tblMember, "memberId", "FirstName1");
            ViewBag.questionChoiceID = new SelectList(db.tblQuestionChoices, "questionChoiceID", "questionChoiceFull");
            return View();
        }

        // POST: tblQuestionAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "questionAnswerID,questionChoiceID,memberID")] tblQuestionAnswers tblQuestionAnswers)
        {
            if (ModelState.IsValid)
            {
                db.tblQuestionAnswers.Add(tblQuestionAnswers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.memberID = new SelectList(db.tblMember, "memberId", "FirstName1", tblQuestionAnswers.memberID);
            ViewBag.questionChoiceID = new SelectList(db.tblQuestionChoices, "questionChoiceID", "questionChoiceFull", tblQuestionAnswers.questionChoiceID);
            return View(tblQuestionAnswers);
        }

        // GET: tblQuestionAnswers/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblQuestionAnswers tblQuestionAnswers = db.tblQuestionAnswers.Find(id);
            if (tblQuestionAnswers == null)
            {
                return HttpNotFound();
            }
            ViewBag.memberID = new SelectList(db.tblMember, "memberId", "FirstName1", tblQuestionAnswers.memberID);
            ViewBag.questionChoiceID = new SelectList(db.tblQuestionChoices, "questionChoiceID", "questionChoiceFull", tblQuestionAnswers.questionChoiceID);
            return View(tblQuestionAnswers);
        }

        // POST: tblQuestionAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "questionAnswerID,questionChoiceID,memberID")] tblQuestionAnswers tblQuestionAnswers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblQuestionAnswers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.memberID = new SelectList(db.tblMember, "memberId", "FirstName1", tblQuestionAnswers.memberID);
            ViewBag.questionChoiceID = new SelectList(db.tblQuestionChoices, "questionChoiceID", "questionChoiceFull", tblQuestionAnswers.questionChoiceID);
            return View(tblQuestionAnswers);
        }

        // GET: tblQuestionAnswers/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblQuestionAnswers tblQuestionAnswers = db.tblQuestionAnswers.Find(id);
            if (tblQuestionAnswers == null)
            {
                return HttpNotFound();
            }
            return View(tblQuestionAnswers);
        }

        // POST: tblQuestionAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tblQuestionAnswers tblQuestionAnswers = db.tblQuestionAnswers.Find(id);
            db.tblQuestionAnswers.Remove(tblQuestionAnswers);
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