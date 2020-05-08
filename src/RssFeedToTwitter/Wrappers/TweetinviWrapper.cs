using System;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Models;

namespace RssFeedToTwitter.Wrappers
{
    public class TweetinviWrapper
    {
        /// <summary>
        /// The TweetinviWrapper constructor ensures that the User Authentication is properly setup 
        /// before invoking any helper methods.
        /// </summary>
        public TweetinviWrapper(string consumerKey, string consumerSecret, string userAccessToken, string userAccessSecret)
        {
            Auth.SetUserCredentials(consumerKey, consumerSecret, userAccessToken, userAccessSecret);
        }

        /// <summary>
        /// Retrieves the latest / top 'n' tweets from the authenticated user's timeline.
        /// </summary>
        /// <param name="maximumNumberOfTweets">The number of tweets to retrieve</param>
        /// <returns></returns>
        public IList<ITweet> GetLastestNTweetsFromTimeLine(int maximumNumberOfTweets)
        {
            System.Console.WriteLine($"[GetLastestNTweetsFromTimeLine]: Attempting retrieval of '{maximumNumberOfTweets}' tweets from the timeline.");

            IAuthenticatedUser user = User.GetAuthenticatedUser();
            List<ITweet> timelineTweets = new List<ITweet>(user.GetUserTimeline(maximumNumberOfTweets: maximumNumberOfTweets));

            System.Console.WriteLine($"[GetLastestNTweetsFromTimeLine]: Retrieved '{timelineTweets.Count}' tweets from the timeline.");
            return timelineTweets;
        }

        /// <summary>
        /// Retrieves the latest (i.e. the top/first) tweet from the authenticated user's timeline.
        /// </summary>
        public ITweet GetLastestTweet()
        {
            System.Console.WriteLine($"[GetLastestTweet]: Retreiving the last tweet from the timeline.");

            IList<ITweet> timelineTweets = GetLastestNTweetsFromTimeLine(maximumNumberOfTweets: 1);

            if (timelineTweets != null & timelineTweets.Count != 0)
            {
                return timelineTweets[0];
            }

            return null;
        }

        /// <summary>
        /// Delete the latest / top 'n' tweets from the authenticated user's timeline.
        /// </summary>
        /// <param name="maximumNumberOfTweets">The number of tweets to delete</param>
        public void DeleteLastestNTweetsFromTimeLine(int maximumNumberOfTweets)
        {
            IList<ITweet> timelineTweets = GetLastestNTweetsFromTimeLine(maximumNumberOfTweets);

            System.Console.WriteLine($"[DeleteLastestNTweetsFromTimeLine]: Deleting the retrieved tweets.");
            foreach (ITweet tweet in timelineTweets)
            {
               System.Console.WriteLine($"[DeleteLastestNTweetsFromTimeLine]: Deleting the tweet with id: '{tweet.Id}' and message: '{tweet.Text}'.");
               Tweet.DestroyTweet(tweet);
            }
        }

        /// <summary>
        /// Delete the latest / top tweet from the authenticated user's timeline.
        /// </summary>
        public void DeleteLastestTweet()
        {
            System.Console.WriteLine($"[DeleteLastestTweet]: Deleting the last tweet from the timeline.");
            DeleteLastestNTweetsFromTimeLine(maximumNumberOfTweets: 1);
        }

        /// <summary>
        /// Given tweet message, publishes the same to the twitter account.
        /// </summary>
        internal void PublishTweet(string twitterMessage)
        {
            ITweet publishedTweet = Tweet.PublishTweet(twitterMessage);
            System.Console.WriteLine($"[PublishTweet]: Published the tweet with id: '{publishedTweet.Id}' and message: '{twitterMessage}'.");
        }
    }
}