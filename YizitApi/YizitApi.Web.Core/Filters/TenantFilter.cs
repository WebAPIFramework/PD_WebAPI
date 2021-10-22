using Core.Infrastructure.ErrorCodes;
using Furion.FriendlyException;
using Furion.Localization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YizitApi.Application.Dtos;

namespace YizitApi.Web.Core.Filters
{
    /// <summary>
    /// Action过滤器
    /// </summary>
    public class TenantFilter : IActionFilter
    {
        // 日志对象
        private readonly ILogger<TenantFilter> _logger;
        public TenantFilter(ILogger<TenantFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 全局拦截器
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            #region 租户ID处理
            //拦截获取companyID(租户标记)
            //_logger.LogInformation($"拦截到当前请求的企业id{context.HttpContext.Request.Headers["CompanyId"]}");
            string controller = context.RouteData.Values["Controller"].ToString();
            string action = context.RouteData.Values["Action"]?.ToString();
            string method = context.HttpContext.Request.Method;
            var stream = context.HttpContext.Request.Body;

            //账号登录时【//登录未有token时获取companyId（考虑多租户的机制，如果没有多租户机制，该拦截器可以忽略）】
            if (controller.ToLower() == "authentication" && action!=null && action.ToLower() == "login" && method.ToLower() == "post")//代表登录
            {
                string body = "";
                if (stream != null)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
                    {
                        body = reader.ReadToEnd();
                    }
                    stream.Seek(0, SeekOrigin.Begin);

                    var model = JsonConvert.DeserializeObject<LoginDto>(body);
                    
                    context.HttpContext.Request.Headers.Add("CompanyId", model?.CompanyId);
                }
                else
                {
                    throw Oops.Oh(ErrorCodes.z1001, L.Text["Param"]); //"login param not allow null"
                }

            }
            #endregion

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //do nothtin
        }
    }
}
