using Core.Infrastructure.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Vo.Company
{
    /// <summary>
    /// 登录页面获取公司信息的实体
    /// </summary>
  public  class ApplicableCompanyInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<LoginType> LoginTypes { get; set; }
    }

    /// <summary>
    /// 支持的登录类型
    /// </summary>
    public class LoginType
    {
        public EnumAccountType AccountType { get; set; }
        public string Name { get; set; }
    }
}
