namespace RssFeedToTwitter.Helpers
{
    public static class StringHelpers
    {
        private const string EllipsisString = "...";

        public static string TruncateString(string originalString, int maxStringLength, bool useEllipsis = false)
        {
            if (originalString.Length < maxStringLength)
            {
                return originalString;
            }
            else
            {
                if (useEllipsis)
                {
                    return originalString.Substring(0, maxStringLength - EllipsisString.Length) + EllipsisString;

                }
                else
                {
                    return originalString.Substring(0, maxStringLength);
                }
            }
        }
    }
}
