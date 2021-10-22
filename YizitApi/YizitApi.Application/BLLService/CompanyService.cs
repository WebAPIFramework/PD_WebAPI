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
using System.Collections.ObjectModel;
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
    public interface ICompanyService
    {
        /// <summary>
        /// 创建企业
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        CompanyDto CreateCompany(CompanyDto dto);
        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <returns></returns>
        List<CompanyDto> GetCompanyList();
        /// <summary>
        /// 更新企业基本信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        CompanyDto UpdateCompany(CompanyDto dto);
        /// <summary>
        /// 删除企业
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        bool DeleteCompany(string companyId);
        /// <summary>
        /// 获取可选时区列表
        /// </summary>
        /// <returns></returns>
        ReadOnlyCollection<TimeZoneInfo> GetTimeZoneList();
        /// <summary>
        /// 更新企业角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        CompanyPrivilege UpdateCompanyPrivileges(CompanyPrivilege dto);
        CompanyPrivilege GetCompanyPrivileges(string companyId);
    }

    public class CompanyService : ITransient, ICompanyService
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
        public CompanyService(
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
        public CompanyDto CreateCompany(CompanyDto dto)
        {
            #region 校验企业重复性
            var duplicateNameCompany = _company.AsQueryable(false).FirstOrDefault(x => x.Name == dto.Name && x.Deleted==0);
            if (duplicateNameCompany != null) throw Oops.Oh(ErrorCodes.z1004, dto.Name); //已存在同名的企业
            var duplicateNoCompany = _company.AsQueryable(false).FirstOrDefault(x => x.No == dto.No && x.Deleted == 0);
            if (duplicateNoCompany != null) throw Oops.Oh(ErrorCodes.z1004, dto.No);//已存在同编号的企业
            #endregion


            var newCompany=  _company.InsertNow(dto.Adapt<Company>());
          dto.Id = newCompany.Entity.Id;
          App.HttpContext.Request.Headers.Add("CompanyId", dto.Id);
            //自动新建管理员
          var user=  _userInfo.InsertNow(new User { Name = dto.Manager, UserType = (int)EnumUserType.Admin, Status =(int) EnumUserStatus.Enabled });
            _staff.Insert(new Staff { Name = dto.Manager, UserId = user.Entity.Id });
            _account.Insert(new Account { LoginName = dto.Manager, Password = MD5Encryption.Encrypt("111111"), AccountType = 0, UserId = user.Entity.Id });

            return dto;
        }

        public bool DeleteCompany(string companyId)
        {
            var company = _company.FirstOrDefault(x => x.Deleted == 0 && x.Id == companyId);
            if (company == null) return true;
            company.Deleted = 1;
            _company.Update(company);
            return true;
        }

        public List<CompanyDto> GetCompanyList()
        {
            var list = _company.Where(x => x.Deleted == 0).ToList();
            var res = list.Select(x =>
             {
                 var data = x.Adapt<CompanyDto>();
                 data.Id = x.Id;
                 return data;
                 
             }).ToList();
            return res;

        }

      

        public ReadOnlyCollection<TimeZoneInfo> GetTimeZoneList()
        {
           return TimeZoneInfo.GetSystemTimeZones();
           
        }

        public CompanyDto UpdateCompany(CompanyDto dto)
        {
            #region 校验企业重复性
            var duplicateNameCompany = _company.AsQueryable(false).FirstOrDefault(x => x.Name == dto.Name && x.Id!=dto.Id && x.Deleted == 0) ;
            if (duplicateNameCompany != null) throw Oops.Oh(ErrorCodes.z1004, dto.Name);//已存在同名的企业
            var duplicateNoCompany = _company.AsQueryable(false).FirstOrDefault(x => x.No == dto.No && x.Id != dto.Id && x.Deleted == 0);
            if (duplicateNoCompany != null) throw Oops.Oh(ErrorCodes.z1004, dto.No);//已存在同编号的企业
            #endregion

            var comapny = dto.Adapt<Company>();
            _company.Update(comapny,true);

            return dto;
        }

        public CompanyPrivilege UpdateCompanyPrivileges(CompanyPrivilege dto)
        {
            var preCompany = _company.FirstOrDefault(x => x.Id == dto.Id);
            if (string.IsNullOrEmpty(dto.Id) || preCompany == null)
                throw Oops.Oh(ErrorCodes.z1002, L.Text["Company"]);



            var alldbCodes = _companyPrivileges.AsQueryable(false).Where(x => x.EnterpriseId == dto.Id).ToList();
            #region 处理模块权限
            //处理传入模块权限数据（去重）
            var menuIds = dto.MenuIds.Distinct().ToList();
            //比较现有库中存储的角色所属的模块权限列表，获取需要删除的权限（库中有，但是传入中没有）
            var dbMenuIds = alldbCodes.Where(x => x.CodeType == (int)EnumPrivilegeCodeType.Menus).ToList();
            var deletedMenuIds = dbMenuIds.Where(x => !menuIds.Contains(x.PrivilegeCode)).ToList();
            _companyPrivileges.DeleteNow(deletedMenuIds);//物理删除
            //比较现有库中存储的角色所属的模块权限列表，获取需要新增的权限（库中无，但是传入有）
            var newMenuIds = menuIds.Where(x => !dbMenuIds.Select(t => t.PrivilegeCode).Contains(x)).Select(x => new CompanyApplicablePrivileges { EnterpriseId = dto.Id, PrivilegeCode = x, CodeType = (int)EnumPrivilegeCodeType.Menus });
            _companyPrivileges.Insert(newMenuIds);
            #endregion

            #region 处理功能点权限
            //处理传入功能点权限数据（去重）
            var entityIds = dto.EntityIds.Distinct().ToList();
            //比较现有库中存储的角色所属的功能点权限列表，获取需要删除的权限（库中有，但是传入中没有）
            var dbentityIds = alldbCodes.Where(x => x.CodeType == (int)EnumPrivilegeCodeType.Entities).ToList();
            var deletedentityIds = dbentityIds.Where(x => !entityIds.Contains(x.PrivilegeCode)).ToList();
            _companyPrivileges.DeleteNow(deletedentityIds);//物理删除
            //比较现有库中存储的角色所属的功能点权限列表，获取需要新增的权限（库中无，但是传入有）
            var newentityIds = entityIds.Where(x => !dbentityIds.Select(t => t.PrivilegeCode).Contains(x)).Select(x => new CompanyApplicablePrivileges { EnterpriseId = dto.Id, PrivilegeCode = x, CodeType = (int)EnumPrivilegeCodeType.Entities });
            _companyPrivileges.Insert(newentityIds);
            #endregion

            return dto;
        }

        /// <summary>
        /// 获取企业权限列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public CompanyPrivilege GetCompanyPrivileges(string companyId)
        {
            var company = _company.FirstOrDefault(x=>x.Id==companyId);
            var companyPrivileges = _companyPrivileges.Where(x => x.EnterpriseId == companyId);
            var menuIds = companyPrivileges.Where(x => x.CodeType == (int)EnumPrivilegeCodeType.Menus)?.Select(x => x.PrivilegeCode)?.ToList();
            var entityIds = companyPrivileges.Where(x => x.CodeType == (int)EnumPrivilegeCodeType.Entities)?.Select(x => x.PrivilegeCode)?.ToList();

            return new CompanyPrivilege { Id = companyId, Name=company?.Name, MenuIds=menuIds,EntityIds=entityIds };
        }
    }


}
