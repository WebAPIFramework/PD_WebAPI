using System;
namespace Core.Infrastructure.Utils
{
    public class LDAPUser
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }

        public bool AddUser(LDAPUser user)
        {
            //数据库操作
            return true;
        }
        public bool UpdateUser(LDAPUser user)
        {
            //数据库操作
            return true;
        }
        public LDAPUser GetUserById(string id)
        {
            //根据Id获取Org
            return new LDAPUser();
        }
    }
}
