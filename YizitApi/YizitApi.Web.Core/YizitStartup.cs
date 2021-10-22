using Core.Infrastructure.AOP;
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
using YizitApi.Application.Hubs;
using YizitApi.Web.Core.Authentication;
using YizitApi.Web.Core.Filters;

namespace Core.Infrastructure
{
   public class YizitStartup : AppStartup
    {
        public void YizitService(IServiceCollection services)
        {
            ////jwt身份验证
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);//全局授权

            //注册Action过滤器（国际化处理）
            services.AddMvcFilter<TenantFilter>();
            // 注册系统消息通知实时通信服务
            services.AddSingleton<ISysNotificationHub, SysNotificationHub>();
        }

      
    }
}
