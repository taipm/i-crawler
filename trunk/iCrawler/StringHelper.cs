﻿// --------------------------------------------------------------------------------------------------------------------- 
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


namespace Helper
{
	public class StringHelper
	{
		public string SubString(string text, string fromString, string toString)
        {
            int first = text.IndexOf(fromString);
            int last = text.IndexOf(toString);
            return text.Substring(first, last);
        }        
	}
}