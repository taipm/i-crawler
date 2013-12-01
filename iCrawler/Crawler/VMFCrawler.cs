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
    public class VMFHelper
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
                        //write = IsArticleUrl(item);

                        string _url = "";                        
                        _url = _node.Attributes[0].Value; 

                        if (_url.Length >=2 &&  write && HtmlHelper.IsValidUrl(_url)&& !new DbHelper().IsExitsUrl(_url))
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
                            _article.Content = _nodeFullArticle.OuterHtml;
                            _article.DownloadTime = DateTime.Now;
                            _article.Tags = "";

                            try
                            {
                                _nodeFullArticle = HtmlHelper.GetNodesByClass("contentheading", _pageContent).FirstOrDefault();
                                link.Title = _nodeFullArticle.OuterHtml;
                                _article.Title = HtmlHelper.RemoveHTMLTagsFromString(_nodeFullArticle.OuterHtml);
                             
                            }
                            catch
                            {
                                _article.Title = "";
                            }

                            //_nodeFullArticle = HtmlHelper.GetNodes("tag", _pageContent).FirstOrDefault();
                            //_article.Tags = _nodeFullArticle.OuterHtml;


                            if (_article.Title.Length >= 2)
                            {
                                db.CurrentLinks.Add(link);
                                try
                                {
                                    db.SaveChanges();
                                    _article = Mapper.ArticleViewToVMF(_article.Process());

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

               
        public DateTime GetWebLastUpdate(string html)
        {
            //class = publishdate
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

        //public bool IsLesson(string url)
        //{
        //    string[] strSplits = new string[] { "/" };
        //    string[] strEs = url.Split(strSplits, StringSplitOptions.None);
        //    if (strEs.Length > 5)
        //        return true;
        //    else
        //        return false;
        //}
        //public bool IsLesson2(string url)
        //{
        //    int index_ = url.LastIndexOf('-');
        //    int indexSplas = url.LastIndexOf('/');
        //    if (index_ > indexSplas)
        //        return true;
        //    else
        //        return false;
        //}

        //public bool IsLesson3(string url)
        //{
        //    if (url.Contains("print=1&layout=default&page="))
        //        return true;
        //    else
        //        return false;
        //}
    }
}
