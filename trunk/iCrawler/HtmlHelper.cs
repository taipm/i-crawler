using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace HtmlHelper
{
    public static class HtmlHelper
    {        
        public static List<HtmlNode> GetNodes(string className, string html)
        {            
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> links = doc.DocumentNode.SelectNodes("//div[@class='" + className + "']").ToList<HtmlNode>();

            return links;
            
        }
        public static List<HtmlNode> GetLinks(string html)
        {
            //List<string> links = new List<string>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> links = doc.DocumentNode.SelectNodes("//a[@href]").ToList<HtmlNode>();

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
}
