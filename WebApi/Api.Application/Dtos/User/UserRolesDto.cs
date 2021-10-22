using Furion.DataEncryption;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Dtos.User
{
    /// <summary>
    /// 批量设置用户角色dto
    /// </summary>
    public class UserRolesDto
    {
        /// <summary>
        /// 要设置的用户id列表
        /// </summary>
        public List<string> UserIds { get; set; }
        /// <summary>
        /// 赋予授权的角色id列表
        /// </summary>
        public List<string> RoleIds { get; set; }
    }
}
