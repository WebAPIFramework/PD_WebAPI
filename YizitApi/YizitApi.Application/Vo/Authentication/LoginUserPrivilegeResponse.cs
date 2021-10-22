using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YizitApi.Application.Vo
{
    /// <summary>
    /// 根据token返回用户的权限列表
    /// </summary>
    public class LoginUserPrivilegeResponse
    {
        public List<string> PrivilegeCodes { get; set; }

    }
    

}
