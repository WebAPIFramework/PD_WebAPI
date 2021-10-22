using Core.Infrastructure.CacheService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Extensions
{
  public static  class CacheServiceCollectionExtensions
    {
        /// <summary>
        /// 缓存注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="redisConfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddCache(this IServiceCollection services, RedisConfig redisConfig)
        {
            if(redisConfig.Enabled)//如果启用redis
            {
                //Use Redis
                services.AddSingleton(new RedisHelper(redisConfig.Default.Connection, redisConfig.Default.InstanceName, redisConfig.Default.DefaultDB));
            
                // 注册非泛型缓存
                services.TryAddScoped<ICacheService, RedisCacheService>();

                // 注册多数据库缓存
                services.TryAddScoped(typeof(ICacheService<,>), typeof(RedisCacheService<,>));

                // 注册泛型缓存
                services.TryAddScoped(typeof(ICacheService<>), typeof(RedisCacheService<>));
            }
            else
            {

                //Use MemoryCache

                //注册非泛型缓存
                services.TryAddScoped<ICacheService, MemoryCacheService>();

                // 注册多数据库缓存
                services.TryAddScoped(typeof(ICacheService<,>), typeof(MemoryCacheService<,>));

                // 注册泛型缓存
                services.TryAddScoped(typeof(ICacheService<>), typeof(MemoryCacheService<>));
            }
            return services;
        }
    }
}
