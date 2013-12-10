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

            List<HtmlNode> _articleNodes = new List<HtmlNode>();
            _articleNodes = HtmlHelper.GetNodesByDiv(article_row, content).ToList();

            if (_links != null && _links.Count > 0)
            {
                foreach (var _node in _links)
                {
                    string item = _node.OuterHtml;
                    if (item.Contains(UrlMaster))
                    {
                        bool write = true;
                        write = IsArticleUrl(item);
                        string _url = "";
                        if (_node.Attributes[1].Value.Contains("http://"))
                            _url = _node.Attributes[1].Value;

                        if (_node.Attributes[0].Value.Contains("http://"))
                            _url = _node.Attributes[0].Value;

                        if (_url.Length >=2 &&  write && !new DbHelper().IsExitsUrl(_url))
                        {
                           
                            CurrentLink link = new CurrentLink();
                            link.Id = Guid.NewGuid();
                            link.Url = _url;
                            link.CreateBy = "TVVLCrawler";
                            link.CreateDate = DateTime.Now;
                            db.CurrentLinks.Add(link);

                            string _pageContent = HtmlHelper.GetHtmlPage(_url);

                            TVVLArticleView _article = new TVVLArticleView();
                            _article.MasterUrl = UrlMaster;
                            _article.Url = _url;
                            _article.PageContent = _pageContent;

                            _article = Mapper.ArticleViewToTVVL(_article.Process());
                         
                            try
                            {
                                db.SaveChanges();

                                WebserviceHelper.PostArticle(crawlerName, WebserviceHelper.CrawlerArticleToObject(_article));                                

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
