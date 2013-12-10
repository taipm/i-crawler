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
        
        HtmlNode _fullNode;

        public void GetTitle()
        {            
            _fullNode = HtmlHelper.GetNodesByDiv("article-rel-wrapper", PageContent).FirstOrDefault();
            this.Title = HtmlHelper.RemoveHTMLTagsFromString(_fullNode.OuterHtml).Trim();
        }

        public void GetSummary()
        {            
            try
            {
                //_fullNode = HtmlHelper.GetNodesByDiv("full-article", PageContent).FirstOrDefault();
                //this.Summary = _fullNode.InnerText.Substring(0, 200);
                string text = HtmlHelper.RemoveHTMLTagsFromString(this.Content).Replace("//document.write('');", "").Trim();
                string strToRemove = new StringHelper().GetFirstWords(text, 30);
                string summary = new StringHelper().GetFirstWords(text, 200);
                this.Summary = summary.Replace(strToRemove,"");
            }
            catch
            {
                this.Summary = "";
            }

        }

        public void GetContent()
        {  
            _fullNode = HtmlHelper.GetNodesByDiv("full-article", PageContent).FirstOrDefault();
            this.Content = new StringHelper().RemoveToEnd(_fullNode.OuterHtml, "Nếu thấy thích, hãy Đăng kí để nhận bài viết mới qua email"); 
        }

        public void GetTags()
        {
            try
            {
                var _fullNode = HtmlHelper.GetNodesByDiv("tag", PageContent).FirstOrDefault();
                this.Tags = "thuvienvatly.com, tvvlcrawler," + HtmlHelper.RemoveHTMLTagsFromString(_fullNode.InnerText).Trim();
            }
            catch
            {
                this.Tags = "thuvienvatly.com, tvvlcrawler,";
            }
        }     
       
        public void ProcessImg()
        {
            this.Content = Content.Replace("/images/", ImageSource);
        }

        public TVVLArticleView Process()
        {
            GetTitle();                        
            GetContent();
            GetSummary();  
            GetTags();
            ProcessImg();

            this.DownloadTime = DateTime.Now;
            this.CreateBy = "TVVLCrawler";
            this.UpdateBy = "TVVLCrawler";
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;

            this.isPublished = true;
            this.IsReviewed = true;
            return this;
        }
    }
}
