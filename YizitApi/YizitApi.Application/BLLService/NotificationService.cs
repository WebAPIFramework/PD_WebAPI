using Core.Infrastructure.CacheService;
using Core.Infrastructure.Common;
using Core.Infrastructure.ErrorCodes;
using Core.Infrastructure.Extensions;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.Localization;
using Furion.TaskScheduler;
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
using YizitApi.Application.Dtos.Notificaiton;
using YizitApi.Application.Dtos.User;
using YizitApi.Application.Enum;
using YizitApi.Application.Vo;
using YizitApi.Application.Vo.Notification;
using YizitApi.Application.Worker;
using YizitApi.Core;
using Role = YizitApi.Core.Role;

namespace YizitApi.Application.BuinessLayer
{
    public interface INotificationService
    {
        NtfReleaseResponse CreateRelease(NotficationDto<ReleaseNtf> dto);
        NtfMaintenanceResponse CreateMaintenance(NotficationDto<MaintenanceNtf> dto);
        NtfOtherResponse CreateOther(NotficationDto<OtherNtf> dto);
        NtfReleaseResponse UpdateRelease(string id, NotficationDto<ReleaseNtf> dto);
        NtfMaintenanceResponse UpdateMaintenance(string id, NotficationDto<MaintenanceNtf> dto);
        NtfOtherResponse UpdateOther(string id, NotficationDto<OtherNtf> dto);
        bool DeleteNotification(string id);
        bool UndoNotification(string id);
        Pagination<NtfReleaseResponse> PaginationNtfRelease(QueryParameter queryParameter);
        Pagination<NtfMaintenanceResponse> PaginationNtfMaintenance(QueryParameter queryParameter);
        Pagination<NtfOtherResponse> PaginationNtfOther(QueryParameter queryParameter);
        int GetUnReadCount();
        Pagination<NotificationCard> PaginationMyAllNotifications(QueryParameter queryParameter);
        Pagination<NotificationCard> PaginationMyUnReadNotifications(QueryParameter queryParameter);
        bool MarkAsRead(List<string> ids);
        bool MarkAsReadAll();
        bool MarkAsDeleted(string id);
        bool MarkAllReadAsDeleted();
        List<ReleaseChangeLog> GetRealeaseChangeLog();
        Pagination<NotificationCard> PaginationNotificationsByType(EnumNotificationType type, QueryParameter queryParameter);
    }

    public class NotificationService : ITransient, INotificationService
    {
        #region services
        private readonly IRepository<NotficationScope, TubroDbContextLocator> _notificationScope;
        private readonly IRepository<Notification_Base, TubroDbContextLocator> _notification_Base;
        private readonly IRepository<NT_Maintenance, TubroDbContextLocator> _nt_Maintenance;
        private readonly IRepository<NT_Other, TubroDbContextLocator> _nt_Other;
        private readonly IRepository<NT_Release, TubroDbContextLocator> _nt_Release;
        private readonly IRepository<NT_Release_Detail, TubroDbContextLocator> _nt_Release_Detail;
        private readonly IRepository<Staff, TubroDbContextLocator> _staff;
        private readonly IRepository<User_Preference_Notification, TubroDbContextLocator> _userPreferenceNotificaton;
        private readonly ICacheService _cacheService;
        public NotificationService(
            IRepository<NotficationScope, TubroDbContextLocator> notificationScope,
            IRepository<Notification_Base, TubroDbContextLocator> notification_Base,
            IRepository<NT_Maintenance, TubroDbContextLocator> nt_Maintenance,
            IRepository<NT_Other, TubroDbContextLocator> nt_Other,
            IRepository<NT_Release, TubroDbContextLocator> nt_Release,
            IRepository<NT_Release_Detail, TubroDbContextLocator> nt_Release_Detail,
            IRepository<User_Preference_Notification, TubroDbContextLocator> userPreferenceNotificaton,
            IRepository<Staff, TubroDbContextLocator> staff,
             ICacheService cacheService
            )
        {
            _notificationScope = notificationScope;
            _notification_Base = notification_Base;
            _nt_Maintenance = nt_Maintenance;
            _nt_Other = nt_Other;
            _nt_Release = nt_Release;
            _nt_Release_Detail = nt_Release_Detail;
            _userPreferenceNotificaton = userPreferenceNotificaton;
            _staff = staff;
            _cacheService = cacheService;
        }

        #endregion

        #region 新建

