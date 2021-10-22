using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion.InstantMessaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;

namespace YizitApi.Application.Hubs
{
    /// <summary>
    /// 系统消息集线器
    /// </summary>
    [MapHub("/hubs/notificationHub")]
    public class SysNTHub: Hub
    {

    }
}
