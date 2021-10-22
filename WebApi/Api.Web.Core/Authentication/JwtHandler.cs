using Core.Infrastructure.CacheService;
using Furion;
using Furion.Authorization;
using Furion.DataEncryption;
using Furion.UnifyResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Api.Web.Core.Authentication
{
    public class JwtHandler : AppAuthorizeHandler
    {

        public readonly ICacheService _cache;
        public JwtHandler(ICacheService cache)
        {
            _cache = cache;
        }
        /// <summary>
        /// 验证管道
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
        {
            // 此处已经自动验证 Jwt token的有效性了，无需手动验证

            // 这里写您的授权判断逻辑，授权通过返回 true，否则返回 false
            // 检查权限，如果方法是异步的就不用 Task.FromResult 包裹，直接使用 async/await 即可
            return Task.FromResult(CheckAuthorzie(httpContext));
            //return Task.FromResult(true);
        }


        /// <summary>
        /// 校验token存在性（在redis中）
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private bool CheckAuthorzie(DefaultHttpContext httpContext)
        {
            // 获取传入的token
            var bearerToken = httpContext.Request.Headers["Authorization"].ToString();
            var token = bearerToken.Substring("Bearer".Length).Trim();

            //判断在redis中是否存在
            var isExist= _cache.CheckToken(token);
           
            return isExist;//"查询数据库返回是否有权限"
        }

        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {

            // 判断是否授权
            var isAuthenticated = context.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {

                // 获取所有未成功验证的需求
                var pendingRequirements = context.PendingRequirements;

                // 获取 HttpContext 上下文
                var httpContext = context.GetCurrentHttpContext();

                // 调用子类管道
                var pipeline = await PipelineAsync(context, httpContext);
                if (pipeline)
                {
                    // 通过授权验证
                    foreach (var requirement in pendingRequirements)
                    {
                        // 验证策略管道
                        var policyPipeline = await PolicyPipelineAsync(context, httpContext, requirement);
                        if (policyPipeline) context.Succeed(requirement);
                    }
                }
                else
                {
                    //context.Fail();

                    context.GetCurrentHttpContext().Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.GetCurrentHttpContext().Response.ContentType = "application/json";
                    await context.GetCurrentHttpContext().Response.CompleteAsync();
                }
            }
            else context.GetCurrentHttpContext()?.SignoutToSwagger();    // 退出Swagger登录

            
        }
    }
}