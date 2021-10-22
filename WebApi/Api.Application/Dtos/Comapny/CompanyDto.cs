using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Dtos
{
    /// <summary>
    /// 企业dto
    /// </summary>
   public class CompanyDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 时区
        /// </summary>
        public string TimezoneId { get; set; }
        /// <summary>
        /// 管理员名称
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string Manager { get; set; }
    }
}
