using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YizitApi.Application.Enum;

namespace YizitApi.Application.Vo
{
    /// <summary>
    /// 角色
    /// </summary>
   public class RoleWithPrivilege
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
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
