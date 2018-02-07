# Identity-Server-4-Demo
使用Identity Server 4的授权服务和权限验证的WebApi、MVC Demo。
程序包管理控制台：Install-package 使用Identity Server 4的授权服务和权限验证的WebApi、MVC Demo。
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();//加上这段代码，将IdentityServer注册到管道上
        }
