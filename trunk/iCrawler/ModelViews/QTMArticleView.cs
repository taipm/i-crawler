using HtmlAgilityPack;
using iCrawler.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCrawler.Models
{
    public class QTMArticleView : ArticleView
    {
        public string ImageSource = "http://www.quantrimang.com.vn/photos/";
        public string PageContent;
        
        HtmlNode _fullNode;

        public void GetTitle()
        {
            _fullNode = HtmlHelper.GetH1NodesByClass("Title_article item", PageContent).FirstOrDefault();            
            this.Title = HtmlHelper.RemoveHTMLTagsFromString(_fullNode.OuterHtml).Trim();
        }

        public void GetSummary()
        {
            try
            {                
                string text = HtmlHelper.RemoveHTMLTagsFromString(this.Content);
                string strToRemove = new StringHelper().GetFirstWords(text, 30);
                string summary = new StringHelper().GetFirstWords(text, 200);
                this.Summary = summary.Replace(strToRemove, "");
            }
            catch
            {
                this.Summary = "";
            }

        }

        public void GetContent()
        {
            _fullNode = HtmlHelper.GetNodesByDiv("infoDetail", PageContent).FirstOrDefault();
            this.Content = _fullNode.OuterHtml; 
            //new StringHelper().RemoveToEnd(_fullNode.OuterHtml, "Nếu thấy thích, hãy Đăng kí để nhận bài viết mới qua email"); 
        }

        public void GetTags()
        {
            try
            {
                var _fullNode = HtmlHelper.GetNodesByDiv("xemthem", PageContent).FirstOrDefault();
                this.Tags = "quantrimang.com, QTMCrawler," + _fullNode.OuterHtml;
            }
            catch
            {
                this.Tags = "quantrimang.com, QTMCrawler,";
            }
        }     
       
        public void ProcessImg()
        {
            this.Content = Content.Replace("/photos/", ImageSource);
        }

        public QTMArticleView Process()
        {
            GetTitle();                        
            GetContent();
            GetSummary();  
            GetTags();
            ProcessImg();

            this.DownloadTime = DateTime.Now;
            this.CreateBy = "QTMCrawler";
            this.UpdateBy = "QTMCrawler";
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;

            this.isPublished = true;
            this.IsReviewed = true;
            return this;
        }
    }
}
