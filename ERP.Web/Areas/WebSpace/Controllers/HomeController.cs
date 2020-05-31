using ERP.Entity;
using ERP.Entity.Models;
using ERP.Entity.Models.Extended;
using ERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ERP.Web.Areas.WebSpace.Controllers
{
    public class HomeController : Controller
    {
        public ERPDbEntities db = new ERPDbEntities();
        // GET: WebSpace/Home
        public ActionResult Index()
        
        {
            ViewBag.Result_Category = db.tbl_MstCategory.Where(x => x.IsActive == 1).ToList();
            var items = db.tbl_Mst_Advertise.Where(x => x.IsActive == 1).ToList();
            ViewBag.TopTenAds = items;
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.ReturnUrl = returnUrl;
               return RedirectToLocal(returnUrl);
            }
            else
            {
                return View();
            }
           
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel form, string returnUrl)
        {
            try
            {
                var user = db.tbl_MstMerchants.Where(x => x.Email_Primary == form.UserName || x.Mobile_Primary == form.UserName && x.PasswordHash == form.Password).FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(Convert.ToString(user.pkMerchantId), form.RememberMe);
                    Session["User"] = user.pkMerchantId;
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    TempData["Error"] = "Your Email/Password is in correct";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error"+ex.Message;
                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserDetail form)
        {
            string email = Convert.ToString(form.Email_Primary);
            var user = db.tbl_MstMerchants.Where(x => x.Email_Primary == email).FirstOrDefault();
            if (user == null)
            {
                string mobile = Convert.ToString(form.Mobile_Primary);
                var mob = db.tbl_MstMerchants.Where(x => x.Mobile_Primary == mobile).FirstOrDefault();
                if (mob == null)
                {
                    try
                    {
                        tbl_MstMerchants obj = new tbl_MstMerchants();
                        obj.FullName = form.FullName;
                        obj.Mobile_Primary = form.Mobile_Primary;
                        obj.Email_Primary = form.Email_Primary;

                        obj.CreatedDate = DateTime.Now;
                        obj.IsActive = 1;
                        Random generator = new Random();
                        String pwd = generator.Next(0, 999999).ToString("D6");
                        obj.PasswordHash = pwd;
                        db.tbl_MstMerchants.Add(obj);
                        db.SaveChanges();
                        int id = obj.pkMerchantId;

                        var Roles = db.tbl_MstRoles.Where(x => x.RoleName == "Customer").FirstOrDefault();
                        tbl_MstMerchantsRoleXref obj2 = new tbl_MstMerchantsRoleXref();
                        obj2.fkMerchantId = id;
                        obj2.fkRoleId = Roles.pkId;
                        obj2.CreatedDate = DateTime.Now;
                        obj2.IsActive = 1;
                        db.tbl_MstMerchantsRoleXref.Add(obj2);
                        db.SaveChanges();

                        tbl_AdvertisePlansXref plan = new tbl_AdvertisePlansXref();
                        plan.fkMerchantId= id;
                        plan.fkPlanId = 1;//Free Plan
                        plan.StartDate = DateTime.Now;
                        plan.CreatedBy = id;
                        plan.CreatedDate = DateTime.Now;
                        plan.IsActive = 1;
                        db.tbl_AdvertisePlansXref.Add(plan);
                        db.SaveChanges();

                        EmailManager em = new EmailManager();
                        em.To = obj.Email_Primary;
                        em.Subject = "your account created on ERP";
                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplates/UserDetail.html")))
                        {
                            body = reader.ReadToEnd();
                        }

                        body = body.Replace("{Message}", "Welcome: You're account on ERP has been created successfully.");
                        body = body.Replace("{UserName}", obj.FullName);
                        var loginUrl = "/WebSpace/Home/Login";
                        var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, loginUrl);
                        body = body.Replace("{LoginUrl}", link);
                        body = body.Replace("{Email}", obj.Email_Primary);
                        body = body.Replace("{Mobile}", obj.Mobile_Primary);
                        body = body.Replace("{Password}", obj.PasswordHash);
                        em.MessageBody = body;
                        em.SendEmail();

                        SMSManager sm = new SMSManager();
                        sm.ToMobile = form.Mobile_Primary;

                        string Mbody = "";
                        Mbody += "Dear " +form.FullName + ",";
                        Mbody += "Welcome To WebSpace Your account details are,";
                        Mbody += "Email: " + form.Email_Primary;
                        Mbody += "Password: " + pwd;
                        sm.MessageBody = Mbody;
                        sm.SendMessage();
                        Session["User"] = id;
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = "Error Occures:" + ex.Message;
                        return View(form);
                    }
                }
                else
                {
                    TempData["Error"] = "Error Occures: A user is already exists in the system with this mobile no";
                    return View(form);
                }

            }
            else
            {
                TempData["Error"] = "Error Occures: A user is already exists in the system with this email address";
                return View(form);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
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
        [Authorize]
        public ActionResult Account()
        {
            int id = Convert.ToInt32(User.Identity.Name);
            tbl_MstMerchants obj= db.tbl_MstMerchants.Find(id);
            return View(obj);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Account(tbl_MstMerchants obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Success"] = "Profile Updated Successfully";
                    return View();
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
              
            
            }
            return View(obj);
        }
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel obj)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(User.Identity.Name);
                var user = db.tbl_MstMerchants.Where(x => x.pkMerchantId == id).FirstOrDefault();
                if (user.PasswordHash == obj.OldPassword)
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
    }
}