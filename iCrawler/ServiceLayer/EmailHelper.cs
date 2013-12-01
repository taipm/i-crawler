using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using iCrawler.Models;
using System.IO;
using iCrawler.ServiceLayer;

public class Email
{
    public string Sender;
    public string SenderName;
    public string Subject;
    public string Body;
    public Dictionary<string, string> Recipients;
    public List<Attachment> Attachments;
}


public static class EmailHelper
{
    public static void SendArticleToEmail(VMFArticleView article)
    {
        Email _email = new Email();
        _email.Sender = "taipm.bidv@gmail.com";
        _email.SenderName = "Phan Minh Tài";
        _email.Subject = article.Title;
        _email.Body = article.Content + "<br />" + article.Tags;


        Dictionary<string, string> _recipients = new Dictionary<string, string>();
        _recipients = new DbHelper().GetContacts(article.MasterUrl, article.Content);
        if (_recipients.Count < 1) return;

        List<string> _files = new List<string>();
        //_files = db.Documents.Where(c => c.UrlId == link.Id).Select(c => c.FileName).ToList();

        EmailHelper.Send(_email, _recipients, _files);

        foreach (string file in _files)
        {
            File.Delete(file);
        }
    }

    public static void SendArticleToEmail(ArticleView article)
    {
        Email _email = new Email();
        _email.Sender = "taipm.bidv@gmail.com";
        _email.SenderName = "Phan Minh Tài";
        _email.Subject = article.Title;
        _email.Body = article.Content + "<br />" + article.Tags;


        Dictionary<string, string> _recipients = new Dictionary<string, string>();
        _recipients = new DbHelper().GetContacts(article.MasterUrl, article.Content);

        List<string> _files = new List<string>();
        //_files = db.Documents.Where(c => c.UrlId == link.Id).Select(c => c.FileName).ToList();

        EmailHelper.Send(_email, _recipients, _files);

        foreach (string file in _files)
        {
            File.Delete(file);
        }
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

