using Core.Infrastructure.Enum;
using Core.Infrastructure.Options;
using Core.Infrastructure.Utils;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.EventBus;
using Furion.FriendlyException;
using Furion.JsonSerialization;
using Newtonsoft.Json;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Application.BuinessLayer;
using Api.Core;

namespace Api.Application.SubscribeHandler
{
    /// <summary>
    /// LDAP用户同步订阅处理程序类
    /// 在 Furion 框架中，事件总线是不支持构造函数注入的，而且构造函数也只会执行一次。所以需要用到服务，应该通过静态类解析，App.GetService<xx>() 或 Db.GetRepository<XX>()
    /// </summary>
    public class LDAPUserSyncSubscribeHandler : ISubscribeHandler
    {
        /// <summary>
        /// 同步用户
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="payload"></param>
        [SubscribeMessage("sync:user")]
        public void SyncUser(string eventId, object payload)
        {
            App.PrintToMiniProfiler("LDAP Sync User SubscribeHandler", "同步用户", $"用户：xxx");
           
            Scoped.CreateUow((factory, scope) => {
                var services = scope.ServiceProvider;

                #region 获取当前的环境（公司id，域配置信息）
                //获取当前公司CompanyId的LDAP Server Setting
                var companyId = App.User.FindFirst("CompanyId")?.Value;
                List<LDAPServerSettingsOptions> ldapServerSettings = App.GetOptionsMonitor<List<LDAPServerSettingsOptions>>();
               
                if (string.IsNullOrEmpty(companyId)) throw Oops.Oh("oh.. Your have to belong to an company to sync ldap user");
                LDAPServerSettingsOptions ldapSetting = ldapServerSettings.FirstOrDefault(x => x.CompanyId == companyId);
                if (ldapSetting == null) throw Oops.Oh("oh.. Your company not configue LDAP Server Setting");

                #endregion

                #region 获取LDAP用户信息
                var entity = (LdapEntry)payload;

                var mail = LDAPUtil.GetAttributeValue("mail", entity);
                var sAMAccountName = LDAPUtil.GetAttributeValue(ldapSetting.LoginNameAttr, entity);
                var cn = LDAPUtil.GetAttributeValue("cn", entity);
                var sn = LDAPUtil.GetAttributeValue("sn", entity);
                var chinaName = LDAPUtil.GetAttributeValue("gecos", entity);
                var userId = LDAPUtil.GetAttributeValue(ldapSetting.UserIdAttr, entity);
                var lastChange = LDAPUtil.GetAttributeValue("shadowLastChange", entity);
                var expire = LDAPUtil.GetAttributeValue("shadowExpire", entity);
                var inactive = LDAPUtil.GetAttributeValue("shadowInactive", entity);
                var uid = LDAPUtil.GetAttributeValue("apple-generateduid", entity);
                var displayName = LDAPUtil.GetAttributeValue("displayName", entity);
                #endregion

                #region 同步
                var userService = App.GetService<IUserService>(services);// 直接用 services 解析
                var accountRepo = App.GetService<IRepository<Account, TubroDbContextLocator>>(services);// 直接用 services 解析
                var userRepo = App.GetService<IRepository<User, TubroDbContextLocator>>(services);// 直接用 services 解析

                //var account = accountRepo.FirstOrDefault(x => x.UserId == userId && x.Deleted==0);
                var user = userRepo.FirstOrDefault(x => x.Id == userId);
               
                var available = expire == "1" || inactive == "1" ? 0 : 1;
                //已存在
                if (user != null)
                {
                   
                    userService.Update(new Dtos.User.UserDto { Id=userId, Name= chinaName ?? displayName, Username = sAMAccountName ?? cn, Email=mail, Available= available, AccountType = (int)EnumAccountType.LDAP });
                }
                else
                {
                    if(available==1)//第一次同步只有激活的用户才需要同步
                    userService.Insert(new Dtos.User.UserDto { Id = userId, Name = chinaName ?? displayName, Username = sAMAccountName ?? cn, Email = mail, Available = available, AccountType= (int)EnumAccountType.LDAP });
                }

                #endregion





            });
        }

       

     
    }
}
