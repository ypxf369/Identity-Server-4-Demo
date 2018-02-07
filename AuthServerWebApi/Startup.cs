using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace AuthServerWebApi
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

            services.AddSwaggerGen(i =>
            {
                i.SwaggerDoc("v1", new Info {Title = "My API", Version = "v1"});
            });
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    //生产环境启用https
                    options.RequireHttpsMetadata = false;
                    options.Authority = "http://localhost:5000";//授权服务地址（Authorization Server地址）
                    options.ApiName = "yepeng";//保持和Authorization Server里配置ApiResource的nmae一样
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //添加验证中间件，在UserMvc之前
            //当在controller或者Action使用[Authorize]属性的时候, 这个中间件就会基于传递给api的Token来验证Authorization, 如果没有token或者token不正确, 这个中间件就会告诉我们这个请求是UnAuthorized(未授权的).
            app.UseAuthentication();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(i =>
            {
                i.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
