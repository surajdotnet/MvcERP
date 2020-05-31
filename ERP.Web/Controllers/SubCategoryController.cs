using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERP.Entity.Models;

namespace ERP.Web.Controllers
{
    [Authorize]
    public class SubCategoryController : Controller
    {
        private ERPDbEntities db = new ERPDbEntities();

        // GET: SubCategory
        public ActionResult Index()
        {
            var tbl_MstSubCategory = db.tbl_MstSubCategory.Include(t => t.tbl_MstCategory);
            return View(tbl_MstSubCategory.ToList());
        }

        // GET: SubCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_MstSubCategory tbl_MstSubCategory = db.tbl_MstSubCategory.Find(id);
            if (tbl_MstSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MstSubCategory);
        }

        // GET: SubCategory/Create
        public ActionResult Create()
        {
            ViewBag.fkCategoryId = new SelectList(db.tbl_MstCategory, "pkId", "CategoryName");
            return View();
        }

        // POST: SubCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pkId,fkCategoryId,SubCategoryName,SubCategoryDesc,CreatedBy,CreatedDate,ModifedBy,ModifedDate,IsActive")] tbl_MstSubCategory tbl_MstSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.tbl_MstSubCategory.Add(tbl_MstSubCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fkCategoryId = new SelectList(db.tbl_MstCategory, "pkId", "CategoryName", tbl_MstSubCategory.fkCategoryId);
            return View(tbl_MstSubCategory);
        }

        // GET: SubCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_MstSubCategory tbl_MstSubCategory = db.tbl_MstSubCategory.Find(id);
            if (tbl_MstSubCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.fkCategoryId = new SelectList(db.tbl_MstCategory, "pkId", "CategoryName", tbl_MstSubCategory.fkCategoryId);
            return View(tbl_MstSubCategory);
        }

        // POST: SubCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pkId,fkCategoryId,SubCategoryName,SubCategoryDesc,CreatedBy,CreatedDate,ModifedBy,ModifedDate,IsActive")] tbl_MstSubCategory tbl_MstSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_MstSubCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fkCategoryId = new SelectList(db.tbl_MstCategory, "pkId", "CategoryName", tbl_MstSubCategory.fkCategoryId);
            return View(tbl_MstSubCategory);
        }

        // GET: SubCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_MstSubCategory tbl_MstSubCategory = db.tbl_MstSubCategory.Find(id);
            if (tbl_MstSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MstSubCategory);
        }

        // POST: SubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_MstSubCategory tbl_MstSubCategory = db.tbl_MstSubCategory.Find(id);
            db.tbl_MstSubCategory.Remove(tbl_MstSubCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
