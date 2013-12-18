using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;
using iCrawler.ServiceLayer;

public static class HtmlHelper
{
    public static List<string> GetImages(string Text)
    {
        List<string> images = new List<string>();
        string pattern = @"<(img)\b[^>]*>";

        Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
        MatchCollection matches = rgx.Matches(Text);

        for (int i = 0, l = matches.Count; i < l; i++)
        {
            images.Add(matches[i].Value);
        }

        return images;        
    }
    public static List<HtmlNode> GetTable(string tableName, string Text)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(Text);
        HtmlNodeCollection _tables;
        List<HtmlNode> _selectTables;
        try
        {
            _tables = doc.DocumentNode.SelectNodes("//table");
            _selectTables = doc.DocumentNode.SelectNodes("//table").Where(c => c.OuterHtml.Contains(tableName)).ToList();
            return _selectTables;
        }
        catch
        {
            Console.WriteLine("");
            return null;
        }
    }
    public static List<HtmlNode> GetRows(string Text)
    {
            List<HtmlNode> _rows = new List<HtmlNode>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Text);
            HtmlNodeCollection _tables;
            List<HtmlNode> _selectTables;
            try
            {
                _tables = doc.DocumentNode.SelectNodes("//table");
                _selectTables = doc.DocumentNode.SelectNodes("//table").Where(c => c.OuterHtml.Contains("contentpaneopen")).ToList();

                foreach (var _selectTable in _selectTables)
                {
                    List<HtmlNode> _links = HtmlHelper.GetLinks(_selectTable.OuterHtml);
                    if (_links != null && _links.Count > 0)
                    {
                        foreach (var _link in _links)
                        {
                            if(_link.OuterHtml.Contains(".htm"))
                                _rows.Add(_link);
                        }
                    }                   
                }                             

            }
            catch
            {
                Console.WriteLine("");
            }

            return _rows;

        }

        public static bool IsValidUrl(string text)
        {            
            Regex rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            return rx.IsMatch(text);
        }

        public static string RemoveScripts(string Text)
        {
            Regex regxScriptRemoval = new Regex(@"<script(.+?)*</script>");
            var newHtml = regxScriptRemoval.Replace(Text, "");

            return newHtml; 
        }
        public static string RemoveHTMLTagsFromString(string Text)
        {
            //Text = RemoveScripts(Text);
            Text = Text.Replace("\r", " ");
            Text = Text.Replace("\n", " ");
            // Remove sTabs
            Text = Text.Replace("\t", string.Empty);
            Text = Text.Trim();

            return Regex.Replace(Text, "<[^>]*>", String.Empty);
        }
        
        public static List<HtmlNode> GetNodesByDiv(string className, string html)
        {            
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> links = doc.DocumentNode.SelectNodes("//div[@class='" + className + "']").ToList<HtmlNode>();

            return links;
            
        }
        
        public static List<HtmlNode> GetNodesByClass(string className, string html)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> links = new List<HtmlNode>();
            try
            {
                links = doc.DocumentNode.SelectNodes("//h2[@class='" + className + "']").ToList<HtmlNode>();
                return links;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                links = doc.DocumentNode.SelectNodes("//h1[@class='" + className + "']").ToList<HtmlNode>();
                return links;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            if(links.Count < 1)
                return null;
            return links;
        }

        public static List<HtmlNode> GetLinks(string html)
        {            
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> links = doc.DocumentNode.SelectNodes("//a[@href]").Distinct().ToList<HtmlNode>();

            return links;
        }


        public static string GetHtmlPage(string url)
        {
            string root = url;
            WebRequest request = WebRequest.Create(root);
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            var dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;       
        }
    
}

