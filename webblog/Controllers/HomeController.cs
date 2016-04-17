using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using webblog.Models;

namespace webblog.Controllers
{
    [RequireHttps]
    public class HomeController : ApplicationBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact([Bind(Include = "Id, Name, Email, Message, Phone, MessageSent")] Contact contact)
        {
            contact.MessageSent = DateTime.Now;
            var svc = new EmailService();
            var msg = new IdentityMessage();
            msg.Subject = "Contact From Website";
            msg.Body = contact.Message;

            await svc.SendAsync(msg);

            return View(contact);
        }

    }
}