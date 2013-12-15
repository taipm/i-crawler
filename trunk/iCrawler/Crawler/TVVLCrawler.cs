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
    public class TVVLCrawler
    {        
        private iCrawlerEntities db = new iCrawlerEntities();
        private ChuyenLy.ChuyenLyEntities _dbLy = new ChuyenLy.ChuyenLyEntities();
        private iTrackingMvc4Services.iTrackingServices _service = new iTrackingMvc4Services.iTrackingServices();

        public string crawlerName = "tvvl";
        public string UrlMaster = "http://360.thuvienvatly.com/bai-viet";
        public string article_row = "article_row";

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
            return true;
        }

    
        public void ProcessTVVL(string url)
        {
            string content = HtmlHelper.GetHtmlPage(url);
            List<HtmlNode> _links = new List<HtmlNode>();
            _links = HtmlHelper.GetLinks(content).Where(c => c.OuterHtml.Contains(UrlMaster)).ToList();

            List<HtmlNode> _newLinks = new List<HtmlNode>();
            _newLinks = HtmlHelper.GetNodesByDiv(article_row, content).ToList();

            if (_links != null && _links.Count > 0)
            {
                foreach (var _link in _newLinks)
                {
                    string item = _link.OuterHtml;
                    if (item.Contains(UrlMaster))
                    {
                        bool write = true;
                        write = IsArticleUrl(item);
                        string _detailUrl = "";

                        if (_link.Attributes[1].Value.Contains("http://"))
                            _detailUrl = _link.Attributes[1].Value;

                        if (_link.Attributes[0].Value.Contains("http://"))
                            _detailUrl = _link.Attributes[0].Value;

                        if (_detailUrl.Length >= 2 && write && !new DbHelper().IsExitsUrl(_detailUrl))
                        {
                            bool _isInserted = new DbHelper().AddCurrentLink(_detailUrl, crawlerName);                            

                            string _pageContent = HtmlHelper.GetHtmlPage(_detailUrl);

                            TVVLArticleView _article = new TVVLArticleView();
                            _article.MasterUrl = UrlMaster;
                            _article.Url = _detailUrl;
                            _article.PageContent = _pageContent;

                            _article = Mapper.ArticleViewToTVVL(_article.Process());
                         
                            try
                            {                                
                                bool _isPosted = WebserviceHelper.PostArticle(crawlerName, WebserviceHelper.CrawlerArticleToObject(_article));                                
                                if(_isPosted)
                                    EmailHelper.SendArticleToEmail(_article);                                

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
    }
}
