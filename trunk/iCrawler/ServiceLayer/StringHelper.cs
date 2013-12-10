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
        public string GetFirstWords(string text, int n)
        {
            string firstWords = Regex.Match(text, @"^(\w+\b.*?){" + n +"}").ToString();
            return firstWords;
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