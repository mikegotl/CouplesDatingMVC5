using CoupleDating_MVC5.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Controllers
{
    public class FriendsController : Controller
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

        // GET: Friends
        public ActionResult Index()
        {
            var ourID = config.LoggedInMember.ID;
            var ourFriends = db.Friends.Include(f => f.Member)
                .Include(f => f.Member1)
                .Include(f => f.Member.Friends)
                .Where(f => (f.Member1ID == ourID || f.Member2ID == ourID) && f.Accepted == true);

            return View(ourFriends.ToList());
        }

        // GET: Friends/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friends friends = db.Friends.Find(id);
            if (friends == null)
            {
                return HttpNotFound();
            }
            return View(friends);
        }

        // GET: Friends/Create
        public ActionResult Create()
        {
            ViewBag.member1ID = new SelectList(db.Member, "ID", "FirstName1");
            ViewBag.member2ID = new SelectList(db.Member, "ID", "FirstName1");
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "friendID,member1ID,member2ID")] Friends friends)
        {
            if (ModelState.IsValid)
            {
                db.Friends.Add(friends);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.member1ID = new SelectList(db.Member, "ID", "FirstName1", friends.Member1ID);
            ViewBag.member2ID = new SelectList(db.Member, "ID", "FirstName1", friends.Member2ID);
            return View(friends);
        }

        // GET: Friends/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friends friends = db.Friends.Find(id);
            if (friends == null)
            {
                return HttpNotFound();
            }
            ViewBag.member1ID = new SelectList(db.Member, "ID", "FirstName1", friends.Member1ID);
            ViewBag.member2ID = new SelectList(db.Member, "ID", "FirstName1", friends.Member2ID);
            return View(friends);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "friendID,member1ID,member2ID")] Friends friends)
        {
            if (ModelState.IsValid)
            {
                db.Entry(friends).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.member1ID = new SelectList(db.Member, "ID", "FirstName1", friends.Member1ID);
            ViewBag.member2ID = new SelectList(db.Member, "ID", "FirstName1", friends.Member2ID);
            return View(friends);
        }

        // GET: Friends/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friends friends = db.Friends.Find(id);
            if (friends == null)
            {
                return HttpNotFound();
            }
            return View(friends);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Friends friends = db.Friends.Find(id);
            db.Friends.Remove(friends);
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

        public ActionResult AddFriendRequest(int friendID)
        {
            long loggedInFriendID = config.LoggedInMember.ID;

            Friends existingFr = db.Friends.Where(f => (f.Member1ID == friendID && f.Member2ID == loggedInFriendID) || (f.Member2ID == friendID && f.Member1ID == loggedInFriendID)).FirstOrDefault();

            if (existingFr == null)
            {
                Friends fr = new Friends();
                fr.Member1ID = loggedInFriendID;
                fr.Member2ID = friendID;
                fr.RequestDate = DateTime.Now;

                db.Friends.Add(fr);
                db.SaveChanges();

                SendFriendRequestMessage(fr);
            }

            return RedirectToAction("Details", "Members", new { id = friendID });
        }

        public void SendFriendRequestMessage(Friends f)
        {
            //create message
            Messages message = new Messages();
            message.ID = (int)f.Member1ID;
            message.toMemberID = (int)f.Member2ID;
            message.subject = "New Friend Request is waiting for you.";
            message.body = "Click <a href='/friends/AcceptFriend/" + config.LoggedInMember.ID.ToString() + "'>here</a> to accept friend request.";
            message.dateCreated = DateTime.Now;

            db.Messages.Add(message);
            db.SaveChanges();

            //send email
            string _toEmail = config.GetEmailAddress((long)f.Member2ID);
            string _fromMember = config.LoggedInMember.ScreenName;

            MailModel mm = new MailModel();
            mm.To = _toEmail;
            mm.Subject = "You have a Friend Request from CouplesDatingScene.com member: " + _fromMember;
            mm.Body = "</br> Click <a href='www.couplesdatingscene.com'>here</a> to go to site and find your new friend request in your Mailbox.";

            string _path = "/content/images/members/profilepics/test.jpg";//TODO
            try
            {
                _path = f.Member.MemberMedia.FirstOrDefault().path.ToString();
            }
            catch (Exception ex)
            {
            }

            SendMailerController.SendEmail(mm, _path);
        }

        //[ActivePaidMember]
        public ActionResult AcceptFriend(string id)
        {
            long longID = Convert.ToInt16(id.ToString());

            Friends existingF = db.Friends.Where(f => f.Member1ID == longID && f.Member2ID == config.LoggedInMember.ID).FirstOrDefault();

            existingF.Accepted = true;
            existingF.AcceptedDate = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("Index", "Messages");
        }

        public ActionResult Unfriend(int friendID)
        {
            long loggedInFriendID = config.LoggedInMember.ID;

            Friends existingFr = db.Friends.Where(f => (f.Member1ID == friendID && f.Member2ID == loggedInFriendID) || (f.Member2ID == friendID && f.Member1ID == loggedInFriendID)).FirstOrDefault();

            if (existingFr != null)
            {
                db.Friends.Remove(existingFr);
                db.SaveChanges();
            }

            return RedirectToAction("Details", "Members", new { id = friendID });
        }

        //public ActionResult SendFriendRequest()
        //{
        //    Friends existingF = db.Friends.Where(m => m.member1ID == (long)RouteID && m.member2ID == (long)Config.LoggedInMember.ID || m.member2ID == (long)RouteID && m.member1ID == (long)Config.LoggedInMember.ID).FirstOrDefault();

        //    return PartialView("_Friend", existingF);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SendFriendRequest([Bind(Include = "friendID,member1ID,member2ID")] Friends friends)
        //{
        //    Friends existingF = db.Friends.Where(m => m.member1ID == (long)RouteID && m.member2ID == (long)Config.LoggedInMember.ID || m.member2ID == (long)RouteID && m.member1ID == (long)Config.LoggedInMember.ID).FirstOrDefault();

        //    if (existingF != null)
        //    {
        //        //exists - remove
        //        db.Friends.Remove(existingF);
        //        db.SaveChanges();

        //        return RedirectToAction("Details", "Members", new { id = (long)RouteID });
        //    }
        //    else
        //    {
        //        //does not exist - add
        //        Friends f = new Friends();
        //        f.member1ID = (long)Config.LoggedInMember.ID;
        //        f.member2ID = (long)RouteID;
        //        f.RequestDate = DateTime.Now;

        //        db.Friends.Add(f);
        //        db.SaveChanges();

        //        //create message
        //        Messages message = new Messages();
        //        message.ID = (long)f.member1ID;
        //        message.toID = (long)f.member2ID;
        //        message.subject = "New Friend Request is waiting for you.";
        //        message.body = "Click <a href='/friends/AcceptFriend/" + Config.LoggedInMember.ID.ToString() + "'>here</a> to accept friend request.";
        //        message.dateCreated = DateTime.Now;

        //        db.Messages.Add(message);
        //        db.SaveChanges();

        //        //send email
        //        string _toEmail = Config.GetEmailAddress((long)f.member2ID);
        //        string _fromMember = Config.LoggedInMember.ScreenName;

        //        MailModel mm = new MailModel();
        //        mm.To = _toEmail;
        //        mm.Subject = "You have a Friend Request from CouplesDatingScene.com member: " + _fromMember;
        //        mm.Body = "</br> Click <a href='www.couplesdatingscene.com'>here</a> to go to site and find your new friend request in your Mailbox.";

        //        string _path = "/content/images/members/profilepics/test.jpg";

        //        SendMailerController.SendEmail(mm, _path);

        //        return RedirectToAction("Details", "Members", new { id = (long)RouteID });
        //    }
        //}
    }
}