        public NtfMaintenanceResponse CreateMaintenance(NotficationDto<MaintenanceNtf> dto)
        {
            #region 生成通知基本信息以及通知内容
            //生成通知基本信息
            var ntBase = dto.Adapt<Notification_Base>();

            //生成通知内容
            var ntContent = dto.Data.Adapt<NT_Maintenance>();
            ntContent.Notification_Id = ntBase.ID;
            _notification_Base.InsertNow(ntBase);
            _nt_Maintenance.InsertNow(ntContent);
            #endregion

            //通知（定时任务，缓存，即时通知）
            #region 定时任务，缓存，即时通知

            //创建定时任务
            if (dto.PublishType==EnumNotificationPublishType.Timed && dto.TimePublish>DateTime.Now)
            {
                //实现创建定时任务和在触发定时任务中即时通知客户端有新消息,以及更新缓存
                NotificationWorkerFactory.CreateNotifcationWorker(ntBase);
            }
            else//即时通知
            {
                //即时通知（更新通知缓存 + 即时通知客户端）
                NotificationWorkerFactory.Notifcation(ntBase);
            }

            #endregion

            //返回
            var res = ntBase.Adapt<NtfMaintenanceResponse>();
            res.MaintenanceTime = ntContent.Time;
            res.Content = ntContent.Content;
            return res;
        }

        public NtfOtherResponse CreateOther(NotficationDto<OtherNtf> dto)
        {
            #region 生成通知基本信息以及通知内容
            //生成通知基本信息
            var ntBase = dto.Adapt<Notification_Base>();

            //生成通知内容
            var ntContent = dto.Data.Adapt<NT_Other>();
            ntContent.Notification_Id = ntBase.ID;
            _notification_Base.InsertNow(ntBase);
            _nt_Other.InsertNow(ntContent);
            #endregion

            //通知（定时任务，缓存，即时通知）
            #region 定时任务，缓存，即时通知

            //创建定时任务
            if (dto.PublishType == EnumNotificationPublishType.Timed && dto.TimePublish > DateTime.Now)
            {
                //实现创建定时任务和在触发定时任务中即时通知客户端有新消息,以及更新缓存
                NotificationWorkerFactory.CreateNotifcationWorker(ntBase);
            }
            else//即时通知
            {
                //即时通知（更新通知缓存 + 即时通知客户端）
                NotificationWorkerFactory.Notifcation(ntBase);
            }

            #endregion


            //返回
            var res = ntBase.Adapt<NtfOtherResponse>();
            res.Title = ntContent.Title;
            res.Content = ntContent.Content;
            return res; 
          
        }

        public NtfReleaseResponse CreateRelease(NotficationDto<ReleaseNtf> dto)
        {
            #region 生成通知基本信息以及通知内容
            //生成通知基本信息
            var ntBase = dto.Adapt<Notification_Base>();

            //生成通知内容
            var ntContent = dto.Data.Adapt<NT_Release>();
            ntContent.Notification_Id = ntBase.ID;

            var details = dto.Data.Details.Select(x => 
            { 
                var t = x.Adapt<NT_Release_Detail>();
                t.Notification_Id = ntBase.ID;
                t.Release_Id = ntContent.ID;
                return t;
            } ).ToList(); 

            _notification_Base.Insert(ntBase);
            _nt_Release.Insert(ntContent);
            _nt_Release_Detail.Insert(details);
            #endregion

            //通知（定时任务，缓存，即时通知）
            #region 定时任务，缓存，即时通知

            //创建定时任务
            if (dto.PublishType == EnumNotificationPublishType.Timed && dto.TimePublish > DateTime.Now)
            {
                //实现创建定时任务和在触发定时任务中即时通知客户端有新消息,以及更新缓存
                NotificationWorkerFactory.CreateNotifcationWorker(ntBase);
            }
            else//即时通知
            {
                //即时通知（更新通知缓存 + 即时通知客户端）
                NotificationWorkerFactory.Notifcation(ntBase);
            }

            #endregion


            //返回
            var res = ntBase.Adapt<NtfReleaseResponse>();
            res.ReleaseTime = ntContent.Time;
            res.Version = ntContent.Version_No;
            res.VersionName = ntContent.Version_Name;
            res.Details = details;
            return res;
        }
        #endregion

        #region 编辑

