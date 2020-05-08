using System;
using RssFeedToTwitter.Wrappers;

namespace RssFeedToTwitter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting console application: 'RssFeedToTwitter'");

            // Initialize the Environment Variables
            string consumerKey = System.Environment.GetEnvironmentVariable("RssFeedToTwitter_consumerKey");
            string consumerSecret = System.Environment.GetEnvironmentVariable("RssFeedToTwitter_consumerSecret");
            string userAccessToken = System.Environment.GetEnvironmentVariable("RssFeedToTwitter_userAccessToken");
            string userAccessSecret = System.Environment.GetEnvironmentVariable("RssFeedToTwitter_userAccessSecret");

            // Initialize the TweetinviWrapper with authentication details.
            TweetinviWrapper tweetinviWrapper = new TweetinviWrapper(consumerKey, consumerSecret, userAccessToken, userAccessSecret);

            // Initilize the program
            FeedToTwitterManager.Run(tweetinviWrapper);

            Console.WriteLine("Ending console application: 'RssFeedToTwitter'");
        }
    }
}
