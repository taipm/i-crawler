using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using iCrawler.Models;
using System.IO;
using iCrawler.ServiceLayer;
using iCrawler.iTrackingMvc4Services;

public static class WebserviceHelper
{
   

   

    public static Article CrawlerArticleToObject(TVVLArticleView _article)
    {
        Article _object = new Article();
        _object.Id = Guid.NewGuid();
        _object.Title = _article.Title;
        _object.Summary = _article.Summary;
        _object.Content = _article.Content;
        _object.Tags = _article.Tags;
        _object.CreateBy = _article.CreateBy;
        _object.UpdateBy = _article.UpdateBy;
        _object.LastUpdate = DateTime.Now;
        _object.CreateDate = DateTime.Now;
        _object.IsPublished = true;
        _object.IsApproved = true;
        _object.IsReviewed = true;
        _object.CountViews = 0;

        return _object;
    }

    

    public static Article CrawlerArticleToObject(ArticleView _article)
    {
        Article _object = new Article();
        _object.Id = Guid.NewGuid();
        _object.Title = _article.Title;
        _object.Summary = _article.Summary;
        _object.Content = _article.Content;
        _object.Tags = _article.Tags;
        _object.CreateBy = _article.CreateBy;
        _object.UpdateBy = _article.UpdateBy;
        _object.LastUpdate = DateTime.Now;
        _object.CreateDate = DateTime.Now;
        _object.IsPublished = true;
        _object.IsApproved = true;
        _object.IsReviewed = true;
        _object.CountViews = 0;
        _object.AvatarPath = _article.AvatarImage;

        return _object;
    }

    public static bool IsPostArticle(Article _article)
    {
        if (_article == null) return false;
        if (_article.Title == null || _article.Title.Length <= 3) return false;
        if (_article.Content == null || _article.Content.Length <= 3) return false;
        return true;

    }

    public static bool PostArticle(string appName, Article _article)
    {
        if (IsPostArticle(_article))
        {
            var _object = new iTrackingServices().CreateArticle(appName,
                                        _article.Title, _article.Summary, _article.Content,
                                        _article.UpdateBy, _article.CreateBy,
                                        _article.Tags, _article.IsPublished.Value,0);
            if (_object != null) return true;            
        }
        return false;
    }
}

