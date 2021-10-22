using System;
using System.Collections.Generic;
using Novell.Directory.Ldap;

namespace Core.Infrastructure.Utils
{
    /// <summary>
    /// AD帮助类
    /// </summary>
    public class ADHelper
    {
        private LdapConnection _connection;
        private string[] _adPaths;
        private string _adHost;
        private string domainName;
        private Org _org;
        private LDAPUser _user;

        /// <summary>
        /// 连接AD
        /// </summary>
        /// <returns></returns>
        public bool ADConnect()
        {
            _adHost = "192.168.1.1";
            
            string adAdminUserName = "root";
            string adAdminPassword = "Luming@yizit.cn";
            _adPaths =new string[] { "cn=users,dc=yizit,dc=cn" };
            domainName = "uid=root,cn=users,dc=yizit,dc=cn";
            if ((string.IsNullOrEmpty(_adHost) || string.IsNullOrEmpty(adAdminUserName)) ||
                string.IsNullOrEmpty(adAdminPassword))
            {
                return false;
            }
            try
            {
                _connection = new LdapConnection();
                _connection.Connect(_adHost, LdapConnection.DefaultPort);
                _connection.Bind(domainName, adAdminPassword);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public List<LdapEntry> GetRootEntries(string[] adPathes, string host)
        {
            List<LdapEntry> list = new List<LdapEntry>();
            foreach (string path in adPathes)
            {
                if (!string.IsNullOrEmpty(host))
                {
                    LdapEntry entry = _connection.Read(path);
                    list.Add(entry);
                }
            }
            return list;
        }

        /// <summary>
        /// 同步方法
        /// </summary>
        /// <returns></returns>
        public bool Sync()
        {
            ADConnect();

            if (_connection == null)
            {
                throw new Exception("AD连接错误，请确认AD相关信息配置正确!");
            }
            bool result = true;
            List<LdapEntry> entryList = this.GetRootEntries(_adPaths, _adHost);
            _org = new Org();
            _user = new LDAPUser();
            Org rootOrg = _org.GetRootOrg();
            foreach (LdapEntry entry in entryList)
            {
                SyncDirectoryEntry(entry, rootOrg, entry);
            }

            return result;
        }

        /// <summary>
        /// 递归操作
        /// </summary>
        /// <param name="rootEntry"></param>
        /// <param name="parentOrg"></param>
        /// <param name="currentEntry"></param>
        private void SyncDirectoryEntry(LdapEntry rootEntry, Org parentOrg, LdapEntry currentEntry)
        {
            List<LdapEntry> entryList = currentEntry.Children(_connection);
            foreach (LdapEntry entry in entryList)
            {
                if (entry.IsOrganizationalUnit())
                {
                    Org org = this.SyncOrgFromEntry(rootEntry, parentOrg, entry);
                    this.SyncDirectoryEntry(rootEntry, org, entry);
                }
                else if (entry.IsUser())
                {
                    this.SyncUserFromEntry(rootEntry, parentOrg, entry);
                }
            }
        }

        /// <summary>
        /// 同步部门
        /// </summary>
        /// <param name="rootEntry"></param>
        /// <param name="parentOrg"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private Org SyncOrgFromEntry(LdapEntry rootEntry, Org parentOrg, LdapEntry entry)
        {
            string orgId = entry.Guid().ToLower();
            Org org = this._org.GetOrgById(orgId) as Org;
            if (org != null)
            {
                if (entry.ContainsAttr("ou"))
                {
                    org.Name = entry.GetAttribute("ou").StringValue + string.Empty;
                }
                //设置其他属性的值
                _org.UpdateOrg(org);
                return org;
            }
            org = new Org
            {
                Id = orgId,
                ParentId = parentOrg.Id,
            };

            //设置其他属性的值
            this._org.AddOrg(org);
            return org;
        }

        /// <summary>
        /// 同步用户
        /// </summary>
        /// <param name="rootEntry"></param>
        /// <param name="parentOrg"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private LDAPUser SyncUserFromEntry(LdapEntry rootEntry, Org parentOrg, LdapEntry entry)
        {
            //uidNumber: entry.GetAttribute("uidNumber").StringValue; //
            string userId = entry.Guid().ToLower();
            LDAPUser user = this._user.GetUserById(userId);
            if (user != null)
            {
                user.ParentId = parentOrg.Id;
                //设置其他属性的值
                this._user.UpdateUser(user);
                  
                return user;
            }
            user = new LDAPUser
            {
                Id = userId,
                ParentId = parentOrg.Id
            };
            //设置其他属性的值
            this._user.AddUser(user);
            return user;
        }
    }
}
