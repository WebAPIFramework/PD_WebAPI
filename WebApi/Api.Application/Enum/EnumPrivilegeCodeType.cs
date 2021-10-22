using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Enum
{
    /// <summary>
    /// 权限类型
    /// code所属，0：menus；1：entities
    /// </summary>
    public enum EnumPrivilegeCodeType
    {
        /// <summary>
        /// 所属菜单
        /// </summary>
        Menus = 0,
        /// <summary>
        /// 所属功能点
        /// </summary>
        Entities = 1,
       
    }
}
