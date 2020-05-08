using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using RssFeedToTwitter.Helpers;
using RssFeedToTwitter.Wrappers;
using Tweetinvi;
using Tweetinvi.Models;

namespace RssFeedToTwitter
{
    public static class FeedToTwitterManager
    {
        // ToDo: Take this as a parameter in the program / from a configuration file.
        public const string RssFeedUrl = "https://azurecomcdn.azureedge.net/en-in/updates/feed/";
        public const string HashTag = "#azure";
        public const int TwitterTweetMaxCharacters = 280;

        /// <summary>
        /// The primary logic which retrieves the feed and accordingly publishes the tweets to the twitter account.
        /// </summary>
        public static void Run(TweetinviWrapper tweetinviWrapper)
        {
            // Retrieve the feed items
            List<SyndicationItem> rssFeedItems = RssFeedHelpers.RetrieveRssFeedItemsInChronologicalOrder(RssFeedUrl);

            // Retrieve the last tweet on the timeline
            ITweet latestTweet = tweetinviWrapper.GetLastestTweet();
            string latestTweetMessage = null;
            if (latestTweet != null)
            {
                latestTweetMessage = GetFeedTextFromTweetMessage(latestTweet.Text);
                System.Console.WriteLine($"[Run]: The lastest tweet on the timeline is '{latestTweetMessage}'.");
            }

            // Identify the position on the feed from where the tweets need to be published.
            // This is to ensure that the same feed item is not published as a tweet twice.
            int index = IdentifyFeedIndexPositionToStartPublishingTweets(rssFeedItems, latestTweetMessage);

            // Publish the rest of the tweets from the feed index position in chronological order.
            PublishTweetsFromFeedItems(tweetinviWrapper, rssFeedItems, index);
        }

        /// <summary>
        /// Identifies the position from which the tweets must be published
        /// This is to avoid publishing the same tweet twice.
        /// </summary>
        /// <returns>The postion from which to start publishing tweets.</returns>
        private static int IdentifyFeedIndexPositionToStartPublishingTweets(List<SyndicationItem> rssFeedItems, string latestTweetMessage)
        {
            int index = 0;

            // If the last tweet message was null then all of the feed items need to be published as tweet.
            if (latestTweetMessage != null)
            {
                for (int i = 0; i < rssFeedItems.Count; i++)
                {
                    string feedText = rssFeedItems[i].Title.Text;
                    if (latestTweetMessage.Equals(feedText))
                    {
                        index = i + 1;
                        System.Console.WriteLine($"[IdentifyFeedIndexPositionToStartPublishingTweets]: The feed at position '{i}' matches" +
                            $"the lastest tweet on the timeline. Start publishing tweets from '{index}' position");
                        break;
                    }
                }
            }

            return index;
        }

        /// <summary>
        /// Publishes the tweets by forming the tweet message and publishing to twitter.
        /// </summary>
        private static void PublishTweetsFromFeedItems(TweetinviWrapper tweetinviWrapper, List<SyndicationItem> rssFeedItems, int startIndex)
        {
            System.Console.WriteLine($"[PublishTweetsFromFeedItems]: Publishing '{rssFeedItems.Count - startIndex}' new tweets to the timeline.");
            for (int i = startIndex; i < rssFeedItems.Count; i++)
            {
                string twitterMessage = GetTweetMessageFromFeedItem(rssFeedItems[i]);
                tweetinviWrapper.PublishTweet(twitterMessage);
            }
        }

        /// <summary>
        /// Retrieves the Feed text from the twitter message.
        /// Needs to remove the additional information in tweet like the HashTag or links.
        /// </summary>
        /// <returns>Returns the main title text of the feed item from the tweet message</returns>
        private static string GetFeedTextFromTweetMessage(string tweetMessage)
        {
            // Retrieve the main text from the tweet which was present in the Feed Item originally published.
            // The additional '-1' is to account for the space before the use of HashTag
            return tweetMessage.Substring(0, tweetMessage.IndexOf(HashTag, StringComparison.OrdinalIgnoreCase) - 1);
        }

        /// <summary>
        /// Constructs a tweet message from a feed item.
        /// It includes the Title Text, the HashTag and the link to the post.
        /// </summary>
        /// <returns>Returns the message which can be used for tweet</returns>
        private static string GetTweetMessageFromFeedItem(SyndicationItem rssFeedItem)
        {
            string tweetContent = rssFeedItem.Title.Text + $" {HashTag} " + rssFeedItem.Links[0].Uri;
            string truncatedTweetContent = StringHelpers.TruncateString(tweetContent, TwitterTweetMaxCharacters, useEllipsis: true);
            return truncatedTweetContent;
        }
    }
}