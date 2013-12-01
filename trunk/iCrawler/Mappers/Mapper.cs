using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCrawler.Models;

namespace iCrawler.Mappers
{
    public static class Mapper
    {
        public static VMFArticleView ArticleViewToVMF(ArticleView article)
        {
            return new VMFArticleView(){
                MasterUrl = article.MasterUrl,
                Url = article.Url,
                Title = "VMFCrawler : " + article.Title,
                Content = article.Content
            };
        }

        public static TVVLArticleView ArticleViewToTVVL(ArticleView article)
        {
            return new TVVLArticleView()
            {
                MasterUrl = article.MasterUrl,
                Url = article.Url,
                Title = "TVVLCrawler : " + article.Title,
                Content = article.Content
            };
        }

        public static Article UrlToArticle(string url)
        {
            if (url.Contains("bidv"))
                return BidvUrlToArticle(url);
            else 
                return null;
        }

        private static Article BidvUrlToArticle(string url)
        {
            string htmlContent = HtmlHelper.GetHtmlPage(url);

            if (htmlContent == null) return null;

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);

            string[] strSplits = new string[] { "<!-- Phần nội dung -->", "plcRoot_Layout_zoneMaster_pageplaceholder_pageplaceholder_Layout_zoneDocumentHome_ChiTietVanBan_hddCaptch", "<td bgcolor=\"#FFFFFF\" class=\"text5\">" };
            string[] arrEs = htmlContent.Split(strSplits, StringSplitOptions.None);

            if (arrEs.Count() < 3) return null;

            Article _article = new Article();

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
