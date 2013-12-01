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
        public string Content;
        public DateTime DownloadTime;
        public string Footer;
        public string Header;
        public string Tags;
        public string Authors;

        public void ProcessImg()
        {            
        }

        public ArticleView Process()
        {
            ProcessImg();
            return this;
        }
    }

    public class TVVLArticleView : ArticleView
    {
        public void ProcessContent()
        {
            this.Content = new StringHelper().RemoveToEnd(this.Content, "Nếu thấy thích, hãy Đăng kí để nhận bài viết mới qua email");
        }
        new public void ProcessImg()
        {
            this.Content = Content.Replace("/images/", "http://360.thuvienvatly.com/images/");
        }

        new public TVVLArticleView Process()
        {
            ProcessImg();
            ProcessContent();
            return this;
        }
    }

    public class VMFArticleView : ArticleView
    {
        new public void ProcessImg()
        {            
        }

        new public VMFArticleView Process()
        {
            ProcessImg();
            return this;
        }
    }

    
    public class Article
    {
        public string Url;
        public string Title;
        public string NoiDung;
        public string NgayPhatHanh;
        public string SoDi;
        public string SoKyHieu;
        public string DonViSoanThao;
        public string NgayKy;
        public string TrichYeu;
        public string DoKhan;
        public string NoiNhan;
    }
}
