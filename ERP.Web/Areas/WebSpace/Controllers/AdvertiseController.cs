using ERP.Entity.Models;
using ERP.Entity.Models.Extended;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Web.Areas.WebSpace.Controllers
{[Authorize]
    public class AdvertiseController : Controller
    {
        public ERPDbEntities db = new ERPDbEntities();
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetSubCat(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<tbl_MstSubCategory> subcat = db.tbl_MstSubCategory.Where(x => x.fkCategoryId == id && x.IsActive == 1).ToList();
            return Json(subcat, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PostAd()
        {
            if (User.Identity.IsAuthenticated)
            {
                int id = Convert.ToInt32(User.Identity.Name);
                var Plan = db.tbl_AdvertisePlansXref.Where(x => x.fkMerchantId == id && x.IsActive == 1).FirstOrDefault();
                var Ads_Added = db.tbl_Mst_Advertise.Where(x => x.CreatedBy == id && x.IsActive == 1).ToList();
                if (Ads_Added.Count < Plan.tbl_Mst_AdvertisePlans.ListingLimit)
                    ViewBag.PlanIsValid = 1;
                else
                    ViewBag.PlanIsValid = 0;

                ViewBag.Category = db.tbl_MstCategory.Where(x => x.IsActive == 1).ToList();
                ViewBag.SubCategory = db.tbl_MstSubCategory.Where(x => x.IsActive == 1).ToList();
                return View();
            }
            else
                return RedirectToAction("Login", "Home", new { ReturnUrl = Request.Url.AbsoluteUri });
        }

        [HttpPost]
        public ActionResult PostAd(Advertise obj)
        {
            if (User.Identity.IsAuthenticated)
            {
                tbl_Mst_Advertise obj1 = new tbl_Mst_Advertise();
                obj1.fkSubCategory = obj.fkSubCategory;
                obj1.Add_Id = Guid.NewGuid().ToString();
                obj1.Add_Title = obj.Add_Title;
                obj1.Add_Description = obj.Add_Description;
                obj1.Brand = obj.Brand;
                obj1.Model = obj.Model;
                obj1.Year = obj.Year;
                obj1.Selling_Price = Convert.ToDecimal(obj.Selling_Price);
                if (obj.IsMobileView == true)
                    obj1.IsMobileView = 1;
                else
                    obj1.IsMobileView = 0;
                obj1.Geo_Lat = obj.Geo_Lat;
                obj1.Geo_Lat = obj.Geo_Lat;
                obj1.Geo_Long = obj.Geo_Long;
                obj1.State = obj.State;
                obj1.City = obj.City;
                obj1.Neighbourhood = obj.Neighbourhood;
                obj1.CreatedBy = Convert.ToInt32(User.Identity.Name);
                obj1.CreatedDate = DateTime.Now;
                obj1.IsActive = 0;
                db.tbl_Mst_Advertise.Add(obj1);
                db.SaveChanges();
                return RedirectToAction("UploadPhotos/" + obj1.Add_Id, "Advertise");
            }
            else
            {
                return RedirectToAction("Login", "Home", new { ReturnUrl = Request.Url.AbsoluteUri });
            }

        }

        [HttpGet]
        public ActionResult EditAd(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                
                ViewBag.Category = db.tbl_MstCategory.Where(x => x.IsActive == 1).ToList();

                ViewBag.SubCategory = db.tbl_MstSubCategory.Where(x => x.IsActive == 1).ToList();

                tbl_Mst_Advertise tbl_Mst_Advertise = db.tbl_Mst_Advertise.Where(x => x.Add_Id == id).FirstOrDefault();
                if (tbl_Mst_Advertise == null)
                {
                    return HttpNotFound();
                }
                ViewBag.fkCategoryId = new SelectList(db.tbl_MstCategory, "pkId", "CategoryName", tbl_Mst_Advertise.tbl_MstSubCategory.fkCategoryId);
                ViewBag.fkSubCategory = new SelectList(db.tbl_MstSubCategory, "pkId", "SubCategoryName", tbl_Mst_Advertise.fkSubCategory);
                return View(tbl_Mst_Advertise);
  
        }
            else
            {
                return RedirectToAction("Login", "Home", new { ReturnUrl = Request.Url.AbsoluteUri });
            }

        }
        [HttpPost]
        public ActionResult EditAd(tbl_Mst_Advertise obj, string id)
        {
            if (User.Identity.IsAuthenticated)
            {

                tbl_Mst_Advertise obj1 = db.tbl_Mst_Advertise.Where(x => x.Add_Id == id).FirstOrDefault();
                obj1.fkSubCategory = obj.fkSubCategory;
                obj1.Add_Title = obj.Add_Title;
                obj1.Add_Description = obj.Add_Description;
                obj1.Brand = obj.Brand;
                obj1.Model = obj.Model;
                obj1.Year = obj.Year;
                obj1.Selling_Price = Convert.ToDecimal(obj.Selling_Price);

                obj1.IsMobileView = obj.IsMobileView;
               
                obj1.Geo_Lat = obj.Geo_Lat;
                obj1.Geo_Long = obj.Geo_Long;
                obj1.State = obj.State;
                obj1.City = obj.City;
                obj1.Neighbourhood = obj.Neighbourhood;
                obj1.ModifiedBy = Convert.ToInt32(User.Identity.Name);
                obj1.ModifiedDate = DateTime.Now;
                obj1.IsActive = obj.IsActive;
                db.Entry(obj1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UploadPhotos/" + obj1.Add_Id, "Advertise");
            }
            else
            {
                return RedirectToAction("Login", "Home", new { ReturnUrl = Request.Url.AbsoluteUri });
            }

        }

        public ActionResult UploadPhotos(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var Ad_Result = db.tbl_Mst_Advertise.Where(x => x.Add_Id == id).FirstOrDefault();
                ViewBag.GUID = id;
                ViewBag.Ad_Result = Ad_Result;
                ViewBag.Result = db.tbl_Mst_AdvertiseImageXref.Where(x => x.fkAdd_Id == Ad_Result.Id).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home", new { ReturnUrl = "/WebSpace/Advertise/UploadPhotos/" + id });
            }
        }
        [HttpPost]
        public ActionResult UploadPhotos(List<HttpPostedFileBase> postedFiles, string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var DbPath = "~/Uploads/Advertise/noimage.jpg";
                string path = Server.MapPath("~/Uploads/Advertise/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (postedFiles != null)
                {
                    foreach (HttpPostedFileBase postedFile in postedFiles)
                    {

                        var filename = DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss") + "_" + Path.GetFileName(postedFile.FileName);
                        DbPath = "~/Uploads/Advertise/" + filename;
                        postedFile.SaveAs(path + filename);

                        tbl_Mst_Advertise add = db.tbl_Mst_Advertise.Where(x => x.Add_Id == id).FirstOrDefault();
                        tbl_Mst_AdvertiseImageXref obj = new tbl_Mst_AdvertiseImageXref();
                        obj.fkAdd_Id = add.Id;
                        obj.ImagePath = DbPath;
                        obj.IsActive = 1;
                        db.tbl_Mst_AdvertiseImageXref.Add(obj);
                        db.SaveChanges();

                        add.IsActive = 1;
                        db.Entry(add).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    TempData["Success"] = "Photo Uploded Successfully! and your add is live";
                }
                else
                {
                    TempData["Error"] = "Please select some files!";
                }


                return RedirectToAction("UploadPhotos");
            }
            else
            {
                return RedirectToAction("Login", "Home", new { ReturnUrl = "/WebSpace/Advertise/UploadPhotos/" + id });
            }
        }

        [HttpPost]
        public ActionResult DeletePhoto(FormCollection form, int id)
        {
            using (ERPDbEntities db = new ERPDbEntities())
            {
                tbl_Mst_AdvertiseImageXref existing = db.tbl_Mst_AdvertiseImageXref.Find(id);
                db.tbl_Mst_AdvertiseImageXref.Remove(existing);
                db.SaveChanges();

                //CustomersViewModel model = new CustomersViewModel();
                //model.Customers = db.Customers.OrderBy(
                //                  m => m.CustomerID).Take(5).ToList();

                //model.SelectedCustomer = null;
                //model.DisplayMode = "";
                // return View("Index", model);
                TempData["Success"] = "Photo Removed Successfully!";
                return RedirectToAction("UploadPhotos/" + form["GUID"] + "");
            }
        }
        public ActionResult MyAds()
        {
            if (User.Identity.IsAuthenticated)
            {
                int id = Convert.ToInt32(User.Identity.Name);
                ViewBag.Result = db.tbl_Mst_Advertise.Where(x => x.CreatedBy == id).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home", new { ReturnUrl = "/WebSpace/Advertise/MyAds" });
            }
        }
        [AllowAnonymous]
        public ActionResult ViewAd(string id)
        {
            if (id != null)
            {
                tbl_Mst_Advertise obj = db.tbl_Mst_Advertise.Where(x => x.Add_Id == id ).FirstOrDefault();
                string name = User.Identity.Name;
                if(name != "")
                {
                    int c_user = Convert.ToInt32(User.Identity.Name);
                    
                        int? ad_user = obj.CreatedBy;
                        if (c_user != ad_user)
                        {
                            if (obj.Ad_Views != null || obj.Ad_Views == 0)
                                obj.Ad_Views = Convert.ToInt32(obj.Ad_Views) + 1;
                            else
                                obj.Ad_Views = 1;
                        }
                    
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();

                }
                else
                {
                    if (obj.Ad_Views != null || obj.Ad_Views == 0)
                        obj.Ad_Views = Convert.ToInt32(obj.Ad_Views) + 1;
                    else
                        obj.Ad_Views = 1;

                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.Result = obj;
                return View();
            }
            else
            {
                return View();
            }
        }
    }
}