        public NtfMaintenanceResponse UpdateMaintenance(string id, NotficationDto<MaintenanceNtf> dto)
        {
           var currentBase= _notification_Base.AsQueryable(false).FirstOrDefault(x => x.ID == id);
            if (currentBase == null) throw Oops.Oh(ErrorCodes.z1002, L.Text["Notification"]);

            //todo...校验 如果该状态已经发送或者撤回，则不允许编辑
            CheckStatus4Update(currentBase);

            #region 更新通知基本信息以及通知内容
            //生成通知基本信息
            var ntBase = dto.Adapt<Notification_Base>();

            ntBase.ID = id;
            _notification_Base.Update(ntBase);

            //生成通知内容
            var currentContent = _nt_Maintenance.AsQueryable(false).FirstOrDefault(x => x.Notification_Id == id);
            var ntContent = dto.Data.Adapt<NT_Maintenance>();
            ntContent.Notification_Id = ntBase.ID;
            ntContent.ID = currentContent.ID;


            _nt_Maintenance.Update(ntContent);
            #endregion

            //通知（定时任务，缓存，即时通知）
            #region 定时任务，缓存，即时通知

            //创建定时任务
            if (dto.PublishType == EnumNotificationPublishType.Timed && dto.TimePublish > DateTime.Now)
            {
                //实现创建定时任务和在触发定时任务中即时通知客户端有新消息,以及更新缓存
                NotificationWorkerFactory.CreateNotifcationWorker(ntBase);
            }
            else//即时通知
            {
                //即时通知（更新通知缓存 + 即时通知客户端）
                NotificationWorkerFactory.Notifcation(ntBase);
            }

            #endregion


            //返回
            var res = ntBase.Adapt<NtfMaintenanceResponse>();
            res.MaintenanceTime = ntContent.Time;
            res.Content = ntContent.Content;
            return res;
        }

        public NtfOtherResponse UpdateOther(string id, NotficationDto<OtherNtf> dto)
        {
            var currentBase = _notification_Base.AsQueryable(false).FirstOrDefault(x => x.ID == id);
            if (currentBase == null) throw Oops.Oh(ErrorCodes.z1002, L.Text["Notification"]);

            //todo...校验 如果该状态已经发送或者撤回，则不允许编辑
            CheckStatus4Update(currentBase);

            #region 生成通知基本信息以及通知内容
            //生成通知基本信息
            var ntBase = dto.Adapt<Notification_Base>();
            ntBase.ID = id;
            _notification_Base.Update(ntBase);

            //生成通知内容
            var currentContent = _nt_Other.AsQueryable(false).FirstOrDefault(x => x.Notification_Id == id);
            var ntContent = dto.Data.Adapt<NT_Other>();
            ntContent.Notification_Id = ntBase.ID;
            ntContent.ID = currentContent.ID;
            _nt_Other.Update(ntContent);
            #endregion

            //通知（定时任务，缓存，即时通知）
            #region 定时任务，缓存，即时通知

            //创建定时任务
            if (dto.PublishType == EnumNotificationPublishType.Timed && dto.TimePublish > DateTime.Now)
            {
                //实现创建定时任务和在触发定时任务中即时通知客户端有新消息,以及更新缓存
                NotificationWorkerFactory.CreateNotifcationWorker(ntBase);
            }
            else//即时通知
            {
                //即时通知（更新通知缓存 + 即时通知客户端）
                NotificationWorkerFactory.Notifcation(ntBase);
            }

            #endregion


            //返回
            var res = ntBase.Adapt<NtfOtherResponse>();
            res.Title = ntContent.Title;
            res.Content = ntContent.Content;
            return res;
        }

