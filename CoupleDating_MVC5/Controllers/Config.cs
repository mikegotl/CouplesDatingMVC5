using CoupleDating_MVC5.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Controllers
{
    public class Config
    {
        public DBEntities db = new DBEntities();

        public string PhotoUploadPath = ConfigurationManager.AppSettings["PhotoUploadPath"];

        private Member thisMember = new Member();

        public Member LoggedInMember
        {
            get
            {
                if (HttpContext.Current.Session["thisMember"] == null)
                {
                    var mm = db.Member.Where(m => m.UserId == UID);
                    thisMember = mm.FirstOrDefault();
                    HttpContext.Current.Session["thisMember"] = thisMember;
                }
                else
                {
                    thisMember = (Member)HttpContext.Current.Session["thisMember"];
                }

                return thisMember;
            }
            set
            {
                HttpContext.Current.Session["thisMember"] = value;
            }
        }

        private string _uID;

        public string UID
        {
            get
            {
                if (_uID == null)
                {
                }
                _uID = System.Web.HttpContext.Current.User.Identity.GetUserId();
                return _uID;
            }
            set
            {
                _uID = value;
            }
        }

        public string GetEmailAddress(long ID)
        {
            AspNetUsers u = (from qq in db.AspNetUsers
                             join mem in db.Member on qq.Id equals mem.UserId
                             where mem.ID == ID
                             select qq).FirstOrDefault();
            return u.Email.ToString();
        }

        public bool isActiveMember
        {
            get
            {
                //if (LoggedInMember.Memberships.Where(m => m.isActive == true && m.endDate >= DateTime.Now && m.effectiveDate <= DateTime.Now).FirstOrDefault() != null)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                return true;
            }
        }

        public void clearThisMember()
        {
            HttpContext.Current.Session["thisMember"] = null;
        }

        public List<SelectListItem> States = new List<SelectListItem>()
    {
        new SelectListItem() {Text="Alabama", Value="AL"},
        new SelectListItem() { Text="Alaska", Value="AK"},
        new SelectListItem() { Text="Arizona", Value="AZ"},
        new SelectListItem() { Text="Arkansas", Value="AR"},
        new SelectListItem() { Text="California", Value="CA"},
        new SelectListItem() { Text="Colorado", Value="CO"},
        new SelectListItem() { Text="Connecticut", Value="CT"},
        new SelectListItem() { Text="District of Columbia", Value="DC"},
        new SelectListItem() { Text="Delaware", Value="DE"},
        new SelectListItem() { Text="Florida", Value="FL"},
        new SelectListItem() { Text="Georgia", Value="GA"},
        new SelectListItem() { Text="Hawaii", Value="HI"},
        new SelectListItem() { Text="Idaho", Value="ID"},
        new SelectListItem() { Text="Illinois", Value="IL"},
        new SelectListItem() { Text="Indiana", Value="IN"},
        new SelectListItem() { Text="Iowa", Value="IA"},
        new SelectListItem() { Text="Kansas", Value="KS"},
        new SelectListItem() { Text="Kentucky", Value="KY"},
        new SelectListItem() { Text="Louisiana", Value="LA"},
        new SelectListItem() { Text="Maine", Value="ME"},
        new SelectListItem() { Text="Maryland", Value="MD"},
        new SelectListItem() { Text="Massachusetts", Value="MA"},
        new SelectListItem() { Text="Michigan", Value="MI"},
        new SelectListItem() { Text="Minnesota", Value="MN"},
        new SelectListItem() { Text="Mississippi", Value="MS"},
        new SelectListItem() { Text="Missouri", Value="MO"},
        new SelectListItem() { Text="Montana", Value="MT"},
        new SelectListItem() { Text="Nebraska", Value="NE"},
        new SelectListItem() { Text="Nevada", Value="NV"},
        new SelectListItem() { Text="New Hampshire", Value="NH"},
        new SelectListItem() { Text="New Jersey", Value="NJ"},
        new SelectListItem() { Text="New Mexico", Value="NM"},
        new SelectListItem() { Text="New York", Value="NY"},
        new SelectListItem() { Text="North Carolina", Value="NC"},
        new SelectListItem() { Text="North Dakota", Value="ND"},
        new SelectListItem() { Text="Ohio", Value="OH"},
        new SelectListItem() { Text="Oklahoma", Value="OK"},
        new SelectListItem() { Text="Oregon", Value="OR"},
        new SelectListItem() { Text="Pennsylvania", Value="PA"},
        new SelectListItem() { Text="Rhode Island", Value="RI"},
        new SelectListItem() { Text="South Carolina", Value="SC"},
        new SelectListItem() { Text="South Dakota", Value="SD"},
        new SelectListItem() { Text="Tennessee", Value="TN"},
        new SelectListItem() { Text="Texas", Value="TX"},
        new SelectListItem() { Text="Utah", Value="UT"},
        new SelectListItem() { Text="Vermont", Value="VT"},
        new SelectListItem() { Text="Virginia", Value="VA"},
        new SelectListItem() { Text="Washington", Value="WA"},
        new SelectListItem() { Text="West Virginia", Value="WV"},
        new SelectListItem() { Text="Wisconsin", Value="WI"},
        new SelectListItem() { Text="Wyoming", Value="WY"}
    };
    }
}