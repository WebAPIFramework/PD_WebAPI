using Core.Infrastructure.Utils;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DynamicApiController;
using Furion.EventBus;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YizitApi.Application.BuinessLayer;
using YizitApi.Application.Dtos;
using YizitApi.Core;

namespace YizitApi.Web.Entry.Controller
{
    /// <summary>
    /// 模拟LDAP域验证
    /// </summary>
    [ApiDescriptionSettings( "Shipment")]
    [AllowAnonymous]
    public class LDAPDemoService : IDynamicApiController
    {
    
       
        public void GetLDAPLogin(string userName, string password)
        {
            var loginFlag = LDAPUtil.Validate(userName, password);
            if (loginFlag)
            {
                throw Oops.Oh("User validate successfully!");
            }
            else
            {
                throw Oops.Oh("User validate unsuccessfully!");
            }
        }

       public void PostSyncUser()
        {
            //ADHelper adhelper = new ADHelper();
            //adhelper.Sync();

            LDAPUtil.Sync();
        }
    }
}