        public NtfReleaseResponse UpdateRelease(string id, NotficationDto<ReleaseNtf> dto)
        {
            var currentBase = _notification_Base.AsQueryable(false).FirstOrDefault(x => x.ID == id);
            if (currentBase == null) throw Oops.Oh(ErrorCodes.z1002, L.Text["Notification"]);

            //todo...校验 如果该状态已经发送或者撤回，则不允许编辑
            CheckStatus4Update(currentBase);
            #region 生成通知基本信息以及通知内容
            //生成通知基本信息
            var ntBase = dto.Adapt<Notification_Base>();
            ntBase.ID = id;
            _notification_Base.Update(ntBase);

            //生成通知内容
            var currentContent = _nt_Release.AsQueryable(false).FirstOrDefault(x => x.Notification_Id == id);
            var ntContent = dto.Data.Adapt<NT_Release>();
            ntContent.Notification_Id = ntBase.ID;
            ntContent.ID = currentContent.ID;
            _nt_Release.Update(ntContent);

            var details = dto.Data.Details.Select(x =>
            {
                var t = x.Adapt<NT_Release_Detail>();
                t.Notification_Id = ntBase.ID;
                t.Release_Id = ntContent.ID;
                return t;
            }).ToList();

            var currentDetails = _nt_Release_Detail.AsQueryable(false).Where(x => x.Release_Id == ntContent.ID && x.Notification_Id == id).ToList();

            //比较传入列表和现有列表，获取新增，更新，删除列表
            var addList = details.Where(x => !currentDetails.Select(t => t.ID).Contains(x.ID)).ToList();
            var updateList= details.Where(x => currentDetails.Select(t => t.ID).Contains(x.ID)).ToList();
            var delList = currentDetails.Where(x => !details.Select(t => t.ID).Contains(x.ID)).ToList();

            _nt_Release_Detail.Insert(addList);
            _nt_Release_Detail.Update(updateList);
            _nt_Release_Detail.Delete(delList);

            #endregion

            //通知（定时任务，缓存，即时通知）
            #region 定时任务，缓存，即时通知

            //创建定时任务
            if (dto.PublishType == EnumNotificationPublishType.Timed && dto.TimePublish > DateTime.Now)
            {
                //实现创建定时任务和在触发定时任务中即时通知客户端有新消息,以及更新缓存
                NotificationWorkerFactory.CreateNotifcationWorker(ntBase);
            }
            else//即时通知
            {
                //即时通知（更新通知缓存 + 即时通知客户端）
                NotificationWorkerFactory.Notifcation(ntBase);
            }

            #endregion


            //返回
            var res = ntBase.Adapt<NtfReleaseResponse>();
            res.ReleaseTime = ntContent.Time;
            res.Version = ntContent.Version_No;
            res.VersionName = ntContent.Version_Name;
            res.Details = details;
            return res;
        }

        private void CheckStatus4Update(Notification_Base baseInfo)
        {
            if(baseInfo.Status==0)//如果通知已发送了，则不允许编辑
            {
                throw Oops.Oh(ErrorCodes.z1006,L.Text["Sent"], L.Text["Notification"]);
            }
        }
        #endregion

        #region 删除与撤回
        public bool DeleteNotification(string id)
        {
            var ntBase = _notification_Base.FirstOrDefault(x => x.ID == id);
            if (ntBase == null) throw Oops.Oh(ErrorCodes.z1002, L.Text["Notification"]);
            ntBase.deleted = 1;
            ntBase.deleted_by= App.User?.FindFirst("UserId")?.Value;
            ntBase.deleted_time= DateTimeOffset.Now.ToUnixTimeMilliseconds();
            _notification_Base.Update(ntBase);
           
            //移除定时任务
            if (ntBase.PublishType == (int)EnumNotificationPublishType.Timed && ntBase.PublishTime > DateTime.Now)
            {
                //移除定时任务
                NotificationWorkerFactory.RemoveNotifcationWorker(ntBase);
            }

            //todo...如果当前通知状态已经是已发送，是否需要移除缓存？？


            return true;

        }

        public bool UndoNotification(string id)
        {
            var ntBase = _notification_Base.FirstOrDefault(x => x.ID == id);
            if (ntBase == null) throw Oops.Oh(ErrorCodes.z1002, L.Text["Notification"]);
            ntBase.Status = 1;
            _notification_Base.Update(ntBase);

            //移除定时任务
            if (ntBase.PublishType == (int)EnumNotificationPublishType.Timed && ntBase.PublishTime > DateTime.Now)
            {
                //移除定时任务
                NotificationWorkerFactory.RemoveNotifcationWorker(ntBase);
            }

            //todo...如果当前通知状态已经是已发送，是否需要移除缓存？？

            return true;
        }


        #endregion

