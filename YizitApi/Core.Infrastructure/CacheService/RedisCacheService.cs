using Core.Infrastructure.Enum;
using Furion;
using Furion.DatabaseAccessor;
using Furion.FriendlyException;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.CacheService
{
    public class RedisCacheService : ICacheService
    {
        protected IDatabase _cache;
        protected  IMemoryCache _memoryCache;
        private readonly string instanceName = App.Configuration.GetSection("Redis").Get<RedisConfig>().Default.InstanceName;
        public RedisCacheService(RedisHelper client, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _cache= client.GetDatabase();            
        }

        #region token相关
        /// <summary>
        /// 校验token是否存在
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        public bool CheckToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;
            if (!_cache.KeyExists(token)) return false;

            return true;
        }
        /// <summary>
        /// 删除token
        /// </summary>
        /// <param name="token"></param>
        public void DeleteToken(string token)
        {
            _cache.KeyDelete(token);
        }
        /// <summary>
        /// 保存token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="user"></param>
        /// <param name="platform"></param>
        /// <param name="isAutoLogin"></param>
        /// <returns></returns>
        public RedisInfo<UserRedisResult> SaveToken(string token, string tokenKey, EnumLoginPlatform platform, bool isAutoLogin)
        {
            if (string.IsNullOrEmpty(token))
                return null;
            var data = new RedisInfo<UserRedisResult>
            {
                Key = token,
                Value = new UserRedisResult { CreateTime = DateTime.Now.ToString() }
            };

            #region 内存缓存中处理同端同用户的上一次登录token
            
            //获取同端同用户的上一次登录token
            if (_memoryCache.TryGetValue<string>(tokenKey+ platform, out var preToken))
            {
                DeleteToken(preToken);//从redis删除该token[实现同端同用户互斥登录]
                _memoryCache.Remove(tokenKey);
            }
            #endregion


            var json = JsonConvert.SerializeObject(data);

            if (isAutoLogin)
            {
                var expireMinutes = 7 * 24 * 60;
                _cache?.StringSet(token, json, TimeSpan.FromMinutes(expireMinutes));
                _memoryCache.GetOrCreate<string>(tokenKey + platform, entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(expireMinutes));
                    return token;
                });
            }

            else
            {
                var expireMinutes = 1 * 24 * 60;
                _cache?.StringSet(token, json, TimeSpan.FromMinutes(expireMinutes));
                _memoryCache.GetOrCreate<string>(tokenKey + platform, entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(expireMinutes));
                    return token;
                });

            }


            return data;
        }
        #endregion

        #region 添加缓存

        public bool Add(string key, object value)
        {
            if (string.IsNullOrEmpty(key)) return false;
            var json = JsonConvert.SerializeObject(value);
            _cache.StringSet(key, json);
            
            return true;
        }

        public async Task<bool> AddAsync(string key, object value)
        {
            if (string.IsNullOrEmpty(key)) return false;

            var json = JsonConvert.SerializeObject(value);
            return await _cache.StringSetAsync(key, json);
           
        }

        public void Push<T>(string key, T value)
        {
            var json= JsonConvert.SerializeObject(value);
            _cache.SortedSetAdd(key,json, json.GetHashCode());
            //_cache.ListLeftPush(key, json);
        }

        #endregion

        #region 验证缓存是否存在

        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key)) return false;
            return _cache.KeyExists(key);    
            
        }

        public async Task<bool> ExistsAsync(string key)
        {
          if (string.IsNullOrEmpty(key)) return false;
          return await  _cache.KeyExistsAsync(key);
        }
        #endregion

        #region 获取缓存

        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }
            try
            {
                T result;
                string json = _cache.StringGet(key);
                if (string.IsNullOrEmpty(json))
                {
                    return default;
                }
                result = JsonConvert.DeserializeObject<T>(json);

                return result;
            }
            catch (Exception e)
            {
                var message = e.Message;
                return default;
            }
           
        }

        public List<string> GetSets(string key)
        {

            if (string.IsNullOrEmpty(key))
            {
                return new List<string>();
            }
            try
            {
                List<string> result = new List<string>();
                var values = _cache.SortedSetRangeByRank(key);
               
                if (values==null || values.Length==0)
                {
                    return new List<string>(); ;
                }
                foreach (var item in values)
                {
                    result.Add(item);
                }
                //var data = values.ToList();


                return result;
            }
            catch (Exception e)
            {
                var message = e.Message;
                return new List<string>(); ;
            }
        }
        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }
            try
            {
                return _cache.StringGet(key);
            }
            catch (Exception)
            {

                return default;
            }
         
           
        }

        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            keys = keys.Distinct();
            foreach (var item in keys)
                {
                    dic.Add(item, _cache.StringGet(item));
                }
            
            return dic;
        }

        public async  Task<IDictionary<string, object>> GetAllAsync(IEnumerable<string> keys)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            keys = keys.Distinct();
            foreach (var item in keys)
            {
                dic.Add(item, _cache.StringGet(item));
            }

            return  dic;
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            try
            {
                T result;
                string json = _cache.StringGet(key);
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }
                result = JsonConvert.DeserializeObject<T>(json);

                return result;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<object> GetAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            try
            {
                return _cache.StringGet(key);
            }
            catch (Exception)
            {

                return null;
            }
        }
        #endregion

        #region 删除缓存

        public bool Remove(string key)
        {
           return _cache.KeyDelete(key);
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                Remove(key);
            }

            
        }

        public async Task RemoveAllAsync(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                Remove(key);
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _cache.KeyDeleteAsync(key);
        }
        #endregion

        #region 更新缓存

        public bool Replace(string key, object value)
        {
            bool ReturnBool = false;
            if (!string.IsNullOrEmpty(key))
            {
                if(Exists(key))
                {
                    Remove(key);
                }

                ReturnBool = Add(key, value);

            }
            return ReturnBool;
        }

       


        #endregion


    }

    public class RedisCacheService<TEntity> : ICacheService<TEntity> where TEntity : class, IPrivateEntity, new()
    {
        private readonly IRepository<TEntity> _entity;
        protected IDatabase _cache;
        private readonly string instanceName = App.Configuration.GetSection("Redis").Get<RedisConfig>().Default.InstanceName;
        public RedisCacheService(RedisHelper client, IRepository<TEntity> entity)
        {
            _cache = client.GetDatabase();
            _entity = entity;
        }
       
        #region 添加缓存

        public TEntity Add(string key, TEntity value)
        {
            

            return _entity.InsertNow(value).Entity;
            
        }

        public Task<TEntity> AddAsync(string key, TEntity value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 验证缓存是否存在
        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string key)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region 获取缓存

        public TEntity Get(string key)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync(string key)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 删除缓存


        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllAsync(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 更新缓存

        public bool Replace(string key, TEntity value)
        {
            throw new NotImplementedException();
        }

        #endregion




    }

    public class RedisCacheService<TEntity, TDbContextLocator> : ICacheService<TEntity, TDbContextLocator>
      where TEntity : class, IPrivateEntity, new()
     where TDbContextLocator : class, IDbContextLocator
    {
        protected IDatabase _cache;
        private readonly IRepository<TEntity, TDbContextLocator> _entity;
        private readonly string instanceName = App.Configuration.GetSection("Redis").Get<RedisConfig>().Default.InstanceName;
        public RedisCacheService(RedisHelper client, IRepository<TEntity, TDbContextLocator> entity)
        {
            _cache = client.GetDatabase();
            _entity = entity;
        }


        #region 添加缓存

        public TEntity Add(string key, TEntity value)
        {
          var data=  _entity.InsertNow(value);
          var json = JsonConvert.SerializeObject(value);
          _cache.ListLeftPush(key, json);
           return data.Entity;
             
        }

        public Task<TEntity> AddAsync(string key, TEntity value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 验证缓存是否存在
        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string key)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region 获取缓存

        public TEntity Get(string key)
        {
            if (string.IsNullOrEmpty(key)) throw Oops.Oh("Key not allow null");
            var val = _cache.StringGet(key);
            if (!val.HasValue)//缓存没值时
            {
                if (typeof(TEntity).GetProperty("Id") != null)//含有主键Id
                {
                    var result = _entity.Find(key);
                    if (null == result) return null;
                   var json = JsonConvert.SerializeObject(result);
                    _cache?.StringSet(key, json);
                    return result;
                }
                else
                    return null;
            }
           
            return JsonConvert.DeserializeObject<TEntity>(val);
         
        }

        public Task<TEntity> GetAsync(string key)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 删除缓存


        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllAsync(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 更新缓存

        public bool Replace(string key, TEntity value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
