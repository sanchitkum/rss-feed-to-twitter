using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RssFeedToTwitter.Helpers
{
    public static class RssFeedHelpers
    {
        /// <summary>
        /// Returns the 'SyndicationFeed' object which represents a top level feed object from the feed URL.
        /// </summary>
        /// <param name="rssFeedUrl">The URL representing the RSS Feed.</param>
        /// <returns></returns>
        public static SyndicationFeed RetrieveRssFeed(string rssFeedUrl)
        {
            System.Console.WriteLine($"[RetrieveRssFeed]: Retrieving RSS feed from url: '{rssFeedUrl}'");
            using (XmlReader xmlReader = XmlReader.Create(rssFeedUrl))
            {
                return SyndicationFeed.Load(xmlReader);
            }
        }

        /// <summary>
        /// Returns the feed items in a list in a chronological order.
        /// </summary>
        /// <param name="rssFeedUrl">The URL representing the RSS Feed.</param>
        /// <returns></returns>
        public static List<SyndicationItem> RetrieveRssFeedItemsInChronologicalOrder(string rssFeedUrl)
        {
            SyndicationFeed syndicationFeed = RetrieveRssFeed(rssFeedUrl);

            List<SyndicationItem> syndicationFeedItems = new List<SyndicationItem>(syndicationFeed.Items);
            // The RSS Feed has the latest item at the top. Reverse the feed to retrieve in chronological order.
            syndicationFeedItems.Reverse();

            System.Console.WriteLine($"[RetrieveRssFeedItemsInChronologicalOrder] Reversed the RSS with '{syndicationFeedItems.Count}' feed items " + 
                "to be in chronological order.");
            return syndicationFeedItems;
        }
    }
}
