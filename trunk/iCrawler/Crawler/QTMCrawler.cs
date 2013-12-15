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
        public string fileConfig = "QuanTriMang.txt";

        public string today = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
        public string UrlMaster = "http://www.quantrimang.com.vn/daily/";
        public string prefixUrl = "http://www.quantrimang.com.vn";

        public bool IsArticleUrl(string url)
        {
            string[] _words = url.Split('-');
            if (StringHelper.IsNumber(_words[_words.Length - 1]))
                return true;
            else
                return false;
        }

        public List<HtmlNode> GetNewLinks()
        {
            List<HtmlNode> _list = new List<HtmlNode>();

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
            foreach(var _link in _newLinks)
            {
                if(_link != null)
                {
                    string _detailUrl = prefixUrl + _link.Attributes[0].Value;                
                    if (IsArticleUrl(_detailUrl) && !new DbHelper().IsExitsUrl(_detailUrl))
                        _list.Add(_link);
                }                
            }
            return _list;
        }
            
        public void Process()
        {
            List<HtmlNode> _newLinks = new List<HtmlNode>();
            _newLinks = GetNewLinks();

            //STEP 4 : for each all link, read and parse to article
            foreach (HtmlNode _link in _newLinks)
            {                
                string _detailUrl = prefixUrl + _link.Attributes[0].Value;                
                bool _isInserted = new DbHelper().AddCurrentLink(_detailUrl, crawlerName);
                if(_isInserted)
                {
                    string _pageContent = HtmlHelper.GetHtmlPage(_detailUrl);
                    
                    ArticleView _object = new ArticleView();
                    _object.MasterUrl = UrlMaster;
                    _object.Url = _detailUrl;
                    _object.PageContent = _pageContent;
                    _object.FileCofig = fileConfig;
                    _object = _object.Process();

                    bool isPosted = WebserviceHelper.PostArticle(appName, WebserviceHelper.CrawlerArticleToObject(_object));
                    if (isPosted) 
                        EmailHelper.SendArticleToEmail(_object);                                                                          
                }                               
            }                       
        }                
    }
}
