using Core.Infrastructure.Common;
using Furion.RemoteRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Http
{
    /// <summary>
    /// 远程请求 范例
    /// </summary>
  //[Headers("X-Authorization", "Bearer your_refresh_token")]//增加表头，可以增加多个
  //[Headers("token", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJVc2VySWQiOiI5ODc5NjI1Ny05YzA5LTQ1NzYtOTgyMS01NWMyNTY4NmU4NDgiLCJVc2VyTmFtZSI6IjkiLCJDcmVhdGVUaW1lIjoiMjAyMS84LzEzIDEzOjE3OjUwIn0.1NMOFWEtJwvUyKNd8sCrpkaLWXyOp0XWo4NS808vfos")]
    public  interface IConfig : IHttpDispatchProxy
    {
        /// <summary>
        /// 调用获取客户数
        /// </summary>
        /// <returns></returns>
        [Get("api/v1/Customer/CustomerTree"),Client("config")]
        Task<ResultSet<List<CustomerTree>>> GetCustomerTreeAsync([Headers] string token=default);

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
    
    #endregion

    #region 返回实体
    public class CustomerTree
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public List<CustomerTree>  Children { get; set; }
    }


    #endregion
}
