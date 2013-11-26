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
    public interface IBidvCrawler
    {                                                  
        Article GetArticleFrom(string url);                       
        void ProcessBIDVPortal(PropertyBag propertyBag);       
    }
}
