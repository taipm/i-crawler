using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCrawler.ServiceLayer;
using System.IO;
using HtmlAgilityPack;

namespace iCrawler.Models
{
    public class ArticleView
    {
        public string PageContent;
        public string FileCofig;

        HtmlNode _fullNode;        

        //Params
        public string TagTitle;
        public string TagSummary;
        public string TagContent;
        public string TagTags;


        public string MasterUrl;
        public string Url;
        public string Title;
        public string Summary;
        public string Content;
        public string Tags;
        public string CreateBy;
        public string UpdateBy;        
        public string Footer;
        public string Header;        
        public string Authors;
        public string AvatarImage;

        public DateTime CreateDate;
        public DateTime UpdateDate;
        public DateTime DownloadTime;
        public int CountViews;

        public bool isPublished;
        public bool IsReviewed;
        
        public void GetParams(string configFile)
        {
            string path = Directory.GetCurrentDirectory();
            path = path.Replace(@"bin\Debug", "");
            string[] lines = System.IO.File.ReadAllLines(path + "\\Config\\" + configFile);
            foreach (string line in lines)
            {
                if (line.Length > 1)
                {
                    if (line.StartsWith("Title:")) this.TagTitle = line.Replace("Title:", "");
                    if (line.StartsWith("Summary:")) this.TagSummary = line.Replace("Summary:", "");
                    if (line.StartsWith("Content:")) this.TagContent= line.Replace("Content:", "");
                    if (line.StartsWith("Tags:")) this.TagTags = line.Replace("Tags:", "");
                }                
            }
        }

        public void GetTitle()
        {
            try
            {
                _fullNode = HtmlHelper.GetH1NodesByClass(this.TagTitle, PageContent).FirstOrDefault();
                this.Title = HtmlHelper.RemoveHTMLTagsFromString(_fullNode.OuterHtml).Trim();
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
                _fullNode = HtmlHelper.GetNodesByDiv(this.TagSummary, PageContent).FirstOrDefault();
                this.Summary = _fullNode.OuterHtml;               
            }
            catch
            {
                if (this.Content.Length > 200)
                {
                    string text = HtmlHelper.RemoveHTMLTagsFromString(this.Content);
                    string strToRemove = StringHelper.GetFirstWords(text, 30);
                    string summary = StringHelper.GetFirstWords(text, 200);
                    this.Summary = summary.Replace(strToRemove, "");
                }
                else
                {
                    string text = HtmlHelper.RemoveHTMLTagsFromString(this.Content);
                    this.Summary = text;
                }
            }

        }
        public void GetContent()
        {
            try
            {
                _fullNode = HtmlHelper.GetNodesByDiv(this.TagContent, PageContent).FirstOrDefault();
                this.Content = _fullNode.OuterHtml;
            }
            catch
            {
                this.Content = "";
            }
        }

        public void GetTags() { }
        public void ProcessImg() { }

        public ArticleView Process()
        {
            GetParams(FileCofig);
            GetTitle();
            GetContent();
            GetSummary();
            GetTags();
            ProcessImg();

            this.DownloadTime = DateTime.Now;
            this.CreateBy = "iCrawler";
            this.UpdateBy = "iCrawler";
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;

            this.isPublished = true;
            this.IsReviewed = true;
            return this;
        }
    }       
}
