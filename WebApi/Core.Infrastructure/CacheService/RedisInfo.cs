using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.CacheService
{
   public class RedisInfo
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }

    public class RedisInfo<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
    }
}
