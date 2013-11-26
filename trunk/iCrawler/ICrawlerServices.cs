using NCrawler;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Data.Entity.Validation;
using iCrawler.Models;

namespace iCrawler
{    
    public interface ICrawlerServices
    {
        //HistoryLink CurrentLinkToLink(CurrentLink link);
        //void CleanDB(string url);
        //void CleanHistory();
        //bool CleanHistory(string url);
        //void CleanDB();
        //void MoveLinksToHistory(List<CurrentLink> links);
        bool IsOutKeywords(string keywords, string text);
        //bool IsInKeywords(string keywords, string text);
        void DownloadAttachments(CurrentLink link, string htmlContent);
        void SendEmail(CurrentLink link);
        //Dictionary<string, string> GetContacts(string url, string htmlContent);
        int GetMaxUrlId();       
    }    
}
