using System;

using Autofac;
using System.Data;
using System.Data.Entity;
using NCrawler.Extensions;
using NCrawler.HtmlProcessor;
using NCrawler.Interfaces;
using NCrawler.Services;
using System.Globalization;
using NCrawler;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using iCrawler.ServiceLayer;
using iCrawler.Models;

namespace iCrawler.Demo
{
    #region Nested type: DumperStep

    /// <summary>
    /// 	Custom pipeline step, to dump url to console
    /// </summary>
    internal class DumperStep : IPipelineStep
    {
        iCrawlerEntities db = new iCrawlerEntities();

        #region IPipelineStep Members

        /// <summary>
        /// </summary>
        /// <param name = "crawler">
        /// 	The crawler.
        /// </param>
        /// <param name = "propertyBag">
        /// 	The property bag.
        /// </param>
        public void Process(Crawler crawler, PropertyBag propertyBag)
        {
            CultureInfo contentCulture = (CultureInfo)propertyBag["LanguageCulture"].Value;
            string cultureDisplayValue = "N/A";
            if (!contentCulture.IsNull())
            {
                cultureDisplayValue = contentCulture.DisplayName;
            }

            lock (this)
            {                
                if (propertyBag.ResponseUri.AbsoluteUri.ToLower().Contains("bidv") && NetworkService.IsConnectedToBIDV())
                {
                    new BidvCrawler().ProcessBIDVPortal(propertyBag);
                }
                else if (propertyBag.ResponseUri.AbsoluteUri.ToLower().Contains("thuvienvatly.com"))
                {
                    new TVVLCrawler().ProcessTVVL(propertyBag.ResponseUri.AbsoluteUri);
                }                
                else if (propertyBag.ResponseUri.AbsoluteUri.ToLower().Contains("diendantoanhoc"))
                {
                    new VMFCrawler().ProcessVMF(propertyBag.ResponseUri.AbsoluteUri);
                }
                else
                {
                    WebContent page = new WebContent();
                    page.Id = Guid.NewGuid();
                    page.TimeDownloaded = DateTime.Now;
                    page.Url = propertyBag.Step.Uri.AbsoluteUri;
                    page.Title = propertyBag.Title;
                    page.Content = propertyBag.Text;
                    page.HtmlContent = propertyBag.HtmlContent;


                    try
                    {
                        db.WebContents.Add(page);
                        db.SaveChanges();
                    }
                    catch
                    {
                        Console.WriteLine("Lỗi insert database");
                    }                    
                }
            }
        }

        #endregion
    }

    #endregion
	/// <summary>
	/// 	Crawl a site and adhere to the robot rules, and also crawl 2 levels of any external
	/// 	links. Dump everything in the same instance of a IPipeline step(DumperStep)
	/// </summary>
	internal class AdvancedCrawlDemo
	{        

		#region Class Methods
        public static IFilter[] ExtensionsToSkip = new[]
			{
				(RegexFilter)new Regex(@"(\.jpg|\.css|\.js|\.gif|\.jpeg|\.png|\.ico)",
					RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
			};

        public static void Run(string url)
        {
            NCrawlerModule.Setup();

            // Register new implementation for ICrawlerRules using our custom class CustomCrawlerRules defined below
            NCrawlerModule.Register(builder =>
                builder.Register((c, p) =>
                {
                    NCrawlerModule.Setup(); // Return to standard setup
                    return new CustomCrawlerRules(p.TypedAs<Crawler>(), c.Resolve<IRobot>(p), p.TypedAs<Uri>(),
                        p.TypedAs<ICrawlerHistory>());
                }).
                As<ICrawlerRules>().
                InstancePerDependency());

            Console.Out.WriteLine("Advanced crawl demo");
            
            using (Crawler c = new Crawler(
            new Uri(url),
            new HtmlDocumentProcessor(), // Process html
            new DumperStep())
            {
                MaximumThreadCount = 2,
                MaximumCrawlDepth = 2,
                ExcludeFilter = ExtensionsToSkip,
            })
            {
                // Begin crawl
                c.Crawl();
            }
            
        }

		public static void Run()
		{
			NCrawlerModule.Setup();

			// Register new implementation for ICrawlerRules using our custom class CustomCrawlerRules defined below
			NCrawlerModule.Register(builder =>
				builder.Register((c, p) =>
					{
						NCrawlerModule.Setup(); // Return to standard setup
						return new CustomCrawlerRules(p.TypedAs<Crawler>(), c.Resolve<IRobot>(p), p.TypedAs<Uri>(),
							p.TypedAs<ICrawlerHistory>());
					}).
				As<ICrawlerRules>().
				InstancePerDependency());			

            List<string> urls = new List<string>();
            iCrawlerEntities _db = new iCrawlerEntities();            
            foreach(var item in _db.Urls)
            {                
                if(item != null && new TimerHelper().IsInTimes(item.Times, true))
                {
                    string _url = item.Url1;                    
                    using (Crawler c = new Crawler(
                    new Uri(_url),
                    new HtmlDocumentProcessor(), // Process html
                    new DumperStep())
                        {
                            MaximumThreadCount = 2,
                            MaximumCrawlDepth = 2,
                            ExcludeFilter = ExtensionsToSkip,
                        })
                    {
                        // Begin crawl
                        c.Crawl();
                    }
                }                
            }			
		}

		#endregion
	}

	public class CustomCrawlerRules : CrawlerRulesService
	{
		#region Readonly & Static Fields

		private readonly ICrawlerHistory m_CrawlerHistory;

		#endregion

		#region Constructors

		public CustomCrawlerRules(Crawler crawler, IRobot robot, Uri baseUri, ICrawlerHistory crawlerHistory)
			: base(crawler, robot, baseUri)
		{
			m_CrawlerHistory = crawlerHistory;
		}

		#endregion

		#region Instance Methods

		public override bool IsExternalUrl(Uri uri)
		{
			// Is External Url
			if (!base.IsExternalUrl(uri))
			{
				return false;
			}

			// Yes, check if we have crawled it before
			if (!m_CrawlerHistory.Register(uri.GetUrlKeyString(m_Crawler.UriSensitivity)))
			{
				return true;
			}

			// Create child crawler to traverse external site with max 2 levels
			using (Crawler externalCrawler = new Crawler(uri,
				new HtmlDocumentProcessor(), // Process html
				new DumperStep())
				{
					MaximumThreadCount = 1,
					MaximumCrawlDepth = 2,
					MaximumCrawlCount = 10,
					//ExcludeFilter = Program.ExtensionsToSkip,
				})
			{
				// Crawl external site
				externalCrawler.Crawl();
			}

			// Do not follow link on this crawler
			return true;
		}

		#endregion
	}
}