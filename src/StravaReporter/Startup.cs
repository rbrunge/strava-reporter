using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StravaReporter.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using StravaReporter.Repositories;
using StravaReporter.Models;
using Nest;
using System;
using StravaReporter.Integration;
using StravaReporter.Models.ActivityViewModels;

namespace StravaReporter
{
    public partial class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            if (env == null) throw new ArgumentNullException(nameof(env));

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment()) 
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                // builder.AddUserSecrets<Startup>();
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            else
            {
                // after deploy, file is manually copied to root of site
                builder.AddJsonFile("appSecrets.json", optional: true, reloadOnChange: true);
            }

            builder.AddEnvironmentVariables();
            if (builder != null)
                Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddAuthentication(options =>
            {
                if (options != null)
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            if (Configuration == null) return;

            services.Configure<AppKeyConfig>(Configuration.GetSection("AppKeys"));
            services.AddMvc(options =>
            {
                // options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                if (options == null) throw new ArgumentNullException(nameof(options));
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.CookieHttpOnly = true;
            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IAccessTokenProvider, HttpContextAccessTokenProvider>();
            services.AddTransient<IUserSessionStateManager, UserSessionStateManager>();
            services.AddTransient<IActivityRepository, DocumentActivityRepository>();
            services.AddTransient<IActivityService, ActivityService>();
            services.AddTransient<IStravaIntegrator, StravaIntegrator>();
            services.AddTransient<IActivityAggregationViewModel, ActivityAggregationViewModel>();
            services.AddSingleton<IElasticClient>(
                new ElasticClient(new Uri(Configuration["RemoteRepository:Elasticsearch:FullAccessUrl"])));
            services.Configure<ElasticsearchSettings>(
                m =>
                {
                    if (m == null) throw new ArgumentNullException(nameof(m));
                    m.FullAccessUrl = Configuration.GetValue<string>("RemoteRepository:Elasticsearch:FullAccessUrl");
                });

            services.AddTransient(
                    s =>
                    {
                        var httpContextAccessor = s.GetService<IHttpContextAccessor>();
                        if (httpContextAccessor?.HttpContext == null)
                            throw new ArgumentNullException(nameof(httpContextAccessor));

                        return httpContextAccessor.HttpContext.User;
                    });

            services.Configure<IISOptions>(options =>
            {
                if (options == null) throw new ArgumentNullException(nameof(options));
                options.ForwardWindowsAuthentication = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (Configuration != null) loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();

            // app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            // app.UseIdentity();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = new PathString("/login"),
                LogoutPath = new PathString("/logout")
            });
            app.UseOAuthAuthentication(StravaOptions);
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Welcome}/{action=Index}/{id?}");
            });
            app.Map("/login", builder =>
            {
                builder.Run(async context =>
                {
                    if (context?.Authentication == null) throw new ArgumentNullException(nameof(context));

                    // Return a challenge to invoke the LinkedIn authentication scheme
                    if (context.Request != null)
                    {
                        var challengeAsync = 
                            context.Authentication.ChallengeAsync(
                                "Strava",
                                new AuthenticationProperties
                                {
                                    RedirectUri = context.Request.Query?["ReturnUrl"] ?? ""
                                });
                        if (challengeAsync != null)
                            await challengeAsync;
                    }
                });
            });
            app.Map("/logout", builder =>
            {
                builder.Run(async context =>
                {
                    if (context?.Authentication == null) throw new ArgumentNullException(nameof(context));

                    // Sign the user out of the authentication middleware (i.e. it will clear the Auth cookie)
                    var signOutAsync = context.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    if (signOutAsync != null)
                        await signOutAsync;

                    // Redirect the user to the home page after signing out
                    context.Response?.Redirect("/");
                });
            });
        }
    }
}
