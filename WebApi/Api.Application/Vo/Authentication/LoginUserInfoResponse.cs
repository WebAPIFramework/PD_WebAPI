using Core.Infrastructure.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Vo
{
    /// <summary>
    /// 根据token返回用户信息实体
    /// </summary>
    public class LoginUserInfoResponse
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 用户类型
        /// 0 普通用户；1：企业管理员；2：超级管理员
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 企业id（多租户标记）
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 账户类型
        /// -1 因致； 0 本地；1 LDAP
        /// </summary>
        public EnumAccountType AccountType { get; set; }

    }
    

}
