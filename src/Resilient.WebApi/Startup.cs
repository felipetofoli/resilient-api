using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Resilient.WebApi.Client;
using System;
using System.Net.Http;

namespace Resilient.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
                    .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(3))
                    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)))
                    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(30));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
