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
        
        public void ProcessVMF(string url)
        {
            string content = HtmlHelper.GetHtmlPage(url);
            List<HtmlNode> _links = new List<HtmlNode>();
            _links = HtmlHelper.GetLinks(content).Where(c => c.OuterHtml.Contains("http://diendantoanhoc.net/home/")).ToList();

            if (_links != null && _links.Count > 0)
            {
                foreach (var _node in _links)
                {
                    string item = _node.OuterHtml;
                    if (item.Contains("http://diendantoanhoc.net/"))
                    {
                        bool write = true;

                        string _url = "";
                        _url = _node.Attributes[0].Value;

                        if (_url.Length >= 2 && write && HtmlHelper.IsValidUrl(_url) && !new DbHelper().IsExitsUrl(_url))
                        {
                            CurrentLink link = new CurrentLink();
                            link.Id = Guid.NewGuid();
                            link.Url = _url;
                            link.CreateBy = "VMFCrawler";
                            link.CreateDate = DateTime.Now;
                            string _pageContent = HtmlHelper.GetHtmlPage(_url);

                            HtmlNode _nodeFullArticle;
                            _nodeFullArticle = HtmlHelper.GetNodesByDiv("article", _pageContent).FirstOrDefault();
                            link.HtmlContent = _nodeFullArticle.OuterHtml;


                            VMFArticleView _article = new VMFArticleView();
                            _article.MasterUrl = "http://diendantoanhoc.net/";
                            _article.Url = _url;
                            _article.PageContent = _pageContent;                            

                            try
                            {
                                _nodeFullArticle = HtmlHelper.GetNodesByClass("contentheading", _pageContent).FirstOrDefault();
                                link.Title = _nodeFullArticle.OuterHtml;                                
                            }
                            catch
                            {
                                _article.Title = "";
                            }

                            _article = Mapper.ArticleViewToVMF(_article.Process());  

                            if (_article.Title.Length >= 2)
                            {                                                                 
                                db.CurrentLinks.Add(link);
                             
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
}
