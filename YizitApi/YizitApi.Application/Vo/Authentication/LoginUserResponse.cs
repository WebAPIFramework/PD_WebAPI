using Core.Infrastructure.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YizitApi.Application.Vo
{
    /// <summary>
    /// 登录返回信息
    /// </summary>
  public  class LoginUserResponse
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 企业id（多租户标记）
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 权限Code列表
        /// </summary>
        public PrivilegeCode PrivilegeCodes { get; set; }
        /// <summary>
        /// 账户类型
        /// -1 因致； 0 本地；1 LDAP
        /// </summary>
        public EnumAccountType AccountType { get; set; }
    }

    /// <summary>
    /// 权限实体（供后续拓展）
    /// </summary>
    public class PrivilegeCode
    {
        /// <summary>
        /// 模块权限
        /// </summary>
        public List<string> MenuIds { get; set; }
        /// <summary>
        /// 功能权限
        /// </summary>
        public List<string> EntityIds { get; set; }
    }
    

}
