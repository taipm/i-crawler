using HtmlAgilityPack;
using iCrawler.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCrawler.Models
{
    public class VnMathArticleView : ArticleView
    {        
        public string PageContent;
        
        HtmlNode _fullNode;

        public void GetTitle()
        {
            try
            {
                _fullNode = HtmlHelper.GetNodesByDiv("single-title", PageContent).FirstOrDefault();
                string titleText = HtmlHelper.RemoveHTMLTagsFromString(_fullNode.OuterHtml).Trim();
                this.Title = StringHelper.RemoveToEnd(titleText, "VNMATH.COM").Trim();
            }
            catch
            {
                this.Title = "";
            }
        }

        public void GetSummary()
        {
            try
            {                
                string text = HtmlHelper.RemoveHTMLTagsFromString(this.Content);
                string strToRemove = StringHelper.GetFirstWords(text, 30);
                string summary = StringHelper.GetFirstWords(text, 200);
                this.Summary = summary.Replace(strToRemove, "");
            }
            catch
            {
                this.Summary = "";
            }

        }

        public void GetContent()
        {
            try
            {
                _fullNode = HtmlHelper.GetNodesByDiv("single-content", PageContent).FirstOrDefault();
                this.Content = _fullNode.OuterHtml;
            }
            catch
            {
                this.Content = "";
            }
        }

        public void GetTags()
        {
            //try
            //{
            //    var _fullNode = HtmlHelper.GetNodesByDiv("widget_tag_cloud", PageContent).FirstOrDefault();
            //    this.Tags = "vnmath.com, VnMathCrawler," + _fullNode.OuterHtml;
            //}
            //catch
            //{
            //    this.Tags = "vnmath.com, VnMathCrawler,";
            //}
            this.Tags = "vnmath.com, VnMathCrawler,";
        }     
       
        public void ProcessImg()
        {
            //this.Content = Content.Replace("/photos/", ImageSource);
        }

        public VnMathArticleView Process()
        {
            GetTitle();                        
            GetContent();
            GetSummary();  
            GetTags();
            ProcessImg();

            this.DownloadTime = DateTime.Now;
            this.CreateBy = "VnMathCrawler";
            this.UpdateBy = "VnMathCrawler";
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;

            this.isPublished = true;
            this.IsReviewed = true;
            return this;
        }
    }
}
