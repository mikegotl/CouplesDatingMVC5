using CoupleDating_MVC5.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Controllers
{
    public class QuestionsController : Controller
    {
        private DBEntities db = new DBEntities();
        private Config config = new Config();
        private string _uID;

        public string UID
        {
            get
            {
                if (_uID == null)
                {
                    _uID = User.Identity.GetUserId();
                }
                return _uID;
            }
            set
            {
                _uID = value;
            }
        }

        // GET: Questions
        public ActionResult Index()
        {
            //get all enabled questions
            var Questions = db.Questions.Include(t => t.QuestionCategory).Where(q => q.enabled == true);
            var questions = Questions.ToList();

            //get member's choices
            List<QuestionAnswers> choices = (from qq in db.QuestionAnswers where qq.memberID == config.LoggedInMember.ID select qq).ToList<QuestionAnswers>();
            ViewBag.choiceIdSelected = choices;

            return View(questions);
        }

        [HttpPost]
        public ActionResult Index(FormCollection c)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                var checkboxlistselections = c.GetValues("choices");
                var dropdownselections = c.GetValues("item.QuestionChoices");

                List<QuestionAnswers> todelete = (
                    from qq in db.QuestionAnswers
                    where qq.memberID == config.LoggedInMember.ID
                    select qq).ToList<QuestionAnswers>();

                //remove current answers for member
                db.QuestionAnswers.RemoveRange(todelete);

                //add dropdown selections to answers table
                for (i = 0; i < dropdownselections.Count(); i++)
                {
                    QuestionAnswers qa = new QuestionAnswers();
                    qa.questionChoiceID = Convert.ToInt16(dropdownselections[i]);
                    qa.memberID = config.LoggedInMember.ID;
                    db.QuestionAnswers.Add(qa);
                }

                //add checkbox selections to answers table
                for (i = 0; i < checkboxlistselections.Count(); i++)
                {
                    QuestionAnswers qa = new QuestionAnswers();
                    qa.questionChoiceID = Convert.ToInt16(checkboxlistselections[i]);
                    qa.memberID = config.LoggedInMember.ID;
                    db.QuestionAnswers.Add(qa);
                }

                db.SaveChanges();
            }

            //get member's choices
            List<QuestionAnswers> choices = db.QuestionAnswers.Where(q => q.memberID == config.LoggedInMember.ID).ToList<QuestionAnswers>();
            ViewBag.choiceIdSelected = choices;

            //get all enabled questions
            var Questions = db.Questions.Include(t => t.QuestionCategory).Where(q => q.enabled == true);
            var questions = Questions.ToList();

            return View(questions);
        }

        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions Questions = db.Questions.Find(id);
            if (Questions == null)
            {
                return HttpNotFound();
            }
            return View(Questions);
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            ViewBag.questionCategoryID = new SelectList(db.QuestionCategory, "questionCategoryID", "questionCategoryName");
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "questionID,questionName,questionFull,questionCategoryID,questionImportance,multipleAnswersAllowed")] Questions Questions)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(Questions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.questionCategoryID = new SelectList(db.QuestionCategory, "questionCategoryID", "questionCategoryName", Questions.questionCategoryID);
            return View(Questions);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions Questions = db.Questions.Find(id);
            if (Questions == null)
            {
                return HttpNotFound();
            }
            ViewBag.questionCategoryID = new SelectList(db.QuestionCategory, "questionCategoryID", "questionCategoryName", Questions.questionCategoryID);
            return View(Questions);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "questionID,questionName,questionFull,questionCategoryID,questionImportance,multipleAnswersAllowed")] Questions Questions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Questions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.questionCategoryID = new SelectList(db.QuestionCategory, "questionCategoryID", "questionCategoryName", Questions.questionCategoryID);
            return View(Questions);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions Questions = db.Questions.Find(id);
            if (Questions == null)
            {
                return HttpNotFound();
            }
            return View(Questions);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Questions Questions = db.Questions.Find(id);
            db.Questions.Remove(Questions);
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