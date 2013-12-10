
using iCrawler.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCrawler.Models
{
    public class VMFArticleView : ArticleView
    {
        public void ProcessContent()
        {
            this.Content = new StringHelper().RemoveToEnd(this.Content, "Stickies");
        }

        new public void ProcessImg()
        {
        }

        new public VMFArticleView Process()
        {
            ProcessImg();
            ProcessContent();
            return this;
        }
    }
}