        #region 分页查询
        /// <summary>
        /// 分页查询-版本更新
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        public Pagination<NtfReleaseResponse> PaginationNtfRelease(QueryParameter queryParameter)
        {
            //获取全部列表
            var result = (from baseInfo in _notification_Base.AsQueryable(false).Where(x => x.deleted == 0 && x.NotficationType == (int)EnumNotificationType.Release)
                          join content in _nt_Release.AsQueryable(false) on baseInfo.ID equals content.Notification_Id
                          select new NtfReleaseResponse
                          {
                              Id=baseInfo.ID,
                              Time=baseInfo.PublishTime,
                              Type=(EnumNotificationType)baseInfo.NotficationType,
                              Sent= baseInfo.Status == -1 ? false : true,
                              ReleaseTime = content.Time,
                              Version=content.Version_No,
                              VersionName=content.Version_Name,
                              //Details=
                              Content=content.Content
                          }).ToList();
            //分页
            result= result.AsQueryable().Where(queryParameter.QueryFields).OrderBy(queryParameter.SortField).ToList();
            var resData = result.AsQueryable().ToPagination(queryParameter.PaginationInfo.Current, queryParameter.PaginationInfo.PageSize);

            return resData;
        }
        /// <summary>
        /// 分页查询-维护通知
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        public Pagination<NtfMaintenanceResponse> PaginationNtfMaintenance(QueryParameter queryParameter)
        {
            //获取全部列表
            var result = (from baseInfo in _notification_Base.AsQueryable(false).Where(x => x.deleted == 0 && x.NotficationType == (int)EnumNotificationType.Maintenance)
                          join content in _nt_Maintenance.AsQueryable(false) on baseInfo.ID equals content.Notification_Id
                          select new NtfMaintenanceResponse
                          {
                              Id = baseInfo.ID,
                              Time = baseInfo.PublishTime,
                              Type = (EnumNotificationType)baseInfo.NotficationType,
                              Sent = baseInfo.Status == -1 ? false : true,
                              MaintenanceTime =content.Time,
                              Content = content.Content
                          }).ToList();
            //分页
            result = result.AsQueryable().Where(queryParameter.QueryFields).OrderBy(queryParameter.SortField).ToList();
            var resData = result.AsQueryable().ToPagination(queryParameter.PaginationInfo.Current, queryParameter.PaginationInfo.PageSize);

            return resData;
        }
        /// <summary>
        /// 分页查询-其他通知
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        public Pagination<NtfOtherResponse> PaginationNtfOther(QueryParameter queryParameter)
        {
            //获取全部列表
            var result = (from baseInfo in _notification_Base.AsQueryable(false).Where(x => x.deleted == 0 && x.NotficationType == (int)EnumNotificationType.Other)
                          join content in _nt_Other.AsQueryable(false) on baseInfo.ID equals content.Notification_Id
                          select new NtfOtherResponse
                          {
                              Id = baseInfo.ID,
                              Time = baseInfo.PublishTime,
                              Type = (EnumNotificationType)baseInfo.NotficationType,
                              Sent = baseInfo.Status == -1 ? false : true,
                              Title = content.Title,
                              Content = content.Content
                          }).ToList();
            //分页
            result = result.AsQueryable().Where(queryParameter.QueryFields).OrderBy(queryParameter.SortField).ToList();
            var resData = result.AsQueryable().ToPagination(queryParameter.PaginationInfo.Current, queryParameter.PaginationInfo.PageSize);

            return resData;
        }


        #endregion

        #region 客户端消息查看与处理

        /// <summary>
        /// 获取未读数量
        /// </summary>
        /// <returns></returns>
        public int GetUnReadCount()
        {
            //【从缓存】获取全部
            var all = _cacheService.GetSets("_allNotifcations");
            //【从缓存】获取已读列表
            var userId = App.User.FindFirst("UserId")?.Value;
            var readList= _cacheService.GetSets(userId);
            //TODO...如果后续有用户自定义不要看的通知，总数中还需排除这些，再计算未读数量

            return all.Count() - readList.Count();

        }

