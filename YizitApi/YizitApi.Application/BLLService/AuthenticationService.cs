using Core.Infrastructure.CacheService;
using Core.Infrastructure.ErrorCodes;
using Core.Infrastructure.Options;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using YizitApi.Application.AOP;
using YizitApi.Application.Dtos;
using YizitApi.Application.Enum;
using YizitApi.Application.Vo;
using YizitApi.Core;
using YizitApi.Core.ConfigureOptions;
using Core.Infrastructure.Enum;
using Role = YizitApi.Core.Role;
using Core.Infrastructure.Utils;
using YizitApi.Application.Vo.Company;

namespace YizitApi.Application.BuinessLayer
{
    public interface IAuthenticationService
    {
        LoginUserResponse Login(LoginDto dto);
        List<ApplicableCompanyInfo> GetCompanyList(string loginName);
        List<ApplicableCompanyInfo> GetCompanyList();
    }

    public class AuthenticationService : ITransient, IAuthenticationService
    {
        #region services
        private readonly IRepository<Account, TubroDbContextLocator> _account;
        private readonly IRepository<User, TubroDbContextLocator> _user;
        private readonly IRepository<Staff, TubroDbContextLocator> _staff;
        private readonly IRepository<UserRoles, TubroDbContextLocator> _userRole;
        private readonly IRepository<Company, TubroDbContextLocator> _company;
        private readonly IRepository<CompanyApplicablePrivileges, TubroDbContextLocator> _companyPrivileges;
        private readonly IRepository<Role, TubroDbContextLocator> _role;
        private readonly IRepository<RolePrivileges, TubroDbContextLocator> _rolePrivilege;
        private readonly IUserService _userService;
        private readonly ICacheService _cache;
        public AuthenticationService(
            IRepository<Account, TubroDbContextLocator> account,
            IRepository<User, TubroDbContextLocator> user,
            IRepository<UserRoles, TubroDbContextLocator> userRole,
            IRepository<Staff, TubroDbContextLocator> staff,
            IRepository<Company, TubroDbContextLocator> company,
            IRepository<CompanyApplicablePrivileges, TubroDbContextLocator> companyPrivileges,
            IRepository<Role, TubroDbContextLocator> role,
            IRepository<RolePrivileges, TubroDbContextLocator> rolePrivilege,
            IUserService userService,
            ICacheService cache
            )
        {
            _account = account;
            _user = user;
            _userRole = userRole;
            _staff = staff;
            _company = company;
            _companyPrivileges = companyPrivileges;
            _role = role;
            _rolePrivilege = rolePrivilege;
            _userService = userService;
            _cache = cache;
        }

       
        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public LoginUserResponse Login(LoginDto dto)
        {
            #region 超级管理员登陆逻辑
            //判断是否是superAdmin，如果是，走superAdmin判断逻辑
            if (dto.Username.ToLower() == "superadmin")
            {
                //获取配置的超级管理员密码，没有则默认111111
                dto.Password = MD5Encryption.Encrypt(dto.Password);
                var superPassword = App.GetOptions<PasswordInfoOptions>().SuperPassword ?? MD5Encryption.Encrypt("111111");  // App.Configuration["SuperPassword"] ?? MD5Encryption.Encrypt("111111");
                if (dto.Password != superPassword) 
                throw Oops.Oh(ErrorCodes.z1000,  L.Text["LoginName"],L.Text["Password"] );

                var userResponse= 
                 new LoginUserResponse
                {
                    UserName = "SuperAdmin",
                    UserType = (int)EnumUserType.SuperAdmin,//超级管理员
                    Token = JWTEncryption.Encrypt(new Dictionary<string, object>()
                     {
                         {"CompanyId",""},//dto.CompanyId??_company.AsQueryable().FirstOrDefault()?.Id
                         {"UserId", "00000000-0000-0000-0000-000000000000"},  // 存储Id
                         {"UserName","SuperAdmin" },
                         {"Account",dto.Username }, // 存储用户名
                         {"CreateTime",DateTime.Now.ToString() },
                    },dto.AutoLogin==true?7*24*60:1*24*60),
                    CompanyId=dto.CompanyId??_company.AsQueryable().FirstOrDefault()?.Id,
                    AccountType=EnumAccountType.Local,
                    PrivilegeCodes = new PrivilegeCode()//当超级管理员时，无需返回权限列表，web根据超级管理员标记展示全部权限列表
                };
                _cache.SaveToken(userResponse.Token, "00000000-0000-0000-0000-000000000000", dto.Platform,dto.AutoLogin);
                return userResponse;

            }
            #endregion

            //判断是否传入companyid，如果未传入，则报错
            if (string.IsNullOrEmpty(dto.CompanyId)) throw Oops.Oh(ErrorCodes.z1001,L.Text["Company"]);

            //var account = _account.FirstOrDefault(x => x.LoginName == dto.Username && x.Password == dto.Password);
            var account = _account.FirstOrDefault(x => x.LoginName == dto.Username && x.AccountType==(int)dto.LoginType && x.Deleted==0);

            //校验用户存在性
            if (account == null) 
            {
                //判断是否是本地管理员登录
                account= _account.FirstOrDefault(x => x.LoginName == dto.Username  && x.Deleted == 0);
                if (account != null)
                {
                    var AdminUser = _user.FirstOrDefault(x => x.Id == account.UserId);
                    if (AdminUser?.UserType==(int)EnumUserType.Admin)//则代表企业管理员登录
                        dto.LoginType =(EnumAccountType) account.AccountType;
                }
              
                else
                {
                    //支持邮箱或者手机号登录
                    var staffs = _staff.AsQueryable(false).Where(x => (x.Phone == dto.Username || x.Email == dto.Username) && x.Deleted == 0).ToList();
                    if (staffs==null || staffs.Count() == 0) throw Oops.Oh("LoginFailed， account not exist");
                    else
                    {
                        var filterUserIds = staffs.Select(t => t.UserId).Distinct().ToList();
                        account = _account.AsQueryable(false).FirstOrDefault(x => filterUserIds.Contains(x.UserId)  && x.Deleted == 0 && x.AccountType == (int)dto.LoginType);
                        if (account == null) throw Oops.Oh("LoginFailed， account not exist");
                        else
                            dto.Username = account.LoginName;
                    }
                }
                
            }
           

            //校验用户有效性
            var user = _user.Find(account.UserId);
            if (user == null || user.Status == 0) throw Oops.Oh("AccountDisabled");

            //校验是否是本地用户
            switch (dto.LoginType)
            {
                case EnumAccountType.Local://本地用户
                    //1. 加密
                    dto.Password = MD5Encryption.Encrypt(dto.Password);
                    CheckPassword(user, account,dto);
                    return UserCheckResponse(user, account,dto);
                case EnumAccountType.LDAP://LDAP用户
                    CheckPassword(user, account,dto);
                    return UserCheckResponse(user, account, dto);
                case EnumAccountType.WeChat://微信用户
                    throw Oops.Oh("Not supported Wechat account yet");
                case EnumAccountType.DingDing://钉钉用户
                    throw Oops.Oh("Not supported Dingding account yet");
                case EnumAccountType.Yizit://因致账户
                    throw Oops.Oh("Not supported Yizit account yet");
                default:
                    throw Oops.Oh("Not supported account type");
            }
        }

