using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Jwt
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
            #region JWT验证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ClockSkew = TimeSpan.FromSeconds(5),
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = Const.Domain,//Audience
                        ValidIssuer = Const.Domain,//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey)),//拿到SecurityKey

                    };
                    // options.ForwardSignIn = "http://localhost:5000/api/login";
                    options.Events = new JwtBearerEvents
                    {
                        OnForbidden = context =>
                        {
                            context.Response.Redirect("/api/login");
                            return Task.CompletedTask;
                        }
                    };
                    // options.ForwardForbid = "http://localhost:5000/api/login";
                });
            #endregion

            services.AddControllers();//.SetCompatibilityVersion(CompatibilityVersion.Latest);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //这是认证
            app.UseAuthentication();
            // app.UseJwtBearerAuthentication(new JwtBearerOptions()
            // {
            //     Audience = "http://localhost:5001/",
            //     Authority = "http://localhost:5000/",
            //     AutomaticAuthenticate = true
            // });

            app.UseAuthorization();

            // app.UseStatusCodePages(async context =>
            // {
            //     var request = context.HttpContext.Request;
            //     var response = context.HttpContext.Response;

            //     if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
            //     // you may also check requests path to do this only for specific methods       
            //     // && request.Path.Value.StartsWith("/specificPath")

            //     {
            //         response.Redirect("http://localhost:5000/api/login");
            //     }
            // });

            // app.UseStatusCodePagesWithRedirects("/api/login");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
