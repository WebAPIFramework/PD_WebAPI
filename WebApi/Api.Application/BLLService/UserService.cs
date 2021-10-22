using Core.Infrastructure.Base;
using Core.Infrastructure.CacheService;
using Core.Infrastructure.Common;
using Core.Infrastructure.Enum;
using Core.Infrastructure.ErrorCodes;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Options;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.Localization;
using Mapster;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Api.Application.AOP;
using Api.Application.Dtos;
using Api.Application.Dtos.User;
using Api.Application.Enum;
using Api.Application.Vo;
using Api.Core;
using Role = Api.Core.Role;

namespace Api.Application.BuinessLayer
{
    public interface IUserService
    {
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        LoginUserInfoResponse GetUserInfo();
        /// <summary>
        /// 获取当前用户权限
        /// </summary>
        /// <returns></returns>
        PrivilegeCode GetUserPrivilege();
        /// <summary>
        /// 更新自己的密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool UpdateCurrentPassword(UserPassword dto);
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        QueryUserResponse Update(UserDto dto);
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        QueryUserResponse Insert(UserDto dto);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(string id);
        /// <summary>
        /// 更新 用户状态（启/停用户）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userStatus"></param>
        /// <returns></returns>
        bool UpdateUserStatus(string id, EnumUserStatus userStatus);
        /// <summary>
        /// 退出系统（清除reids中的token缓存）
        /// </summary>
        /// <returns></returns>
        bool Logout();

        /// <summary>
        /// 获取当前企业的全部权限
        /// </summary>
        /// <returns></returns>
        PrivilegeCode GetCompanyPrivilege();
     

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool ResetPassword(string id);
        /// <summary>
        /// 更新用户角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool UpdateUsersRoles(UserRolesDto dto);
        /// <summary>
        /// 分页查询用户
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        Pagination<QueryUserResponse> PaginationUsers(QueryParameter queryParameter);

        /// <summary>
        /// 指定用户获取用户权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        PrivilegeCode GetUserPrivilege(User user);
    }

    public class UserService : ITransient, IUserService
    {
        #region services
        private readonly IRepository<Account, TubroDbContextLocator> _account;
        private readonly IRepository<User, TubroDbContextLocator> _userInfo;
        private readonly IRepository<UserRoles, TubroDbContextLocator> _userRole;
        private readonly IRepository<Staff, TubroDbContextLocator> _staff;
        private readonly IRepository<Company, TubroDbContextLocator> _company;
        private readonly IRepository<CompanyApplicablePrivileges, TubroDbContextLocator> _companyPrivileges;
        private readonly IRepository<Role, TubroDbContextLocator> _role;
        private readonly IRepository<RolePrivileges, TubroDbContextLocator> _rolePrivilege;
        private readonly PasswordInfoOptions _passwordOption; //IWritableOptions<PasswordInfoOptions> _passwordOption;
        private readonly IWritableOptions<PasswordInfoOptions> _writeableOption;
        private readonly ICacheService _cache;
        private readonly ICacheService<User, TubroDbContextLocator> _cacheUser;
        public UserService(
            IRepository<Account, TubroDbContextLocator> account,
            IRepository<User, TubroDbContextLocator> userInfo,
            IRepository<Staff, TubroDbContextLocator> staff,
            IRepository<UserRoles, TubroDbContextLocator> userRole,
            IRepository<Company, TubroDbContextLocator> company,
            IRepository<CompanyApplicablePrivileges, TubroDbContextLocator> companyPrivileges,
            IRepository<Role, TubroDbContextLocator> role,
            IRepository<RolePrivileges, TubroDbContextLocator> rolePrivilege,
           IWritableOptions<PasswordInfoOptions> writeableOption,
           IOptions<PasswordInfoOptions> passwordOption,
            ICacheService cache,
            ICacheService<User, TubroDbContextLocator> cacheUser
            )
        {
            _account = account;
            _userInfo = userInfo;
            _staff = staff;
            _userRole = userRole;
            _company = company;
            _companyPrivileges = companyPrivileges;
            _role = role;
            _rolePrivilege = rolePrivilege;
            _passwordOption = passwordOption.Value;
            _writeableOption = writeableOption;
            _cache = cache;
            _cacheUser = cacheUser;
        }


