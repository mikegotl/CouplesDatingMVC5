using CoupleDating_MVC5.Models;
using System.Linq;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private object limit;

        public DBEntities db = new DBEntities();
        private Config config = new Config();

        [Authorize]
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                //user authenticated
                //check if has profile (Member) record
                if (config.LoggedInMember == null)
                {
                    //has no profile. redirect to member/create
                    return RedirectToAction("Create", "Members");
                }
                else
                {
                    if (config.LoggedInMember.City == null || config.LoggedInMember.City == "")
                    {
                        return RedirectToAction("Edit", "Members");
                    }
                    //has profile render places from Google Places API
                    //String _city;
                    //_city = Config.LoggedInMember.City;

                    //string _photoCat = Request.QueryString["photoCat"];

                    //ViewBag.lastUpdated = RenderGPlacesHome(_city, _photoCat);
                }
            }
            else
            {
                //user is not authenticated
                return RedirectToAction("login", "account");
            }

            ViewBag.articles = db.Article.ToList();


            //return View();

            return RedirectToAction("Edit", "Members");
        }

        //private string RenderGPlacesHome(string City, string photoCat)
        //{
        //    GoogleAPIs gapi = new GoogleAPIs();
        //    string location = City;
        //    ViewBag.location = location;

        //    //string photoCat = Request.QueryString["photoCat"];

        //    string strQuery;

        //    if (String.IsNullOrEmpty(photoCat))
        //    {
        //        strQuery = "nightlife+in+" + location;
        //    }
        //    else
        //    {
        //        strQuery = photoCat + "+in+" + location;
        //        ViewBag.photoCat = photoCat;
        //    }

        //    int limit = 20;

        //    try
        //    {
        //        var lastPage = Request.UrlReferrer;

        //        if (Session["places"] == null || lastPage.LocalPath == "/Members/Edit" || photoCat != null)
        //        {
        //            //Fresh load from google places api
        //            FreshGPlacesLoad(gapi, strQuery, limit);
        //        }
        //        else
        //        {
        //            ViewBag.places = Session["places"];
        //        }
        //    }
        //    catch (System.Exception)
        //    {
        //        //Fresh load from google places api
        //        FreshGPlacesLoad(gapi, strQuery, limit);
        //    }
        //    return DateTime.Now.ToString("T");
        //}

        //private void FreshGPlacesLoad(GoogleAPIs gapi, string strQuery, int limit)
        //{
        //    List<GPlace> places = gapi.GetPlaces(strQuery, limit);
        //    ViewBag.places = places;
        //    Session["places"] = places;
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Couples Double-Dating Site.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}