using HtmlAgilityPack;
using NCrawler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCrawler.Models;
using iCrawler.ServiceLayer;
using iCrawler.Mappers;
using ChuyenLy = iCrawler.Models.ChuyenLy;

namespace iCrawler
{
    public class QTMCrawler
    {        
        private iCrawlerEntities db = new iCrawlerEntities();        
        private iTrackingMvc4Services.iTrackingServices _service = new iTrackingMvc4Services.iTrackingServices();

        public string crawlerName = "qtmCrawler";
        public string appName = "chuyentin";

        public string today = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
        public string UrlMaster = "http://www.quantrimang.com.vn/daily/";
        public string prefixUrl = "http://www.quantrimang.com.vn";
        public bool IsArticleUrl(string url)
        {
            string[] _words = url.Split('-');
            if (new StringHelper().IsNumber(_words[_words.Length - 1]))
                return true;
            else
                return false;
        }
        
            
        public void ProcessQTM()
        {
            //STEP 1 : Get HtmlContent of MasterUrl
            string url = UrlMaster + today + "/index.aspx";
            string masterContent = HtmlHelper.GetHtmlPage(url);

            //STEP 2 : Find all link have masterContent            
            List<HtmlNode> _links = new List<HtmlNode>();
            _links = HtmlHelper.GetLinks(masterContent).ToList();

            //STEP 3 : Select all links what it is news
            List<HtmlNode> _newLinks = new List<HtmlNode>();
            _newLinks = _links.Where(c => (!c.OuterHtml.Contains("timkiem") && !c.OuterHtml.Contains(".aspx")) &&
                                            (c.OuterHtml.Contains("tintuc/") 
                                            || c.OuterHtml.Contains("thietbiso/"))).ToList();

            //STEP 4 : for each all link, read and parse to article
            foreach (HtmlNode _link in _newLinks)
            {
                if(_link != null)
                {
                    string _detailUrl = prefixUrl + _link.Attributes[0].Value;
                    if (IsArticleUrl(_detailUrl) && !new DbHelper().IsExitsUrl(_detailUrl))
                    {
                        bool _isInserted = new DbHelper().AddCurrentLink(_detailUrl, crawlerName);
                        if(_isInserted)
                        {
                            string _pageContent = HtmlHelper.GetHtmlPage(_detailUrl);

                            QTMArticleView _article = new QTMArticleView();
                            _article.MasterUrl = UrlMaster;
                            _article.Url = _detailUrl;
                            _article.PageContent = _pageContent;

                            _article = _article.Process(); 

                            WebserviceHelper.PostArticle(appName, WebserviceHelper.CrawlerArticleToObject(_article));

                            EmailHelper.SendArticleToEmail(_article);  
                        }
                    }                    
                }
                
                //Save to CurrentLink

                //Save to Webservice
            }
            
            //List<HtmlNode> _articleNodes = new List<HtmlNode>();
            //_articleNodes = HtmlHelper.GetNodesByDiv(article_row, content).ToList();

            //if (_links != null && _links.Count > 0)
            //{
            //    foreach (var _node in _links)
            //    {
            //        string item = _node.OuterHtml;
            //        if (item.Contains(UrlMaster))
            //        {
            //            bool write = true;
            //            write = IsArticleUrl(item);
            //            string _url = "";
            //            if (_node.Attributes[1].Value.Contains("http://"))
            //                _url = _node.Attributes[1].Value;

            //            if (_node.Attributes[0].Value.Contains("http://"))
            //                _url = _node.Attributes[0].Value;

            //            if (_url.Length >=2 &&  write && !new DbHelper().IsExitsUrl(_url))
            //            {
                           
            //                CurrentLink link = new CurrentLink();
            //                link.Id = Guid.NewGuid();
            //                link.Url = _url;
            //                link.CreateBy = "TVVLCrawler";
            //                link.CreateDate = DateTime.Now;
            //                db.CurrentLinks.Add(link);

            //                string _pageContent = HtmlHelper.GetHtmlPage(_url);

            //                TVVLArticleView _article = new TVVLArticleView();
            //                _article.MasterUrl = UrlMaster;
            //                _article.Url = _url;
            //                _article.PageContent = _pageContent;

            //                _article = Mapper.ArticleViewToTVVL(_article.Process());
                         
            //                try
            //                {
            //                    db.SaveChanges();

            //                    WebserviceHelper.PostArticle(crawlerName, WebserviceHelper.CrawlerArticleToObject(_article));                                

            //                    EmailHelper.SendArticleToEmail(_article);                                

            //                }
            //                catch (Exception ex)
            //                {
            //                    Console.WriteLine(ex.Message);
            //                }
            //            }
            //        }
            //    }
            //}

        }                
    }
}
