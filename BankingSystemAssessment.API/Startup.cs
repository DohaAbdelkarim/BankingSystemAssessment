using BankingSystemAssessment.API.Infrastructure;
using BankingSystemAssessment.API.Infrastructure.Extensions;
using BankingSystemAssessment.API.Infrastructure.Services;
using BankingSystemAssessment.Core.ErrorHandling.Helpers;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace BankingSystemAssessment.API
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
            services.AddControllers()
                .AddFluentValidation(fvc =>
                 {
                     fvc.RegisterValidatorsFromAssemblyContaining<Startup>();
                     fvc.ConfigureClientsideValidation(enabled: false);
                 });

            services.AddDbContext<BankingSystemContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("BankingSystem"));
            });

            //GSProblemDetailsFactory in Core.ErrorHandling, override CreateProblemDetails to manipulate the error response
            services.AddTransient<ProblemDetailsFactory, GSProblemDetailsFactory>();

            services.AddAutoMapper(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);
            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);

            services.Configure<SwaggerUIOptions>(Configuration.GetSection("SwaggerUI"));
            services.Configure<SwaggerGenOptions>(Configuration.GetSection("SwaggerGen"));
            services.AddSwaggerGen();


            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<ITransactionService, TransactionService>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ExceptionHandlerOptions exceptionHandlerOptions = new ExceptionHandlerOptions()
            {
                AllowStatusCode404Response = true,
                ExceptionHandlingPath = "/error"
            };
            app.UseExceptionHandler(exceptionHandlerOptions);

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //migrate the database to build the schema
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var serviceScope = serviceScopeFactory.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<BankingSystemContext>();
            if (dbContext.Database.IsSqlServer())
            {
                app.Migrate();
            }
        }
    }
}