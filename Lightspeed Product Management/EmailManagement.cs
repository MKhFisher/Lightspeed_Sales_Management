using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightspeed_Product_Management
{
    class EmailManagement
    {
        public static void SendEmail(string[] emails, string subject, string body, string[] attachments)
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
            //service.UseDefaultCredentials = true;
            string password = SQLManagement.RetrievePassword("michaelf");
            service.Credentials = new WebCredentials("michaelf", password);
            service.AutodiscoverUrl("MichaelF@511tactical.com");
            service.KeepAlive = true;

            EmailMessage mail = new EmailMessage(service);
            mail.Subject = subject;

            mail.Body = body;

            foreach (string email in emails)
            {
                mail.ToRecipients.Add(email.TrimEnd(','));
            }

            if (attachments != null && attachments.Count() > 0)
            {
                foreach (string attachment in attachments)
                {
                    mail.Attachments.AddFileAttachment(attachment);
                }
            }
            mail.SendAndSaveCopy();
        }
    }
}
