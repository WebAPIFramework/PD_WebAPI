using Core.Infrastructure.Common;
using Furion.DataEncryption;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YizitApi.Application.Enum;

namespace YizitApi.Application.Dtos.User
{
    /// <summary>
    /// 分页查询用户列表参数
    /// </summary>
    public class UserPaginationDto: BasePagination
    {
      
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
     
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name { get; set; }
        public List<string> Roles { get; set; }

        public EnumUserStatus? Available { get; set; }
       
    }

}
