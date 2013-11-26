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

    public class Helper
    {
        private iCrawlerEntities db = new iCrawlerEntities();
        
        public bool IsInKeywords(string keywords, string text)
        {
            string[] strSplits = new string[] { "," };
            string[] arrEs = keywords.Split(strSplits, StringSplitOptions.None);

            if (keywords.ToLower().Contains("all")) return true;

            foreach (string e in arrEs)
            {
                if (text.ToLower().Contains(e.ToLower())) return true;
            }

            return false;
        }


        public Dictionary<string, string> GetContacts(string url, string htmlContent)
        {
            List<int> _contacts = new List<int>();
            Guid _urlId = db.Urls.Where(c => url.Contains(c.Url1)).FirstOrDefault().Id;
            _contacts = db.UrlOfUsers.Where(t => t.UrlId == _urlId).Select(c => c.UserId).ToList();

            Dictionary<string, string> _recipients = new Dictionary<string, string>();

            foreach (int _userId in _contacts)
            {
                string _keywords = db.UrlOfUsers.Where(c => c.UserId == _userId && c.UrlId == _urlId).FirstOrDefault().Keywords;
                string _outKeywords = db.UrlOfUsers.Where(c => c.UserId == _userId && c.UrlId == _urlId).FirstOrDefault().OutKeywords;
                string _email = db.UrlOfUsers.Where(c => c.UserId == _userId && c.UrlId == _urlId).FirstOrDefault().Emails;

                //if (_keywords != null && IsInKeywords(_keywords, htmlContent) && !IsOutKeywords(_outKeywords, htmlContent) && _email != null)
                if (_keywords != null && IsInKeywords(_keywords, htmlContent) && _email != null)
                {
                    if (_email.Contains(",") || _email.Contains(";"))
                    {
                        string[] strSplits = new string[] { ",", ";" };
                        string[] arrEs = _email.Split(strSplits, StringSplitOptions.None);
                        if (arrEs != null && arrEs.Count() > 1)
                        {
                            foreach (var _e in arrEs)
                            {
                                if (_e.Contains("@"))
                                {
                                    _recipients.Add(_e, _e);
                                }
                            }
                        }
                    }
                    else
                    {
                        _recipients.Add(_email, _email);
                    }
                }
            }

            //_recipients.Add("thiendc@bidc.vn", "Đinh Công Thiện");
            //_recipients.Add("minhsangbidv@gmail.com", "Phan Minh Sang");
            //_recipients.Add("loan.ntk0705@gmail.com", "Loan - Ban QLCT Phia Nam");

            return _recipients;
        }
    }

    public class DbHelper
    {
        private iCrawlerEntities db = new iCrawlerEntities();

        public HistoryLink CurrentLinkToLink(CurrentLink link)
        {
            return new HistoryLink
            {
                Id = link.Id,
                Url = link.Url,
                DownloadTime = link.DownloadTime,
                HtmlContent = link.HtmlContent,
                TextContent = link.TextContent,
                Title = link.Title
            };
        }

        public void CleanDB(string url)
        {
            string _lastUrl = "";
            int _maxId = 0;
            bool _isClean = true;
            List<CurrentLink> _listToMove = new List<CurrentLink>();

            var _lastLink = db.CurrentLinks.Where(c => c.Url.Contains(url)).OrderByDescending(c => c.Url).FirstOrDefault();
            if (db.CurrentLinks.Where(c=>c.Url.Contains(url)).Count() <= 10) _isClean = false;

            if ((_lastLink != null) && _isClean)
            {
                //_currentMaxId
                _lastUrl = _lastLink.Url;
                string[] strEs = _lastUrl.Split(new string[] { "/" }, StringSplitOptions.None);

                _maxId = int.Parse(strEs[strEs.Length - 1]);

                //Link to Move
                string currentMaxId = _maxId.ToString();
                DateTime _pointToDelete = DateTime.Now.AddDays(-1);
                _listToMove = db.CurrentLinks.Where(c => c.CreateDate.Value < _pointToDelete).ToList();

                MoveLinksToHistory(_listToMove);
            }
        }

        public void CleanHistory()
        {
            List<Url> _urls = new List<Url>();
            _urls = db.Urls.ToList();
            if (_urls != null && _urls.Count > 1)
            {
                foreach (Url _url in _urls)
                {
                    CleanHistory(_url.Url1);
                }
            }
        }

        public bool CleanHistory(string url)
        {
            DateTime _pointToDelete = DateTime.Now.AddDays(-7);
            List<HistoryLink> _histories = new List<HistoryLink>();
            _histories = db.HistoryLinks.Where(c => c.Url == url && c.CreateDate < _pointToDelete).ToList();

            if (_histories != null && _histories.Count > 1)
            {
                foreach (HistoryLink _his in _histories)
                {
                    db.HistoryLinks.Remove(_his);
                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void CleanDB()
        {
            List<Url> _urls = new List<Url>();
            _urls = db.Urls.ToList();

            if (_urls != null && _urls.Count > 0)
            {
                foreach (var _url in _urls)
                {
                    CleanDB(_url.Url1);
                    CleanHistory(_url.Url1);
                }
            }
        }

        public void MoveLinksToHistory(List<CurrentLink> links)
        {
            foreach (var link in links)
            {
                HistoryLink _link = CurrentLinkToLink(link);
                db.HistoryLinks.Add(_link);
                db.CurrentLinks.Remove(link);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    public class BidvCrawler : ICrawlerServices, IBidvCrawler
    {
        private iCrawlerEntities db = new iCrawlerEntities();
        private iTrackingMvc4Services.iTrackingServices _service = new iTrackingMvc4Services.iTrackingServices();

        public bool IsOutKeywords(string keywords, string text)
        {
            string[] strSplits = new string[] { "," };
            string[] arrEs = keywords.Split(strSplits, StringSplitOptions.None);
            
            foreach (string e in arrEs)
            {
                if (text.ToLower().Contains(e.ToLower())) return false;
            }

            return true;
        }

        

        public void DownloadAttachments(CurrentLink link, string htmlContent)
        {         
            if (link.Title == null) return;
            List<HtmlNode> nodes = new List<HtmlNode>();
            nodes = HtmlHelper.HtmlHelper.GetLinks(htmlContent);
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
            _recipients = new Helper().GetContacts(link.Url, link.HtmlContent);

            List<string> _files = new List<string>();
            _files = db.Documents.Where(c => c.UrlId == link.Id).Select(c => c.FileName).ToList();

            EmailHelper.Send(_email, _recipients, _files);            

            foreach (string file in _files)
            {
                File.Delete(file);
            }
        }
        
        public Article GetArticleFrom(string url)
        {            
            string htmlContent = HtmlHelper.HtmlHelper.GetHtmlPage(url);

            if (htmlContent == null) return null;

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);
            
            string[] strSplits = new string[] { "<!-- Phần nội dung -->", "plcRoot_Layout_zoneMaster_pageplaceholder_pageplaceholder_Layout_zoneDocumentHome_ChiTietVanBan_hddCaptch", "<td bgcolor=\"#FFFFFF\" class=\"text5\">"};
            string[] arrEs = htmlContent.Split(strSplits, StringSplitOptions.None);

            if (arrEs.Count() < 3) return null;

            Article _article = new Article();
            
            if (arrEs.Count() > 3)
            {
                _article.Url = url;
                _article.Title = arrEs[6].Split(new string[] { "</td>" }, StringSplitOptions.None)[0].Trim().Replace("\n", " ").Replace("\r", " "); //Chinh la Trich Yeu                

                strSplits = new string[] { "<!-- Phần nội dung -->", "plcRoot_Layout_zoneMaster_pageplaceholder_pageplaceholder_Layout_zoneDocumentHome_ChiTietVanBan_hddCaptch", "plcRoot_Layout_zoneMaster_pageplaceholder_pageplaceholder_Layout_zoneDocumentHome_ChiTietVanBan_panelComment" };
                arrEs = htmlContent.Split(strSplits, StringSplitOptions.None);
                _article.NoiDung = arrEs[3].Replace(" />", "").Trim();                            
            }
                                    
            if (_article.Title == null) return null;
            if (_article.NoiDung == null) return null;
            if (_article.NoiDung.ToLower().Contains("bạn chưa được cấp quyền truy cập tài liệu này"))
            {
                return null;
            }     

            return _article;
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
                _article = GetArticleFrom(tmp);                                
                                
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
