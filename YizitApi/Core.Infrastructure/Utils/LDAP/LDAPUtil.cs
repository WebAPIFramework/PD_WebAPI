using Core.Infrastructure.Common;
using Core.Infrastructure.Options;
using Furion;
using Furion.DependencyInjection;
using Furion.EventBus;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authentication;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Utils
{
    /// <summary>
    ///  LDAP工具类
    /// </summary>
    public static class LDAPUtil
    {
        #region 原静态赋值LDAP 设置
        //public static string Domain = App.GetOptionsMonitor<List<LDAPServerSettingsOptions>>()[0].UserDN; //"uid=root,cn=users,dc=yizit,dc=cn";//域名称
        //public static string Host = App.GetOptionsMonitor<List<LDAPServerSettingsOptions>>()[0].Host;//"192.168.1.1";//域服务器地址
        //public static string BaseDC = App.GetOptionsMonitor<List<LDAPServerSettingsOptions>>()[0].BaseDN;//"cn=users,dc=yizit,dc=cn";//根据上面的域服务器地址，每个点拆分为一个DC，例如上面的apac.contoso.com，拆分后就是DC=apac,DC=contoso,DC=com
        //public static int Port = App.GetOptionsMonitor<List<LDAPServerSettingsOptions>>()[0].Port;//389;//域服务器端口，一般默认就是389
        //public static string DomainAdminUser = "root";//域管理员账号用户名，如果只是验证登录用户，不对域做修改，可以就是登录用户名
        //public static string DomainAdminPassword = App.GetOptionsMonitor<List<LDAPServerSettingsOptions>>()[0].Password;//"Luming@yizit.cn";//域管理员账号密码，如果只是验证登录用户，不对域做修改，可以就是登录用户的密码

        #endregion

        //获取LDAP Server配置列表
        public static List<LDAPServerSettingsOptions> ldapServerSettings = App.GetOptionsMonitor<List<LDAPServerSettingsOptions>>();

        /// <summary>
        /// 验证域用户的账号和密码
        /// </summary>
        /// <param name="username">域用户的账号</param>
        /// <param name="password">域用户的密码</param>
        /// <returns>true验证成功，false验证失败</returns>
        public static bool Validate(string username, string password)
        {
            try
            {
                using (var conn = new LdapConnection())
                {
                    #region 获取当前请求上下文的公司所属的LDAP Server配置
                    //获取当前公司CompanyId的LDAP Server Setting
                    var companyId = App.HttpContext.Request.Headers["CompanyId"].ToString();
                    //companyId = "59A7016A-CB64-42C9-94A2-174A8AA51A54";
                    LDAPServerSettingsOptions ldapSetting = ldapServerSettings.FirstOrDefault(x => x.CompanyId == companyId);
                    if (ldapSetting == null) throw Oops.Oh("oh.. Your company not configue LDAP Server Setting");

                    #endregion

                    #region 验证连接登录
                    conn.Connect(ldapSetting.Host, ldapSetting.Port);
                    conn.Bind(ldapSetting.UserDN, ldapSetting?.Password);//这里用户名或密码错误会抛出异常LdapException
                    #endregion

                    #region 获取指定BaseDN下的根据用户名（cn）的列表
                    //string sFilter = String.Format("(&(objectClass=user)(memberOf=*))");
                    var entities =
                        conn.Search(ldapSetting.BaseDN, LdapConnection.ScopeSub,
                          $"cn={username}",//注意一个多的空格都不能打，否则查不出来
                            new string[] { "cn", "mail", "sn", "gecos" }, false);
                    #endregion

                    #region 用户查询与校验
                    string userDn = null;
                    while (entities.HasMore())
                    {
                        var entity = entities.Next();
                        var sAMAccountName = entity.GetAttribute("cn")?.StringValue;
                        var cn = entity.GetAttribute("cn")?.StringValue;
                        var mail = entity.GetAttribute("mail")?.StringValue;
                        var sn = entity.GetAttribute("sn")?.StringValue;
                        var chinaName = entity.GetAttribute("gecos")?.StringValue;

                        Console.WriteLine($"User name : {sAMAccountName}");//james
                        Console.WriteLine($"User full name : {cn}");//James, Clark [james]
                        Console.WriteLine($"User mail address : {mail}");//james@contoso.com

                        //If you need to Case insensitive, please modify the below code.
                        if (sAMAccountName != null && sAMAccountName == username)
                        {
                            userDn = entity.Dn;
                            break;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(userDn)) return false;
                    conn.Bind(userDn, password);//这里用户名或密码错误会抛出异常LdapException
                                                // LdapAttribute passwordAttr = new LdapAttribute("userPassword", password);
                                                // var compareResult = conn.Compare(userDn, passwordAttr);
                    #endregion

                    conn.Disconnect();
                    return true;
                }
            }
            catch (LdapException ldapEx)
            {
                string message = ldapEx.Message;
                throw ldapEx;
                //return false;
            }
            catch (Exception e)
            {
                string message = e.Message;
                //return false;
                throw e;
            }
        }

        /// <summary>
        /// 同步用户
        /// </summary>
        /// <returns></returns>
        public static bool Sync()
        {
            try
            {
                using (var conn = new LdapConnection())
                {
                    #region 获取当前请求上下文的公司所属的LDAP Server配置
                    //获取当前公司CompanyId的LDAP Server Setting
                    var companyId = App.User.FindFirst("CompanyId")?.Value;
                    if(string.IsNullOrEmpty(companyId)) throw Oops.Oh("oh.. Your have to belong to an company to sync ldap user");
                    LDAPServerSettingsOptions ldapSetting = ldapServerSettings.FirstOrDefault(x => x.CompanyId == companyId);
                    if (ldapSetting == null) throw Oops.Oh("oh.. Your company not configue LDAP Server Setting");

                    #endregion

                    #region 验证连接登录
                    conn.Connect(ldapSetting.Host, ldapSetting.Port);
                    conn.Bind(ldapSetting.UserDN, ldapSetting?.Password);//这里用户名或密码错误会抛出异常LdapException
                    #endregion

                    #region 获取根节点
                    string[] _adPaths = new string[] { ldapSetting.BaseDN };
                    
                    List<LdapEntry> list = new List<LdapEntry>();
                    foreach (string path in _adPaths)
                    {
                        if (!string.IsNullOrEmpty(ldapSetting.Host))
                        {
                            LdapEntry entry = conn.Read(path);
                            list.Add(entry);
                        }
                    }
                    #endregion

                    #region 遍历同步
                    foreach (LdapEntry entry in list)
                    {
                        SyncDirectoryEntry(entry, conn, entry);
                    }

                    #endregion

                    conn.Disconnect();
                    return true;
                }
            }
            catch (LdapException ldapEx)
            {
                string message = ldapEx.Message;
                throw ldapEx;
                //return false;
            }
            catch (Exception e)
            {
                string message = e.Message;
                //return false;
                throw e;
            }
        }


        /// <summary>
        /// 递归操作
        /// </summary>
        /// <param name="rootEntry"></param>
        /// <param name="parentOrg"></param>
        /// <param name="currentEntry"></param>
        private static void SyncDirectoryEntry(LdapEntry rootEntry, LdapConnection conn, LdapEntry currentEntry)
        {
            List<LdapEntry> entryList = currentEntry.Children(conn);
            foreach (LdapEntry entry in entryList)
            {
                ////递归组织
                //if (entry.IsOrganizationalUnit())
                //{
                //    Org org = SyncOrgFromEntry(rootEntry, parentOrg, entry);
                //    this.SyncDirectoryEntry(rootEntry, org, entry);
                //}
                //else 
                if (entry.IsUser())
                {
                    SyncUserFromEntry(rootEntry, entry);
                }
            }
        }

        /// <summary>
        /// 同步用户
        /// </summary>
        /// <param name="rootEntry"></param>
        /// <param name="parentOrg"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static void SyncUserFromEntry(LdapEntry rootEntry,  LdapEntry entry)
        {
          
           MessageCenter.Send("sync:user", entry, isSync: true);
           
        }


        public static string GetAttributeValue(string attributeName, LdapEntry entity)
        {
            try
            {
              return  entity.GetAttribute(attributeName)?.StringValue;
            }
            catch (Exception)
            {

                return string.Empty;
            }
           
        }
    }
}
