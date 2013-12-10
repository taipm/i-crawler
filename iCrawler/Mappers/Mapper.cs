using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCrawler.Models;
using ChuyenToan = iCrawler.Models.ChuyenToan;
using ChuyenLy = iCrawler.Models.ChuyenLy;
using HtmlAgilityPack;

namespace iCrawler.Mappers
{
    public static class Mapper
    {
        public static ChuyenToan.Article ArticleViewToArticle(VMFArticleView article)
        {
            ChuyenToan.Article _article = new ChuyenToan.Article();
            _article.Id = Guid.NewGuid();
            _article.Title = article.Title;
            _article.Content = article.Content;
            _article.CountViews = 0;
            _article.CreateDate = DateTime.Now;
            _article.LastUpdate = DateTime.Now;
            _article.CreateBy = "VMFCrawler";
            _article.UpdateBy = "VMFCrawler";
            _article.IsApproved = false;
            _article.IsPublished = false;
            _article.IsReviewed = false;
            _article.IsSent = false;
            _article.CategoryId = Guid.Empty;

            
            return _article;
        }

        public static VMFArticleView ArticleViewToVMF(ArticleView article)
        {           
            return new VMFArticleView(){
                MasterUrl = article.MasterUrl,
                Url = article.Url,
                Title = "VMFCrawler : " + article.Title,
                Content = article.Content,
                DownloadTime = article.DownloadTime,
                Tags = article.Tags,
                Authors = article.Authors
            };
        }

        public static ChuyenLy.Article ArticleViewToObject(TVVLArticleView article)
        {
            ChuyenLy.Article _article = new ChuyenLy.Article();
            _article.CreateDate = DateTime.Now;
            _article.Content = article.Content;
            _article.CountViews = 0;
            _article.CreateBy = "TVVLCrawler";
            _article.Title = article.Title;
            _article.Tags = article.Tags;
            _article.UpdateBy = "TVVLCrawler";
            _article.CategoryId = Guid.Empty;
            _article.IsReviewed = false;
            _article.IsApproved = false;
            _article.IsPublished = false;
            _article.LastUpdate = DateTime.Now;

            return _article;            
        }

        public static TVVLArticleView ArticleViewToTVVL(ArticleView article)
        {
            TVVLArticleView _view = new TVVLArticleView();
            _view.MasterUrl = article.MasterUrl;
            _view.Url = article.Url;
            _view.Title = article.Title;
            _view.Summary = article.Summary;
            _view.Content = article.Content;
            _view.Tags = article.Tags;

            _view.CreateDate = article.CreateDate;
            _view.CreateBy = article.CreateBy;
            _view.UpdateDate = article.UpdateDate;
            _view.UpdateBy = article.UpdateBy;

            _view.DownloadTime = article.DownloadTime;
            
            return _view;            
        }

        public static BIDVArticleView UrlToArticle(string url)
        {
            if (url.Contains("bidv"))
                return BidvUrlToArticle(url);
            else 
                return null;
        }

        private static BIDVArticleView BidvUrlToArticle(string url)
        {
            string htmlContent = HtmlHelper.GetHtmlPage(url);

            if (htmlContent == null) return null;

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);

            string[] strSplits = new string[] { "<!-- Phần nội dung -->", "plcRoot_Layout_zoneMaster_pageplaceholder_pageplaceholder_Layout_zoneDocumentHome_ChiTietVanBan_hddCaptch", "<td bgcolor=\"#FFFFFF\" class=\"text5\">" };
            string[] arrEs = htmlContent.Split(strSplits, StringSplitOptions.None);

            if (arrEs.Count() < 3) return null;

            BIDVArticleView _article = new BIDVArticleView();

            if (arrEs.Count() > 3)
            {
                _article.Url = url;
                _article.Title = arrEs[6].Split(new string[] { "</td>" }, StringSplitOptions.None)[0].Trim().Replace("\n", " ").Replace("\r", " "); //Chinh la Trich Yeu                

                strSplits = new string[] { "<!-- Phần nội dung -->", "plcRoot_Layout_zoneMaster_pageplaceholder_pageplaceholder_Layout_zoneDocumentHome_ChiTietVanBan_hddCaptch", "plcRoot_Layout_zoneMaster_pageplaceholder_pageplaceholder_Layout_zoneDocumentHome_ChiTietVanBan_panelComment" };
                arrEs = htmlContent.Split(strSplits, StringSplitOptions.None);
                _article.NoiDung = arrEs[3].Replace(" />", "").Trim();
            }

            if (_article.Title == null) return null;
            if (_article.NoiDung == null) return null;
            if (_article.NoiDung.ToLower().Contains("bạn chưa được cấp quyền truy cập tài liệu này"))
            {
                return null;
            }

            return _article;
        }
    }
}
