using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YizitApi.Application.Enum
{
    /// <summary>
    /// 通知类型
    /// </summary>
  public  enum EnumNotificationType
    {
        /// <summary>
        /// 其他
        /// </summary>
        Other=0,
        /// <summary>
        /// 版本更新
        /// </summary>
        Release=1,
        /// <summary>
        /// 维护通知
        /// </summary>
        Maintenance=2
    }

    /// <summary>
    /// 通知发布方式
    /// </summary>
    public enum EnumNotificationPublishType
    {
        /// <summary>
        /// 立即发送
        /// </summary>
        Immediate=0,
        /// <summary>
        /// 定时发送
        /// </summary>
        Timed=1
    }

    /// <summary>
    /// 通知范围
    /// </summary>
    public enum EnumNotificationRange
    {
        /// <summary>
        /// 全部
        /// </summary>
        All=0,
        /// <summary>
        /// 指定公司
        /// </summary>
        Company=1,
        /// <summary>
        /// 指定角色
        /// </summary>
        Role=2,
        /// <summary>
        /// 指定用户
        /// </summary>
        User=3
    }

    /// <summary>
    /// 发布更新类型
    /// </summary>
    public enum EnumReleaseType
    {
        Fix=0,
        Feature=1,
        Improvement=2,
        Design=3,
        Doc=4

    }
}
