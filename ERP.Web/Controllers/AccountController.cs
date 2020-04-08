using ERP.Entity;
using ERP.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Web.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        public ERPDbEntities db = new ERPDbEntities();



        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = "";
            var result = db.tbl_MstMerchants.Where(x => x.Email_Primary == model.UserName || x.Email_Primary == model.UserName && x.PasswordHash == model.Password).FirstOrDefault();
            if (result.IsActive == 1)
                return RedirectToLocal(returnUrl);
            else
                ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}