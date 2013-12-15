using iCrawler;
using iCrawler.Models;
using iCrawler.ServiceLayer;
using NCrawler;
using NCrawler.Interfaces;
using System;
using System.Globalization;
using Autofac;
using System.Data;
using System.Data.Entity;
using NCrawler.Extensions;
using NCrawler.HtmlProcessor;
using NCrawler.Services;

using System.Text.RegularExpressions;
using System.Collections.Generic;

internal class DumperStep : IPipelineStep
{
    iCrawlerEntities db = new iCrawlerEntities();    

    /// <summary>
    /// </summary>
    /// <param name = "crawler">
    /// 	The crawler.
    /// </param>
    /// <param name = "propertyBag">
    /// 	The property bag.
    /// </param>
    public void Process(Crawler crawler, PropertyBag propertyBag)
    {
        CultureInfo contentCulture = (CultureInfo)propertyBag["LanguageCulture"].Value;
        string cultureDisplayValue = "N/A";
        if (!contentCulture.IsNull())
        {
            cultureDisplayValue = contentCulture.DisplayName;
        }

        lock (this)
        {
            if (propertyBag.ResponseUri.AbsoluteUri.ToLower().Contains("bidv") && NetworkService.IsConnectedToBIDV())
            {
                //new BidvCrawler().ProcessBIDVPortal(propertyBag);
            }
            else if (propertyBag.ResponseUri.AbsoluteUri.ToLower().Contains("thuvienvatly.com"))
            {
                new TVVLCrawler().ProcessTVVL(propertyBag.ResponseUri.AbsoluteUri);
            }
            else if (propertyBag.ResponseUri.AbsoluteUri.ToLower().Contains("diendantoanhoc"))
            {
                new VMFCrawler().ProcessVMF(propertyBag.ResponseUri.AbsoluteUri);
            }
            else if (propertyBag.ResponseUri.AbsoluteUri.ToLower().Contains("quantrimang"))
            {
                new QTMCrawler().Process();
            }
            else if (propertyBag.ResponseUri.AbsoluteUri.ToLower().Contains("tinhte"))
            {
                new TinhTeCrawler().Process();
            }
            else if (propertyBag.ResponseUri.AbsoluteUri.ToLower().Contains("vnmath"))
            {
                new VnMathCrawler().Process();
            }
            else if (propertyBag.ResponseUri.AbsoluteUri.ToLower().Contains("vatlyvietnam"))
            {
                new VLVNCrawler().Process();
            }
            else if (propertyBag.ResponseUri.AbsoluteUri.ToLower().Contains("y2graphic"))
            //if (propertyBag.ResponseUri.AbsoluteUri.ToLower().Contains("y2graphic"))
            {
                new Y2GraphicCrawler().Process();
            }
            else
            {
                WebContent page = new WebContent();
                page.Id = Guid.NewGuid();
                page.TimeDownloaded = DateTime.Now;
                page.Url = propertyBag.Step.Uri.AbsoluteUri;
                page.Title = propertyBag.Title;
                page.Content = propertyBag.Text;
                page.HtmlContent = propertyBag.HtmlContent;


                try
                {
                    db.WebContents.Add(page);
                    db.SaveChanges();
                }
                catch
                {
                    Console.WriteLine("Lỗi insert database");
                }
            }
        }
    }


}