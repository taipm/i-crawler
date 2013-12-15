// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SimpleCrawlDemo.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the SimpleCrawlDemo type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;


namespace iCrawler.ServiceLayer
{
	public static class StringHelper
	{

        //public static string RemoveInvalidFilenameCharacters(string filename) 
        //{
        //    int i;
        //    if (!string.IsNullOrEmpty(filename)) {
        //        List invalidchars=new List();
        //        invalidchars.AddRange(""#%&*/:<>?\{|}~".ToCharArray());
        //        invalidchars.AddRange(System.IO.Path.GetInvalidPathChars());
        //        invalidchars.AddRange(System.IO.Path.GetInvalidFileNameChars());
        //        invalidchars.AddRange(new char[]{System.IO.Path.PathSeparator,System.IO.Path.AltDirectorySeparatorChar});
        //        for(i=0;i<invalidchars.Count;++i) {
        //            filename = filename.Replace(invalidchars[i].ToString(), string.Empty);
        //        }
        //    }
        //    return filename;
        //}

        public static bool IsValidUrl(string url)
        {
                string strRegex = "^(https?://)"
                    + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //user@
                    + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184
                    + "|" // allows either IP or domain
                    + @"([0-9a-z_!~*'()-]+\.)*" // tertiary domain(s)- www.
                    + @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]" // second level domain
                    + @"(\.[a-z]{2,6})?)" // first level domain- .com or .museum is optional
                    + "(:[0-9]{1,5})?" // port number- :80
                    + "((/?)|" // a slash isn't required if there is no file name
                    + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
                return new Regex(strRegex).IsMatch(url);
        }

        public static bool IsNumber(string text)
        {
            try
            {
                int.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string Standard(string text)
        {
            while (text.IndexOf("  ") >= 0)    //tim trong chuoi vi tri co 2 khoang trong tro len      
                text = text.Replace("  ", " ");   //sau do thay the bang 1 khoang trong     
            while (text.IndexOf("\t") >= 0)    //tim trong chuoi vi tri co 2 khoang trong tro len      
                text = text.Replace("\t", "");   //sau do thay the bang 1 khoang trong     
            while (text.IndexOf("\n") >= 0)    //tim trong chuoi vi tri co 2 khoang trong tro len      
                text = text.Replace("\n", "");   //sau do thay the bang 1 khoang trong     
            return text;
        }

        public static string GetFirstWords(string text, int n)
        {
            text = Standard(text);
            string[] words = text.Split(' ');
            if(words.Length > n)
            {
                return text.Substring(0, text.IndexOf(words[n]));
            }
            else
            {
                return text;
            }            
        }

		public static string SubString(string text, string fromString, string toString)
        {
            int first = text.IndexOf(fromString);
            int last = text.IndexOf(toString);
            return text.Substring(first, last);
        }
        
        public static string RemoveToEnd(string text, string fromString)
        {
            int first = text.IndexOf(fromString);            
            return text.Remove(first);
        }        
	}
}