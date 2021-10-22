using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion;
using Furion.ConfigurableOptions;
using Furion.DataEncryption;
using Microsoft.Extensions.Configuration;

namespace Core.Infrastructure.Options
{
    /// <summary>
    /// 密码选项实体
    /// 
    /// </summary>
   public class LDAPServerSettingsOptions : IConfigurableOptionsListener<LDAPServerSettingsOptions>
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// LDAP Server别名
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// LDAP Host
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 389;
        /// <summary>
        /// 账号（DN）
        /// </summary>
        public string UserDN { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 基础domain connection string
        /// </summary>
        public string BaseDN { get; set; }
        /// <summary>
        /// Attribute name of the user ID/GUID
        /// </summary>
        public string UserIdAttr { get; set; }
        /// <summary>
        /// Attribute name of login name
        /// </summary>
        public string LoginNameAttr { get; set; }

        /// <summary>
        /// 选项更改通知
        /// 在对应配置修改时会触发修改
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        public void OnListener(LDAPServerSettingsOptions options, IConfiguration configuration)
        {
            CompanyId = options.CompanyId;
            ServerName = options.ServerName;
            Host = options.Host;
            Port = options.Port;
            UserDN = options.UserDN;
            Password = options.Password;
            BaseDN= options.BaseDN; 
            UserIdAttr = options.UserIdAttr;    
            LoginNameAttr = options.LoginNameAttr;  
            //App.GetOptions<LDAPServerSettingsOptions>().SuperPassword= options.SuperPassword;

        }

        public void PostConfigure(LDAPServerSettingsOptions options, IConfiguration configuration)
        {
           // options.SuperPassword??= MD5Encryption.Encrypt("111111");
        }
    }
}
