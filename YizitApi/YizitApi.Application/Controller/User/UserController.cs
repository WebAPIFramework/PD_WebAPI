using Core.Infrastructure.Common;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Utils;
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
using YizitApi.Core;

namespace YizitApi.Application
{
    /// <summary>
    /// 用户模块
    /// </summary>
    [ApiDescriptionSettings("Turbo@2")]
    public  class UserController : IDynamicApiController
    {
        private readonly IUserService _userService;
        private readonly IRepository<User,TubroDbContextLocator> _repository4User;
        public UserController(IUserService userService, IRepository<User, TubroDbContextLocator> repository4User)
        {
            _userService = userService;
            _repository4User = repository4User;
        }

        #region 根据token获取用户信息及权限
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public LoginUserInfoResponse GetInfo()
        {
            return _userService.GetUserInfo();
        }
        /// <summary>
        /// 获取用户的权限
        /// </summary>
        /// <returns></returns>
        public PrivilegeCode GetPrivilege()
        {
            return _userService.GetUserPrivilege();
        }

        /// <summary>
        /// 获取当前用户所属的企业的全部权限列表
        /// </summary>
        /// <returns></returns>
        public PrivilegeCode GetCompanyPrivileges()
        {
            return _userService.GetCompanyPrivilege();
        }
        /// <summary>
        /// 修改当前操作人的密码
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        public bool UpdatePassword(UserPassword dto)
        {
            return _userService.UpdateCurrentPassword(dto);
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        public bool Logout()
        {
            return _userService.Logout();
        }

        #endregion

        #region 用户管理

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]
        public QueryUserResponse Post([FromBody] UserDto dto)
        {
            return _userService.Insert(dto);
        }
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]
        public QueryUserResponse Put([ApiSeat(ApiSeats.ActionEnd)]string id,[FromBody] UserDto dto)
        {
          dto.Id = id;
          return  _userService.Update(dto);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public bool Delete([ApiSeat(ApiSeats.ActionEnd)] string id)
        {
            return _userService.Delete(id);
        }

        /// <summary>
        /// 启用/停用客户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="userStatus">0 停用；1 启用</param>
        /// <returns></returns>
        [UnitOfWork]
        public bool UpdateStatus(string id, EnumUserStatus userStatus)
        {
            
            return _userService.UpdateUserStatus(id, userStatus); ;
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public bool UpdateUserPassword(string id)
        {
            //重置密码
            return _userService.ResetPassword(id); ;
        }

        /// <summary>
        /// 授权用户角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]
        public bool UpdateRoles([FromBody] UserRolesDto dto)
        {
            return _userService.UpdateUsersRoles(dto);
        }

        /// <summary>
        /// 分页查询用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        
        [UnitOfWork]
        [HttpGet]
        //[Route("api/user/page")]
        public Pagination<QueryUserResponse> GetPage([FromQuery]UserPaginationDto dto)
        {
            #region 分页

            BasePagination paginationInfo = new BasePagination
            {
                Current = dto.Current == 0 ? 1 : dto.Current,
                PageSize = dto.PageSize == 0 ? 10 : dto.PageSize
            };

            #endregion

            #region 顶部查询

            var queryFields = new List<QueryField>();
           

            if (!string.IsNullOrEmpty(dto.UserName)) //用户名（主表）
            {
                queryFields.Add(new QueryField
                {
                    Name = "Username",
                    Value = dto.UserName,
                    Operator=EnumQueryOperator.Contains
                });
            }

            if (!string.IsNullOrEmpty(dto.Name)) //员工姓名（员工从表）
            {
                queryFields.Add(new QueryField
                {
                    Name = "Name",
                    Value = dto.Name,
                    Operator = EnumQueryOperator.Contains
                });
            }

            if (dto.Roles!=null && dto.Roles.Count>0) //角色查询（用户角色表）
            {
                queryFields.Add(new QueryField
                {
                    Name = "RoleIds",
                    Value = string.Join(",", dto.Roles) ,
                    Operator=EnumQueryOperator.Intersect
                });
            }


            if (dto.Available!=null) //按状态
            {
                queryFields.Add(new QueryField
                {
                    Name = "Available",
                    Value = ((int)dto.Available).ToString()
                });
            }

            #endregion

            #region 表头过滤
            var headerFilters = dto.Filters.ConvertHeaderFilterConditions(new QueryUserResponse());
            #endregion

            #region 排序

            var sortField = dto.Sorter.ConvertSortField();

            #endregion

            QueryParameter queryParameter = new QueryParameter
            {
                PaginationInfo=paginationInfo,
                QueryFields=queryFields,
                HeaderFilters=headerFilters,
                SortField= sortField
            };


            return _userService.PaginationUsers(queryParameter);
        }

        /// <summary>
        /// LDAP用户同步
        /// </summary>
        [UnitOfWork]
        public void PostSync()
        {
            LDAPUtil.Sync();
        }

        #endregion

        #region LDAP用户同步


        #endregion
    }
}
