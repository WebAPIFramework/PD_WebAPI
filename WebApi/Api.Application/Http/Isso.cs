using Core.Infrastructure.Common;
using Furion.ClayObject;
using Furion.DataValidation;
using Furion.RemoteRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Http
{
    /// <summary>
    /// 远程请求 范例
    /// </summary>
  [Headers("X-Authorization", "Bearer your_refresh_token")]//增加表头，可以增加多个
  public  interface Isso: IHttpDispatchProxy
    {
        /// <summary>
        /// 调用登录接口
        /// Post
        /// 支持
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Post("Sso/Passport/login"),Client("sso"), Headers("Authorization", "Bearer your_token")]
        Task<ResultSet<UserResponse>> PostLoginAsync([Body] User user);

        /// <summary>
        /// 远程调用登录接口
        /// 返回用dynamic，无需再创建返回实体
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Post("Sso/Passport/login"), Client("sso"), Headers("Authorization", "Bearer your_token")]
        Task<ResultSet<dynamic>> PostLoginAsDynamicAsync([Body] User user);
        #region 拦截器

        [Interceptor(InterceptorTypes.Request)]

        //全局请求拦截
        static void onRequestInterceptor(HttpRequestMessage req)
        {
            req.AppendQueries(new Dictionary<string, object>
            {
                {"xxx", "yyy"},
                {"token","xxxxx" }
            });

        }

        // 全局成功拦截，类中每一个方法都会触发
        [Interceptor(InterceptorTypes.Response)]
        static void OnResponsing1(HttpResponseMessage req)
        {

        }

        // 全局请求异常拦截，类中每一个方法都会触发
        [Interceptor(InterceptorTypes.Exception)]
        static void OnException1(HttpResponseMessage req, string errors)
        {

        }
        #endregion

    }

    #region 请求参数
    /// <summary>
    /// 登录用户dto
    /// </summary>
    public class User
    {
        [Required(ErrorMessage="UserName must required")]
        public string UserName { get; set; }
        public string Password { get; set; }
        [DataValidation(ValidationTypes.PositiveNumber)]
        public int Type { get; set; }
    }
    #endregion

    #region 返回实体
    public class UserResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        //public object Permission { get; set; }
        public string Token { get; set; }
    }

    #endregion
}