        #endregion

        #region 获取用户的用户信息和权限列表
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public LoginUserInfoResponse GetUserInfo()
        {
            //判断用户
            var userName = App.User?.FindFirst("UserName")?.Value;
            if (userName.ToLower() == "superadmin") return new LoginUserInfoResponse
            {
                UserId = "00000000-0000-0000-0000-000000000000",
                Username="SuperAdmin",
                UserType=(int)EnumUserType.SuperAdmin,
                CompanyId= App.User?.FindFirst("CompanyId")?.Value,
                AccountType=EnumAccountType.Local

            };

            var user = CheckUserValidByToken();
            var res= user.Adapt<LoginUserInfoResponse>();
            var account = _account.FirstOrDefault(x => x.UserId == user.Id && x.Deleted==0);
            res.AccountType =(EnumAccountType) account.AccountType;
            return res;
        }

        
        /// <summary>
        /// 获取权限
        /// </summary>
        /// <returns></returns>
        public PrivilegeCode GetUserPrivilege()
        {
            #region 超级管理员
            //判断是否是superAdmin，如果是，走superAdmin判断逻辑
            var userName = App.User?.FindFirst("UserName")?.Value;
            if (userName.ToLower() == "superadmin")
            {
                return new PrivilegeCode();//当超级管理员时，无需返回权限列表，web根据超级管理员标记展示全部权限列表
               
            }
            #endregion

            var user = CheckUserValidByToken();

            return GetUserPrivilege(user);
            
        }

