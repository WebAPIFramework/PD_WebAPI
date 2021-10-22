using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DataEncryption;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YizitApi.Application.BuinessLayer;
using YizitApi.Application.Dtos;
using YizitApi.Application.Vo;
using YizitApi.Application.Vo.Company;
using YizitApi.Core;

namespace YizitApi.Application
{
    /// <summary>
    /// 认证模块
    /// </summary>
    [ApiDescriptionSettings("Turbo@2")]
    [AllowAnonymous]
    public  class AuthenticationController : IDynamicApiController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public LoginUserResponse PostLogin([FromBody] LoginDto dto)
        {
           

            LoginUserResponse res = _authenticationService.Login(dto);

            return res;
        }
        /// <summary>
        /// 根据用户名获取企业列表
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("api/[controller]/username/{username}/[action]")]
        public List<ApplicableCompanyInfo> GetCompanies(string username)
        {
           return _authenticationService.GetCompanyList(username);
        }

        /// <summary>
        /// 获取当前系统全部企业列表
        /// </summary>
        /// <returns></returns>
        [Route("api/[controller]/[action]")]
        public List<ApplicableCompanyInfo> GetCompanies()
        {
            return _authenticationService.GetCompanyList();
        }
    }
}
