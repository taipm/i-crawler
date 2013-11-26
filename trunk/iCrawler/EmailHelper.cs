using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

public class Email
{
    public string Sender;
    public string SenderName;    
    public string Subject;
    public string Body;
    public Dictionary<string, string> Recipients;
    public List<Attachment> Attachments;
}

public class Attachment
{
    public string FileName;
    public string FilePath;
}

public static class EmailHelper
{ 
        ///
        /// Sends an Email.
        ///
        public static bool Send(string sender, string senderName, string recipient, string recipientName, string subject, string body)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(sender, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(recipient, recipientName));

            try
            {
                var client = new SmtpClient();
                client.Send(message);
            }
            catch (Exception ex)
            {
                //handle exeption
                return false;
            }

            return true;
        }

        public static bool Send(Email email, Dictionary<string, string> recipients, List<string> attachments)
        {
            
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("taipm.bidv@gmail.com", "P@$$w0rd");
            SmtpServer.EnableSsl = true;
             
            foreach (var recipient in recipients)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(email.Sender, email.SenderName);
                mail.Subject = email.Subject;
                mail.Body = email.Body;
                mail.IsBodyHtml = true;

                List<System.Net.Mail.Attachment> _attachments = new List<System.Net.Mail.Attachment>();
                foreach (var file in attachments)
                {
                    _attachments.Add(new System.Net.Mail.Attachment(file));
                }                               

                mail.To.Add(new MailAddress(recipient.Key, recipient.Value));

                foreach (var attachment in _attachments)
                {
                    mail.Attachments.Add(attachment);
                }

                try
                {
                    //var client = new SmtpClient();
                    SmtpServer.Send(mail);
                }
                catch (Exception ex)
                {
                    //handle exeption
                    return false;
                }
            }            

            return true;
        }
        ///
        /// Sends an Email asynchronously
        ///
        public static void SendAsync(string sender, string senderName, string recipient, string recipientName, string subject, string body)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(sender, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(recipient, recipientName));

            var client = new SmtpClient();
            client.SendCompleted += MailDeliveryComplete;
            client.SendAsync(message, message);
        }

        private static void MailDeliveryComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //handle error
            }
            else if (e.Cancelled)
            {
                //handle cancelled
            }
            else
            {
                //handle sent email
                MailMessage message = (MailMessage)e.UserState;
            }
        }
    }

