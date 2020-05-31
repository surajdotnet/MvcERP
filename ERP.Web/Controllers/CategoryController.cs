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
    public class CategoryController : Controller
    {
        private ERPDbEntities db = new ERPDbEntities();

        // GET: Category
        public ActionResult Index()
        {
            return View(db.tbl_MstCategory.ToList());
        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_MstCategory tbl_MstCategory = db.tbl_MstCategory.Find(id);
            if (tbl_MstCategory == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MstCategory);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pkId,CategoryName,CategoryDescription,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,IsActive")] tbl_MstCategory tbl_MstCategory)
        {
            if (ModelState.IsValid)
            {
                db.tbl_MstCategory.Add(tbl_MstCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_MstCategory);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_MstCategory tbl_MstCategory = db.tbl_MstCategory.Find(id);
            if (tbl_MstCategory == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MstCategory);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pkId,CategoryName,CategoryDescription,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,IsActive")] tbl_MstCategory tbl_MstCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_MstCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_MstCategory);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_MstCategory tbl_MstCategory = db.tbl_MstCategory.Find(id);
            if (tbl_MstCategory == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MstCategory);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_MstCategory tbl_MstCategory = db.tbl_MstCategory.Find(id);
            db.tbl_MstCategory.Remove(tbl_MstCategory);
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
