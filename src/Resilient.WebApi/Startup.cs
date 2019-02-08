using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Resilient.WebApi.Client;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace Resilient.WebApi
{
    public class Startup
    {
        private readonly IDictionary<string, int> _defaultValues = new Dictionary<string, int>()
        {
            { "RetryCount", 3},
            { "HandledEventsAllowedBeforeBreaking", 5},
            { "DurationOfBreakInSeconds", 30},
            { "TimeOutInSeconds", 30},
        };

        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpClient<PostClient>(client =>
            {
                client.BaseAddress = new Uri("http://jsonplaceholder.typicode.com/");
                client.DefaultRequestHeaders.Add("User-Agent", "resilient-api");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(GetConfigurationValue("RetryCount")))
            .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(GetConfigurationValue("HandledEventsAllowedBeforeBreaking"), TimeSpan.FromSeconds(GetConfigurationValue("DurationOfBreakInSeconds"))))
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(GetConfigurationValue("TimeOutInSeconds")));

            // Register the Swagger generator.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Posts API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Posts API V1");
            });

            app.UseMvc();
        }

        private int GetConfigurationValue(string key)
        {
            var applicationConfigurationSection = Configuration.GetSection("ApplicationConfiguration");

            if (string.IsNullOrEmpty(applicationConfigurationSection.GetSection(key).Value))
                return _defaultValues[key];
            else
                return Convert.ToInt32(applicationConfigurationSection.GetSection(key).Value);
        }
    }
}