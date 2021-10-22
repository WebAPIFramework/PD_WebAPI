using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DataEncryption;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using YizitApi.Application.BuinessLayer;
using YizitApi.Application.Dtos;
using YizitApi.Application.Dtos.User;
using YizitApi.Application.Enum;
using YizitApi.Application.Vo;

namespace YizitApi.Application
{
    /// <summary>
    /// 角色
    /// </summary>
    [ApiDescriptionSettings("Turbo@2")]
    public  class RoleController : IDynamicApiController
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public List<RoleResponse> GetList()
        {
          return  _roleService.GetRoleList();
        }
        /// <summary>
        /// 获取角色及权限信息
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>

        public RoleWithPrivilege GetPrivilege([ApiSeat(ApiSeats.ActionStart)] string roleId)
        {
            return _roleService.GetPrivilege(roleId);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]
        public RoleResponse Post(RoleWithPrivilege dto)
        {
            return _roleService.CreateRole(dto);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public bool Delete(string roleId)
        {
            return _roleService.DeleteRole(roleId);
        }


        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]
        public RoleResponse Put(string id,RoleWithPrivilege dto)
        {
            dto.Id = id;
            return _roleService.UpdateRole(dto);
        }


    }
}
