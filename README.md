# strava-reporter

A way to see your strava-data in another way. For me, this is a way to try to play with code.


To get this running, you'll need
- ElasticSearch somewhere. I use Bonsai.io
- create your own strava App to run this


After that, store the _client id_ and _client secret_ in local configuration:

In command prompt goto same folder as projects.json is in. Run the following with your own key:

**dotnet user-secrets set Authentication:Strava:ClientId _www_**

**dotnet user-secrets set Authentication:Strava:ClientSecret _zzz_**

**dotnet user-secrets set RemoteRepository:Elasticsearch:FullAccessUrl _zzz_**

