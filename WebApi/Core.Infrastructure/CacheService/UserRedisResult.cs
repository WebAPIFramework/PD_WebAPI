using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.CacheService
{
   public class UserRedisResult
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string CompanyId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}
