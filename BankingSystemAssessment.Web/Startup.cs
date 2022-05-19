using BankingSystemAssessment.Core.ErrorHandling;
using BankingSystemAssessment.Core.ErrorHandling.Extensions;
using BankingSystemAssessment.Web.ApiClients;
using BankingSystemAssessment.Web.ApiClients.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using BankingSystemAssessment.Web.Infrastructure;

namespace BankingSystemAssessment.Web
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
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Customer/Index", "");
            });

            services.UsePollyPolicies(Configuration);

            services.AddRazorPages();

            services.AddSingleton<ICorrelationIdResolver, CorrelationIdResolver>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpClient<ICustomerApiClient, CustomerApiClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["BankingSystemAssessment:EndpointUrl"]);
            }).AddAllPolicies();

            services.AddHttpClient<IAccountApiClient, AccountApiClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["BankingSystemAssessment:EndpointUrl"]);
            }).AddAllPolicies();

            services.AddHeaderPropagation(options =>
            {
                options.Headers.Add(Constants.CorrelationIdHeaderKey);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.Use((context, next) =>
            {
                var headers = context.Request.Headers;
                if (!headers.ContainsKey(Constants.CorrelationIdHeaderKey))
                {
                    headers[Constants.CorrelationIdHeaderKey] = Guid.NewGuid().ToString();
                }
                context.Response.Headers[Constants.CorrelationIdHeaderKey] = headers[Constants.CorrelationIdHeaderKey];
                return next.Invoke();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
