using ERP.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Web.Controllers
{[Authorize]
    public class RolesController : Controller
    {
        private ERPDbEntities db = new ERPDbEntities();
        // GET: Roles
        public ActionResult Index()
        {
          
                ViewBag.Result = db.tbl_MstRoles.ToList();
            
                return View();
        }

        // GET: Roles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            try
            {
                // TODO: Add insert logic here
                tbl_MstRoles obj = new tbl_MstRoles();
                obj.RoleName =form["RoleName"];
                obj.RoleDescription = form["RoleDescription"];
                obj.CreatedBy = Convert.ToInt32(User.Identity.Name);
                obj.CreatedDate = DateTime.Now;

                obj.IsActive = Convert.ToInt32(form["IsActive"]);
                db.tbl_MstRoles.Add(obj);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int id)
        {

            ViewBag.Result = db.tbl_MstRoles.Where(x => x.pkId == id).FirstOrDefault();
            return View();
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection form)
        {
            try
            {
                // TODO: Add update logic here
                tbl_MstRoles obj = db.tbl_MstRoles.Where(x => x.pkId == id).FirstOrDefault();
                obj.RoleName = form["RoleName"];
                obj.RoleDescription = form["RoleDescription"];
                obj.ModifiedBy = Convert.ToInt32(User.Identity.Name);
                obj.ModifiedDate = DateTime.Now;
                obj.IsActive = Convert.ToInt32(form["IsActive"]);
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Roles/Delete/5
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
