# rss-feed-to-twitter

A simple console application that uses rss feed to publish tweets.

The application does not do periodic updates, but that can be configured via an automated scheduled task which runs this every 'n' minutes / hours.

The twitter API keys are setup as environment variables. Refer: [Program.cs](src/RssFeedToTwitter/Program.cs).

[ToDo.md](ToDo.md) contains a list of all pending tasks.

## Package / Release

Build the application using: `dotnet build --configuration Release` from the project location: `src/RssFeedToTwitter`

The package at location `src/RssFeedToTwitter/bin/Release/netcoreapp3.1` can be deployed to Azure WebJobs.