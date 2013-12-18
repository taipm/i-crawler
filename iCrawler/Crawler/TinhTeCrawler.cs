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
        public string fileConfig = "TinhTe.txt";

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
        public List<HtmlNode> GetNewLinks()
        {
            List<HtmlNode> _list = new List<HtmlNode>();

            //STEP 1 : Get HtmlContent of MasterUrl
            string url = UrlMaster;
            string masterContent = HtmlHelper.GetHtmlPage(url);

            //STEP 2 : Find all link have masterContent            
            List<HtmlNode> _links = new List<HtmlNode>();
            _links = HtmlHelper.GetLinks(masterContent).ToList();

            //STEP 3 : Select all links what it is news
            List<HtmlNode> _newLinks = new List<HtmlNode>();
            _newLinks = _links.Where(c => c.OuterHtml.Contains("/threads/") && !c.OuterHtml.Contains("/noi-quy-dien-dan.20/")).Distinct().ToList(); ;
            foreach (var _link in _newLinks)
            {
                if (_link != null)
                {
                    string _detailUrl = prefixUrl + _link.Attributes[0].Value;
                    if (IsArticleUrl(_detailUrl) && !new DbHelper().IsExitsUrl(_detailUrl))
                        _list.Add(_link);
                }
            }
            return _list.Distinct().ToList();
        }

        public void Process()
        {
            List<HtmlNode> _newLinks = new List<HtmlNode>();
            _newLinks = GetNewLinks();

            //STEP 4 : for each all link, read and parse to article
            foreach (HtmlNode _link in _newLinks)
            {
                string _detailUrl = "";
                string _pageContent = "";
                bool _isInserted = false;                
                _detailUrl = _link.Attributes[0].Value;                
                try
                {
                    _pageContent = HtmlHelper.GetHtmlPage(_detailUrl);
                }
                catch
                {
                    _pageContent = "";
                }

                if (_pageContent.Length > 1 && !new DbHelper().IsExitsUrl(_detailUrl))
                    _isInserted = new DbHelper().AddCurrentLink(_detailUrl, crawlerName);

                if (_isInserted)
                {
                    ArticleView _object = new ArticleView();
                    _object.MasterUrl = UrlMaster;
                    _object.Url = _detailUrl;
                    _object.PageContent = _pageContent;
                    _object.FileCofig = fileConfig;

                    try
                    {
                        _object = _object.Process();
                        bool isPosted = WebserviceHelper.PostArticle(appName, WebserviceHelper.CrawlerArticleToObject(_object));
                        if (isPosted)
                            EmailHelper.SendArticleToEmail(_object);
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
