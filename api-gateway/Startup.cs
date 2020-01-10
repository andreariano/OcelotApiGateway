using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;

namespace Gateway.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication()
                .AddJwtBearer("AzureAD", options =>
                {
                    options.Audience = Configuration.GetValue<string>("AzureAd:Audience");
                    options.Authority = Configuration.GetValue<string>("AzureAd:Instance") + Configuration.GetValue<string>("AzureAd:TenantId");

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration.GetValue<string>("AzureAd:Issuer"),
                        ValidAudience = Configuration.GetValue<string>("AzureAd:Audience")
                    };
                }
            );

            services.AddOcelot(Configuration);
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseAuthentication();
            app.UseOcelot();
        }
    }
}
