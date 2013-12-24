using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCrawler
{
    interface ICrawler
    {
        bool IsArticleUrl(string url);
        void Process();
    }
}
