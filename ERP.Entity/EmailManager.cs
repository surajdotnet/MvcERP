using ERP.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entity
{
   public class EmailManager
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }


        public string SendEmail()
        {
            string rsp = "";
            try
            {
                using (ERPDbEntities db = new ERPDbEntities())
                {
                    var res = db.tbl_MstEmails.Where(x => x.IsActive == 1).FirstOrDefault();
                    MailMessage mm = new MailMessage(res.Email, To);

                    MailAddress from = new MailAddress(res.Email, res.DisplayName);
                    mm.Subject = Subject;
                    mm.Body = MessageBody;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = res.Smtp_Server;
                    smtp.EnableSsl = Convert.ToBoolean(res.IsEnableSSL);
                    NetworkCredential NetworkCred = new NetworkCredential(res.Email, res.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = Convert.ToInt32(res.Outgoing_Port);
                    smtp.Send(mm);
                    return rsp = "1";
                }
            }
            catch (Exception ex)
            {
                return rsp = Convert.ToString(ex);
            }

        }
    }
}
