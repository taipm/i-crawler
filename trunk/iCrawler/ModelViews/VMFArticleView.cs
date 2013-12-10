
using HtmlAgilityPack;
using iCrawler.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCrawler.Models
{
    public class VMFArticleView : ArticleView
    {
        public string ImageSource;
        public string PageContent;        
        HtmlNode _fullNode;
        
        public void GetTitle()
        {
            _fullNode = HtmlHelper.GetNodesByClass("contentheading", PageContent).FirstOrDefault();
            this.Title = HtmlHelper.RemoveHTMLTagsFromString(_fullNode.OuterHtml);            
        }

        public void GetAvatarImage()
        {
           
            try
            {
                _fullNode = HtmlHelper.GetNodesByClass("jsn-article-content img", PageContent).FirstOrDefault();
                this.AvatarImage = _fullNode.OuterHtml;
            }
            catch
            {
                this.AvatarImage = "";
            }
        }

        public void GetAuthors()
        {
            _fullNode = HtmlHelper.GetNodesByDiv("jsn-article-toolbar", PageContent).FirstOrDefault();
            this.Authors = _fullNode.OuterHtml;
        }

        public void GetSummary()
        {
            _fullNode = HtmlHelper.GetNodesByDiv("jsn-article-content", PageContent).FirstOrDefault();
            try
            {
                this.Summary = _fullNode.InnerText.Substring(0, 300) + "...";
            }
            catch
            {
                this.Summary = _fullNode.InnerText;
            }
        }

        public void GetContent()
        {
            _fullNode = HtmlHelper.GetNodesByDiv("article", PageContent).FirstOrDefault();
            this.Content = new StringHelper().RemoveToEnd(_fullNode.OuterHtml, "Stickies");         
        }

        public void GetTags()
        {
            this.Tags = "diendantoanhoc.net, vmfcrawler";
        }

        public void ProcessImg()
        {            
        }

        public VMFArticleView Process()
        {
            GetTitle();
            GetSummary();
            GetContent();
            GetTags();
            ProcessImg();
            GetAvatarImage();

            this.DownloadTime = DateTime.Now;
            this.CreateBy = "VMFCrawler";
            this.UpdateBy = "VMFCrawler";
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;

            this.isPublished = true;
            this.IsReviewed = true;
            return this;
        }        
    }
}
