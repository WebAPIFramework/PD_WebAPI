using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YizitApi.Application.Dtos.Notificaiton;

namespace YizitApi.Application.Hubs
{
  public  interface ISysNotificationHub
    {
        /// <summary>
        /// 更新通知
        /// </summary>
        void HandleNotificationChange(NotificationModel model);
    }
}
