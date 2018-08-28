using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SampleUserService.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace SampleUserService
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddAuthentication(sharedOptions =>
            {
                //sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddAzureAd(options => Configuration.Bind("AzureAd", options));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("BearerTokenAuthentication", policyBuilder =>
                {
                    policyBuilder.RequireClaim("AuthenticationType", "OAuth2Bearer");
                });
            });

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Sample User API",
                    Description = "Sample User API",
                    TermsOfService = "TBD"
                });

                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    //AuthorizationUrl = string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}/oauth2/authorize", this.Configuration["Swagger:TenantId"]),
                    AuthorizationUrl = string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}/oauth2/authorize", "common"),
                    Scopes = new Dictionary<string, string>
                        {
                            { "access_as_user", "Access " + this.Configuration["Swagger:AppName"] }
                        }
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseCors("CorsPolicy");

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample User API V1");
                c.OAuthClientId(this.Configuration["Swagger:ClientId"]);
                c.OAuthClientSecret(this.Configuration["Swagger:ClientSecret"]);
                c.OAuthRealm(this.Configuration["Swagger:Realm"]);
                c.OAuthAppName(this.Configuration["Swagger:AppName"]);
                c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>() { { "resource", this.Configuration["Swagger:Audience"] } });
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });
        }
    }
}
