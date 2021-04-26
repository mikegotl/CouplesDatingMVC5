using CoupleDating_MVC5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Controllers
{
    public class DoubleDateRequestsController : Controller
    {
        private long? _routeID;

        public long? RouteID
        {
            get
            {
                if (_routeID == null || _routeID == 0)
                {
                    _routeID = (long)Convert.ToInt16(Url.RequestContext.RouteData.Values["id"]);
                }

                return _routeID;
            }
            set { _routeID = value; }
        }

        private DBEntities db = new DBEntities();

        private Config config = new Config();

        // GET: DoubleDateRequests
        public ActionResult Index()
        {
            var doubleDateRequest = db.DoubleDateRequest.Include(d => d.Member).Include(d => d.Member1);
            return View(doubleDateRequest.ToList());
        }

        // GET: DoubleDateRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoubleDateRequest doubleDateRequest = db.DoubleDateRequest.Find(id);
            if (doubleDateRequest == null)
            {
                return HttpNotFound();
            }
            return View(doubleDateRequest);
        }

        // GET: DoubleDateRequests/Create
        public ActionResult Create()
        {
            ViewBag.requestingMemberID = new SelectList(db.Member, "memberId", "FirstName1");
            ViewBag.requestToMemberID = new SelectList(db.Member, "memberId", "FirstName1");
            return View();
        }

        // POST: DoubleDateRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "doubleDateRequestID,requestDate,accepted,acceptedDate,requestingMemberID,requestToMemberID,memo,requestedDateMeetTime,requestingMemberReview,requestedMemberReview,requestingMemberRating,requestedMemberRating")] DoubleDateRequest doubleDateRequest)
        {
            if (ModelState.IsValid)
            {
                db.DoubleDateRequest.Add(doubleDateRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.requestingMemberID = new SelectList(db.Member, "memberId", "FirstName1", doubleDateRequest.RequestingMemberID);
            ViewBag.requestToMemberID = new SelectList(db.Member, "memberId", "FirstName1", doubleDateRequest.RequestToMemberID);
            return View(doubleDateRequest);
        }

        //[OutputCache(CacheProfile = "Client1Hour")]
        public ActionResult DDRequest()
        {
            List<FavoritePlaces> myfavoritePlaces = db.FavoritePlaces.Where(f => f.MemberID == config.LoggedInMember.ID).ToList<FavoritePlaces>();
            List<FavPl_GPlace> places = GetFavGPlaces(myfavoritePlaces);
            ViewBag.FavPl_GPlaces = places;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DDRequest([Bind(Include = "doubleDateRequestID,requestDate,accepted,acceptedDate,requestingMemberID,requestToMemberID,memo,requestedDateMeetTime,requestingMemberReview,requestedMemberReview,requestingMemberRating,requestedMemberRating")] DoubleDateRequest doubleDateRequest, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                Dictionary<string, bool> places = new Dictionary<string, bool>();
                foreach (var key in fc.AllKeys)
                {
                    if (key.Contains("places"))
                    {
                        string _selected = fc.Get(key);
                        string _name = key.ToString();
                        bool _checked;

                        if (_selected == "on")
                        {
                            _checked = true;
                        }
                        else
                        {
                            _checked = false;
                        }
                        places.Add(_name, _checked);
                    }
                }

                DateTime rDate = Convert.ToDateTime(fc.GetValues("requestDate")[0]);
                string[] memo = fc.GetValues("Memo");

                doubleDateRequest.RequestDate = rDate;
                doubleDateRequest.RequestingMemberID = config.LoggedInMember.ID;
                doubleDateRequest.RequestToMemberID = (long)RouteID;

                db.DoubleDateRequest.Add(doubleDateRequest);
                db.SaveChanges();

                if (places != null)
                {
                    foreach (var p in places)
                    {
                        string[] _name = p.Key.ToString().Split('-');

                        bool _checked = p.Value;

                        string gPID = _name[1].ToString();

                        DoubleDateDetails newDDDetail = new DoubleDateDetails();
                        newDDDetail.DoubleDateRequestID = doubleDateRequest.ID;
                        newDDDetail.gPlaceID = gPID;
                        newDDDetail.DateCreated = DateTime.Now;
                        //newDDDetail.meetTime = rDate;
                        newDDDetail.Memo = memo[0];

                        db.DoubleDateDetails.Add(newDDDetail);
                        db.SaveChanges();
                    }
                }

                //send email to

                //create message
                Messages message = new Messages();
                message.memberID = config.LoggedInMember.ID;
                message.toMemberID = (long)RouteID;
                message.subject = "New Date Request is waiting for you!";
                message.body = "Click <a href='/DoubleDateRequests/Details/"
                    + doubleDateRequest.ID
                    + "'>here</a> to see the Date Request and to Accept or Deny it. ";

                message.dateCreated = DateTime.Now;

                db.Messages.Add(message);
                db.SaveChanges();

                //send email
                string _toEmail = config.GetEmailAddress((long)RouteID);
                string _fromMember = config.LoggedInMember.ScreenName;

                MailModel mm = new MailModel();
                mm.To = _toEmail;
                mm.Subject = "You have a Date Request from CouplesDatingScene.com member: " + _fromMember;
                mm.Body = "</br> Click <a href='www.couplesdatingscene.com'>here</a> to go to site and find your new Date Request in your Mailbox. Click or use this url if the previous one doesn't work https://www.CouplesDatingScene.com";

                string _path = "/content/images/members/profilepics/test.jpg";
                _path = config.LoggedInMember.MemberMedia.FirstOrDefault().path.Replace("~", "");

                SendMailerController.SendEmail(mm, _path);

                //redirect to confirmation page?
                return RedirectToAction("Details", "Members", new { id = (long)RouteID });
            }

            ViewBag.requestingMemberID = new SelectList(db.Member, "memberId", "FirstName1", doubleDateRequest.RequestingMemberID);
            ViewBag.requestToMemberID = new SelectList(db.Member, "memberId", "FirstName1", doubleDateRequest.RequestToMemberID);
            return View(doubleDateRequest);
        }

        public ActionResult AcceptDate(int ID)
        {
            var dd = db.DoubleDateRequest.Where(m => m.ID == ID).FirstOrDefault();
            dd.Accepted = true;
            dd.AcceptedDate = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Details", "DoubleDateRequests", new { id = ID });
        }

        public ActionResult DenyDate(int ID)
        {
            var dd = db.DoubleDateRequest.Where(m => m.ID == ID).FirstOrDefault();
            dd.Accepted = false;
            dd.AcceptedDate = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Details", "DoubleDateRequests", new { id = ID });
        }

        // GET: DoubleDateRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoubleDateRequest doubleDateRequest = db.DoubleDateRequest.Find(id);
            if (doubleDateRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.requestingMemberID = new SelectList(db.Member, "memberId", "FirstName1", doubleDateRequest.RequestingMemberID);
            ViewBag.requestToMemberID = new SelectList(db.Member, "memberId", "FirstName1", doubleDateRequest.RequestToMemberID);
            return View(doubleDateRequest);
        }

        // POST: DoubleDateRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "doubleDateRequestID,requestDate,accepted,acceptedDate,requestingMemberID,requestToMemberID,memo,requestedDateMeetTime,requestingMemberReview,requestedMemberReview,requestingMemberRating,requestedMemberRating")] DoubleDateRequest doubleDateRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doubleDateRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.requestingMemberID = new SelectList(db.Member, "memberId", "FirstName1", doubleDateRequest.RequestingMemberID);
            ViewBag.requestToMemberID = new SelectList(db.Member, "memberId", "FirstName1", doubleDateRequest.RequestToMemberID);
            return View(doubleDateRequest);
        }

        // GET: DoubleDateRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoubleDateRequest doubleDateRequest = db.DoubleDateRequest.Find(id);
            if (doubleDateRequest == null)
            {
                return HttpNotFound();
            }
            return View(doubleDateRequest);
        }

        // POST: DoubleDateRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoubleDateRequest doubleDateRequest = db.DoubleDateRequest.Find(id);
            db.DoubleDateRequest.Remove(doubleDateRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private List<FavPl_GPlace> GetFavGPlaces(List<FavoritePlaces> myfavoritePlaces)
        {
            GoogleAPIs gapi = new GoogleAPIs();
            List<GPlace> gPlaces = new List<GPlace>();
            List<FavPl_GPlace> favPl_GPlaces = new List<FavPl_GPlace>();
            bool favsAreEqual = true;

            //favsAreEqual = CheckFavPlSessionChanged(myfavoritePlaces, favsAreEqual);

            favsAreEqual = false;//do this to force refresh

            if (Session["gPlaces"] != null && Session["filteredModel"] != null && favsAreEqual)
            {
                favPl_GPlaces = (List<FavPl_GPlace>)Session["favPl_GPlaces"];
            }
            else
            {
                //for each Favorite Place, get Place info from Google API and add to gPlaces list.
                foreach (FavoritePlaces fp in myfavoritePlaces)
                {
                    if (fp.GPlace_Id != null && fp.GPlace_Id != "0")
                    {
                        //get place infor from google api
                        GPlace gplace = new GPlace();
                        gplace = gapi.GetPlaceDetails(fp.GPlace_Id);

                        FavPl_GPlace favPl_Gplace = new FavPl_GPlace();
                        favPl_Gplace.FavoritePlaceID = fp.ID;
                        try
                        {
                            favPl_Gplace.GPhotoUrl = gplace.photoUrls.FirstOrDefault().photoUrl;
                        }
                        catch (Exception ex)
                        {
                        }

                        favPl_Gplace.GPlaceID = gplace.place_id;
                        favPl_Gplace.GPlaceName = gplace.name;
                        favPl_Gplace.href = "/places/details/" + gplace.place_id;

                        favPl_GPlaces.Add(favPl_Gplace);
                    }
                }

                //store session objects
                Session["myfavoritePlaces"] = favPl_GPlaces;
            }
            return favPl_GPlaces;
        }

        private bool CheckFavPlSessionChanged(List<FavoritePlaces> myfavoritePlaces, bool favsAreEqual)
        {
            if (Session["myfavoritePlaces"] != null)
            {
                List<FavoritePlaces> sMyFavoritePlaces = (List<FavoritePlaces>)Session["myfavoritePlaces"];

                if (sMyFavoritePlaces != null)
                {
                    foreach (FavoritePlaces f_db in myfavoritePlaces)
                    {
                        var found = sMyFavoritePlaces.Where(m => m.GPlace_Id == f_db.GPlace_Id);
                        if (found == null)
                        {
                            favsAreEqual = false;
                        }
                    }
                }
            }
            else
            {
                favsAreEqual = false;
            }
            return favsAreEqual;
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