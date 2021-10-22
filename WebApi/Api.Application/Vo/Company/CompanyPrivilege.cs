using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Application.Enum;

namespace Api.Application.Vo
{
    /// <summary>
    /// 企业权限
    /// </summary>
   public class CompanyPrivilege
    {
        /// <summary>
        /// 企业id
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string Id { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 企业模块权限
        /// </summary>
        public List<string> MenuIds { get; set; } = new List<string>();
        /// <summary>
        /// 企业功能权限
        /// </summary>
        public List<string> EntityIds { get; set; } = new List<string>();
    }

}
