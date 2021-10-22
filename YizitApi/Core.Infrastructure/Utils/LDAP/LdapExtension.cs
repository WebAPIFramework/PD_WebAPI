using System;
using System.Collections.Generic;
using System.Linq;
using Novell.Directory.Ldap;

namespace Core.Infrastructure.Utils
{
    /// <summary>
    /// LDAP辅助方法
    /// </summary>
    public static class LdapExtension
    {
        /// <summary>
        /// 获取Entry的GUID
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string Guid(this LdapEntry entry)
        {
            //var bytes = (byte[])(entry.GetAttribute("objectGUID").ByteValue as object);
            //var guid = new Guid(bytes);
            //return guid.ToString();

            return entry.GetAttribute("uidNumber")?.ToString();
        }

        /// <summary>
        /// 获取entry的子级
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<LdapEntry> Children(this LdapEntry entry, LdapConnection connection)
        {
            //string filter = "(&(objectclass=user))";
            List<LdapEntry> entryList = new List<LdapEntry>();
            var lsc = connection.Search(entry.Dn, LdapConnection.ScopeSub, "objectClass=*", null, false);
            if (lsc == null) return entryList;

            while (lsc.HasMore())
            {
                LdapEntry nextEntry = null;
                try
                {
                    nextEntry = lsc.Next();

                    if (nextEntry.IsUser() || nextEntry.IsOrganizationalUnit())
                    {
                        entryList.Add(nextEntry);
                    }
                }
                catch (LdapException e)
                {
                    continue;
                }
            }
            return entryList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static List<string> ObjectClass(this LdapEntry entry)
        {
            List<string> list = new List<string>();
            byte[][] bytes = (byte[][])(entry.GetAttribute("objectClass").ByteValueArray as object);
            for (var i = 0; i < bytes.Length; i++)
            {
                string str = System.Text.Encoding.Default.GetString(bytes[i]);
                list.Add(str.ToLower());
            }
            return list;
        }

        /// <summary>
        /// 判断 Entry 是否为用户
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static bool IsUser(this LdapEntry entry)
        {
            return entry.ObjectClass().Contains("person");
        }

        /// <summary>
        /// 判断 Entry 是否为部门
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static bool IsOrganizationalUnit(this LdapEntry entry)
        {
            return entry.ObjectClass().Contains("organizationalunit");
        }

        /// <summary>
        /// 获取 Entry 的修改时间
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static DateTime WhenChanged(this LdapEntry entry)
        {
            string value = entry.GetAttribute("whenChanged").StringValue;
            if (value.Split('.').Length > 1)
            {
                value = value.Split('.')[0];
            }
            DateTime whenChanged = DateTime.ParseExact(value, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
            return whenChanged;
        }

        /// <summary>
        /// 判断 Entry 中属性是否存在
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public static bool ContainsAttr(this LdapEntry entry, string attrName)
        {
            //LdapAttribute ldapAttribute = new LdapAttribute(attrName);
            //var res = entry.GetAttributeSet();
            //return res.Contains(ldapAttribute);

            return entry.ContainsAttr(attrName);
                       
        }
        /// <summary>
        /// 根据名称获取 Entry 中的属性值
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public static string AttrStringValue(this LdapEntry entry, string attrName)
        {
            if (!entry.ContainsAttr(attrName))
            {
                return string.Empty;
            }
            return entry.GetAttribute(attrName).StringValue;
        }
    }
}
