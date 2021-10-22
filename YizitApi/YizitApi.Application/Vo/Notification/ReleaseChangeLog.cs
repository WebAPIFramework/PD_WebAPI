using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YizitApi.Application.Enum;

namespace YizitApi.Application.Vo.Notification
{
    /// <summary>
    /// 版本更新记录
    /// </summary>
  public  class ReleaseChangeLog
    {
        /// <summary>
        /// 版本更新id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionNo { get; set; }
        /// <summary>
        /// 版本名称
        /// </summary>
        public string VersionName { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }
        public List<ReleaseChangeLogDetail> Detail { get; set; }

    }
    /// <summary>
    /// 版本更新记录详情
    /// </summary>
    public class ReleaseChangeLogDetail
    {
        /// <summary>
        /// 更新类型 （0： fix; 1: feature；2: improvement; 3: design ; 4:doc）
        /// </summary>
        public EnumReleaseType Type { get; set; }
        /// <summary>
        /// 更新内容
        /// </summary>
        public string Content { get; set; }
    }
}
