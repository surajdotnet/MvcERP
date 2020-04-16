using ERP.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Web.Controllers
{[Authorize]
    public class ControlPannelController : Controller
    {
        private ERPDbEntities db = new ERPDbEntities();
        // GET: ControlPannel
        public ActionResult EmailConfiguration()
        {
            ViewBag.Result = db.tbl_MstEmails.ToList();
            return View();
        }
    }
}