using ERP.Entity;
using ERP.Entity.Models;
using ERP.Entity.Models.Extended;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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
            var result = db.tbl_MstMerchants.Where(x =>( x.Email_Primary == model.UserName || x.Mobile_Primary == model.UserName) && x.PasswordHash == model.Password && x.IsActive==1).FirstOrDefault();
           
            if (result != null)
            {
                FormsAuthentication.SetAuthCookie(Convert.ToString(result.pkMerchantId), model.RememberMe);
                Session["User"] = result;
                return RedirectToLocal(returnUrl);
            }
            else
            {

                ModelState.AddModelError("", "No account associated with this uername/password");
            }
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
                return RedirectToAction("MyAccount", "Account");
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
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel obj)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(User.Identity.Name);
                var user = db.tbl_MstMerchants.Where(x => x.pkMerchantId == id).FirstOrDefault();
                if (user.PasswordHash==obj.OldPassword)
                {
                    user.PasswordHash = obj.NewPassword;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    TempData["Success"] = "Your Password has been changed successfully";
                }
                else
                {
                    TempData["Error"] = "Please enter your correct current password.";
                }
            }
            else
            {
                TempData["Error"] = "Somthing went wrong";
            }
            return View(obj);

        }

        [AllowAnonymous]
        public ActionResult RecoverPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RecoverPassword(string EmailID)
        {
            var result = db.tbl_MstMerchants.Where(x => x.Email_Primary == EmailID && x.IsActive == 1).FirstOrDefault();
            if (result != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                var resetUrl = "/Account/ResetPassword/" + resetCode;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, resetUrl);

                result.PasswordResetCode = resetCode;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                EmailManager em = new EmailManager();
                em.To = EmailID;
                em.Subject = "Password Reset Link:: ERP";
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplates/passwordreset.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{UserName}", result.FullName);
                body = body.Replace("{Url}", link);
                em.MessageBody = body;
                em.SendEmail();

                TempData["Success"] = "Password Reset instruction has been sent to your email id";
                return View();

            }
            else
            {
                TempData["Error"] = "Their is no account associated with this email id";
                return View();
            }

        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //find account associated with it
            //redirect to reset password page

            var user = db.tbl_MstMerchants.Where(x => x.PasswordResetCode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel obj)
        {
            if (ModelState.IsValid)
            {
                var user = db.tbl_MstMerchants.Where(x => x.PasswordResetCode == obj.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.PasswordHash = obj.NewPassword; //Crypto.Hash(obj.NewPassword);
                    user.PasswordResetCode = "";
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();

                    EmailManager em = new EmailManager();
                    em.To = user.Email_Primary;
                    em.Subject = "Password Changed:: ERP";
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplates/UserDetail.html")))
                    {
                        body = reader.ReadToEnd();
                    }

                    body = body.Replace("{Message}", "You're password on ERP has been Changed successfully.");
                    body = body.Replace("{UserName}", user.FullName);
                    var loginUrl = "/Account/Login";
                    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, loginUrl);
                    body = body.Replace("{LoginUrl}", link);
                    body = body.Replace("{Email}", user.Email_Primary);
                    body = body.Replace("{Mobile}", user.Mobile_Primary);
                    body = body.Replace("{Password}", user.PasswordHash);
                    em.MessageBody = body;
                    em.SendEmail();
                    TempData["Success"] = "Your Password has been changed successfully";
                }
            }
            else
            {
                TempData["Error"] = "Somthing went wrong";
            }
            return View(obj);
        }

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