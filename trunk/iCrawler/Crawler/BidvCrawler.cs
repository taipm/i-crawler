using NCrawler;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Data.Entity.Validation;
using iCrawler.Models;
using iCrawler.ServiceLayer;

namespace iCrawler
{
    public class TimerHelper
    {
        public bool IsInTimes(string times, bool isSkip)
        {
            if (isSkip == true) return true;
            if (times == null) return false;

            string[] strSplits = new string[] { ";"};
            string[] arrEs = times.Split(strSplits, StringSplitOptions.None);
            if(arrEs != null && arrEs.Count() > 0)
            {
                foreach(string arr in arrEs)
                {
                    if(DateTime.Now.ToString().ToLower().Contains(arr)) return true;
                }
            }
            return false;
        }
    }
    
   

    public class BidvCrawler : ICrawlerServices, IBidvCrawler
    {
        private iCrawlerEntities db = new iCrawlerEntities();
        private iTrackingMvc4Services.iTrackingServices _service = new iTrackingMvc4Services.iTrackingServices();


        public void DownloadAttachments(CurrentLink link, string htmlContent)
        {         
            if (link.Title == null) return;
            List<HtmlNode> nodes = new List<HtmlNode>();
            nodes = HtmlHelper.GetLinks(htmlContent);
            foreach (var node in nodes)
            {
                if (node.OuterHtml.Contains("/eDocman/downloadvanbanpublic.aspx"))
                {
                    string[] strSplits = new string[] { "href=\"", "\" target" };
                    string[] Es = node.OuterHtml.Split(strSplits, StringSplitOptions.None);
                    string _url = Es[1];
                    System.Net.WebClient _WebClient = new System.Net.WebClient();

                    // Downloads the resource with the specified URI to a local file.
                    string filePath = "http://bidvportal.vn" + _url;
                    string _fileName = @"E:\" + Es[2].Replace("=\"_blank\">", "").Replace("</a>", "");
                    Document _doc = new Document();
                    _doc.Id = Guid.NewGuid();
                    _doc.FileName = _fileName;
                    _doc.FilePath = filePath;
                    _doc.DownloadTime = DateTime.Now;
                    _doc.Url = link.Url;
                    _doc.UrlId = link.Id;
                    
                    db.Documents.Add(_doc);

                    try
                    {
                        _WebClient.DownloadFile(filePath, @"E:\" + Es[2].Replace("=\"_blank\">", "").Replace("</a>", ""));
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                link.TextContent += node.OuterHtml + ";";
            }

            try
            {
                SendEmail(link);                
            }
            catch
            {
                Console.WriteLine("LOI : ");
            }
        }

        public void SendEmail(CurrentLink link)
        {
            Email _email = new Email();
            _email.Sender = "taipm.bidv@gmail.com";
            _email.SenderName = "Phan Minh Tài";
            _email.Subject = "iCrawler : " + link.Title;
            _email.Body = link.HtmlContent;

            Dictionary<string, string> _recipients = new Dictionary<string, string>();
            _recipients = new DbHelper().GetContacts(link.Url, link.HtmlContent);

            List<string> _files = new List<string>();
            _files = db.Documents.Where(c => c.UrlId == link.Id).Select(c => c.FileName).ToList();

            EmailHelper.Send(_email, _recipients, _files);            

            foreach (string file in _files)
            {
                File.Delete(file);
            }
        }
        
        
        
        public int GetMaxUrlId()
        {
            int _currentMaxId = 0;
            string _lastUrl = "";            

            var _lastLink = db.CurrentLinks.Where(c => c.Url.Contains("bidvportal")).OrderByDescending(c => c.Url).FirstOrDefault();
            List<CurrentLink> _listToMove = new List<CurrentLink>();
            
            _lastUrl = _lastLink.Url;
            string[] strEs = _lastUrl.Split(new string[] { "/" }, StringSplitOptions.None);

            _currentMaxId = int.Parse(strEs[strEs.Length - 1]);
            return _currentMaxId;
        }

        public void ProcessBIDVPortal(PropertyBag propertyBag)
        {
            new DbHelper().CleanDB("http://bidvportal.vn/");

            int _currentMaxId = 0;
            int countFalse = 0;                     
            bool isDone = true;
            string url = "http://bidvportal.vn/eDocman/chitietvanbanpublic.aspx?id=/root/eDocman/";

            _currentMaxId = GetMaxUrlId();

            int i = _currentMaxId + 1;
            while (isDone)
            {                
                //SET URL TO INITIAL START CRAW
                string tmp = url;
                tmp = url + i.ToString();
                
                Article _article = new Article();
                _article = Mappers.Mapper.UrlToArticle(tmp);                                
                                
                if (_article != null)
                {
                    string _fullHtmlContent = _article.NoiDung;

                    try
                    {
                        int index = _article.NoiDung.IndexOf("Xem thêm các văn bản đính kèm");
                        _article.NoiDung = _article.NoiDung.Replace(" />", "").Trim().Remove(index);
                    }
                    catch
                    {
                        _fullHtmlContent = "";
                    }

                    CurrentLink link = new CurrentLink();
                    link.Id = Guid.NewGuid();
                    link.Url = tmp;
                    link.Title = _article.Title;
                    link.CreateBy = "iCrawler";
                    link.CreateDate = DateTime.Now;
                    link.HtmlContent = _article.NoiDung;
                    link.Depth = propertyBag.Step.Depth;
                    link.CharacterSet = propertyBag.CharacterSet;
                    link.ContentEncoding = propertyBag.ContentEncoding;
                    link.DownloadTime = propertyBag.DownloadTime;
                    link.Server = propertyBag.Server;
                    link.Method = propertyBag.Method;
                    link.IsFromCache = propertyBag.IsFromCache;
                    link.Headers = propertyBag.Headers[1].ToString() + " | " + propertyBag.Headers[2].ToString();
                    link.StatusCode = propertyBag.StatusCode.ToString();
                    link.StatusDescription = propertyBag.StatusDescription;
                    link.LastModified = propertyBag.LastModified;
                    link.ContentType = propertyBag.ContentType;
                    link.TextContent = propertyBag.Text;

                    bool actionAdd = true;

                    if (link.HtmlContent == null) actionAdd = false;
                    if (link.Title == null) actionAdd = false;

                    if (actionAdd)
                    {
                        db.CurrentLinks.Add(link);
                        try
                        {                            
                            db.SaveChanges();
                            _service.CreateArticle(link.Title, link.HtmlContent.Substring(0, 100), link.HtmlContent, "BidvCrawler");
                        }
                        catch (DbEntityValidationException e)
                        {
                                throw;
                        }

                        if (_fullHtmlContent.Length > 1)
                        {
                            try
                            {
                                DownloadAttachments(link, _fullHtmlContent);
                            }
                            catch
                            {
                                SendEmail(link);
                            }
                        }
                        else
                        {
                            SendEmail(link);
                        }
                    }
                                
                }
                else
                {
                    countFalse = countFalse + 1;
                    if (countFalse > 4)
                    {
                        isDone = false;
                    }                    
                }
                i = i + 1;                   
            }
        }
    }
}
