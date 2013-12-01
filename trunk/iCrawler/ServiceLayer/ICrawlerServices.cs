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
        void DownloadAttachments(CurrentLink link, string htmlContent);
        void SendEmail(CurrentLink link);   
        int GetMaxUrlId();       
    }    
}
