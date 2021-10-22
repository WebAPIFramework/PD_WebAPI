using Core.Infrastructure.Enum;
using Furion.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YizitApi.Application.Enum;

namespace YizitApi.Application.Vo
{
    /// <summary>
    /// 查询用户返回实体
    /// </summary>
  public  class QueryUserResponse
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string JobNo { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>

        public string Email { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public List<RoleResponse> Roles { get; set; }
        /// <summary>
        /// 0-禁用，1-启用
        /// </summary>
        public int Available { get; set; }
        /// <summary>
        /// 用户类别
        /// </summary>
        public EnumUserType UserType { get; set; }
        /// <summary>
        /// 账户类型
        /// -1 因致； 0 本地；1 LDAP
        /// </summary>
        public EnumAccountType AccountType { get; set; }
        /// <summary>
        /// 账户类型名称
        /// </summary>
        public string AccountTypeName { get { return L.Text[AccountType.ToString()]; } }
    }


    /// <summary>
    /// 角色
    /// </summary>
   public class RoleResponse
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
    }

}
