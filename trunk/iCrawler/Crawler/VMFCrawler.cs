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
        
        public string appName = "chuyentoan";
        public string UrlMaster = "http://diendantoanhoc.net/home";

        public string crawlerName = "VMFCrawler";        
        public string fileConfig = "VMF.txt";

        public string prefixUrl = "http://diendantoanhoc.net/home";

        public bool IsArticleUrl(string url)
        {
            string[] strSplits = new string[] { "/" };
            if (url.Split(strSplits, StringSplitOptions.None).Length >= 3)
                return true;
            else
                return false;                          
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
            _newLinks = _links;
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

        public void Process(string url)
        {
            List<HtmlNode> _newLinks = new List<HtmlNode>();
            _newLinks = GetNewLinks();

            //STEP 4 : for each all link, read and parse to article
            foreach (HtmlNode _link in _newLinks)
            {
                string _detailUrl = prefixUrl + _link.Attributes[0].Value;
                string _pageContent = "";
                bool _isInserted = false;
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
