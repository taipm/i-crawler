
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
            try
            {
                _fullNode = HtmlHelper.GetNodesByClass("contentheading", PageContent).FirstOrDefault();
                this.Title = HtmlHelper.RemoveHTMLTagsFromString(_fullNode.OuterHtml);
            }
            catch
            {
                this.Title = "";
            }
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
            try
            {
                _fullNode = HtmlHelper.GetNodesByDiv("jsn-article-toolbar", PageContent).FirstOrDefault();
                this.Authors = _fullNode.OuterHtml;
            }
            catch
            {
                this.Authors = "";
            }
        }

        public void GetSummary()
        {
            try
            {
                _fullNode = HtmlHelper.GetNodesByDiv("jsn-article-content", PageContent).FirstOrDefault();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (_fullNode != null)
            {
                this.Summary = _fullNode.InnerText.Substring(0, 300) + "...";
            }
            this.Summary = _fullNode.InnerText;            
        }

        public void GetContent()
        {
            _fullNode = HtmlHelper.GetNodesByDiv("article", PageContent).FirstOrDefault();
            string _content = StringHelper.RemoveToEnd(_fullNode.OuterHtml, "Stickies");
            List<string> files = GetFiles();
            if (files != null && files.Count >= 1)
            {
                this.Content += "<br /> Đính kèm : <br />";
                foreach (var file in files)
                {
                    this.Content += file + "<br />";
                }            
            }            
        }

        public void GetTags()
        {
            this.Tags = "diendantoanhoc.net, vmfcrawler";
        }

        public void ProcessImg()
        {            
        }

        List<string> Files;
        public List<string> GetFiles()
        {
            Files = new List<string>();
            try
            {
                List<HtmlNode> _fileLinks = HtmlHelper.GetLinks(this.Content).Where(c => c.OuterHtml.Contains(";module=attach&amp;section=attach&amp;attach_id")).ToList();
                foreach (var _fileLink in _fileLinks)
                {
                    this.Files.Add("http://diendantoanhoc.net" + _fileLink.Attributes[0].Value.Replace(";module=attach&amp;section=attach&amp;attach_id", "&module=attach&section=attach&attach_id"));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return this.Files;
        }

        public VMFArticleView Process()
        {
            GetTitle();
            GetSummary();
            GetContent();
            GetTags();
            ProcessImg();
            GetAvatarImage();
            GetFiles();

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
