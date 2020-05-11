using ERP.Entity;
using ERP.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Web.Controllers
{[Authorize]
    public class ControlPannelController : Controller
    {
        private ERPDbEntities db = new ERPDbEntities();
        // GET: ControlPannel
        public ActionResult EmailConfiguration()
        {
            ViewBag.Result = db.tbl_MstEmails.ToList();
            return View();
        }

        [HttpGet]
        public JsonResult GetEmail(int id)
        {
            tbl_MstEmails email = db.tbl_MstEmails.Where(x => x.pkId == id).FirstOrDefault();            
            return Json(email, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PostEmail(FormCollection form)
        {
            try
            {

                tbl_MstEmails obj = new tbl_MstEmails();
                obj.DisplayName = form["DisplayName1"];
                obj.Email = form["Email1"];
                obj.Password = form["Password1"];
                obj.Smtp_Server = form["Smtp_Server1"];
                obj.IsEnableSSL = Convert.ToInt32(form["IsEnableSSL1"]);
                obj.Incomming_Port = Convert.ToInt32(form["Incomming_Port1"]);
                obj.Outgoing_Port = Convert.ToInt32(form["Outgoing_Port1"]);
                obj.CreatedBy = Convert.ToInt32(User.Identity.Name);
                obj.CreatedDate = DateTime.Now;
                obj.IsActive = Convert.ToInt32(form["IsActive1"]);
                db.tbl_MstEmails.Add(obj);
                db.SaveChanges();
                TempData["Success"] = "Email Account Added Successfully!";
                return RedirectToAction("EmailConfiguration", "ControlPannel");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Occured: " + ex.Message;
                return RedirectToAction("EmailConfiguration", "ControlPannel");
            }
        }
        [HttpPost]
        public ActionResult PutEmail(FormCollection form)
        {
            try
            {
                int id = Convert.ToInt32(form["Id"]);
                tbl_MstEmails obj = db.tbl_MstEmails.Where(x => x.pkId == id).FirstOrDefault();
                obj.DisplayName= form["DisplayName"];
                obj.Email = form["Email"];
                obj.Password = form["Password"];
                obj.Smtp_Server = form["Smtp_Server"];
                obj.IsEnableSSL = Convert.ToInt32(form["IsEnableSSL"]);
                obj.Incomming_Port = Convert.ToInt32(form["Incomming_Port"]);
                obj.Outgoing_Port = Convert.ToInt32(form["Outgoing_Port"]);
                obj.ModifiedBy = Convert.ToInt32(User.Identity.Name);
                obj.ModifiedDate = DateTime.Now;
                obj.IsActive= Convert.ToInt32(form["IsActive"]);
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Email Account Modified Successfully!";
                return RedirectToAction("EmailConfiguration", "ControlPannel");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Occured: " + ex.Message;
                return RedirectToAction("EmailConfiguration", "ControlPannel");
            }          
        }

        [HttpPost]
        public ActionResult SendTestEmail(FormCollection form)
        {
            EmailManager em = new EmailManager();
            em.To = form["email"];
            em.MessageBody = form["MessageBody"];
            em.Subject = "Test Mail from ERP";
            string response=em.SendEmail();

            if (response == "1")
            {
                TempData["Success"] = "Test Email Sent Successfully!";
                return RedirectToAction("EmailConfiguration", "ControlPannel");
            }
            else {
                TempData["Error"] = "Error Occured: " + response;
                return RedirectToAction("EmailConfiguration", "ControlPannel");
            }            
        }
    }
}