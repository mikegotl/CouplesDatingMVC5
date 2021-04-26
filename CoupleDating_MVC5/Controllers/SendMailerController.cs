using System.Net.Mail;
using System.Net.Mime;
using System.Web.Hosting;
using System.Web.Mvc;

namespace CoupleDating_MVC5.Controllers
{
    public class SendMailerController : Controller
    {
        //
        // GET: /SendMailer/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Index(CoupleDating_MVC5.Models.MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                SendEmail(_objModelMail);
                return View("Index", _objModelMail);
            }
            else
            {
                return View();
            }
        }

        public static void SendEmail(CoupleDating_MVC5.Models.MailModel _objModelMail)
        {
            MailMessage mail = PrepareMailObject(_objModelMail);

            //prepare smtp send
            PrepareSMTPSend(mail);
        }

        public static void SendEmail(CoupleDating_MVC5.Models.MailModel _objModelMail, string attachFilePath)
        {
            MailMessage mail = PrepareMailObject(_objModelMail);
            mail.IsBodyHtml = true;

            //Create Attachment
            try
            {
                string attachmentPath = HostingEnvironment.MapPath(attachFilePath);
                LinkedResource profilePic = new LinkedResource(attachmentPath);
                profilePic.ContentId = "profPic";

                AlternateView av1 = AlternateView.CreateAlternateViewFromString(
                    "<span><img src=cid:profPic style='width:200px;'></span>" + _objModelMail.Body, null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(profilePic);
                mail.AlternateViews.Add(av1);
            }
            catch (System.Exception)
            {
            }

            //prepare smtp send
            PrepareSMTPSend(mail);
        }

        private static MailMessage PrepareMailObject(CoupleDating_MVC5.Models.MailModel _objModelMail)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(_objModelMail.To);
            mail.From = new MailAddress("mikegotl@gmail.com");
            mail.Subject = _objModelMail.Subject;
            string Body = _objModelMail.Body;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            return mail;
        }

        private static void PrepareSMTPSend(MailMessage mail)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            ("mikegotl", "1136newlife");// Enter seders User name and password
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(mail);
            }
            catch (System.Exception ex)
            {
                var error = ex.Message;
            }
        }
    }
}