        public PrivilegeCode GetCompanyPrivilege()
        {
            #region 超级管理员
            //判断是否是superAdmin，如果是，走superAdmin判断逻辑
            var userName = App.User?.FindFirst("UserName")?.Value;
            if (userName.ToLower() == "superadmin")
            {
                return new PrivilegeCode();//当超级管理员时，无需返回权限列表，web根据超级管理员标记展示全部权限列表

            }
            #endregion
            
            var companyId = App.User?.FindFirst("CompanyId")?.Value;
           var companyPrivileges= _companyPrivileges.Where(x => x.EnterpriseId == companyId);
            var menuIds = companyPrivileges.Where(x => x.CodeType == (int)EnumPrivilegeCodeType.Menus)?.Select(x=>x.PrivilegeCode)?.ToList();
            var entityIds= companyPrivileges.Where(x => x.CodeType == (int)EnumPrivilegeCodeType.Entities)?.Select(x => x.PrivilegeCode)?.ToList();

            return new PrivilegeCode { MenuIds=menuIds,EntityIds=entityIds };
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool UpdateCurrentPassword(UserPassword dto)
        {

            #region 判断是否是超级管理员
            var isSuperAdmin = App.User?.FindFirst("UserName")?.Value.ToLower() == "superadmin";
            if(isSuperAdmin)
            {
                var orisuperPassword = App.GetOptions<PasswordInfoOptions>().SuperPassword;
                //判断原密码是否一致
                var oriMatch = MD5Encryption.Compare(dto.OriginPassword, orisuperPassword);
                if (!oriMatch) throw Oops.Oh(ErrorCodes.z1003, L.Text["OriginalPassword"]);

                //如果原密码一致，则更新新密码
                //App.GetOptions<PasswordInfoOptions>().SuperPassword = MD5Encryption.Encrypt(dto.Password);
                _writeableOption.Update(opt =>
                {
                    opt.SuperPassword = MD5Encryption.Encrypt(dto.Password);
                });
               // _passwordOption.SuperPassword= MD5Encryption.Encrypt(dto.Password);

                return true;
            }

            #endregion

            //获取当前用户的userid
            var userId = App.User?.FindFirst("UserId")?.Value;
            
            //根据userid，获取当前用户的密码
            var account = _account.AsQueryable(false).FirstOrDefault(x => x.UserId == userId && x.Deleted == 0 && x.AccountType == 0);//获取本地账户
            var oriPassword = MD5Encryption.Compare(dto.OriginPassword,account.Password);
            
            //是否和输入的原密码一致
            if (!oriPassword) throw Oops.Oh(ErrorCodes.z1003, L.Text["OriginalPassword"]);
            account.Password = MD5Encryption.Encrypt(dto.Password);
            
            //如果一致，则更新密码
            _account.Update(account);
            return true;
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            var bearerToken = App.HttpContext.Request.Headers["Authorization"].ToString();
            var token = bearerToken.Substring("Bearer".Length).Trim();
            _cache.DeleteToken(token);
            return true;
        }


        #endregion

        #region 用户管理
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public QueryUserResponse Update(UserDto dto)
        {
            //判断是否已存在同名的用户
            var duplicateNameUser = _userInfo.AsQueryable(false).FirstOrDefault(x => x.Name == (dto.Name??dto.Username) && x.UserType==0 && x.Deleted == 0 && x.Id!=dto.Id);
            if (duplicateNameUser != null)
            {
                var duplicateAccount = _account.AsQueryable(false).FirstOrDefault(x => x.UserId == duplicateNameUser.Id);
                if (duplicateAccount?.AccountType == dto.AccountType)

                throw Oops.Oh(ErrorCodes.z1004, dto.Username);

            }
                


            //判断是否存在相同的手机号和邮箱
            if (!string.IsNullOrEmpty(dto.Email))
            {
                var duplicatestaff = _staff.AsQueryable(false).FirstOrDefault(x => x.Email == dto.Email && x.Deleted == 0 && x.UserId!=dto.Id);

                if (duplicatestaff != null)
                {
                    var duplicateAccount = _account.AsQueryable(false).FirstOrDefault(x => x.UserId == duplicatestaff.UserId);
                    if(duplicateAccount?.AccountType==dto.AccountType)
                        
                        throw Oops.Oh(ErrorCodes.z1004, dto.Email);
                }

                
            }

            if (!string.IsNullOrEmpty(dto.Mobile))
            {
                var duplicatestaff = _staff.AsQueryable(false).FirstOrDefault(x => x.Phone == dto.Mobile && x.Deleted == 0 && x.UserId != dto.Id);
                if (duplicatestaff != null)
                {
                    var duplicateAccount = _account.AsQueryable(false).FirstOrDefault(x => x.UserId == duplicatestaff.UserId);
                    if (duplicateAccount?.AccountType == dto.AccountType)

                        throw Oops.Oh(ErrorCodes.z1004, dto.Mobile);
                }
               
            }


            var user = _userInfo.FirstOrDefault(x => x.Id == dto.Id ); //dto.Adapt<User>();
            //var user = _cacheUser.Get(dto.Id);
           
            user.Name = dto.Username;
            user.Status = dto.Available??0;
            user.Deleted = 0;//有可能通过LDAP等同步用户时，之前手工删除的LDAP用户，同步后，需要重新激活
            _userInfo.Update(user, ignoreNullValues:true);
            
            var currentAccount = _account.FirstOrDefault(x => x.UserId == dto.Id,false);
            var staffId = _staff.FirstOrDefault(x => x.UserId == dto.Id ,false)?.Id;

            var account = dto.Adapt<Account>();
            var staff=dto.Adapt<Staff>();
            if (currentAccount!=null) 
            {
                account.Id = currentAccount?.Id; 
                account.AccountType = currentAccount.AccountType; //用户修改后，账户类型不变
                account.Deleted = 0;
                dto.AccountType= currentAccount.AccountType;
            }

            if (!string.IsNullOrEmpty(staffId)) staff.Id = staffId;
            _account.Update(account, true);
            staff.Deleted = 0;
            _staff.Update(staff, true);

            dto.Available = user.Status;
            var res = dto.Adapt<QueryUserResponse>();
            var userRoles = _userRole.Where(x => x.UserId == dto.Id);
            var roles = _role.Where(x => userRoles.Select(t => t.RoleId).Contains(x.Id) && x.Deleted == 0).Select(x => new RoleResponse { Id = x.Id, Name = x.Name }).ToList(); ;
            res.Roles = roles;
            return res;
        }

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public QueryUserResponse Insert(UserDto dto)
        {
            //判断是否已存在同名的用户
            var duplicateNameUser = _userInfo.AsQueryable(false).FirstOrDefault(x => x.Name == (dto.Name ?? dto.Username) && x.UserType == 0 && x.Deleted == 0);
            if (duplicateNameUser != null)
            {
                var duplicateAccount = _account.AsQueryable(false).FirstOrDefault(x => x.UserId == duplicateNameUser.Id);
                if (duplicateAccount?.AccountType == dto.AccountType)

                    throw Oops.Oh(ErrorCodes.z1004, dto.Username);

            }

            //判断是否存在相同的手机号和邮箱
            if (!string.IsNullOrEmpty(dto.Email)) 
            {
                var duplicatestaff = _staff.AsQueryable(false).FirstOrDefault(x => x.Email == dto.Email && x.Deleted == 0);
                if (duplicatestaff != null)
                {
                    var duplicateAccount = _account.AsQueryable(false).FirstOrDefault(x => x.UserId == duplicatestaff.UserId);
                    if (duplicateAccount?.AccountType == dto.AccountType)

                        throw Oops.Oh(ErrorCodes.z1004, dto.Email);
                }
            }

            if (!string.IsNullOrEmpty(dto.Mobile))
            {
                var duplicatestaff = _staff.AsQueryable(false).FirstOrDefault(x => x.Phone == dto.Mobile && x.Deleted == 0);
                if (duplicatestaff != null)
                {
                    var duplicateAccount = _account.AsQueryable(false).FirstOrDefault(x => x.UserId == duplicatestaff.UserId);
                    if (duplicateAccount?.AccountType == dto.AccountType)

                        throw Oops.Oh(ErrorCodes.z1004, dto.Mobile);
                }
            }

            var user = dto.Adapt<User>();
            user.Status =(int) EnumUserStatus.Enabled;
            var data= _userInfo.InsertNow(user);
            dto.Id = user.Id;

            var staff = dto.Adapt<Staff>();
            var account=dto.Adapt<Account>();

            if (!string.IsNullOrEmpty(staff.Name))
            {
                staff.UserId = data.Entity.Id;
                _staff.Insert(staff);
            }

            account.UserId = data.Entity.Id;
            if(dto.AccountType==(int)EnumAccountType.Local) account.Password = MD5Encryption.Encrypt("111111");//新建用户默认密码111111

            _account.Insert(account);

            var res = dto.Adapt<QueryUserResponse>();

            return res;

        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(string id)
        {
            //删除自己，不允许
            if (id == App.User?.FindFirst("UserId").Value) throw Oops.Oh(ErrorCodes.z1005,L.Text["Self"]);//Not allow to delete self

            //更新用户，账户，员工的delete标记为1

            var user = _userInfo.Find(id);
            if (user == null || user.Deleted==1) return true;// throw Oops.Oh(ErrorCodes.z1002, "User");
            if (user.UserType == (int)EnumUserType.Admin) throw Oops.Oh("Not allow to delete company administrator");
            var accounts = _account.Where(x => x.UserId == id && x.Deleted==0).ToList();
            var staff = _staff.FirstOrDefault(x => x.UserId == id && x.Deleted == 0);

            #region 持久化

            user.Deleted = 1;
            _userInfo.Update(user);

            accounts.ForEach(x =>
            {
                x.Deleted = 1;
            });
            _account.Update(accounts);

            if (staff != null)
            {
                staff.Deleted = 1;
                _staff.Update(staff);
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 更新 用户状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userStatus"></param>
        /// <returns></returns>
        public bool UpdateUserStatus(string id, EnumUserStatus userStatus)
        {
            #region 系统管理员和 拥有用户管理页面的用户 不可对自己及管理员进行密码重置
            //启用/禁用自己，不允许
            if (id == App.User?.FindFirst("UserId").Value) throw Oops.Oh(ErrorCodes.z1006, L.Text["Account"], L.Text["Status"]);//Not allow to Change self account status

            var user = _userInfo.FirstOrDefault(X => X.Id == id);
            if (user == null) throw Oops.Oh(ErrorCodes.z1002, L.Text["User"]);

            if (user.UserType == (int)EnumUserType.Admin) 
                throw Oops.Oh("Not allow to Change status for ownself");
            #endregion



            user.Status = (int)userStatus;
            _userInfo.Update(user);
            return true;
        }
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ResetPassword(string id)
        {
            var user = _userInfo.FirstOrDefault(x=>x.Id==id && x.Deleted==0);
            if (user == null) throw Oops.Oh(ErrorCodes.z1002, L.Text["User"]);
            var account = _account.FirstOrDefault(x => x.UserId == id && x.Deleted == 0);
            if(account==null) throw Oops.Oh(ErrorCodes.z1002, L.Text["Account"]);

            #region 系统管理员和 拥有用户管理页面的用户 不可对自己及管理员进行密码重置
            if(user.UserType==(int)EnumUserType.Admin) throw Oops.Oh("Not allow to reset password for company Admin");
            if (App.User.FindFirst("UserId").Value==id) throw Oops.Oh("Not allow to reset password for ownself");
            #endregion 


            account.Password = MD5Encryption.Encrypt("111111");//密码默认重置为111111
            _account.Update(account);
            return true;
        }

        /// <summary>
        /// 用户授权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool UpdateUsersRoles(UserRolesDto dto)
        {
            #region 系统管理员和 拥有用户管理页面的用户 不可对自己及管理员进行授权
            var adminUsers = _userInfo.Where(x => dto.UserIds.Contains(x.Id) && x.UserType == (int)EnumUserType.Admin);
            if (adminUsers != null && adminUsers.Count() > 0) throw Oops.Oh("Not allow to grant privileges to company Admin");
            if (dto.UserIds.Contains(App.User.FindFirst("UserId").Value)) throw Oops.Oh("Not allow to grant privileges to ownself");
            #endregion 

            //获取当前要更新用户（列表）的所有权限列表
            var userRoles = _userRole.Where(x => dto.UserIds.Contains(x.UserId));

            #region 新的处理逻辑
            //获取传入的全部列表
            var assignedUserRoles = (from c in dto.UserIds
                                     from d in dto.RoleIds
                                     select new { UserId = c, RoleId = d }).ToList();

            foreach (var userId in dto.UserIds)
            {
                //存储列表
                var savedList = userRoles.Where(x => x.UserId == userId);
                //传入列表
                var passList = assignedUserRoles.Where(x => x.UserId == userId);

                //比较获取删除列表（存储有，传入无）

                var deleteList = savedList.Where(x => !passList.Select(t => t.RoleId).Distinct().ToList().Contains(x.RoleId));
                _userRole.Delete(deleteList);
                //比较获取新增列表（传入有，存储无）
                var addList = passList.Where(x => !savedList.Select(t => t.RoleId).Distinct().ToList().Contains(x.RoleId)).Select(x=> new UserRoles { UserId=userId,RoleId=x.RoleId });
                _userRole.Insert(addList);
            }


            #endregion

          
            return true;
        }

        public Pagination<QueryUserResponse> PaginationUsers(QueryParameter queryParameter)
        {
            #region 分页思路
            //1. 联表查询（获取返回体的全部列表）
            var sysUsers =(
                         from u in _userInfo.AsQueryable(false).Where(x => x.Deleted == 0)
                      
                         select new QueryUserResponse
                         {
                             Id = u.Id,
                             Username = u.Name,
                             Available = u.Status ,
                             UserType = (EnumUserType)u.UserType
                         }).ToList();

            sysUsers.ForEach(x =>
            {
                var staff = _staff.FirstOrDefault(t => t.UserId == x.Id);
                if(staff!=null)
                {
                    x.Name = staff.Name;
                    x.JobNo = staff.No;
                    x.Mobile = staff.Phone;
                    x.Email=staff.Email;
                }
                var userRoles = from userrole in _userRole.AsQueryable()
                                join role in _role.AsQueryable() on userrole.RoleId equals role.Id
                                where userrole.UserId == x.Id
                                select new Vo.RoleResponse { Id = role.Id, Name = role.Name };
                if (userRoles != null && userRoles.Count() > 0) x.Roles = userRoles.ToList();
                else x.Roles = new List<Vo.RoleResponse>();

                var account = _account.FirstOrDefault(t => t.UserId == x.Id);
                if(account!=null)
                {
                    x.AccountType = (EnumAccountType)account.AccountType;
                }
            });

            #region 先过滤按角色集合查询
            if(queryParameter.QueryFields.FirstOrDefault(x=>x.Name== "RoleIds")!=null)
            {
                var roleIdsQuery = queryParameter.QueryFields.Find(t => t.Name == "RoleIds").Value.Split(",").ToList();
                var datas = sysUsers.AsQueryable().Where(f => f.Roles.Select(t => t.Id).Intersect(roleIdsQuery).Count() > 0).ToList();
                //更新剩余查询条件
                queryParameter.QueryFields = queryParameter.QueryFields.Where(x => x.Name != "RoleIds").ToList();
                sysUsers = datas;
            }
          
            #endregion

            //2. 根据顶部查询条件 以及排序过滤列表
            var queryUsers = sysUsers.AsQueryable().Where(queryParameter.QueryFields).OrderBy(queryParameter.SortField);

            //3. 根据表头查询条件，过滤列表（tobe...待改进，实现动态查询）
            var result = queryUsers.ToList();
            foreach (var item in queryParameter.HeaderFilters)
            {
                var tempresult = new List<QueryUserResponse>();
                if (item.Name == "UserName")
                    tempresult= queryUsers.Where(x => item.Value.Contains(x.Username)).ToList();
                if(item.Name== "Name")
                    tempresult = queryUsers.Where(x => item.Value.Contains(x.Name)).ToList();
                if(item.Name=="JobNo")
                    tempresult = queryUsers.Where(x => item.Value.Contains(x.JobNo)).ToList();
                if (item.Name == "Mobile")
                    tempresult = queryUsers.Where(x => item.Value.Contains(x.Mobile)).ToList();
                if (item.Name == "Email")
                    tempresult = queryUsers.Where(x => item.Value.Contains(x.Email)).ToList();
                if (item.Name == "UserType")
                    queryUsers.Where(x => item.Value.Contains(x.UserType.ToString())).ToList();

                result = tempresult;
            }

            //4. 根据分页条件，进行分页
            var resData = result.AsQueryable().ToPagination(queryParameter.PaginationInfo.Current, queryParameter.PaginationInfo.PageSize);

            #endregion

            #region  原实现方式
            //var paginationUsers = _userInfo.Where(x => x.Deleted == 0 && x.UserType == 0).AsQueryable().ToPagination(queryParameter.PaginationInfo.Current, queryParameter.PaginationInfo.PageSize);


            //var userIds = paginationUsers.Data.Select(x => x.Id);
            //var staffs = _staff.Where(x => userIds.Contains(x.UserId) && x.Deleted == 0);
            //var roles = _userRole.Where(x => userIds.Contains(x.UserId)).ToList();
            //var roleInfos = _role.Where(x => roles.Select(t => t.RoleId).Contains(x.Id)).ToList();

            //var applicableRoleInfos = (from c in roles
            //                           join t in roleInfos on c.RoleId equals t.Id
            //                           select new { UserId = c.UserId, Id = t.Id, Name = t.Name }).ToList();
            //List<QueryUserResponse> xxRoles = new List<QueryUserResponse>();

            //foreach (var item in paginationUsers.Data)
            //{
            //    var data = item.Adapt<QueryUserResponse>();
            //    var staff = staffs.FirstOrDefault(t => t.UserId == item.Id);
            //    data.JobNo = staff.No;
            //    data.Mobile = staff.Phone;
            //    data.Name = staff.Name;
            //    data.Email = staff.Email;
            //    var dataRoles = applicableRoleInfos.Where(t => t.UserId == item.Id).Select(t => new Api.Application.Vo.Role { Id = t.Id, Name = t.Name }).ToList();
            //    data.Roles = dataRoles;
            //    xxRoles.Add(data);
            //}

            //var retData = paginationUsers.ReplaceItems<QueryUserResponse>(xxRoles);
            #endregion

            //分页查询
            return resData;
        }

        #endregion

        #region 辅助方法
        public PrivilegeCode GetUserPrivilege(User user)
        {
            var enterpriseId = App.User?.FindFirst("CompanyId")?.Value;
           var adminMenuIds = _companyPrivileges.Where(x => x.CodeType == (int)EnumPrivilegeCodeType.Menus && x.EnterpriseId == enterpriseId)?.Select(x => x.PrivilegeCode).ToList();
          var  adminentitiesIds = _companyPrivileges.Where(x => x.CodeType == (int)EnumPrivilegeCodeType.Entities && x.EnterpriseId == enterpriseId)?.Select(x => x.PrivilegeCode).ToList();
            //如果是管理员
            if (user.UserType==(int)EnumUserType.Admin)
            {
                
                return new PrivilegeCode { MenuIds = adminMenuIds, EntityIds = adminentitiesIds };
            }
            //如果是普通用户，获取用户的角色对应的code列表
            var userRoles = _userRole.Where(x => x.UserId == user.Id);

            var rolePrivileges = _rolePrivilege.Where(x => userRoles.Select(t => t.RoleId).Contains(x.RoleId)).ToList();

            var menuIds = rolePrivileges.Where(x=>x.CodeType==(int)EnumPrivilegeCodeType.Menus).GroupBy(x => x.PrivilegeCode).Select(group => group.Key).ToList();
            var entitIds = rolePrivileges.Where(x => x.CodeType == (int)EnumPrivilegeCodeType.Entities).GroupBy(x => x.PrivilegeCode).Select(group => group.Key).ToList();

            #region 过滤超出企业授权范围（后续企业授权有变化（减少），但是减少的授权已被分配时）
            var filterMenuIds = menuIds.Intersect(adminMenuIds).ToList();
            var filterEntiIds = entitIds;//[经后续讨论，superadmin给企业授权时，只控制到有页面层级] entitIds.Intersect(adminentitiesIds).ToList();
            #endregion


            return new PrivilegeCode { MenuIds= filterMenuIds, EntityIds= filterEntiIds };
        }

        private User CheckUserValidByToken()
        {
            var userId = App.User?.FindFirst("UserId")?.Value;

            //判断是否传入companyid，如果未传入，则报错
            if (string.IsNullOrEmpty(App.User?.FindFirst("CompanyId")?.Value)) throw Oops.Oh(ErrorCodes.z1001,L.Text["Company"]);//Company Must be required

            var account = _account.FirstOrDefault(x => x.UserId == userId);

            //校验用户存在性
            if (account == null) throw Oops.Oh("LoginFailed");

            //校验用户有效性
            var user = _userInfo.Find(account.UserId);
            if (user == null || user.Status == 0) throw Oops.Oh("AccountDisabled");

            return user;
        }

    






        #endregion

    }


}
