using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCrawler.ServiceLayer;

namespace iCrawler.Models
{
    public class ArticleView
    {
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
    }       
}
