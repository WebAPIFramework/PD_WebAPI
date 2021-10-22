using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Enum
{
    /// <summary>
    /// 登录账户类型
    /// </summary>
   public enum EnumAccountType
    {
        /// <summary>
        /// 因致
        /// </summary>
        Yizit=-1,
        /// <summary>
        /// 本地
        /// </summary>
        Local=0,
        /// <summary>
        /// LDAP用户
        /// </summary>
        LDAP=1,
        /// <summary>
        /// 微信用户
        /// </summary>
        WeChat=2,
        /// <summary>
        /// 钉钉用户
        /// </summary>
        DingDing=3
    }
}
