using HtmlAgilityPack;
using iCrawler.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace iCrawler.Models
{
    public class TinhTeArticleView : ArticleView
    {        
        public string PageContent;        
        HtmlNode _fullNode;

        public string TitleTag = "titleBar";
        public string ContentTag = "messageContent";
        public string TagTag = "xemthem";
        public string SummaryTag;
        
        public void GetTitle()
        {
            try
            {
                _fullNode = HtmlHelper.GetNodesByDiv(TitleTag, PageContent).FirstOrDefault();
                string titleText = HtmlHelper.RemoveHTMLTagsFromString(_fullNode.OuterHtml).Trim();
                this.Title = StringHelper.RemoveToEnd(titleText, "Thảo luận").Trim();
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
                _fullNode = HtmlHelper.GetNodesByDiv(ContentTag, PageContent).FirstOrDefault();
                this.Content = _fullNode.OuterHtml;
            }
            catch
            {
                this.Content = "";
            }
        }

        public void GetTags()
        {
            try
            {
                var _fullNode = HtmlHelper.GetNodesByDiv(TagTag, PageContent).FirstOrDefault();
                this.Tags = "tinhte.vn, TinhTeCrawler," + _fullNode.OuterHtml;
            }
            catch
            {
                this.Tags = "tinhte.vn, TinhTeCrawler,";
            }
        }     
       
        public void ProcessImg()
        {
            //this.Content = Content.Replace("/photos/", ImageSource);
        }

        public TinhTeArticleView Process()
        {            
            GetTitle();                        
            GetContent();
            GetSummary();  
            GetTags();
            ProcessImg();

            this.DownloadTime = DateTime.Now;
            this.CreateBy = "TinhTeCrawler";
            this.UpdateBy = "TinhTeCrawler";
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;

            this.isPublished = true;
            this.IsReviewed = true;
            return this;
        }
    }
}
