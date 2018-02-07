using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthServerMVcClient
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
            services.AddMvc();
            //关闭JWT的Claim类型映射，以便允许well-known claims.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    //当用户需要登录的时候，将使用的是OpenId Connect Scheme
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    //这个flow主要应用于客户端应用程序, 这里的客户端应用程序主要是指javascript应用程序. implicit flow是很简单的重定向flow, 它允许我们重定向到authorization server, 然后带着id token重定向回来, 这个 id token就是openid connect 用来识别用户是否已经登陆了. 同时也可以获得access token
                    //options.ClientId = "mvc_implicit";

                    //options.ResponseType = "id_token token";

                    options.ClientId = "mvc_code";
                    options.ClientSecret = "secret";
                    options.ResponseType = "id_token code";
                    options.Scope.Add("yepeng");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("email");
                    options.GetClaimsFromUserInfoEndpoint = true;

                    //将Authorization Server的Reponse中返回的token持久化在cookie中
                    options.SaveTokens = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
