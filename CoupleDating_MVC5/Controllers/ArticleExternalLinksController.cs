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
    public class ArticleExternalLinksController : Controller
    {
        private Couples_Entities db = new Couples_Entities();

        // GET: ArticleExternalLinks
        public ActionResult Index()
        {
            var articleExternalLinks = db.ArticleExternalLinks.Include(a => a.Article);
            return View(articleExternalLinks.ToList());
        }

        // GET: ArticleExternalLinks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleExternalLinks articleExternalLinks = db.ArticleExternalLinks.Find(id);
            if (articleExternalLinks == null)
            {
                return HttpNotFound();
            }
            return View(articleExternalLinks);
        }

        // GET: ArticleExternalLinks/Create
        public ActionResult Create()
        {
            ViewBag.articleID = new SelectList(db.Article, "articleID", "title");
            return View();
        }

        // POST: ArticleExternalLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "articleExternalLinkID,articleID,url")] ArticleExternalLinks articleExternalLinks)
        {
            if (ModelState.IsValid)
            {
                db.ArticleExternalLinks.Add(articleExternalLinks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.articleID = new SelectList(db.Article, "articleID", "title", articleExternalLinks.articleID);
            return View(articleExternalLinks);
        }

        // GET: ArticleExternalLinks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleExternalLinks articleExternalLinks = db.ArticleExternalLinks.Find(id);
            if (articleExternalLinks == null)
            {
                return HttpNotFound();
            }
            ViewBag.articleID = new SelectList(db.Article, "articleID", "title", articleExternalLinks.articleID);
            return View(articleExternalLinks);
        }

        // POST: ArticleExternalLinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "articleExternalLinkID,articleID,url")] ArticleExternalLinks articleExternalLinks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articleExternalLinks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.articleID = new SelectList(db.Article, "articleID", "title", articleExternalLinks.articleID);
            return View(articleExternalLinks);
        }

        // GET: ArticleExternalLinks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleExternalLinks articleExternalLinks = db.ArticleExternalLinks.Find(id);
            if (articleExternalLinks == null)
            {
                return HttpNotFound();
            }
            return View(articleExternalLinks);
        }

        // POST: ArticleExternalLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArticleExternalLinks articleExternalLinks = db.ArticleExternalLinks.Find(id);
            db.ArticleExternalLinks.Remove(articleExternalLinks);
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
