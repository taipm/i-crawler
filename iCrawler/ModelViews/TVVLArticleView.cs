using HtmlAgilityPack;
using iCrawler.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCrawler.Models
{
    public class TVVLArticleView : ArticleView
    {
        public string ImageSource = "http://360.thuvienvatly.com/images/";
        public string PageContent;
        public string Url;

        new public void GetContent()
        {
            HtmlNode _nodeFullArticle;
            _nodeFullArticle = HtmlHelper.GetNodesByDiv("full-article", PageContent).FirstOrDefault();            

            this.Content = _nodeFullArticle.OuterHtml;            
        }
        
        public void ProcessContent()
        {
            this.Content = new StringHelper().RemoveToEnd(this.Content, "Nếu thấy thích, hãy Đăng kí để nhận bài viết mới qua email");
        }

        public string GetSummary()
        {
            //leading
            string summary = this.Title;
            return summary;
        }

        new public void GetTitle()
        {
            var _nodeFullArticle = HtmlHelper.GetNodesByDiv("article-rel-wrapper", PageContent).FirstOrDefault();            
            this.Title = HtmlHelper.RemoveHTMLTagsFromString(_nodeFullArticle.OuterHtml).Trim();
        }

        new public void GetTags()
        {
            try
            {
                var _nodeFullArticle = HtmlHelper.GetNodesByDiv("tag", PageContent).FirstOrDefault();
                this.Tags = HtmlHelper.RemoveHTMLTagsFromString(_nodeFullArticle.OuterHtml).Trim();
            }
            catch
            {
                this.Tags = "";
            }
        }
        

        new public void ProcessImg()
        {
            this.Content = Content.Replace("/images/", ImageSource);
        }

        new public TVVLArticleView Process()
        {
            GetContent();
            ProcessContent();
            GetTitle();
            GetSummary();
            GetTags();
            ProcessImg();
            
            this.DownloadTime = DateTime.Now;            
            
            return this;
        }
    }
}
