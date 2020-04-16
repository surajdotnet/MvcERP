using ERP.Entity;
using ERP.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
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
            var result = db.tbl_MstMerchants.Where(x => x.Email_Primary == model.UserName || x.Mobile_Primary == model.UserName && x.PasswordHash == model.Password).FirstOrDefault();
            FormsAuthentication.SetAuthCookie(Convert.ToString(result.pkMerchantId), model.RememberMe);
            Session["User"] = result;
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
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult MyAccount()
        {
            if (User.Identity.IsAuthenticated)
            {
                int id = Convert.ToInt32(User.Identity.Name);
                //ViewBag.Result = (from t1 in db.tbl_MstMerchants
                //                  join t2 in db.tbl_MstMerchants on t1.fkBossId equals t2.pkMerchantId
                //                  select new
                //                  {
                //                      t1.pkMerchantId,
                //                      t1.FirstName,
                //                      t1.LastName,
                //                      t1.FullName,
                //                      t1.Gender,
                //                      t1.EmployeeCode,
                //                      t1.Department,
                //                      t1.Designation,
                //                      t1.Email_Primary,
                //                      t1.Mobile_Primary,
                //                      t1.Mobile_Alternate,
                //                      t1.MerchantImage,
                //                      t1.DateofBirth,
                //                      t1.DateofJoining,
                //                      ReportingManager = t2.FullName
                //                  }
                //                ).ToList();
                ViewBag.Result = db.tbl_MstMerchants.Where(x => x.pkMerchantId == id).FirstOrDefault();
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        [HttpPost]
        public ActionResult UpdateProfile(FormCollection form)
        {
            try
            {
                int id = Convert.ToInt32(User.Identity.Name);
                tbl_MstMerchants obj = db.tbl_MstMerchants.Where(x => x.pkMerchantId == id).FirstOrDefault();
                obj.Title = form["Title"];
                obj.FirstName = form["FirstName"];
                obj.LastName = form["LastName"];
                obj.FullName = form["FirstName"] + " " + form["LastName"];
                obj.Gender = form["Gender"];
                obj.EmployeeCode = form["EmployeeCode"];
                obj.Department = form["Department"];
                obj.Designation = form["Designation"];
                obj.Mobile_Alternate = form["Mobile_Alternate"];
                obj.Email_Alternate = form["Email_Alternate"];

                DateTime dt = DateTime.ParseExact(form["DateofBirth"], "mm/dd/yyyy", CultureInfo.InvariantCulture);
                obj.DateofBirth = Convert.ToDateTime(dt.ToString("yyyy-mm-dd"));

               
                obj.ModifiedBy = Convert.ToInt32(User.Identity.Name);
                obj.ModifedDate = DateTime.Now;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Profile Updated Successfully!";
                return RedirectToAction("MyAccount","Account");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Occured: " + ex.ToString();
                return RedirectToAction("MyAccount", "Account");
            }
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult UpdateProfilePhoto(HttpPostedFileBase postedFile)
        {
            
                // TODO: Add update logic here
                int id = Convert.ToInt32(User.Identity.Name);
                tbl_MstMerchants obj = db.tbl_MstMerchants.Where(x => x.pkMerchantId == id).FirstOrDefault();
                var DbPath = "~/Uploads/MerchantImages/noimage.jpg";
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/MerchantImages/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var filename = DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss") + "_" + Path.GetFileName(postedFile.FileName);
                    // postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                    DbPath = "~/Uploads/MerchantImages/" + filename;
                    postedFile.SaveAs(path + filename);
                    obj.MerchantImage = DbPath;
                    obj.ModifiedBy = Convert.ToInt32(User.Identity.Name);
                    obj.ModifedDate = DateTime.Now;

                    try
                    {
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["Success"] = "Photo Updated Successfully!";
                        return RedirectToAction("MyAccount", "Account");
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = "Error Occured: " + ex.ToString();
                        return RedirectToAction("MyAccount", "Account");
                    }
                    
                }
                else
                {
                    TempData["Error"] = "Please select a file";
                    return RedirectToAction("MyAccount", "Account");
                }
            
        }

        public ActionResult ChangePassword()
        {
            if (User.Identity.IsAuthenticated)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult RecoverPassword()
        {
            if (User.Identity.IsAuthenticated)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        //[HttpPost]
        //public ActionResult RecoverPassword(string UserName)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        if (WebSecurity.UserExists(UserName))
        //        {
        //            string To = UserName, UserID, Password, SMTPPort, Host;
        //            string token = WebSecurity.GeneratePasswordResetToken(UserName);
        //            if (token == null)
        //            {
        //                // If user does not exist or is not confirmed.  

        //                return View("Index");

        //            }
        //            else
        //            {
        //                //Create URL with above token  

        //                var lnkHref = "<a href='" + Url.Action("ResetPassword", "Account", new { email = UserName, code = token }, "http") + "'>Reset Password</a>";


        //                //HTML Template for Send email  

        //                string subject = "Your changed password";

        //                string body = "<b>Please find the Password Reset Link. </b><br/>" + lnkHref;


        //                //Get and set the AppSettings uscing configuration manager.  

        //                EmailManager.AppSettings(out UserID, out Password, out SMTPPort, out Host);


        //                //Call send email methods.  

        //                EmailManager.SendEmail(UserID, subject, body, To, UserID, Password, SMTPPort, Host);

        //            }

        //        }

        //    }
        //    return View();
        //}



        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

    }
}