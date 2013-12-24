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
        public string PrefixUrl;
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
            int _first = this.TagTitle.IndexOf('|');
            int _last = this.TagTitle.LastIndexOf('|');            
            string _begin = this.TagTitle.Substring(_first+1,_last-_first-1);            
            string _end = this.TagTitle.Remove(0, this.TagTitle.LastIndexOf('|')+1);
            this.TagTitle = this.TagTitle.Substring(0, _first);

            try
            {
                _fullNode = HtmlHelper.GetNodesByClass(this.TagTitle, PageContent).FirstOrDefault();
                this.Title = HtmlHelper.RemoveHTMLTagsFromString(_fullNode.OuterHtml).Trim();
                //if (_begin == " ") _begin = StringHelper.GetFirstWords(this.Title, 1);
                //if (_end== " ") _begin = StringHelper.GetWordsBy(this.Title, this.Title.LastIndexOf(_end));
                //if (_begin == " " && _end == " ") return;
                //if(this.Title.Contains(_begin)&&this.Title.Contains(_end))
                //    this.Title = StringHelper.SubString(this.Title, _begin, _end).Replace(_end, "").Trim();
                return;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {                
                _fullNode = HtmlHelper.GetNodesByDiv(this.TagTitle, PageContent).FirstOrDefault();
                this.Title = HtmlHelper.RemoveHTMLTagsFromString(_fullNode.OuterHtml).Trim();
                //if (_begin == " ") _begin = StringHelper.GetFirstWords(this.Title, 1);
                //if (this.Title.Contains(_begin) && this.Title.Contains(_end))
                //    this.Title = StringHelper.SubString(this.Title, _begin, _end).Replace(_end, "").Trim();
                return;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            this.Title = "";
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
                if (this.Content.Length > 100)
                {
                    string text = HtmlHelper.RemoveHTMLTagsFromString(this.Content).Trim();
                    string _firstWord = StringHelper.GetWordsBy(text, 0);
                    string _endWord = StringHelper.GetWordsBy(text, 100);
                    this.Summary = StringHelper.SubString(text, _firstWord, _endWord);
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
        public void GetAvatarImg()
        {
            try
            {
                this.AvatarImage = HtmlHelper.GetImages(this.Content).FirstOrDefault();
            }
            catch
            {
                this.AvatarImage = string.Empty;
            }

            if (!String.IsNullOrEmpty(this.PrefixUrl))
            {
                this.AvatarImage = this.PrefixUrl + this.AvatarImage;
            }
        }

        public ArticleView Process()
        {
            GetParams(FileCofig);
            GetTitle();
            GetContent();
            GetSummary();
            GetTags();
            ProcessImg();
            GetAvatarImg();

            this.DownloadTime = DateTime.Now;
            this.CreateBy = "iCrawler";
            this.UpdateBy = "iCrawler";
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;

            this.isPublished = true;
            this.IsReviewed = true;

            this.Content += " <br /> Nguồn : " + this.Url;

            return this;
        }
    }       
}
