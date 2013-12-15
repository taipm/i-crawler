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
using ChuyenToan = iCrawler.Models.ChuyenToan;

namespace iCrawler
{
    public class VMFCrawler
    {
        private iCrawlerEntities db = new iCrawlerEntities();
        private ChuyenToan.ChuyenToanEntities _dbToan = new ChuyenToan.ChuyenToanEntities();
        private iTrackingMvc4Services.iTrackingServices _service = new iTrackingMvc4Services.iTrackingServices();
        public string crawlerName = "vmf";
        public string appName = "chuyentoan";
        public string masterUrl = "http://diendantoanhoc.net/home";

        public void ProcessVMF(string url)
        {
            string content = HtmlHelper.GetHtmlPage(url);
            List<HtmlNode> _links = new List<HtmlNode>();
            _links = HtmlHelper.GetLinks(content).Where(c => c.OuterHtml.Contains(masterUrl)).ToList();

            //STEP 3 : Select all links what it is news
            List<HtmlNode> _newLinks = new List<HtmlNode>();
            _newLinks = _links.Where(c => c.OuterHtml.Contains("http://diendantoanhoc.net/")).Distinct().ToList();

            foreach (var _link in _newLinks)                
            {                    
                string item = _link.OuterHtml;
                string _detailUrl = masterUrl + _link.Attributes[0].Value;

                if (_detailUrl.Length >= 2 && HtmlHelper.IsValidUrl(_detailUrl) && !new DbHelper().IsExitsUrl(_detailUrl))
                {                                                                                    
                    bool _isInserted = new DbHelper().AddCurrentLink(_detailUrl, crawlerName);                            
                    if (_isInserted)                            
                    {
                        string _pageContent = HtmlHelper.GetHtmlPage(_detailUrl);
                        VMFArticleView _article = new VMFArticleView();
                        _article.MasterUrl = _detailUrl;
                        _article.Url = _detailUrl;
                        _article.PageContent = _pageContent;

                        _article = Mapper.ArticleViewToVMF(_article.Process());
                        bool isPosted = WebserviceHelper.PostArticle(appName, WebserviceHelper.CrawlerArticleToObject(_article));
                        if (isPosted)
                            EmailHelper.SendArticleToEmail(_article);                   
                     }                                                                                                                             
                }                
            }
        }
    }                       
}
