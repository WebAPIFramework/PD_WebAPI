using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YizitApi.Application.Enum;

namespace YizitApi.Application.Vo.Notification
{
    /// <summary>
    /// 客户端通知消息卡片实体
    /// 
    /// </summary>
  public  class NotificationCard
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息发送时间
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public EnumNotificationType Type { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 消息创建人/通知人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 创建人/通知人 图标地址
        /// </summary>
        public string CreaterAvatar { get; set; }
    }
}
