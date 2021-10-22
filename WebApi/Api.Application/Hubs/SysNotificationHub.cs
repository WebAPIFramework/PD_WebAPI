using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Application.Dtos.Notificaiton;

namespace Api.Application.Hubs
{
    public class SysNotificationHub : ISysNotificationHub
    {
        private readonly IHubContext<SysNTHub> _serviceContext;
        public SysNotificationHub(IHubContext<SysNTHub> serviceContext)
        {
            _serviceContext = serviceContext;
        }
        public void HandleNotificationChange(NotificationModel model)
        {
            // 触发客户端定义监听的方法
             _serviceContext.Clients.All.SendAsync("ReceiveNotification");
        }
    }
}
