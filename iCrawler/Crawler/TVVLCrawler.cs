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

namespace iCrawler
{
    public class TVVLHelper
    {        
        private iCrawlerEntities db = new iCrawlerEntities();

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
            if (url == "http://diendantoanhoc.net/") return false;
                
            return true;
        }

        
        
        public void ProcessTVVL(string url)
        {
            string content = HtmlHelper.GetHtmlPage(url);
            List<HtmlNode> _links = new List<HtmlNode>();
            _links = HtmlHelper.GetLinks(content).Where(c => c.OuterHtml.Contains("http://360.thuvienvatly.com/bai-viet")).ToList();

            List<HtmlNode> _articleNodes = new List<HtmlNode>();
            _articleNodes = HtmlHelper.GetNodesByDiv("article_row", content).ToList();

            if (_links != null && _links.Count > 0)
            {
                foreach (var _node in _links)
                {
                    string item = _node.OuterHtml;
                    if (item.Contains("http://360.thuvienvatly.com/bai-viet"))
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
                            string _pageContent = HtmlHelper.GetHtmlPage(_url);

                            HtmlNode _nodeFullArticle;
                            _nodeFullArticle = HtmlHelper.GetNodesByDiv("full-article", _pageContent).FirstOrDefault();
                            link.HtmlContent = _nodeFullArticle.OuterHtml;



                            TVVLArticleView _article = new TVVLArticleView();
                            _article.MasterUrl = "http://360.thuvienvatly.com/bai-viet";
                            _article.Url = _url;
                            _article.Content = _nodeFullArticle.OuterHtml;
                            _article.DownloadTime = DateTime.Now;
                            _article.Tags = "";

                            _nodeFullArticle = HtmlHelper.GetNodesByDiv("article-rel-wrapper", _pageContent).FirstOrDefault();
                            link.Title = _nodeFullArticle.OuterHtml;
                            _article.Title = HtmlHelper.RemoveHTMLTagsFromString(_nodeFullArticle.OuterHtml);

                            _nodeFullArticle = HtmlHelper.GetNodesByDiv("tag", _pageContent).FirstOrDefault();
                            _article.Tags = _nodeFullArticle.OuterHtml;

                            db.CurrentLinks.Add(link);
                            try
                            {
                                db.SaveChanges();
                                //_article = _article.Process();
                                _article = Mapper.ArticleViewToTVVL(_article.Process());
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
        

        public DateTime GetWebLastUpdate(string html)
        {            
            HtmlDocument htmlDoc = new HtmlDocument
            {
                OptionAddDebuggingAttributes = false,
                OptionAutoCloseOnEnd = true,
                OptionFixNestedTags = true,
                OptionReadEncoding = true
            };
            
            

            htmlDoc.LoadHtml(html);
            
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//p[@class='publishdate']");
            // Extract Title
            if (nodes != null)
            {
                string time = nodes[1].InnerHtml.Trim(); //Thứ hai, 29 Tháng 7 2013 11:40"
                int indexComma = time.IndexOf(",");
                time = time.Replace(" Tháng ", "/").Remove(0, indexComma + 2); //" 29:7 2013 11:40"
                int index2Cham = time.LastIndexOf(":");
                time = time.Remove(index2Cham - 3, 6).Replace(" ","/"); //"29/7/2013"                                
                string[] strSplits = new string[] { "/"};
                string[] strArrays = time.Split(strSplits, StringSplitOptions.None);

                return DateTime.Parse(strArrays[2] + "/" + strArrays[1] + "/" + strArrays[0]);
            }
            else
            {
                return DateTime.Now;
            }
        }       
    }
}
