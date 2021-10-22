using Core.Infrastructure.Common;
using Core.Infrastructure.Extensions;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DataEncryption;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Furion.JsonSerialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using YizitApi.Application.BuinessLayer;
using YizitApi.Application.Dtos;
using YizitApi.Application.Dtos.Notificaiton;
using YizitApi.Application.Dtos.User;
using YizitApi.Application.Enum;
using YizitApi.Application.Vo;
using YizitApi.Application.Vo.Notification;

namespace YizitApi.Application
{
    /// <summary>
    /// 消息模块
    /// </summary>
    [ApiDescriptionSettings("Turbo@2")]
    public  class NotificationController : IDynamicApiController
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        #region 消息管理
        /// <summary>
        /// 新建通知 - 版本更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]
        public NtfReleaseResponse PostRelease([FromBody]NotficationDto<ReleaseNtf> dto)
        {
            // var releasestr = JSON.Serialize(dto.Data);
            // var releasedata = JSON.Deserialize<ReleaseNtf>(releasestr);
            dto.Type = EnumNotificationType.Release;
          var details=  dto.Data.Details.Select(x =>
            {
                if (!string.IsNullOrEmpty(x.ReleaseId))
                { x.ReleaseId = null; return x; }
                return x;
            }
         ).ToList();
            dto.Data.Details = details;
            //if (dto.TimePublish < DateTime.Now && dto.PublishType==EnumNotificationPublishType.Immediate) dto.TimePublish = DateTime.Now;
            return _notificationService.CreateRelease(dto);
        }
        /// <summary>
        /// 新建通知 - 维护通知
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]
        public NtfMaintenanceResponse PostMaintenance([FromBody] NotficationDto<MaintenanceNtf> dto)
        {
            dto.Type = EnumNotificationType.Maintenance;
           // if (dto.TimePublish < DateTime.Now && dto.PublishType == EnumNotificationPublishType.Immediate) dto.TimePublish = DateTime.Now;
            return _notificationService.CreateMaintenance(dto);
        }
        /// <summary>
        /// 新建通知 - 其他
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]
        public NtfOtherResponse PostOther([FromBody] NotficationDto<OtherNtf> dto)
        {
            dto.Type = EnumNotificationType.Other;
            //if (dto.TimePublish < DateTime.Now && dto.PublishType == EnumNotificationPublishType.Immediate) dto.TimePublish = DateTime.Now;
            return _notificationService.CreateOther(dto);
        }
        /// <summary>
        /// 编辑通知 -版本更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]
        public NtfReleaseResponse PutRelease([ApiSeat(ApiSeats.ActionEnd)] string id, [FromBody]NotficationDto<ReleaseNtf> dto)
        {
            dto.Type = EnumNotificationType.Release;
            return _notificationService.UpdateRelease(id,dto);
        }

        /// <summary>
        /// 编辑通知 - 维护通知
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]
        public NtfMaintenanceResponse PutMaintenance([ApiSeat(ApiSeats.ActionEnd)] string id, [FromBody] NotficationDto<MaintenanceNtf> dto)
        {
            dto.Type = EnumNotificationType.Maintenance;
            return _notificationService.UpdateMaintenance(id, dto);
        }
        /// <summary>
        /// 编辑通知 - 其他
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [UnitOfWork]
        public NtfOtherResponse PutOther([ApiSeat(ApiSeats.ActionEnd)] string id, [FromBody] NotficationDto<OtherNtf> dto)
        {
            dto.Type = EnumNotificationType.Other;
            return _notificationService.UpdateOther(id, dto);
        }

        /// <summary>
        /// 删除消息通知
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public bool Delete([ApiSeat(ApiSeats.ActionEnd)] string id)
        {
            return _notificationService.DeleteNotification(id);
        }
        /// <summary>
        /// 撤回消息通知
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public bool Undo([ApiSeat(ApiSeats.ActionEnd)] string id)
        {
            return _notificationService.UndoNotification(id);
        }
        /// <summary>
        /// 查看列表（分页查询） --版本更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        [HttpGet]
        public Pagination<NtfReleaseResponse> GetReleasePage([FromQuery]BasePagination dto)
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


            //if (!string.IsNullOrEmpty(dto.UserName)) //用户名（主表）
            //{
            //    queryFields.Add(new QueryField
            //    {
            //        Name = "Username",
            //        Value = dto.UserName,
            //        Operator = EnumQueryOperator.Contains
            //    });
            //}


            #endregion

            #region 表头过滤
            var headerFilters = dto.Filters.ConvertHeaderFilterConditions(new NtfReleaseResponse());
            #endregion

            #region 排序

            var sortField = dto.Sorter.ConvertSortField();

            #endregion

            QueryParameter queryParameter = new QueryParameter
            {
                PaginationInfo = paginationInfo,
                QueryFields = queryFields,
                HeaderFilters = headerFilters,
                SortField = sortField
            };

            return _notificationService.PaginationNtfRelease(queryParameter);
        }
        /// <summary>
        /// 查看列表（分页查询） --维护通知
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public Pagination<NtfMaintenanceResponse> GetMaintenancePage([FromQuery] BasePagination dto)
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


            //if (!string.IsNullOrEmpty(dto.UserName)) //用户名（主表）
            //{
            //    queryFields.Add(new QueryField
            //    {
            //        Name = "Username",
            //        Value = dto.UserName,
            //        Operator = EnumQueryOperator.Contains
            //    });
            //}


            #endregion

            #region 表头过滤
            var headerFilters = dto.Filters.ConvertHeaderFilterConditions(new NtfReleaseResponse());
            #endregion

            #region 排序

            var sortField = dto.Sorter.ConvertSortField();

            #endregion

            QueryParameter queryParameter = new QueryParameter
            {
                PaginationInfo = paginationInfo,
                QueryFields = queryFields,
                HeaderFilters = headerFilters,
                SortField = sortField
            };

            return _notificationService.PaginationNtfMaintenance(queryParameter);
        }
        /// <summary>
        /// 查看列表（分页查询） --其他
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public Pagination<NtfOtherResponse> GetOtherPage([FromQuery] BasePagination dto)
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


            //if (!string.IsNullOrEmpty(dto.UserName)) //用户名（主表）
            //{
            //    queryFields.Add(new QueryField
            //    {
            //        Name = "Username",
            //        Value = dto.UserName,
            //        Operator = EnumQueryOperator.Contains
            //    });
            //}


            #endregion

            #region 表头过滤
            var headerFilters = dto.Filters.ConvertHeaderFilterConditions(new NtfReleaseResponse());
            #endregion

            #region 排序

            var sortField = dto.Sorter.ConvertSortField();

            #endregion

            QueryParameter queryParameter = new QueryParameter
            {
                PaginationInfo = paginationInfo,
                QueryFields = queryFields,
                HeaderFilters = headerFilters,
                SortField = sortField
            };

            return _notificationService.PaginationNtfOther(queryParameter);
        }
        
        //todo...版本更新 （查询列表）【不分页】 

        #endregion

        #region 消息查看
        /// <summary>
        /// 获取未读通知数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
          return  _notificationService.GetUnReadCount();
        }
        /// <summary>
        /// 分页获取全部通知
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Pagination<NotificationCard> GetAllPage([FromQuery] BasePagination dto)
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


            //if (!string.IsNullOrEmpty(dto.UserName)) //用户名（主表）
            //{
            //    queryFields.Add(new QueryField
            //    {
            //        Name = "Username",
            //        Value = dto.UserName,
            //        Operator = EnumQueryOperator.Contains
            //    });
            //}


            #endregion

            #region 表头过滤
            var headerFilters = dto.Filters.ConvertHeaderFilterConditions(new NotificationCard());
            #endregion

            #region 排序

            var sortField = dto.Sorter.ConvertSortField();

            #endregion

            QueryParameter queryParameter = new QueryParameter
            {
                PaginationInfo = paginationInfo,
                QueryFields = queryFields,
                HeaderFilters = headerFilters,
                SortField = sortField
            };

            return _notificationService.PaginationMyAllNotifications(queryParameter);
        }
        /// <summary>
        /// 分页获取未读通知
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Pagination<NotificationCard> GetUnReadPage([FromQuery] BasePagination dto)
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


            //if (!string.IsNullOrEmpty(dto.UserName)) //用户名（主表）
            //{
            //    queryFields.Add(new QueryField
            //    {
            //        Name = "Username",
            //        Value = dto.UserName,
            //        Operator = EnumQueryOperator.Contains
            //    });
            //}


            #endregion

            #region 表头过滤
            var headerFilters = dto.Filters.ConvertHeaderFilterConditions(new NotificationCard());
            #endregion

            #region 排序

            var sortField = dto.Sorter.ConvertSortField();

            #endregion

            QueryParameter queryParameter = new QueryParameter
            {
                PaginationInfo = paginationInfo,
                QueryFields = queryFields,
                HeaderFilters = headerFilters,
                SortField = sortField
            };

            return _notificationService.PaginationMyUnReadNotifications(queryParameter);
        }

        /// <summary>
        /// 按类型获取分页通知
        /// </summary>
        /// <param name="type"> 0 其他；1 发布；2 维护</param>
        /// <param name="dto"></param>
        /// <returns></returns>

        public Pagination<NotificationCard> GetPage(EnumNotificationType type,[FromQuery] BasePagination dto)
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


            //if (!string.IsNullOrEmpty(dto.UserName)) //用户名（主表）
            //{
            //    queryFields.Add(new QueryField
            //    {
            //        Name = "Username",
            //        Value = dto.UserName,
            //        Operator = EnumQueryOperator.Contains
            //    });
            //}


            #endregion

            #region 表头过滤
            var headerFilters = dto.Filters.ConvertHeaderFilterConditions(new NotificationCard());
            #endregion

            #region 排序

            var sortField = dto.Sorter.ConvertSortField();

            #endregion

            QueryParameter queryParameter = new QueryParameter
            {
                PaginationInfo = paginationInfo,
                QueryFields = queryFields,
                HeaderFilters = headerFilters,
                SortField = sortField
            };

            return _notificationService.PaginationNotificationsByType(type,queryParameter);

           
        }


        /// <summary>
        /// 查询更新日志
        /// </summary>
        /// <returns></returns>
        public List<ReleaseChangeLog> GetRealeaseChangeLog()
        {
            return _notificationService.GetRealeaseChangeLog();
        }


        /// <summary>
        /// 设置消息已读（指定消息卡片）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool PutRead([FromBody] List<string> ids)
        {
            return _notificationService.MarkAsRead(ids);
        }
        /// <summary>
        /// 设置所有消息已读
        /// </summary>
        /// <returns></returns>
        public bool PutReadAll()
        {
            return _notificationService.MarkAsReadAll();
        }
        ///// <summary>
        ///// 标记为删除的消息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public bool DeleteNotification([ApiSeat(ApiSeats.ActionEnd)] string id)
        //{
        //    return _notificationService.MarkAsDeleted(id);
        //}

        ///// <summary>
        ///// 标记所有已读为删除
        ///// </summary>
        ///// <returns></returns>
        //public bool DeleteAllRead()
        //{
        //    return _notificationService.MarkAllReadAsDeleted();
        //}


        #endregion
    }
}
