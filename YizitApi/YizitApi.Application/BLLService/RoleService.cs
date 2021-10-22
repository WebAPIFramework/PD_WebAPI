using Core.Infrastructure.ErrorCodes;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.Localization;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using YizitApi.Application.AOP;
using YizitApi.Application.Dtos;
using YizitApi.Application.Dtos.User;
using YizitApi.Application.Enum;
using YizitApi.Application.Vo;
using YizitApi.Core;
using Role = YizitApi.Core.Role;

namespace YizitApi.Application.BuinessLayer
{
    public interface IRoleService
    {
        /// <summary>
        /// 获取企业角色列表
        /// </summary>
        /// <returns></returns>
        List<RoleResponse> GetRoleList();
        /// <summary>
        /// 获取指定角色及权限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        RoleWithPrivilege GetPrivilege(string roleId);
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        RoleResponse CreateRole(RoleWithPrivilege dto);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        bool DeleteRole(string roleId);
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        RoleResponse UpdateRole(RoleWithPrivilege dto);
    }

    public class RoleService : ITransient, IRoleService
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
        public RoleService(
            IRepository<Account, TubroDbContextLocator> account,
            IRepository<User, TubroDbContextLocator> userInfo,
            IRepository<Staff, TubroDbContextLocator> staff,
            IRepository<UserRoles, TubroDbContextLocator> userRole,
            IRepository<Company, TubroDbContextLocator> company,
            IRepository<CompanyApplicablePrivileges, TubroDbContextLocator> companyPrivileges,
            IRepository<Role, TubroDbContextLocator> role,
            IRepository<RolePrivileges, TubroDbContextLocator> rolePrivilege
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
        }

    
        #endregion

        /// <summary>
        /// 获取企业角色列表
        /// </summary>
        /// <returns></returns>
        public List<RoleResponse> GetRoleList()
        {
            var roleLists = _role.AsQueryable()?.Where(x => x.Deleted == 0)?.Select(x => new RoleResponse { Id = x.Id, Name = x.Name })?.ToList();
            return roleLists;
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public RoleResponse CreateRole(RoleWithPrivilege dto)
        {
           var role= _role.InsertNow(new Role { Name = dto.Name });

            var menuRolePrivileges = dto.MenuIds.Select(x => new RolePrivileges { RoleId = role.Entity.Id, PrivilegeCode = x, CodeType =(int) EnumPrivilegeCodeType.Menus });

            var entitiesRolePrivilegs = dto.EntityIds.Select(x => new RolePrivileges { RoleId = role.Entity.Id, PrivilegeCode = x, CodeType = (int)EnumPrivilegeCodeType.Entities });

            _rolePrivilege.Insert(menuRolePrivileges);
            _rolePrivilege.Insert(entitiesRolePrivilegs);

            return new RoleResponse { Id = role.Entity.Id, Name = role.Entity.Name };
        }
        /// <summary>
        /// 获取角色及权限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public RoleWithPrivilege GetPrivilege(string roleId)
        {
            var role = _role.FirstOrDefault(x=>x.Id== roleId);
            if (role == null) throw Oops.Oh(ErrorCodes.z1002, L.Text["Role"]);
            var menuPrivileges = _rolePrivilege.AsQueryable(false)?.Where(x => x.RoleId == roleId && x.CodeType == (int)EnumPrivilegeCodeType.Menus)?.Select(x=>x.PrivilegeCode).ToList() ;
            var entitiesPrivileges= _rolePrivilege.AsQueryable(false)?.Where(x => x.RoleId == roleId && x.CodeType == (int)EnumPrivilegeCodeType.Entities )?.Select(x => x.PrivilegeCode).ToList();


            return new RoleWithPrivilege { Id = role.Id, Name = role.Name, MenuIds = menuPrivileges, EntityIds = entitiesPrivileges };
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool DeleteRole(string roleId)
        {
            var role = _role.FirstOrDefault(x=>x.Id==roleId);
            if (role == null) return true;
            role.Deleted = 1;
            _role.Update(role);
            return true;
        }
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public RoleResponse UpdateRole(RoleWithPrivilege dto)
        {
            var preRole = _role.FirstOrDefault(x => x.Id == dto.Id);
            if (string.IsNullOrEmpty(dto.Id) || preRole == null) 
                throw Oops.Oh(ErrorCodes.z1002,L.Text["Role"] );

            #region 更新角色基本信息
            //更新角色名称
            preRole.Name = dto.Name;
            _role.Update(preRole);
            #endregion

            var alldbCodes = _rolePrivilege.AsQueryable(false).Where(x => x.RoleId == dto.Id).ToList();
            #region 处理模块权限
            //处理传入模块权限数据（去重）
            var menuIds = dto.MenuIds.Distinct().ToList();
            //比较现有库中存储的角色所属的模块权限列表，获取需要删除的权限（库中有，但是传入中没有）
            var dbMenuIds = alldbCodes.Where(x => x.CodeType == (int)EnumPrivilegeCodeType.Menus).ToList();
            var deletedMenuIds = dbMenuIds.Where(x => !menuIds.Contains(x.PrivilegeCode)).ToList();
            _rolePrivilege.DeleteNow(deletedMenuIds);//物理删除
            //比较现有库中存储的角色所属的模块权限列表，获取需要新增的权限（库中无，但是传入有）
            var newMenuIds= menuIds.Where(x => !dbMenuIds.Select(t=>t.PrivilegeCode).Contains(x)).Select(x=>new RolePrivileges { RoleId = dto.Id, PrivilegeCode = x, CodeType = (int)EnumPrivilegeCodeType.Menus });
            _rolePrivilege.Insert(newMenuIds);
            #endregion

            #region 处理功能点权限
            //处理传入功能点权限数据（去重）
            var entityIds = dto.EntityIds.Distinct().ToList();
            //比较现有库中存储的角色所属的功能点权限列表，获取需要删除的权限（库中有，但是传入中没有）
            var dbentityIds = alldbCodes.Where(x => x.CodeType == (int)EnumPrivilegeCodeType.Entities).ToList();
            var deletedentityIds = dbentityIds.Where(x => !entityIds.Contains(x.PrivilegeCode)).ToList();
            _rolePrivilege.DeleteNow(deletedentityIds);//物理删除
            //比较现有库中存储的角色所属的功能点权限列表，获取需要新增的权限（库中无，但是传入有）
            var newentityIds = entityIds.Where(x => !dbentityIds.Select(t => t.PrivilegeCode).Contains(x)).Select(x => new RolePrivileges { RoleId = dto.Id, PrivilegeCode = x, CodeType = (int)EnumPrivilegeCodeType.Entities });
            _rolePrivilege.Insert(newentityIds);
            #endregion



            return new RoleResponse { Id = preRole.Id, Name = preRole.Name };
        }
    }


}
