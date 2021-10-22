using Core.Infrastructure.AOP;
using Core.Infrastructure.Authentication;
using Core.Infrastructure.CacheService;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Options;
using Furion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure
{
    [AppStartup(800)]
    public class CoreStartup : AppStartup
    {
        public void YizitCoreService(IServiceCollection services)
        {
            //注册可写的选项（允许用户修改密码）
            services.ConfigureWritable<PasswordInfoOptions>(App.Configuration.GetSection("PasswordInfo"));
            services.ConfigureWritable<List<LDAPServerSettingsOptions>>(App.Configuration.GetSection("LDAPServerSettings"));

            //注册缓存
            var redisConfig = App.Configuration.GetSection("Redis").Get<RedisConfig>();
            services.AddCache(redisConfig);

           

            //跨域
            services.AddCorsAccessor();

            services.AddControllers().AddAppLocalization();//国际化

            services
            //注入furion（基础配置和规范化结果）
            .AddInjectWithUnifyResult()
            //注册选项服务
            .AddConfigurableOptions<PasswordInfoOptions>()
            //注册请求客户端
            //注册远程请求服务
            .AddRemoteRequest(options =>
            {
                    //配置SSO请求客户端
                    options.AddHttpClient("sso", c =>
                {
                    c.BaseAddress = new Uri(App.Configuration["RemoteApi:sso"]);
                    c.DefaultRequestHeaders.Add("Accept", "text/plain");
                        //c.DefaultRequestHeaders.Add("Content-Type", "application/json-patch+json");
                    });
                    //配置config请求客户端
                    options.AddHttpClient("config", c =>
                {
                    c.BaseAddress = new Uri(App.Configuration["RemoteApi:config"]);
                    c.DefaultRequestHeaders.Add("Accept", "text/plain");
                        //c.DefaultRequestHeaders.Add("Content-Type", "application/json-patch+json");
                    });
            })
            //注册轻量级事件总线服务
            .AddSimpleEventBus()
            //注册Action过滤器（国际化处理）
            .AddMvcFilter<LangFilter>()
            .AddSignalR();


        }

        public void CoreConfigure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //注入furion
            app.UseInject(string.Empty);

            //国际化
            app.UseAppLocalization();

            app.UseHttpsRedirection();

            app.UseRouting();

            //跨域
            app.UseCorsAccessor();

            //授权中间件
            app.UseAuthentication();
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                // 注册集线器
                endpoints.MapHubs();

                endpoints.MapControllers();
            });
        }
    }
}
