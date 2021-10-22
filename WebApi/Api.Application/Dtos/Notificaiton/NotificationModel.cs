using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Dtos.Notificaiton
{
    /// <summary>
    /// 与客户端交互的通知模型
    /// </summary>
   public class NotificationModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Type { get; set; }
        public NotificationtContent Data { get; set; }
    }

    /// <summary>
    /// 暂不定，可以考虑notitionbase 或者notificationcard
    /// </summary>
    public class NotificationtContent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
    }
}
