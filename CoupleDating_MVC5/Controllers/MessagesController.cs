using CoupleDating_MVC5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Controllers
{
    public class MessagesController : Controller
    {
        private DBEntities db = new DBEntities();

        private long? _routeID;

        public long? RouteID
        {
            get
            {
                _routeID = (long)Convert.ToInt16(Url.RequestContext.RouteData.Values["id"]);

                return _routeID;
            }
            set { _routeID = value; }
        }

        private Config config = new Config();

        // GET: Messages
        [Authorize]
        public ActionResult Index()
        {
            var messages = db.Messages.Include(m => m.Member).Where(m => m.toMemberID == config.LoggedInMember.ID);
            return View(messages.ToList());
        }

        // GET: Messages/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get current message
            Messages message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }

            //mark as read
            message.isRead = true;
            db.SaveChanges();

            if (message.replyMessageID != null)
            {
                List<Messages> previousMessages = new List<Messages>();
                int? _previousMessageID = (int)message.replyMessageID;

                if (_previousMessageID > 0)
                {
                    //get previous message and add to list
                    Messages previousMessage = db.Messages.Where(m => m.ID == _previousMessageID).FirstOrDefault();
                    previousMessages.Add(previousMessage);

                    //get next previousMessageID
                    _previousMessageID = previousMessage.replyMessageID;
                }

                List<Messages> prevs = previousMessages.OrderByDescending(x => x.ID).ToList<Messages>();
                ViewBag.PreviousMessages = prevs;
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ActivePaidMember]
        public ActionResult Create([Bind(Include = "messageID,subject,body,ID,dateCreated,replyMessageID,toID")] Messages messages)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(messages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", messages.ID);
            return View(messages);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = db.Messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", messages.ID);
            return View(messages);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "messageID,subject,body,ID,dateCreated,replyMessageID,toID")] Messages messages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", messages.ID);
            return View(messages);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = db.Messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Messages messages = db.Messages.Find(id);
            db.Messages.Remove(messages);
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

        public ActionResult New()
        {
            Messages m = new Messages();
            return PartialView("_Message", m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ActivePaidMember]
        public ActionResult New([Bind(Include = "messageID,subject,body,ID,dateCreated,replyMessageID,toID")] Messages messages)
        {
            Messages m = new Messages();
            if (ModelState.IsValid)
            {
                //get subject & body
                m.body = messages.body;
                m.dateCreated = DateTime.Now;
                m.subject = messages.subject;

                //From
                m.ID = (int)config.LoggedInMember.ID;

                //To
                //if not reply
                m.toMemberID = RouteID;

                Messages origMess = db.Messages.Where(q => q.ID == RouteID).FirstOrDefault();

                if (origMess != null)
                {
                    m.toMemberID = origMess.ID;
                    m.replyMessageID = origMess.ID;
                }

                //Add message to database and save
                db.Messages.Add(m);
                db.SaveChanges();

                //Send email
                string _toEmail = config.GetEmailAddress((long)m.toMemberID);
                string _fromMember = config.LoggedInMember.ScreenName;

                MailModel mm = new MailModel();
                mm.To = _toEmail;
                mm.Subject = "New mail message from CouplesDatingScene.com member: " + _fromMember;
                mm.Body = "SUBJECT " + messages.subject + " </hr> MESSAGE: " + messages.body + "</br> Click <a href='www.couplesdatingscene.com'>here</a> to go to site";

                SendMailerController.SendEmail(mm);

                //return screen
                return PartialView("_Message", m);
            }

            //ViewBag.ID = new SelectList(db.Member, "ID", "FirstName1", messages.ID);
            return PartialView("_Message", m);
        }

        public ActionResult MailboxCount()
        {
            var _count = db.Messages.Where(m => m.toMemberID == config.LoggedInMember.ID && m.isRead != true).Count();

            ViewBag.count = _count;
            return PartialView("_MailboxNotify");
        }
    }
}