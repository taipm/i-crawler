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
    public static bool IsPostArticle(Article _article)
    {
        if (_article == null) return false;
        if (_article.Title == null) return false;
        if (_article.Content == null) return false;
        return true;

    }

    public static void PostArticle(string crawlerName, Article _article)
    {
        if (IsPostArticle(_article))
        {
            var _object = new iTrackingServices().CreateArticle(crawlerName,
                                        _article.Title,
                                        _article.Summary,
                                        _article.Content,
                                        _article.UpdateBy,
                                        _article.CreateBy,
                                        _article.Tags, _article.IsPublished.Value,0);
            
        }
    }
}

