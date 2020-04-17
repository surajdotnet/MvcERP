using ERP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Web.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            EmailManager em = new EmailManager();
            em.To = "surajkumar.nic@gmail.com";
            em.Subject = "your account created on ERP";
            em.MessageBody = "test";
            em.SendEmail();
            return View();
        }
    }
}