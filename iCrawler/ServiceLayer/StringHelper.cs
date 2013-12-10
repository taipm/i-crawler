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
	public class StringHelper
	{
        public string Standard(string text)
        {
            while (text.IndexOf("  ") >= 0)    //tim trong chuoi vi tri co 2 khoang trong tro len      
                text = text.Replace("  ", " ");   //sau do thay the bang 1 khoang trong     
            return text;
        }

        public string GetFirstWords(string text, int n)
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
            //try
            //{
                
            //    string firstWords = Regex.Match(text, @"^(\w+\b.*?){" + n + "}").ToString();
            //    return firstWords;
            //}
            //catch
            //{
            //    return text;
            //}
        }

		public string SubString(string text, string fromString, string toString)
        {
            int first = text.IndexOf(fromString);
            int last = text.IndexOf(toString);
            return text.Substring(first, last);
        }
        
        public string RemoveToEnd(string text, string fromString)
        {
            int first = text.IndexOf(fromString);            
            return text.Remove(first);
        }        
	}
}