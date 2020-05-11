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
{
    public class AdvertiseController : Controller
    {
        public ERPDbEntities db = new ERPDbEntities();
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
                var Plan = db.tbl_AdvertisePlansXref.Where(x => x.fkMerchantId == id && x.IsActive==1).FirstOrDefault();
                var Ads_Added = db.tbl_Mst_Advertise.Where(x => x.CreatedBy == id && x.IsActive == 1).ToList();
                //if(Ads_Added.Count>)
                ViewBag.Category = db.tbl_MstCategory.Where(x => x.IsActive == 1).ToList();
                ViewBag.SubCategory = db.tbl_MstSubCategory.Where(x => x.IsActive == 1).ToList();
                return View();
            }               
            else
                return RedirectToAction("Login", "Home", new { ReturnUrl = Request.Url.AbsoluteUri});
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
                if(obj.IsMobileView==true)
                    obj1.IsMobileView = 1;
                else
                    obj1.IsMobileView = 0;
                obj1.Geo_Lat = obj.Geo_Lat;
                obj1.Geo_Lat = obj.Geo_Lat;
                obj1.Geo_Long = obj.Geo_Long;
                obj1.State = obj.State;
                obj1.City = obj.City;
                obj1.Neighbourhood = obj.Neighbourhood;
                obj1.CreatedBy =Convert.ToInt32(User.Identity.Name);
                obj1.CreatedDate = DateTime.Now;
                obj1.IsActive = 0;
                db.tbl_Mst_Advertise.Add(obj1);
                db.SaveChanges();
                return RedirectToAction("UploadPhotos/"+obj1.Add_Id, "Advertise");
            }                
            else{
                return RedirectToAction("Login", "Home", new { ReturnUrl = Request.Url.AbsoluteUri });
            }
               
        }

        public ActionResult UploadPhotos(string id)
        {
            if (Session["User"] != null)
            {
                tbl_Mst_Advertise Ad_Result = db.tbl_Mst_Advertise.Where(x => x.Add_Id == id).FirstOrDefault();
                ViewBag.Result = db.tbl_Mst_AdvertiseImageXref.Where(x => x.fkAdd_Id == Ad_Result.Id).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home", new { ReturnUrl = "/WebSpace/Advertise/UploadPhotos/"+id });
            }
        }
        [HttpPost]
        public ActionResult UploadPhotos(List<HttpPostedFileBase> postedFiles, string id)
        {
            if (Session["User"] != null)
            {
                var DbPath = "~/Uploads/Advertise/noimage.jpg";
                string path = Server.MapPath("~/Uploads/Advertise/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (HttpPostedFileBase postedFile in postedFiles)
                {
                    if (postedFile != null)
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
                    else
                    {
                        TempData["Error"] = "Please select some files!";
                    }
                }
                TempData["Success"] = "Photo Uploded Successfully! and your add is live";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home", new { ReturnUrl = "/WebSpace/Advertise/UploadPhotos/" + id });
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
        public ActionResult ViewAd(string id)
        {
            if (id != null)
            {
                tbl_Mst_Advertise obj = db.tbl_Mst_Advertise.Where(x => x.Add_Id == id).FirstOrDefault();
                if (obj.Ad_Views != null || obj.Ad_Views == 0)
                    obj.Ad_Views = Convert.ToInt32(obj.Ad_Views) + 1;                
                else
                    obj.Ad_Views = 1;

                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();

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