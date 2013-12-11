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
    public class TinhTeCrawler
    {
        public string appName = "chuyentin";
        public string crawlerName = "TinhTeCrawler";
        

        private iCrawlerEntities db = new iCrawlerEntities();        
        private iTrackingMvc4Services.iTrackingServices _service = new iTrackingMvc4Services.iTrackingServices();        
        
        public string UrlMaster = "http://tinhte.vn/";
        public string prefixUrl = "http://tinhte.vn/";

        public bool IsArticleUrl(string url)
        {
            //string[] _words = url.Split('-');
            //if (new StringHelper().IsNumber(_words[_words.Length - 1]))
            //    return true;
            //else
            //    return false;
            return true;
        }
        
            
        public void Process()
        {
            //STEP 1 : Get HtmlContent of MasterUrl
            string url = UrlMaster;
            string masterContent = HtmlHelper.GetHtmlPage(url);

            //STEP 2 : Find all link have masterContent            
            List<HtmlNode> _links = new List<HtmlNode>();
            _links = HtmlHelper.GetLinks(masterContent).ToList();

            //STEP 3 : Select all links what it is news
            List<HtmlNode> _newLinks = new List<HtmlNode>();
            _newLinks = _links.Where(c => c.OuterHtml.Contains("/threads/") && !c.OuterHtml.Contains("/noi-quy-dien-dan.20/")).Distinct().ToList();

            //STEP 4 : for each all link, read and parse to article
            foreach (HtmlNode _link in _newLinks)
            {
                if(_link != null)
                {
                    string _detailUrl = _link.Attributes[0].Value;
                    if (IsArticleUrl(_detailUrl) && !new DbHelper().IsExitsUrl(_detailUrl))
                    {
                        bool _isInserted = new DbHelper().AddCurrentLink(_detailUrl, crawlerName);
                        if(_isInserted)
                        {
                            string _pageContent = HtmlHelper.GetHtmlPage(_detailUrl);

                            TinhTeArticleView _article = new TinhTeArticleView();
                            _article.MasterUrl = UrlMaster;
                            _article.Url = _detailUrl;
                            _article.PageContent = _pageContent;

                            _article = _article.Process(); 

                            bool isPosted = WebserviceHelper.PostArticle(appName, WebserviceHelper.CrawlerArticleToObject(_article));

                            if(isPosted)
                                EmailHelper.SendArticleToEmail(_article);  
                        }
                    }                    
                }                               
            }                       

        }                
    }
}
