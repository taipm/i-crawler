using HtmlAgilityPack;
using NCrawler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCrawler.Models;

namespace iCrawler
{
    public class VMFHelper
    {        
        private iCrawlerEntities db = new iCrawlerEntities();

        public Article GetLastestArticle()
        {
            return null;    
        }

        public string ReadWebPage(string url)
        {
            string htmlContent = HtmlHelper.HtmlHelper.GetHtmlPage(url);
            return htmlContent;
        }

        

        public Article ParseToArticle(string url)
        {
            Article _article = new Article();

            string htmlContent = ReadWebPage(url);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);

            return _article;
        }

        public bool IsArticleUrl(string url)
        {            
            if (url.Contains("/home/images/")) return false;
            if (url.Contains("/add-ons/")) return false;
            if (url.Contains("/templates/")) return false;
            if (url.Contains("/home/cache/")) return false;
            if (url.Contains("/home/component")) return false;
            if (url.Contains("/home/media/")) return false;
            if (url.Contains("/home/index.php/")) return false;
            if (url.Contains("/home/plugins/")) return false;
            if (url == "http://diendantoanhoc.net/") return false;
                
            return true;
        }

        public bool IsExits(string url)
        {
            if(db.CurrentLinks.Where(c=>c.Url.Contains(url)) != null)
                return true;
            else 
                return false;
        }
        
