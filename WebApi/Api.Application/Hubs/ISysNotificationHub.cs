using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Application.Dtos.Notificaiton;

namespace Api.Application.Hubs
{
  public  interface ISysNotificationHub
    {
        /// <summary>
        /// 更新通知
        /// </summary>
        void HandleNotificationChange(NotificationModel model);
    }
}