        /// <summary>
        ///  密码校验
        /// </summary>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <param name="dto"></param>
        private void CheckPassword(User user, Account account, LoginDto dto)
        {
            switch (dto.LoginType)
            {
                case EnumAccountType.Yizit:
                    break;
                case EnumAccountType.Local://本地用户（本地校验）
                   var accountWithCheckPassword= _account.FirstOrDefault(x => x.LoginName == dto.Username && x.Password==dto.Password && x.AccountType == (int)dto.LoginType && x.Deleted == 0);
                    if (accountWithCheckPassword == null) throw Oops.Oh("LoginFailed");
                    break;
                case EnumAccountType.LDAP:
                    var isExist = LDAPUtil.Validate(dto.Username,dto.Password);
                    if(!isExist) throw Oops.Oh("LoginFailed");
                    break;
                case EnumAccountType.WeChat:
                    break;
                case EnumAccountType.DingDing:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 本地用户判断逻辑
        /// </summary>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        private LoginUserResponse UserCheckResponse(User user, Account account, LoginDto dto)
        {
            
            #region 如果是企业管理员
            //判断是否是管理员
            if (user.UserType == (int)EnumUserType.Admin)
            {
                //如果是管理员，获取授权企业的权限列表
                var codes = _companyPrivileges.AsQueryable().ToList();
                var privilegeCodes = _userService.GetUserPrivilege(user); //codes.Select(x => new PrivilegeCode { Code = x.PrivilegeCode }).ToList();
                var Res=
                 new LoginUserResponse
                {
                    UserName = user.Name,
                    UserType = (int)EnumUserType.Admin,//管理员
                    Token = JWTEncryption.Encrypt(new Dictionary<string, object>()
                     {
                         {"CompanyId",user.CompanyId},
                         {"UserId", user.Id},  // 存储Id
                         {"UserName",user.Name },//用户名
                         {"Account",account.LoginName }, // 登陆账户
                         {"CreateTime",DateTime.Now.ToString() },
                    }, dto.AutoLogin == true ? 7 * 24 * 60 : 1 * 24 * 60),
                    CompanyId = user.CompanyId,
                     AccountType = dto.LoginType,
                     PrivilegeCodes = privilegeCodes //当管理员时，返回当前企业的全部权限列表
                };
                _cache.SaveToken(Res.Token, user.Id, dto.Platform,dto.AutoLogin);
                return Res;
            }
            #endregion


            //如果是普通用户，获取用户的角色对应的code列表
            var userRoles = _userRole.Where(x => x.UserId == user.Id);

            var rolePrivileges = _rolePrivilege.Where(x => userRoles.Select(t => t.RoleId).Contains(x.RoleId)).ToList();

            var actualPrivilegeCodes = _userService.GetUserPrivilege(user);//rolePrivileges.GroupBy(x => x.PrivilegeCode).Select(group => new PrivilegeCode { Code = group.Key }).ToList();

            var userResponse =
             new LoginUserResponse
            {
                UserName = user.Name,
                UserType = (int)EnumUserType.Normal,//普通用户
                Token = JWTEncryption.Encrypt(new Dictionary<string, object>()
                     {
                         {"CompanyId",user.CompanyId},
                         {"UserId", user.Id},  // 存储Id
                         {"UserName",user.Name },//用户名
                         {"Account",account.LoginName }, // 登陆账户
                         {"CreateTime",DateTime.Now.ToString() },
                    }, dto.AutoLogin == true ? 7 * 24 * 60 : 1 * 24 * 60),
                CompanyId = user.CompanyId,
                 AccountType = dto.LoginType,
                 PrivilegeCodes = actualPrivilegeCodes //当普通用户时，获取用户角色对应的权限列表并集
            };
            _cache.SaveToken(userResponse.Token,  user.Id, dto.Platform,dto.AutoLogin);
            return userResponse;
        }

     

        public List<ApplicableCompanyInfo> GetCompanyList(string userName)
        {
            //根据登录名，从staff表获取全部满足的userid列表
            var accounts = $"Select ID,LoginName,User_Id as UserId from Account where LoginName='{userName}'".Change<TubroDbContextLocator>().SqlQuery<Account>();

            //支持手机号和邮箱查询
            var staffAccounts = $@"select Account.ID, Account.LoginName, Account.User_Id as UserId from Staff 
left join Account on Staff.User_Id = Account.User_Id
where Phone = N'{userName}' or Email = N'{userName}'".Change<TubroDbContextLocator>().SqlQuery<Account>();

            accounts = accounts.Concat(staffAccounts).ToList<Account>();

            if (accounts == null || accounts.Count == 0) return new List<ApplicableCompanyInfo>();
            var userIds = string.Join(",", accounts.Select(x =>  $"'{x.UserId}'").Distinct().ToList<string>()) ;
            
            //根据userid列表，从user表获取所属的全部企业列表
            var users = $"select ID,Name,Company_Id as CompanyId from dbo.[User] where Id in({userIds}) and Deleted=0".Change<TubroDbContextLocator>().SqlQuery<User>();
            if (users == null || users.Count == 0) return new List<ApplicableCompanyInfo>();

            var companyIds = string.Join(",", users.Select(x =>$"'{x.CompanyId}'" ).Distinct().ToList<string>());
            //根据companyid列表，从company表获取所有的企业信息（id，name返回）
            var companys = $"Select * from Company where id in({companyIds}) and deleted=0".Change<TubroDbContextLocator>().SqlQuery<Company>();
            return GetCompanyRes(companys);
            //return companys;
        }

        public List<ApplicableCompanyInfo> GetCompanyList()
        {
            var companys = $"Select * from Company where  deleted=0".Change<TubroDbContextLocator>().SqlQuery<Company>();
            return GetCompanyRes(companys);
            //return companys;
        }

        private List<ApplicableCompanyInfo> GetCompanyRes(List<Company> companies)
        {
            List<ApplicableCompanyInfo> res = new List<ApplicableCompanyInfo>();
            foreach (var company in companies)
            {
                List<LoginType> accountTypes = new List<LoginType>();
                //通过查询该企业下所有的普通用户，属于几种类型，则支持几种类型
                var userIds = $"Select Id from [dbo].[User] where Company_Id='{company.Id}' and Deleted=0 and User_Type={(int)EnumUserType.Normal}".Change<TubroDbContextLocator>().SqlQuery<string>(); //_user.Where(x => x.CompanyId == company.Id && x.Deleted == 0 && x.UserType == (int)EnumUserType.Normal)?.Select(x=>x.Id)?.ToList();
                if(userIds.Count!=0)
                {
                   var accounts = $"Select *,Account_Type as AccountType from [dbo].[Account] where Company_Id='{company.Id}' and Deleted=0 and User_Id in ({string.Join(",", userIds.Select(a => "'" + a + "'")) }) ".Change<TubroDbContextLocator>().SqlQuery<Account>();
                    accountTypes= accounts?.Select(x => new LoginType { AccountType = (EnumAccountType)x.AccountType, Name = L.Text[((EnumAccountType)x.AccountType).ToString()] })?.Distinct(new LoginTypeComparer()).ToList();
                }
                
                    //_account.Where(x => x.CompanyId == company.Id && x.Deleted == 0 && userIds.Contains(x.UserId)).Select(x=> new LoginType { AccountType=(EnumAccountType)x.AccountType, Name=L.Text[((EnumAccountType)x.AccountType).ToString()] }).Distinct().ToList();

                res.Add(new ApplicableCompanyInfo
                {
                    Id=company.Id,
                    Name=company.Name,
                    LoginTypes= accountTypes
                });
            }

            return res;
        }
    }

    /// <summary>
    /// 比较器
    /// </summary>
    public class LoginTypeComparer : IEqualityComparer<LoginType>
    {
        public bool Equals(LoginType x, LoginType y)
        {
            return (int)x.AccountType == (int)y.AccountType;//可以自定义去重规则
        }
        public int GetHashCode(LoginType obj)
        {
            return obj.AccountType.GetHashCode();
        }
    }

}
