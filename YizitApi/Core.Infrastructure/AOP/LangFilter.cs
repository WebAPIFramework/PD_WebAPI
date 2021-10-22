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

namespace Core.Infrastructure.AOP
{
    /// <summary>
    /// Action过滤器
    /// </summary>
    public class LangFilter : IActionFilter
    {
        // 日志对象
        private readonly ILogger<LangFilter> _logger;
        public LangFilter(ILogger<LangFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 全局拦截器
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            #region 国际化参数
            _logger.LogInformation($"拦截到当前请求国际化语言是{context.HttpContext.Request.Headers["locale"]}");
            //获取传入的语言
            var currentCulture = context.HttpContext.Request.Headers["locale"];
            if (string.IsNullOrEmpty(currentCulture)) L.SetCulture("zh-CN");
            else L.SetCulture(currentCulture);
            #endregion


        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //do nothtin
        }
    }
}
