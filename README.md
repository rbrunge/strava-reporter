# strava-reporter
Project has 2 purposes: 
- 1. After each run send a mail to user
- 2. See same data in a web site

Prerequisites:
- ElasticSearch somewhere. I use Bonsai.io
- You will need to create your own strava App to run this:

After that, store the _client id_ and _client secret_ in local configuration:

In command prompt goto same folder as projects.json is in. Run the following with your own key:

**dotnet user-secrets set Authentication:Strava:ClientId _www_**

**dotnet user-secrets set Authentication:Strava:ClientSecret _zzz_**

**dotnet user-secrets set RemoteRepository:Elasticsearch:FullAccessUrl _zzz_**
