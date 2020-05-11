using ERP.Entity;
using ERP.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace ERP.Web.Controllers
{[Authorize]
    public class PlanningController : Controller
    {
        // GET: Planning
        public ActionResult ShowData(string id)
        {
            using (ERPDbEntities db = new ERPDbEntities())
            {
                if (id != null)
                {
                    ViewBag.Result = db.tbl_RMASHEET.Where(x => x.DEPARTMENT == id).ToList();
                    TempData["Success"] = "Showing Data for" + id;
                    return View();
                }
                else
                {
                    TempData["Error"] = "Please Select a divison First";
                    return View();
                }  
            }
        }

        // GET: Planning
        public ActionResult ImportData()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportData(HttpPostedFileBase file,FormCollection form)
        {
            DataSet ds = new DataSet();
            if (form["dep"] != "0")
            {
                if (Request.Files["file"].ContentLength > 0)
                {

                    string path = Server.MapPath("~/Uploads/ExcelFiles/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileExtension =Path.GetExtension(Request.Files["file"].FileName);
                    var filename = DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss") + "_" + Path.GetFileName(file.FileName);
                
                    //DbPath = "~/Uploads/MerchantImages/" + filename;
                    //postedFile.SaveAs(path + filename);                   

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        string fileLocation = Server.MapPath("~/Uploads/ExcelFiles/") + filename;
                        //if (System.IO.File.Exists(fileLocation))
                        //{
                        //    System.IO.File.Delete(fileLocation);
                        //}
                        Request.Files["file"].SaveAs(fileLocation);


                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        //connection String for xls file format.
                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        //connection String for xlsx file format.
                        else if (fileExtension == ".xlsx")
                        {
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }
                        //Create Connection to Excel work book and add oledb namespace
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }
                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                        string query = string.Format("Select * from [{0}]", ""+form["dep"]+"$"); 
                        //string.Format("Select * from [{0}]", excelSheets[0]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(ds);
                        }
                    }
                    else
                    {
                        TempData["Error"] = "only excel file allowed";
                        return RedirectToAction("ImportData", "Planning");
                    }
                    //if (fileExtension.ToString().ToLower().Equals(".xml"))
                    //{
                    //    string fileLocation = Server.MapPath("~/Uploads/ExcelFiles/") + Request.Files["FileUpload"].FileName;
                    //    if (System.IO.File.Exists(fileLocation))
                    //    {
                    //        System.IO.File.Delete(fileLocation);
                    //    }

                    //    Request.Files["FileUpload"].SaveAs(fileLocation);
                    //    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    //    // DataSet ds = new DataSet();
                    //    ds.ReadXml(xmlreader);
                    //    xmlreader.Close();
                    //}
                    //SqlConnection con = new dbConnection().getconnection();
                    //con.Open();
                    //SqlCommand cmd = new SqlCommand("delete from tbl_rmasheet where department=" + form["dep"] + "", con);
                    //cmd.ExecuteNonQuery();
                    //con.Close();

                    DataTable dt2 = ds.Tables[0];
                    int? count = Convert.ToInt32(dt2.Rows.Count);
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            using (ERPDbEntities db = new ERPDbEntities())
                            {
                                tbl_RMASHEET obj = new tbl_RMASHEET();
                                obj.CP_NO = ds.Tables[0].Rows[i]["cp no"].ToString();
                                obj.JOB_NO = ds.Tables[0].Rows[i]["Job No"].ToString();
                                obj.PARTY = ds.Tables[0].Rows[i]["PARTY"].ToString();
                                obj.UID = ds.Tables[0].Rows[i]["UID"].ToString();
                                obj.GRADE = ds.Tables[0].Rows[i]["Grade"].ToString();
                                obj.BILLET_SIZE = ds.Tables[0].Rows[i]["Billet Size"].ToString();
                                obj.APPROX_TON_PENDING = ds.Tables[0].Rows[i]["Approx Ton Pending"].ToString();
                                obj.S_PER = ds.Tables[0].Rows[i]["S%"].ToString();
                                obj.P_PER = ds.Tables[0].Rows[i]["P%"].ToString();
                                obj.LEN_FINISH_MAT = ds.Tables[0].Rows[i]["Length of finished material"].ToString();
                                obj.B = ds.Tables[0].Rows[i]["B"].ToString();
                                obj.ED_DATE = ds.Tables[0].Rows[i]["ED DATE"].ToString();
                                obj.TDC_REF = ds.Tables[0].Rows[i]["TDC Ref"].ToString();
                                obj.INT_PRODUCT_STD_SERIES_NO = ds.Tables[0].Rows[i]["Internal Product Standard series No"].ToString();
                                obj.BILLET_SIZE = ds.Tables[0].Rows[i]["Billet Length"].ToString();  //Billet Length
                                obj.NO_OF_PCS = ds.Tables[0].Rows[i]["No of Pcs"].ToString();
                                obj.APPROX_QTY_BILLET = ds.Tables[0].Rows[i]["Apprx Qty of Billet"].ToString();
                                obj.SIZE_FOR_ROLLING = ds.Tables[0].Rows[i]["Size for Rolling"].ToString();
                                obj.SHAPE = ds.Tables[0].Rows[i]["shape"].ToString();
                                obj.FINISHED_SIZE = ds.Tables[0].Rows[i]["Finished size"].ToString();
                                obj.RMA_NO = ds.Tables[0].Rows[i]["RMA No"].ToString();
                                obj.HEAT_NO = ds.Tables[0].Rows[i]["Heat No"].ToString();
                                obj.PCS = ds.Tables[0].Rows[i]["pcs"].ToString();
                                obj.REMARK = ds.Tables[0].Rows[i]["Remarks"].ToString();
                                obj.STATUS = ds.Tables[0].Rows[i]["Status"].ToString();
                                obj.RMA_DT = ds.Tables[0].Rows[i]["RMA dt"].ToString();
                                obj.REPLAN = ds.Tables[0].Rows[i]["REPLAN"].ToString();
                                obj.DEPARTMENT = form["dep"];
                                db.tbl_RMASHEET.Add(obj);
                                db.SaveChanges();
                            }
                         
                        }
                        catch (Exception ex)
                        {
                            TempData["Error"] = "Error Occured: " + ex.Message.ToString();
                            return RedirectToAction("ImportData", "Planning");
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Please select a file";
                    return RedirectToAction("ImportData", "Planning");
                }
            }
            else
            {
                TempData["Error"] = "Please select a department first";
                return RedirectToAction("ImportData", "Planning");
            }

            TempData["Success"] = "Data Import Successfully!";
            return RedirectToAction("ImportData", "Planning");
            
        }


    }
}