using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Lightspeed_Product_Management
{
    class EmailManagement
    {
        public static void SendImpersonationEmail(string[] emails, string subject, string body, string[] attachments)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.511tactical.com", 25);
                mail.From = new MailAddress("Reports@511tactical.com");

                foreach (string e in emails)
                {
                    mail.To.Add(e);
                }

                mail.Subject = subject;
                mail.Body = body;

                if (attachments != null && attachments.Count() > 0)
                {
                    foreach (string s in attachments)
                    {
                        if (s != "")
                        {
                            mail.Attachments.Add(new System.Net.Mail.Attachment(s, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
                        }
                    }
                }

                mail.Priority = MailPriority.High;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("511LDAP", "511Stryk3!");
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        internal static void SendEmail(string[] v1, string v2, string v3, object p)
        {
            throw new NotImplementedException();
        }
    }
}
