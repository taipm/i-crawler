using HtmlAgilityPack;
using iCrawler.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCrawler.Models
{
    public class VLVNArticleView : ArticleView
    {
        public string ImageSource;
        public string PageContent;        
        HtmlNode _fullNode;
        
        public void GetTitle()
        {
            try
            {

                _fullNode = HtmlHelper.GetTable("contentpagetitle", PageContent).First();
                this.Title = HtmlHelper.RemoveHTMLTagsFromString(_fullNode.OuterHtml).Trim();
            }
            catch
            {
                this.Title = "";
            }
        }

        public void GetAvatarImage()
        {
                       
        }

        public void GetAuthors()
        {           
        }

        public void GetSummary()
        {            
        }

        public void GetContent()
        {

            //_fullNode = HtmlHelper.GetTable("contentpaneopen", PageContent).FirstOrDefault();
            this.Content = PageContent;            
        }

        public void GetTags()
        {
            this.Tags = "vatlyvietnam.org, VLVNCrawler";
        }

        public void ProcessImg()
        {            
        }

        List<string> Files;
        public List<string> GetFiles()
        {
            Files = new List<string>();           
            return this.Files;
        }
        
        public VLVNArticleView Process()
        {
            GetTitle();            
            GetContent();
            GetSummary();
            GetTags();
            ProcessImg();
            GetAvatarImage();
            GetFiles();            

            this.DownloadTime = DateTime.Now;
            this.CreateBy = "VLVNCrawler";
            this.UpdateBy = "VLVNCrawler";
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;

            this.isPublished = true;
            this.IsReviewed = true;
            return this;
        }        
    }
}
