using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCrawler.ServiceLayer
{
    public class TimerHelper
    {
        public bool IsInTimes(string times, bool isSkip)
        {
            if (isSkip == true) return true;
            if (times == null) return false;

            string[] strSplits = new string[] { ";" };
            string[] arrEs = times.Split(strSplits, StringSplitOptions.None);
            if (arrEs != null && arrEs.Count() > 0)
            {
                foreach (string arr in arrEs)
                {
                    if (DateTime.Now.ToString().ToLower().Contains(arr)) return true;
                }
            }
            return false;
        }
    }
    
}
