using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Application.Enum;
using Api.Core;

namespace Api.Application.Vo.Notification
{
  public  class NotificationBaseResponse
    {
        public string Id { get; set; }
        /// <summary>
        /// 消息通知时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public EnumNotificationType Type { get; set; }
        /// <summary>
        /// 是否已发送
        /// </summary>
        public bool Sent { get; set; }
    }

    public class NtfReleaseResponse: NotificationBaseResponse
    {
        public DateTime ReleaseTime { get; set; }
        public string Version { get; set; }
        public string VersionName { get; set; }
        public List<NT_Release_Detail> Details { get; set; }
        public string Content { get; set; }
    }

    public class NtfMaintenanceResponse : NotificationBaseResponse
    {
        public DateTime MaintenanceTime { get; set; }
        public string Content { get; set; }
    }

    public class NtfOtherResponse : NotificationBaseResponse
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
