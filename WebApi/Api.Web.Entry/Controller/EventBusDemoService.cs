using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DynamicApiController;
using Furion.EventBus;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Application.BuinessLayer;
using Api.Application.Dtos;
using Api.Core;

namespace Api.Web.Entry.Controller
{
    /// <summary>
    /// 模拟事件总线
    /// </summary>
    [ApiDescriptionSettings( "Shipment")]
    [AllowAnonymous]
    public class EventBusDemoService : IDynamicApiController
    {
    
        /// <summary>
        /// 发布方发送消息
        /// </summary>
        /// <returns></returns>
        public void PostCreateUser()
        {
            MessageCenter.Send("create:user", new  { }, isSync: true);
        }

        public void DeleteUser()
        {
            MessageCenter.Send("delete:user", new { }, isSync: true);
        }
    }
}
