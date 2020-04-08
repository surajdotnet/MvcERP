using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Web.Controllers
{
    [AllowAnonymous]
    public class DashbordController : Controller
    {
        // GET: Dashbord
        public ActionResult Index()
        {
            return View();
        }
    }
}