        public void ProcessVMF(string url)
        {
            //new DbHelper().CleanDB(url);

            string content = ReadWebPage(url);
            List<HtmlNode> _links = new List<HtmlNode>();
            _links = HtmlHelper.HtmlHelper.GetLinks(content).Where(c => c.OuterHtml.Contains("http://diendantoanhoc.net/home/")).ToList();
         
            if (_links != null && _links.Count > 0)
            {
                foreach (var _node in _links)
                {
                    string item = _node.OuterHtml;
                    if (item.Contains("http://diendantoanhoc.net/"))
                    {
                        bool write = true;
                        write = IsArticleUrl(item);

                        if (write && !IsExits(_node.Attributes[0].Value))
                        {
                           
                            CurrentLink link = new CurrentLink();
                            link.Id = Guid.NewGuid();
                            link.Url = _node.Attributes[0].Value;
                            //link.Title = _article.Title;
                            link.CreateBy = "VMFCrawler";
                            link.CreateDate = DateTime.Now;
                            string _pageContent = HtmlHelper.HtmlHelper.GetHtmlPage(_node.Attributes[0].Value);
                            //List<string> _strLinks = new List<string>();
                            //List<HtmlNode> _linkss = new List<HtmlNode>();
                            
                            //_linkss = GetLinks(_pageContent).Where(c => c.OuterHtml.Contains("print=1")).ToList();
                            //_strLinks = GetLinks(_pageContent).Select(c=>c.OuterHtml).ToList();
                            string _printLink = HtmlHelper.HtmlHelper.GetLinks(_pageContent).Where(c => c.OuterHtml.Contains("print=1")).ToList().FirstOrDefault().Attributes[0].Value;
                            Article _article = new Article();

                            link.HtmlContent = HtmlHelper.HtmlHelper.GetHtmlPage("http://diendantoanhoc.net" + _printLink)
                                                .Replace("<script type=\"text/javascript\"  src=\"/add-ons/MathJax/MathJax.js?config=TeX-AMS-MML_HTMLorMML-full&locale=vi\"> </script> ", 
                                                        "<script src='http://cdn.mathjax.org/mathjax/latest/MathJax.js?config=TeX-AMS-MML_HTMLorMML' type='text/javascript'> MathJax.Hub.Config({        extensions: [\"tex2jax.js\",\"TeX/noErrors.js\",\"TeX/AMSsymbols.js\"],jax: [\"input/TeX\",\"output/HTML-CSS\"],       tex2jax: {inlineMath: [['$','$'],[\"\\(\",\"\\)\"]], displayMath:[['\\[','\\]'], ['$$','$$']]},\"HTML-CSS\": {availableFonts:[\"TeX\"]}      });</script>");

                            //link.Depth = propertyBag.Step.Depth;
                            //link.CharacterSet = propertyBag.CharacterSet;
                            //link.ContentEncoding = propertyBag.ContentEncoding;
                            //link.DownloadTime = propertyBag.DownloadTime;
                            //link.Server = propertyBag.Server;
                            //link.Method = propertyBag.Method;
                            //link.IsFromCache = propertyBag.IsFromCache;
                            //link.Headers = propertyBag.Headers[1].ToString() + " | " + propertyBag.Headers[2].ToString();
                            //link.StatusCode = propertyBag.StatusCode.ToString();
                            //link.StatusDescription = propertyBag.StatusDescription;
                            //link.LastModified = propertyBag.LastModified;
                            //link.ContentType = propertyBag.ContentType;
                            //link.TextContent = propertyBag.Text;

                            db.CurrentLinks.Add(link);
                            try
                            {
                                db.SaveChanges();                                
                                SendEmail(link);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }

        }

        //public Dictionary<string, string> GetContacts(string url, string htmlContent)
        //{
        //    List<int> _contacts = new List<int>();
        //    Guid _urlId = db.Urls.Where(c => url.Contains(c.Url1)).FirstOrDefault().Id;
        //    _contacts = db.UrlOfUsers.Where(t => t.UrlId == _urlId).Select(c => c.UserId).ToList();

        //    Dictionary<string, string> _recipients = new Dictionary<string, string>();

        //    foreach (int _userId in _contacts)
        //    {
        //        string _keywords = db.UrlOfUsers.Where(c => c.UserId == _userId && c.UrlId == _urlId).FirstOrDefault().Keywords;
        //        string _outKeywords = db.UrlOfUsers.Where(c => c.UserId == _userId && c.UrlId == _urlId).FirstOrDefault().OutKeywords;
        //        string _email = db.UrlOfUsers.Where(c => c.UserId == _userId && c.UrlId == _urlId).FirstOrDefault().Emails;

        //        //if (_keywords != null && IsInKeywords(_keywords, htmlContent) && !IsOutKeywords(_outKeywords, htmlContent) && _email != null)
        //        if (_keywords != null && new Helper().IsInKeywords(_keywords, htmlContent) && _email != null)
        //        {
        //            if (_email.Contains(",") || _email.Contains(";"))
        //            {
        //                string[] strSplits = new string[] { ",", ";" };
        //                string[] arrEs = _email.Split(strSplits, StringSplitOptions.None);
        //                if (arrEs != null && arrEs.Count() > 1)
        //                {
        //                    foreach (var _e in arrEs)
        //                    {
        //                        if (_e.Contains("@"))
        //                        {
        //                            _recipients.Add(_e, _e);
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                _recipients.Add(_email, _email);
        //            }
        //        }
        //    }

        //    //_recipients.Add("thiendc@bidc.vn", "Đinh Công Thiện");
        //    //_recipients.Add("minhsangbidv@gmail.com", "Phan Minh Sang");
        //    //_recipients.Add("loan.ntk0705@gmail.com", "Loan - Ban QLCT Phia Nam");

        //    return _recipients;
        //}

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

        public DateTime GetWebLastUpdate(string html)
        {
            //class = publishdate
            HtmlDocument htmlDoc = new HtmlDocument
            {
                OptionAddDebuggingAttributes = false,
                OptionAutoCloseOnEnd = true,
                OptionFixNestedTags = true,
                OptionReadEncoding = true
            };
            
            

            htmlDoc.LoadHtml(html);
            
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//p[@class='publishdate']");
            // Extract Title
            if (nodes != null)
            {
                string time = nodes[1].InnerHtml.Trim(); //Thứ hai, 29 Tháng 7 2013 11:40"
                int indexComma = time.IndexOf(",");
                time = time.Replace(" Tháng ", "/").Remove(0, indexComma + 2); //" 29:7 2013 11:40"
                int index2Cham = time.LastIndexOf(":");
                time = time.Remove(index2Cham - 3, 6).Replace(" ","/"); //"29/7/2013"                                
                string[] strSplits = new string[] { "/"};
                string[] strArrays = time.Split(strSplits, StringSplitOptions.None);

                return DateTime.Parse(strArrays[2] + "/" + strArrays[1] + "/" + strArrays[0]);
            }
            else
            {
                return DateTime.Now;
            }
        }

        //public bool IsLesson(string url)
        //{
        //    string[] strSplits = new string[] { "/" };
        //    string[] strEs = url.Split(strSplits, StringSplitOptions.None);
        //    if (strEs.Length > 5)
        //        return true;
        //    else
        //        return false;
        //}
        //public bool IsLesson2(string url)
        //{
        //    int index_ = url.LastIndexOf('-');
        //    int indexSplas = url.LastIndexOf('/');
        //    if (index_ > indexSplas)
        //        return true;
        //    else
        //        return false;
        //}

        //public bool IsLesson3(string url)
        //{
        //    if (url.Contains("print=1&layout=default&page="))
        //        return true;
        //    else
        //        return false;
        //}
    }
}
