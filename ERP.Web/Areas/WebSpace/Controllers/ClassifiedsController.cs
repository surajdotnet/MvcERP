using ERP.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Web.Areas.WebSpace.Controllers
{
    public class ClassifiedsController : Controller
    {
        private ERPDbEntities db = new ERPDbEntities();
        // GET: WebSpace/Classifieds
        //Category And Sub Category Selection Page
        public ActionResult Category()
        {
            ViewBag.CategoryList =  db.tbl_MstCategory.Where(x => x.IsActive == 1).ToList();
            return View();
        }

        public ActionResult CategoryAllAds()
        {
            return View();
        }
        
        public ActionResult CategorySubCategoryAllAds()
        {
            if (Request.QueryString["SubCategory"] != null || Request.QueryString["SubCategory"] != "")
            {
                string Category = Request.QueryString["Category"].ToString();
                ViewBag.Category = Category;
                string SubCategory = Request.QueryString["SubCategory"].ToString();
                ViewBag.SubCategory = SubCategory;
                ViewBag.Result = db.tbl_Mst_Advertise.Where(x => x.tbl_MstSubCategory.SubCategoryName == SubCategory && x.IsActive == 1).ToList();
            }
               
            else
                ViewBag.Result = null;
            return View();
        }
    }
}