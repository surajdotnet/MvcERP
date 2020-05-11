using ERP.Entity.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Web.Models
{
    public class SMSManager
    {
        public string ToMobile { get; set; }
        public string MessageBody { get; set; }

        public string SendMessage()
        {
            using (ERPDbEntities db = new ERPDbEntities())
            {
                tbl_MstSMSAPI obj = db.tbl_MstSMSAPI.Where(x => x.IsActive == 1).FirstOrDefault();
                if (obj != null)
                {
                    var client = new RestClient(""+obj.Base_Url+"");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("authkey", ""+obj.Auth_Key+"");
                    request.AddParameter("application/json", "{ \"sender\": \""+obj.Sender_Id+"\", \"route\": \"4\", \"country\": \"91\", \"sms\": [ { \"message\": \"" + MessageBody + "\", \"to\": [ \"" + ToMobile + "\" ] } ] }", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful == true)
                        return response.StatusCode.ToString();
                    else
                        return response.StatusCode.ToString();
                }
                else
                {
                    return "No Default Profile found";
                }
            }
            
        }
    }
}