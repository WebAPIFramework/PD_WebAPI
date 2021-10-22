using Core.Infrastructure.Enum;
using Furion.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YizitApi.Application.Dtos
{
    /// <summary>
    /// 登录dto
    /// </summary>
   public class LoginDto
    {
        /// <summary>
        /// 登录名
        /// </summary>

        [Required(ErrorMessage = "Required")]
        public string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 选择的公司
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 是否自动登陆（自动登陆则生成7天有效期的token，否则生成1天有效期的token）
        /// </summary>
        public bool AutoLogin { get; set; }
        /// <summary>
        /// 登录端(默认web端)
        /// 0:web端；1：手机端；2：pda端；3：客户端
        /// </summary>
        public EnumLoginPlatform Platform { get; set; } = EnumLoginPlatform.Web;

        /// <summary>
        /// 登录类型（-1 因致账户；0 本地用户； 1 LDAP用户；2 微信用户；3 钉钉用户）
        /// </summary>
        public EnumAccountType LoginType { get; set; } = EnumAccountType.Local;
    }
}