        /// <summary>
        /// 分页获取全部通知
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        public Pagination<NotificationCard> PaginationMyAllNotifications(QueryParameter queryParameter)
        {
            var userId = App.User.FindFirst("UserId")?.Value;

            //从通知表中联合用户已读/已删通知表中，获取到总的
            //联合户已读/已删通知表中中 获取是否已读
            var result = from baseInfo in _notification_Base.AsQueryable(false).Where(x => x.deleted == 0 && x.Status == 0).OrderByDescending(x=>x.PublishTime)
                             //左联
                         join userPreference in _userPreferenceNotificaton.AsQueryable(false).Where(x => x.User_Id == userId && x.deleted == 0)
                          on baseInfo.ID equals userPreference.Notification_Id
                          into gu
                          from perUP in gu.DefaultIfEmpty()

                          join manintenance in _nt_Maintenance.AsQueryable(false) on baseInfo.ID equals manintenance.Notification_Id
                          into gm 
                          from perMain in gm.DefaultIfEmpty()

                          join other in _nt_Other.AsQueryable(false) on baseInfo.ID equals other.Notification_Id
                          into go
                          from perOth in go.DefaultIfEmpty()

                          join release in _nt_Release.AsQueryable(false) on baseInfo.ID equals release.Notification_Id
                          into gr 
                          from perRel in gr.DefaultIfEmpty()

                          join staff in _staff.AsQueryable(false) on baseInfo.creator equals staff.UserId
                          into gs
                          from perSta in gs.DefaultIfEmpty()
                          select new NotificationCard
                          {
                              Id = baseInfo.ID,
                              Title = baseInfo.NotficationType==(int)EnumNotificationType.Maintenance? perMain.Title:(baseInfo.NotficationType == (int)EnumNotificationType.Other? perOth.Title: perRel.Version_No+ perRel.Version_Name),  //GetCardTitle(baseInfo, manintenance, other, release),
                              Time =baseInfo.PublishTime.ToString("yyyy-MM-dd HH:mm:ss"),
                              Type=(EnumNotificationType)baseInfo.NotficationType,
                              IsRead= perUP != null?true:false,
                              Creator= perSta.Name,
                              CreaterAvatar=string.Empty
                          };
            
            //调用分页方法
            var resData =result.ToPagination(queryParameter.PaginationInfo.Current, queryParameter.PaginationInfo.PageSize);
            return resData;
            
        }

       
        /// <summary>
        /// 分页获取未读的通知
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        public Pagination<NotificationCard> PaginationMyUnReadNotifications(QueryParameter queryParameter)
        {
            //从通知表中联合用户已读/已删通知表中，获取到未读的通知
            var userId = App.User.FindFirst("UserId")?.Value;
            
            //已读列表(或从缓存中获取)
            var readList = _userPreferenceNotificaton.AsQueryable(false).Where(x => x.User_Id == userId && x.deleted == 0).Select(x => x.Notification_Id).Distinct().ToList();

            var result = from baseInfo in _notification_Base.AsQueryable(false).Where(x => x.deleted == 0 && x.Status == 0 && !readList.Contains(x.ID) ).OrderByDescending(x => x.PublishTime)
                           
                         join manintenance in _nt_Maintenance.AsQueryable(false) on baseInfo.ID equals manintenance.Notification_Id
                         into gm
                         from perMain in gm.DefaultIfEmpty()

                         join other in _nt_Other.AsQueryable(false) on baseInfo.ID equals other.Notification_Id
                         into go
                         from perOth in go.DefaultIfEmpty()

                         join release in _nt_Release.AsQueryable(false) on baseInfo.ID equals release.Notification_Id
                         into gr
                         from perRel in gr.DefaultIfEmpty()

                         join staff in _staff.AsQueryable(false) on baseInfo.creator equals staff.UserId
                         into gs
                         from perSta in gs.DefaultIfEmpty()
                         select new NotificationCard
                         {
                             Id = baseInfo.ID,
                             Title = baseInfo.NotficationType == (int)EnumNotificationType.Maintenance ? perMain.Title : (baseInfo.NotficationType == (int)EnumNotificationType.Other ? perOth.Title : perRel.Version_No + perRel.Version_Name),  //GetCardTitle(baseInfo, manintenance, other, release),
                             Time = baseInfo.PublishTime.ToString("yyyy-MM-dd HH:mm:ss"),
                             Type = (EnumNotificationType)baseInfo.NotficationType,
                             IsRead =  false,
                             Creator = perSta.Name,
                             CreaterAvatar = string.Empty
                         };

            //调用分页方法
            var resData = result.ToPagination(queryParameter.PaginationInfo.Current, queryParameter.PaginationInfo.PageSize);
            return resData;
        }

