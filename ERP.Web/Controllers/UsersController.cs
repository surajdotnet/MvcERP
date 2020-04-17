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

namespace ERP.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private ERPDbEntities db = new ERPDbEntities();
        // GET: Users
        public ActionResult Index()
        {
            using (ERPDbEntities db = new ERPDbEntities())
            {
                ViewBag.Result = db.tbl_MstMerchants.ToList();
                //ViewBag.Result = (from t1 in db.tbl_MstMerchants
                //                  select new
                //                  {
                //                      pkMerchantId = t1.pkMerchantId,
                //                      FirstName = t1.FirstName,
                //                      LastName = t1.LastName,
                //                      EmployeeCode = t1.EmployeeCode,
                //                       Department = t1.Department,
                //                      Designation = t1.Designation,
                //                      Email_Primary = t1.Email_Primary,
                //                      Mobile_Primary = t1.Mobile_Primary,
                //                      MerchantImage = t1.MerchantImage,
                //                      RoleList = t1.tbl_MstMerchantsRoleXref.Select(t2 => t2.tbl_MstRoles.RoleName).ToList()
                //                  }
                //                ).ToList();
                return View(ViewBag.Result);
            }

        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.ActiveUsers = (from t1 in db.tbl_MstMerchants
                                   where t1.IsActive == 1
                                   select new
                                   {
                                       t1.pkMerchantId,
                                       t1.FullName
                                   }
                                 ).ToList();
            ViewBag.ActiveRoles = db.tbl_MstRoles.Where(x => x.IsActive == 1).ToList();
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(FormCollection form, HttpPostedFileBase postedFile)
        {
            try
            {
                // TODO: Add insert logic here
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

                }

                tbl_MstMerchants obj = new tbl_MstMerchants();
                obj.Title = form["Title"];
                obj.FirstName = form["FirstName"];
                obj.LastName = form["LastName"];
                obj.FullName = form["FirstName"] + " " + form["LastName"];
                obj.Gender = form["Gender"];
                obj.EmployeeCode = form["EmployeeCode"];
                obj.Department = form["Department"];
                obj.Designation = form["Designation"];
                obj.fkBossId = Convert.ToInt32(form["fkBossId"]);
                obj.Mobile_Primary = form["Mobile_Primary"];
                obj.Mobile_Alternate = form["Mobile_Alternate"];
                obj.Email_Primary = form["Email_Primary"];
                obj.Email_Alternate = form["Email_Alternate"];
                obj.MerchantImage = DbPath;

                DateTime dt = DateTime.ParseExact(form["DateofBirth"], "mm/dd/yyyy", CultureInfo.InvariantCulture);
                obj.DateofBirth = Convert.ToDateTime(dt.ToString("yyyy-mm-dd"));

                DateTime dt2 = DateTime.ParseExact(form["DateofJoining"], "mm/dd/yyyy", CultureInfo.InvariantCulture);
                obj.DateofJoining = Convert.ToDateTime(dt2.ToString("yyyy-mm-dd"));

                obj.CreatedBy = Convert.ToInt32(User.Identity.Name);
                obj.CreatedDate = DateTime.Now;
                obj.IsActive = Convert.ToInt32(form["IsActive"]);
                obj.PasswordHash = form["PasswordHash"];
                db.tbl_MstMerchants.Add(obj);
                db.SaveChanges();
                int id = obj.pkMerchantId;

                string RoleIds = form["RoleIds"];
                string[] values = RoleIds.Split(',');
                int isdef = 0;
                for (int i = 0; i < values.Length; i++)
                {

                    tbl_MstMerchantsRoleXref obj2 = new tbl_MstMerchantsRoleXref();
                    obj2.fkMerchantId = id;
                    values[i] = values[i].Trim();
                    obj2.fkRoleId = Convert.ToInt32(values[i]);

                    if (isdef != 1)
                        obj2.IsDefaultRole = 1;

                    obj2.CreatedBy = Convert.ToInt32(User.Identity.Name);
                    obj2.CreatedDate = DateTime.Now;
                    obj2.IsActive = 1;
                    db.tbl_MstMerchantsRoleXref.Add(obj2);
                    db.SaveChanges();
                    isdef = 1;
                }

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
                var loginUrl = "/Account/Login";
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, loginUrl);
                body = body.Replace("{LoginUrl}", link);
                body = body.Replace("{Email}", obj.Email_Primary);
                body = body.Replace("{Mobile}", obj.Mobile_Primary);
                body = body.Replace("{Password}", obj.PasswordHash);
                em.MessageBody = body;
                em.SendEmail();


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.ActiveUsers = (from t1 in db.tbl_MstMerchants
                                   where t1.IsActive == 1
                                   select new
                                   {
                                       t1.pkMerchantId,
                                       t1.FullName
                                   }
                                 ).ToList();
            ViewBag.ActiveRoles = db.tbl_MstRoles.Where(x => x.IsActive == 1).ToList();



            ViewBag.Result = db.tbl_MstMerchants.Where(x => x.pkMerchantId == id).FirstOrDefault();
            var ResultRoles = db.tbl_MstMerchantsRoleXref.Where(x => x.fkMerchantId == id && x.IsActive == 1).ToList();
            List<string> RoleList = new List<string>();
            foreach (var item in ResultRoles)
            {
                //using string builder with comma separated
                RoleList.Add(Convert.ToString(item.fkRoleId));
            }
            string RoleString = String.Join(",", RoleList.ToArray());
            ViewBag.RoleString = RoleString;
            return View();
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection form, HttpPostedFileBase postedFile)
        {
            try
            {
                // TODO: Add update logic here
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
                }
                obj.Title = form["Title"];
                obj.FirstName = form["FirstName"];
                obj.LastName = form["LastName"];
                obj.FullName = form["FirstName"] + " " + form["LastName"];
                obj.Gender = form["Gender"];
                obj.EmployeeCode = form["EmployeeCode"];
                obj.Department = form["Department"];
                obj.Designation = form["Designation"];
                obj.fkBossId = Convert.ToInt32(form["fkBossId"]);
                obj.Mobile_Primary = form["Mobile_Primary"];
                obj.Mobile_Alternate = form["Mobile_Alternate"];
                obj.Email_Primary = form["Email_Primary"];
                obj.Email_Alternate = form["Email_Alternate"];


                DateTime dt = DateTime.ParseExact(form["DateofBirth"], "mm/dd/yyyy", CultureInfo.InvariantCulture);
                obj.DateofBirth = Convert.ToDateTime(dt.ToString("yyyy-mm-dd"));

                DateTime dt2 = DateTime.ParseExact(form["DateofJoining"], "mm/dd/yyyy", CultureInfo.InvariantCulture);
                obj.DateofJoining = Convert.ToDateTime(dt2.ToString("yyyy-mm-dd"));

                obj.ModifiedBy = Convert.ToInt32(User.Identity.Name);
                obj.ModifedDate = DateTime.Now;
                obj.IsActive = Convert.ToInt32(form["IsActive"]);
                obj.PasswordHash = form["PasswordHash"];
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();

                //Deleting Old Roles
                var ExistingRoles = db.tbl_MstMerchantsRoleXref.Where(x => x.fkMerchantId == id && x.IsActive == 1).ToList();
                foreach (var item in ExistingRoles)
                {
                    tbl_MstMerchantsRoleXref Ex_Role = db.tbl_MstMerchantsRoleXref.Where(x => x.pkId == item.pkId).FirstOrDefault();
                    Ex_Role.ModifiedBy = Convert.ToInt32(User.Identity.Name);
                    Ex_Role.ModifiedDate = DateTime.Now;
                    Ex_Role.IsActive = 0;
                    db.Entry(Ex_Role).State = EntityState.Modified;
                    db.SaveChanges();
                }

                //Adding New Roles
                string RoleIds = form["RoleIds"];
                string[] values = RoleIds.Split(',');
                int isdef = 0;
                for (int i = 0; i < values.Length; i++)
                {

                    tbl_MstMerchantsRoleXref obj2 = new tbl_MstMerchantsRoleXref();
                    obj2.fkMerchantId = id;
                    values[i] = values[i].Trim();
                    obj2.fkRoleId = Convert.ToInt32(values[i]);

                    if (isdef != 1)
                        obj2.IsDefaultRole = 1;

                    obj2.CreatedBy = Convert.ToInt32(User.Identity.Name);
                    obj2.CreatedDate = DateTime.Now;
                    obj2.IsActive = 1;
                    db.tbl_MstMerchantsRoleXref.Add(obj2);
                    db.SaveChanges();
                    isdef = 1;
                }


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
