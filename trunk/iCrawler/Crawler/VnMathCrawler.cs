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
using iCrawler;

namespace iCrawler
{
    public class VnMathCrawler : ICrawler
    {
        public string appName = "chuyentoan";
        public string crawlerName = "VnMathCrawler";
        

        private iCrawlerEntities db = new iCrawlerEntities();        
        private iTrackingMvc4Services.iTrackingServices _service = new iTrackingMvc4Services.iTrackingServices();        
        
        public string UrlMaster = "http://vnmath.com/";
        public string prefixUrl = "http://vnmath.com/";


        public bool IsArticleUrl(string url)
        {
            if (StringHelper.IsValidUrl(url))
                return true;
            else
                return false;            
            
        }

        public List<HtmlNode> GetNewLinks(string html)
        {
            //STEP 1 : Get HtmlContent of MasterUrl
            string url = UrlMaster;
            
            //STEP 2 : Find all link have masterContent            
            List<HtmlNode> _links = new List<HtmlNode>();
            _links = HtmlHelper.GetLinks(html).ToList();

            //STEP 3 : Select all links what it is news
            List<HtmlNode> _newLinks = new List<HtmlNode>();
            _newLinks = _links.Where(c => c.OuterHtml.Contains(".html")).Distinct().ToList();

            return _newLinks;
        }

        public string GetDetailUrl(HtmlNode link)
        {            
            string _detailUrl = link.Attributes[0].Value;
            return _detailUrl;
        }

        public void Process()
        {
            string masterContent = HtmlHelper.GetHtmlPage(UrlMaster);

            List<HtmlNode> _newLinks = new List<HtmlNode>();
            _newLinks = GetNewLinks(masterContent);

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

                            VnMathArticleView _article = new VnMathArticleView();
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
