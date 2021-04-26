using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoupleDating_MVC5.Models;

namespace CoupleDating_MVC5.Controllers
{
    public class QuestionChoicesController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: QuestionChoices
        public ActionResult Index()
        {
            var QuestionChoices = db.QuestionChoices.Include(t => t.Questions);
            return View(QuestionChoices.ToList());
        }

        // GET: QuestionChoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionChoices QuestionChoices = db.QuestionChoices.Find(id);
            if (QuestionChoices == null)
            {
                return HttpNotFound();
            }
            return View(QuestionChoices);
        }

        // GET: QuestionChoices/Create
        public ActionResult Create()
        {
            ViewBag.questionID = new SelectList(db.Questions, "questionID", "questionName");
            return View();
        }

        // POST: QuestionChoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "questionChoiceID,questionChoiceFull,questionID,ordinal")] QuestionChoices QuestionChoices)
        {
            if (ModelState.IsValid)
            {
                db.QuestionChoices.Add(QuestionChoices);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.questionID = new SelectList(db.Questions, "questionID", "questionName", QuestionChoices.questionID);
            return View(QuestionChoices);
        }

        // GET: QuestionChoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionChoices QuestionChoices = db.QuestionChoices.Find(id);
            if (QuestionChoices == null)
            {
                return HttpNotFound();
            }
            ViewBag.questionID = new SelectList(db.Questions, "questionID", "questionName", QuestionChoices.questionID);
            return View(QuestionChoices);
        }

        // POST: QuestionChoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "questionChoiceID,questionChoiceFull,questionID,ordinal")] QuestionChoices QuestionChoices)
        {
            if (ModelState.IsValid)
            {
                db.Entry(QuestionChoices).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.questionID = new SelectList(db.Questions, "questionID", "questionName", QuestionChoices.questionID);
            return View(QuestionChoices);
        }

        // GET: QuestionChoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionChoices QuestionChoices = db.QuestionChoices.Find(id);
            if (QuestionChoices == null)
            {
                return HttpNotFound();
            }
            return View(QuestionChoices);
        }

        // POST: QuestionChoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuestionChoices QuestionChoices = db.QuestionChoices.Find(id);
            db.QuestionChoices.Remove(QuestionChoices);
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