        /// <summary>
        /// 按类型分页查询
        /// </summary>
        /// <param name="type"></param>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        public Pagination<NotificationCard> PaginationNotificationsByType(EnumNotificationType type, QueryParameter queryParameter)
        {
            //从通知表中联合用户已读/已删通知表中，获取到未读的通知
            var userId = App.User.FindFirst("UserId")?.Value;

            var result = from baseInfo in _notification_Base.AsQueryable(false).Where(x => x.deleted == 0 && x.Status == 0 && x.NotficationType== (int)type).OrderByDescending(x => x.PublishTime)
                             //左联
                         join userPreference in _userPreferenceNotificaton.AsQueryable(false).Where(x => x.User_Id == userId && x.deleted == 0)
                          on baseInfo.ID equals userPreference.Notification_Id
                          into gu
                         from perUP in gu.DefaultIfEmpty()

                         join release in _nt_Release.AsQueryable(false) on baseInfo.ID equals release.Notification_Id
                         into gr
                         from perRel in gr.DefaultIfEmpty()

                         join staff in _staff.AsQueryable(false) on baseInfo.creator equals staff.UserId
                         into gs
                         from perSta in gs.DefaultIfEmpty()

                           

                         select new NotificationCard
                         {
                             Id = baseInfo.ID,
                             Title = perRel.Version_No + perRel.Version_Name,
                             Time = baseInfo.PublishTime.ToString("yyyy-MM-dd HH:mm:ss"),
                             Type = (EnumNotificationType)baseInfo.NotficationType,
                             IsRead = perUP != null ? true : false,
                             Creator = perSta.Name,
                             CreaterAvatar = string.Empty
                         };

            //调用分页方法
            var resData = result.ToPagination(queryParameter.PaginationInfo.Current, queryParameter.PaginationInfo.PageSize);
            return resData;
        }

        /// <summary>
        /// 标记为已读
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool MarkAsRead(List<string> ids)
        {
            var userId = App.User.FindFirst("UserId")?.Value;
            foreach (var id in ids)
            {
                //缓存更新
                _cacheService.Push<string>(userId, id);

                //持久化
                //用户已读/已删通知表中,新增已读记录（是否需判断存在性？）
                if(_userPreferenceNotificaton.AsQueryable(false).FirstOrDefault(x=>x.User_Id== userId && x.Notification_Id==id)==null)
                _userPreferenceNotificaton.Insert(new User_Preference_Notification { User_Id=userId,Notification_Id=id,deleted=0});
            }

            return true;
                 
        }
        /// <summary>
        /// 标记全部为已读
        /// </summary>
        /// <returns></returns>
        public bool MarkAsReadAll()
        {
            var userId = App.User.FindFirst("UserId")?.Value;
            var allNotifications = _cacheService.GetSets("_allNotifcations");
            foreach (var id in allNotifications)
            {
                //缓存更新
                _cacheService.Push<string>(userId, id);

                //持久化
                //用户已读/已删通知表中,新增已读记录（是否需判断存在性？）
                if (_userPreferenceNotificaton.AsQueryable(false).FirstOrDefault(x => x.User_Id == userId && x.Notification_Id == id) == null)
                    _userPreferenceNotificaton.Insert(new User_Preference_Notification { User_Id = userId, Notification_Id = id, deleted = 0 });
            }

            return true;
        }
        /// <summary>
        /// 标记为删除的
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool MarkAsDeleted(string id)
        {
            throw new NotImplementedException();
            //用户已读/已删通知表中,新增已删记录（是否需判断存在性？）
        }

        /// <summary>
        /// 标记所有已读为已删
        /// </summary>
        /// <returns></returns>
        public bool MarkAllReadAsDeleted()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 版本更新日志
        /// </summary>
        /// <returns></returns>
        public List<ReleaseChangeLog> GetRealeaseChangeLog()
        {
            //获取所有已发送的类型时版本更新的通知
            var releases = _notification_Base.AsQueryable(false).Where(x => x.deleted == 0 && x.Status == 0 && x.NotficationType == (int)EnumNotificationType.Release).OrderByDescending(t=>t.creation_time);

            //组织返回
            var res = (from release in releases
                      join version in _nt_Release.AsQueryable(false) on release.ID equals version.Notification_Id
                      select new ReleaseChangeLog
                      {
                          Id = release.ID,
                          VersionNo = version.Version_No,
                          VersionName = version.Version_Name,
                          Time = version.Time.ToString("yyyy-MM-dd HH-mm-ss"),
                          Detail=(from changelogDetail in _nt_Release_Detail.AsQueryable(false).Where(t => t.Notification_Id == release.ID && t.Release_Id == version.ID)
                                  select new ReleaseChangeLogDetail 
                                  {
                                      Type=(EnumReleaseType)changelogDetail.Type,
                                      Content=changelogDetail.Content
                                  }).ToList()


                      }).ToList();
          
            return res;
        }

        

        #endregion

    }


}
