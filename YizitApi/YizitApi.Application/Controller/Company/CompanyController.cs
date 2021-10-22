using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DataEncryption;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using YizitApi.Application.BuinessLayer;
using YizitApi.Application.Dtos;
using YizitApi.Application.Dtos.User;
using YizitApi.Application.Enum;
using YizitApi.Application.Vo;

namespace YizitApi.Application
{
    /// <summary>
    /// 企业模块
    /// </summary>
    [ApiDescriptionSettings("Turbo@2")]
    public  class CompanyController : IDynamicApiController
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        
        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <returns></returns>
        public List<CompanyDto> GetList()
        {

          return  _companyService.GetCompanyList();
        }

        /// <summary>
        /// 新建企业
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]
        public CompanyDto Post(CompanyDto dto)
        {
          return  _companyService.CreateCompany(dto);
        }

        /// <summary>
        /// 编辑企业基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]

        public CompanyDto Put([ApiSeat(ApiSeats.ActionEnd)] string id,[FromBody]CompanyDto dto)
        {
            dto.Id = id;
            return _companyService.UpdateCompany(dto);
        }


        /// <summary>
        /// 删除企业
        /// </summary>
        /// <param name="companyId">企业id</param>
        /// <returns></returns>
        [UnitOfWork]
        public bool Delete(string companyId)
        {
            return _companyService.DeleteCompany(companyId);
        }

        /// <summary>
        /// 获取时区列表
        /// </summary>
        /// <returns>返回时区列表信息</returns>
        public ReadOnlyCollection<TimeZoneInfo> GetTimeZones()
        {
            return _companyService.GetTimeZoneList();
        }

        /// <summary>
        /// 企业授权
        /// </summary>
        /// <param name="dto">企业授权基本信息以及授权权限列表</param>
        /// <returns></returns>
        [UnitOfWork]
        public CompanyPrivilege UpdatePrivileges(CompanyPrivilege dto)
        {
            return _companyService.UpdateCompanyPrivileges(dto);
        }

        /// <summary>
        /// 获取企业的已授权列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public CompanyPrivilege GetPrivileges(string companyId)
        {
            return _companyService.GetCompanyPrivileges(companyId);
        }

    }
}
