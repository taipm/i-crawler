using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCrawler.ServiceLayer
{
    public class FilterService
    {
        public string InKeywords;
        public string OutKeywords;
        public string Text;

        public bool IsInKeywords()
        {
            string[] strSplits = new string[] { "," };
            string[] arrEs = InKeywords.Split(strSplits, StringSplitOptions.None);

            if (InKeywords.ToLower().Contains("all")) return true;

            foreach (string e in arrEs)
            {
                if (Text.ToLower().Contains(e.ToLower())) return true;
            }

            return false;
        }

        public bool IsOutKeywords()
        {
            string[] strSplits = new string[] { "," };
            if (OutKeywords == null)
                return false;

            string[] arrEs = OutKeywords.Split(strSplits, StringSplitOptions.None);

            foreach (string e in arrEs)
            {
                if (Text.ToLower().Contains(e.ToLower())) return false;
            }

            return true;
        }

        public bool IsPassFilter()
        {
            return IsInKeywords() && !IsOutKeywords();
        }
    }
}
