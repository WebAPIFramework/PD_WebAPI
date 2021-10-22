using Core.Infrastructure.Enum;
using Furion.DatabaseAccessor;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.CacheService
{
    public class MemoryCacheService : ICacheService
    {
        protected IMemoryCache _cache;
        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        #region token相关
        /// <summary>
        /// 校验token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool CheckToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;

            object cached;
            return _cache.TryGetValue(token, out cached);

        }
        /// <summary>
        /// 删除token
        /// </summary>
        /// <param name="token"></param>
        public void DeleteToken(string token)
        {
            _cache.Remove(token);
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
            if (_cache.TryGetValue<string>(tokenKey + platform, out var preToken))
            {
                DeleteToken(preToken);//从redis删除该token[实现同端同用户互斥登录]
                _cache.Remove(tokenKey);
            }
            #endregion


            var json = JsonConvert.SerializeObject(data);

            if (isAutoLogin)
            {
                var expireMinutes = 7 * 24 * 60;
                _cache?.Set(token, json, TimeSpan.FromMinutes(expireMinutes));
                _cache.GetOrCreate<string>(tokenKey + platform, entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(expireMinutes));
                    return token;
                });
            }

            else
            {
                var expireMinutes = 1 * 24 * 60;
                _cache?.Set(token, json, TimeSpan.FromMinutes(expireMinutes));
                _cache.GetOrCreate<string>(tokenKey + platform, entry =>
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
            _cache.Set(key, json);

            return true;
        }

        public async Task<bool> AddAsync(string key, object value)
        {
            if (string.IsNullOrEmpty(key)) return false;

            var json = JsonConvert.SerializeObject(value);
              _cache.Set(key, json);
            return true;

        }
        #endregion

        #region 验证缓存是否存在

        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key)) return false;
         
            object cached;
            return _cache.TryGetValue(key, out cached);

        }

        public async Task<bool> ExistsAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) return false;
            object cached;
            return _cache.TryGetValue(key, out cached);
        }
        #endregion

        #region 获取缓存

        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            try
            {
                T result;
                var json = _cache.Get<string>(key);
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

        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            try
            {
                return _cache.Get(key);
            }
            catch (Exception)
            {

                return null;
            }


        }

        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            keys = keys.Distinct();
            foreach (var item in keys)
            {
                dic.Add(item, _cache.Get(item));
            }

            return dic;
        }

        public async Task<IDictionary<string, object>> GetAllAsync(IEnumerable<string> keys)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            keys = keys.Distinct();
            foreach (var item in keys)
            {
                dic.Add(item, _cache.Get(item));
            }

            return dic;
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
                string json = _cache.Get<string>(key);
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
                return _cache.Get(key);
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
            _cache.Remove(key);
            return true;
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
              _cache.Remove(key);
            return true;
        }
        #endregion

        #region 更新缓存

        public bool Replace(string key, object value)
        {
            bool ReturnBool = false;
            if (!string.IsNullOrEmpty(key))
            {
                if (Exists(key))
                {
                    Remove(key);
                }

                ReturnBool = Add(key, value.ToString());

            }
            return ReturnBool;
        }

        public void Push<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public List<string> GetSets(string key)
        {
            throw new NotImplementedException();
        }


        #endregion


    }

    public class MemoryCacheService<TEntity> : ICacheService<TEntity> where TEntity : class, IPrivateEntity, new()
    {
        protected IMemoryCache _cache;
        private readonly IRepository<TEntity> _entity;
        public MemoryCacheService(IMemoryCache cache, IRepository<TEntity> entity)
        {
            _cache = cache;
            _entity = entity;
        }

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string key)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(string key)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        public TEntity Add(string key, TEntity value)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> AddAsync(string key, TEntity value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(string key)
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

        public bool Replace(string key, TEntity value)
        {
            throw new NotImplementedException();
        }
    }

    public class MemoryCacheService<TEntity, TDbContextLocator> : ICacheService<TEntity, TDbContextLocator>
        where TEntity : class, IPrivateEntity, new()
       where TDbContextLocator : class, IDbContextLocator
    {
        protected IMemoryCache _cache;
        private readonly IRepository<TEntity, TDbContextLocator> _entity;
        public MemoryCacheService(IMemoryCache cache, IRepository<TEntity, TDbContextLocator> entity)
        {
            _cache = cache;
            _entity = entity;
        }

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string key)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(string key)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        public TEntity Add(string key, TEntity value)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> AddAsync(string key, TEntity value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(string key)
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

        public bool Replace(string key, TEntity value)
        {
            throw new NotImplementedException();
        }
    }
}
