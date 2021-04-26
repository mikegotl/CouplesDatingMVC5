using CoupleDating_MVC5.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Controllers
{
    public class MembersController : Controller
    {
        private DBEntities db = new DBEntities();
        private Config config = new Config();

        private Member ThisMember
        {
            get
            {
                if (Session["thisMember"] == null)
                {
                    if (config.LoggedInMember != null)
                    {
                        Session["thisMember"] = config.LoggedInMember;
                        return (Member)Session["thisMember"];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return (Member)Session["thisMember"];
                }
            }
            set
            {
                Session["thisMember"] = value;
            }
        }

        // GET: Members
        public ActionResult Index()
        {
            List<Member> members = db.Member.Include(t => t.MemberStatus).ToList();
            return View(members);
        }

        [HttpGet]
        public JsonResult GetMostRecentCouples(int max)
        {
            var memberList = db.Member
                .Take(max)
                .Include(t => t.MemberMedia).ToList()
                .Where(w => w.MemberMedia.Count > 0);

            var members = memberList
                .Select(t => new
                {
                    ID = t.ID,
                    ScreenName = t.ScreenName,
                    ImgPath = t.MemberMedia.Count > 0 ? t.MemberMedia.FirstOrDefault().path : "",
                    FirstName1 = t.FirstName1,
                    FirstName2 = t.FirstName2,
                    Status = t.MemberStatus.Description,
                    YearMet = t.YearMet
                });

            return Json(members, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CouplesSearchAdvanced(FormCollection form)
        {
            var ages = form["sliderValue"];
            var agesSplit = ages.ToString().Split(',');
            var ageStart = agesSplit[0];
            var ageEnd = agesSplit[1];

            int startYear = DateTime.Now.Year - Convert.ToInt16(ageStart);
            int endYear = DateTime.Now.Year - Convert.ToInt16(ageEnd);

            //build list of members where dob's are in age range passed in
            List<Member> members = SearchAges(ages, startYear, endYear);

            //Search ScreenName
            members = SearchScreenName(form, members);

            //Years together
            members = SearchYearsTogether(form, members);

            //Has Pics
            members = SearchHasPics(form, members);

            //Genders in Couple
            members = SearchGenders(form, members);

            //Distance
            members = SearchDistance(form, members);

            return View(members);
        }

        private List<Member> SearchDistance(FormCollection form, List<Member> members)
        {
            int distanceMiles = Convert.ToInt16(form["distance"]);
            if (distanceMiles > 0)
            {
                //check to see which members in list have calculated zipcode records to the zipcode of logged in user
                string loggedUserZip = ThisMember.Zipcode;

                //List<ZipcodeCalculations> calcdZipCodeMatchesToUser = db.ZipcodeCalculations.Where(m => m.zipcode1 == loggedUserZip || m.zipcode2 == loggedUserZip).ToList<ZipcodeCalculations>();
                var membersWithZCMatchesToUserZip = (from mm in members
                                                     from zz in db.ZipcodeCalculations
                                                     where
                                                     (zz.zipcode1 == loggedUserZip && zz.zipcode2 == mm.Zipcode) ||
                                                     (zz.zipcode2 == loggedUserZip && zz.zipcode1 == mm.Zipcode)
                                                     select new { mm.ID, zz.distanceMilesDec }
                                                     ).ToList();

                List<ZipDistancesFromZC> zdList = new List<ZipDistancesFromZC>();

                //remove members with no zipcode -- comment below line to include members with no zip code saved
                members = members.Where(m => m.Zipcode != null).ToList();

                foreach (Member m in members)
                {
                    var zcLookupMemberZip = membersWithZCMatchesToUserZip.Where(x => x.ID == m.ID).ToList();

                    if (zcLookupMemberZip.Count > 0)
                    {
                        //has zipcode distance record in ZC that matches with User's zipcode i.e. has saved Distance
                        //get distance from record
                        decimal _dist = (decimal)zcLookupMemberZip.FirstOrDefault().distanceMilesDec;
                        //add member & distance to zdList
                        ZipDistancesFromZC zd = new ZipDistancesFromZC();
                        zd.ID = m.ID;
                        zd.distance = _dist;
                        zdList.Add(zd);
                    }
                    else
                    {
                        //NO saved Distance
                        //calculate

                        decimal _dist = CalculateDistanceZips(loggedUserZip, m.Zipcode.ToString());
                        //add member & distance to zdList
                        ZipDistancesFromZC zd = new ZipDistancesFromZC();
                        zd.ID = m.ID;
                        zd.distance = _dist;
                        zdList.Add(zd);

                        //save new ZC record in table
                        ZipcodeCalculations ZC = new ZipcodeCalculations();
                        ZC.zipcode1 = loggedUserZip;
                        ZC.zipcode2 = m.Zipcode;
                        ZC.distanceMilesDec = _dist;

                        db.ZipcodeCalculations.Add(ZC);
                        db.SaveChanges();
                    }
                }
                //use zdList to REMOVE MEMBERS from members list that has a distance > greater than selected distance
                foreach (ZipDistancesFromZC i in zdList)
                {
                    if (i.distance > distanceMiles && i.distance != null)
                    {
                        //remove member
                        Member r = db.Member.Find(i.ID);
                        members.Remove(r);
                    }
                }

                //make sure viewbag maintains distance selected by user
                ViewBag.distance = distanceMiles;
            }
            return members;
        }

        private decimal CalculateDistanceZips(string zip1, string zip2)
        {
            WebClient webClient = new WebClient();

            string Url = "http://www.zipcodeapi.com/rest/";
            string api_key = "KPtVG70eiGuSpLinZ8pF58aFd8JlpWHaMb6si6HANbtcWMAWUZB08MFmUpHkmov8";
            string completeUrl = Url + api_key + "/distance.json/" + zip1 + "/" + zip2 + "/mile";

            string result = webClient.DownloadString(completeUrl);
            dynamic dist = JObject.Parse(result);
            decimal calculatedDistance_decimal = Convert.ToDecimal(dist.distance.Value);

            return calculatedDistance_decimal;
        }

        private List<Member> SearchScreenName(FormCollection form, List<Member> members)
        {
            string screenName = form["screenName"];
            if (screenName != null)
            {
                ViewBag.screenName = screenName;
                members = members.Where(m => m.ScreenName.Contains(screenName)).ToList<Member>();
            }
            return members;
        }

        private List<Member> SearchHasPics(FormCollection form, List<Member> members)
        {
            var hasPics = form["hasPics"];
            if (hasPics != null && hasPics == "on")
            {
                members = members.Where(m => m.MemberMedia.ToList().Count() > 0).ToList<Member>();
                ViewBag.hasPics = "checked";
            }
            return members;
        }

        private List<Member> SearchYearsTogether(FormCollection form, List<Member> members)
        {
            int yearsTogether = Convert.ToInt16(form["yearsTogether"]);
            if (yearsTogether > 0)
            {
                int yearMetStart = DateTime.Now.Year - yearsTogether;
                ViewBag.yearsTogether = yearsTogether;

                members = members.Where(x => Convert.ToInt16(x.YearMet) <= yearMetStart).ToList();
            }
            return members;
        }

        private List<Member> SearchAges(string ages, int startYear, int endYear)
        {
            List<Member> members = db.Member.Include(t => t.MemberStatus).Where(m =>
                (((int?)m.DOB1.Value.Year ?? startYear) <= startYear && ((int?)m.DOB1.Value.Year ?? endYear) >= endYear)
                && (((int?)m.DOB2.Value.Year ?? startYear) <= startYear && ((int?)m.DOB2.Value.Year ?? endYear) >= endYear)
                ).ToList<Member>();

            ViewBag.ageRange = ages;

            //remove members where both dob's are null
            List<Member> toRemove = members.Where(x => x.DOB1 == null && x.DOB2 == null).ToList<Member>();

            foreach (var i in toRemove)
            {
                members.Remove(i);
            }
            return members;
        }

        private List<Member> SearchGenders(FormCollection form, List<Member> members)
        {
            string M = form["M"];
            string F = form["F"];

            if (M == "on" && F != "on")
            {
                //both Males
                members = members.Where(m => m.Gender1 == "M" && m.Gender2 == "M").ToList();
                ViewBag.isMale = "checked";
                ViewBag.isFemale = null;
            }

            if (M != "on" && F == "on")
            {
                //both Females
                members = members.Where(m => m.Gender1 == "F" && m.Gender2 == "F").ToList();
                ViewBag.isMale = null;
                ViewBag.isFemale = "checked";
            }

            if (M == "on" && F == "on")
            {
                //Male and Female
                members = members.Where(m => (m.Gender1 == "M" && m.Gender2 == "F") || (m.Gender1 == "F" && m.Gender2 == "M")).ToList();
                ViewBag.isMale = "checked";
                ViewBag.isFemale = "checked";
            }

            if (M != "on" && F != "on")
            {
                //Any combo Male/Female
                //do nothing
                ViewBag.isMale = null;
                ViewBag.isFemale = null;
            }

            return members;
        }

        [ChildActionOnly]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public PartialViewResult ListCouples(int howMany)
        {
            var Member = db.Member.Include(t => t.MemberStatus).Take(3);

            return PartialView("_CouplesList", Member.ToList());
        }

        // GET: Members/Details/5
        [Authorize]
        //[OutputCache(CacheProfile = "ByID")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member Member = db.Member.Find(id);
            if (Member == null)
            {
                return HttpNotFound();
            }

            //return q&a
            List<QuestionAnswers> memberAnswers = Member.QuestionAnswers.ToList<QuestionAnswers>();
            ViewBag.memberAnswers = memberAnswers;
            return View(Member);
        }

        // GET: Members/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.StatusID = new SelectList(db.MemberStatus, "ID", "Description");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,FirstName1,LastName1,DOB1,Gender1,FirstName2,LastName2,DOB2,Gender2,Address,City,State,Zipcode,StatusID,UserId,YearMet,ScreenName")] Member Member, HttpPostedFileBase file, FormCollection fc)
        {
            Member.UserId = config.UID;
            int year1 = Convert.ToInt16(fc.GetValues("DOB1.years")[0]);
            var month1 = Convert.ToInt16(fc.GetValues("DOB1.months")[0]);
            var day1 = Convert.ToInt16(fc.GetValues("DOB1.days")[0]);

            DateTime dt1 = new DateTime(year1, month1, day1);
            Member.DOB1 = dt1;

            int year2 = Convert.ToInt16(fc.GetValues("DOB2.years")[0]);
            var month2 = Convert.ToInt16(fc.GetValues("DOB2.months")[0]);
            var day2 = Convert.ToInt16(fc.GetValues("DOB2.days")[0]);

            DateTime dt2 = new DateTime(year2, month2, day2);
            Member.DOB2 = dt2;
            Member.City = Member.City.ToUpper();
            Member.State = Member.State.ToUpper();
            Member.DateCreated = Convert.ToDateTime(DateTime.Now);

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                //save member record
                db.Member.Add(Member);
                db.SaveChanges();

                //SAVE INTERESTS
                SaveInterests(Member, fc);

                //save physical pic to images location
                ProcessAndUploadPicture(Member, file);
                ThisMember = Member;

                //return RedirectToAction("Edit");
                return View("ProfileCompletionThankYou");
            }
            else
            {
                ThisMember = Member;
                ViewBag.StatusID = new SelectList(db.MemberStatus, "ID", "Description", ThisMember.StatusID);
                return View(Member);
            }
        }

        // GET: Members/Edit/5
        [Authorize]
        public ActionResult Edit()
        {
            if (config.UID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Member member = new Member();
                member = db.Member.Where(m => m.UserId == config.UID).Include("MemberMedia").FirstOrDefault();
                if (member != null)
                {
                    ViewBag.StatusID = new SelectList(db.MemberStatus, "ID", "Description", member.StatusID);
                    return View(member);
                }
                else
                {
                    ViewBag.StatusID = new SelectList(db.MemberStatus, "ID", "Description");
                    return View();
                }
            }
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,FirstName1,LastName1,DOB1,Gender1,FirstName2,LastName2,DOB2,Gender2,Address,City,State,Zipcode,StatusID,UserId,YearMet,ScreenName,Tagline,OurStory,DateCreated")] Member Member, HttpPostedFileBase file, FormCollection fc)
        {
            Member.UserId = config.UID;
            int year1 = Convert.ToInt16(fc.GetValues("DOB1.years")[0]);
            var month1 = Convert.ToInt16(fc.GetValues("DOB1.months")[0]);
            var day1 = Convert.ToInt16(fc.GetValues("DOB1.days")[0]);

            DateTime dt1 = new DateTime(year1, month1, day1);
            Member.DOB1 = dt1;

            int year2 = Convert.ToInt16(fc.GetValues("DOB2.years")[0]);
            var month2 = Convert.ToInt16(fc.GetValues("DOB2.months")[0]);
            var day2 = Convert.ToInt16(fc.GetValues("DOB2.days")[0]);

            DateTime dt2 = new DateTime(year2, month2, day2);
            Member.DOB2 = dt2;
            Member.City = Member.City.ToUpper();
            Member.State = Member.State.ToUpper();
            Member.DateCreated =Convert.ToDateTime(Member.DateCreated);
            Member.DateModified = DateTime.Now;

            //check modelstate
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                //save member record
                db.Entry(Member).State = EntityState.Modified;
                db.SaveChanges();

                //SAVE INTERESTS
                SaveInterests(Member, fc);

                //save physical pic to images location
                string resultMessage = ProcessAndUploadPicture(Member, file);
                ViewBag.result = resultMessage;
                ThisMember = Member;

                //return RedirectToAction("Edit");
                return View("ProfileCompletionThankYou");
            }
            else
            {
                ThisMember = Member;
                ViewBag.StatusID = new SelectList(db.MemberStatus, "statusID", "statusDescription", Member.StatusID);

                return View(Member);
            }
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member Member = db.Member.Find(id);
            if (Member == null)
            {
                return HttpNotFound();
            }
            return View(Member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            Member Member = db.Member.Find(id);
            db.Member.Remove(Member);
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

        private void SaveInterests(Member Member, FormCollection fc)
        {
            try
            {
                string[] interestSelections = fc.GetValues("interests");

                //remove interests
                List<MemberInterest> toRemove = db.MemberInterest.Where(m => m.MemberID == Member.ID || m.MemberID == null).ToList<MemberInterest>();

                db.MemberInterest.RemoveRange(toRemove);

                //add interests
                for (int i = 0; i < interestSelections.Count(); i++)
                {
                    MemberInterest mi = new MemberInterest();
                    mi.MemberID = (long)Member.ID;
                    mi.InterestID = Convert.ToInt16(interestSelections[i]);
                    db.MemberInterest.Add(mi);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }

        private string ProcessAndUploadPicture(Member Member, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string _fileName = Path.GetFileName(file.FileName);
                string _fileExt = _fileName.Substring(_fileName.Length - 4);

                //Process Picture
                //from http://www.codeproject.com/Tips/481015/Rename-Resize-Upload-Image-ASP-NET-MVC
                ImageUpload imageUpload = new ImageUpload { Width = 600 }; //set Width here
                //Upload Picture
                ImageResult imageResult = imageUpload.RenameUploadFile(file);

                if (imageResult.Success)
                {
                    //TODO: write the filename to the db
                    //save path for image to membermedia in db
                    MemberMedia mm = new MemberMedia();
                    mm.ID = (int)Member.ID;
                    string uploadPath = config.PhotoUploadPath;
                    mm.path = uploadPath + imageResult.ImageName;

                    Member.MemberMedia.Add(mm);
                    db.SaveChanges();
                    return "Successfully Uploaded Image";
                }
                else
                {
                    // use imageResult.ErrorMessage to show the error
                    return "Failed to Upload Image";
                }
            }
            else
            {
                return "File is empty";
            }
        }

        //private void SavePicToImages(HttpPostedFileBase file, long _ID, ref string _path, string _serverPath, ref string _fileName, string _fileExt)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        try
        //        {
        //            _fileName = _ID.ToString() + "_pImage_1" + _fileExt;

        //            _path = Path.Combine(_serverPath, _fileName);
        //            int i = 1;

        //            while (System.IO.File.Exists(_path))
        //            {
        //                //file with this name already exists
        //                int imgIDStart = _fileName.IndexOf("_pImage_") + 8;
        //                _fileName = _fileName.Substring(0, imgIDStart) + i.ToString() + _fileExt;

        //                //_fileName = _fileName.Substring()
        //                _path = Path.Combine(_serverPath, _fileName);

        //                i += 1;
        //            }

        //            file.SaveAs(_path);
        //            ViewBag.Message = "Pic uploaded successfully";
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Message = "Error:" + ex.Message.ToString();
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Message = "You have not specified a file.";
        //    }
        //}

        // GET: Members/Delete/5
    }
}