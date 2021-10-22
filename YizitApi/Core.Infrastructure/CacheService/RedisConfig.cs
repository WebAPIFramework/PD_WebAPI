using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.CacheService
{
    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        /// 是否启用Redis
        /// </summary>
        public bool Enabled { get; set; }
        public RedisSetting Default { get; set; }
    }
    /// <summary>
    /// Redis具体链接配置
    /// </summary>
    public class RedisSetting
    {
        public string Connection { get; set; }
        public string InstanceName { get; set; }
        public int DefaultDB { get; set; }
    }
}
