using Core.Infrastructure.CacheService;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Api.Application.Enum;
using Api.Application.Hubs;
using Api.Core;
using Api.Application.Dtos.Notificaiton;

namespace Api.Application.Worker
{
    /// <summary>
    /// 定时任务消息通知工厂
    /// </summary>
  public static  class NotificationWorkerFactory
    {

        public static void InitLoadNotificationWorkers()
        {
            Scoped.Create((_, scope) =>
            {
                var services = scope.ServiceProvider;

                // 解析服务
                var _notification_Base = App.GetService< IRepository < Notification_Base, TubroDbContextLocator>  > (services);

                //获取【发布时间未到】的定时发布的消息通知（未发布的）---需要创建定时任务跟踪
                var tobeNotificationWorkers = _notification_Base.AsQueryable(false).Where(x => x.deleted == 0 && x.Status == -1 && x.PublishType == (int)EnumNotificationPublishType.Timed && x.PublishTime > DateTime.Now).ToList();
                foreach (var item in tobeNotificationWorkers)
                {
                    CreateNotifcationWorker(item);
                }
                //获取定时发布的消息通知（发布时间已到，但是未发布的） --需要即时发布
                var tobeImmediateNotification = _notification_Base.AsQueryable(false).Where(x => x.deleted == 0 && x.Status == -1 && x.PublishType == (int)EnumNotificationPublishType.Timed && x.PublishTime <= DateTime.Now).ToList();
                foreach (var item in tobeImmediateNotification)
                {
                    Notifcation(item);
                }
            });
        }
        /// <summary>
        /// 创建或者更新定时消息相关的 定时任务（并在定时任务内更新缓存以及即时通知客户端）
        /// </summary>
        /// <param name="notification"></param>
        public static void CreateNotifcationWorker(Notification_Base notification)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                var worker = SpareTime.GetWorker(notification.ID);
                if (worker != null)//已存在一样的任务
                    SpareTime.Cancel(notification.ID);

                var interval = (notification.PublishTime - DateTime.Now).TotalMilliseconds;
                SpareTime.DoOnce(interval, (timer, count) =>
                {
                    Scoped.CreateUow((_, scope) =>
                    {
                        var services = scope.ServiceProvider;

                        // 解析缓存服务
                        var _cacheService = App.GetService<ICacheService>(services);
                        var client = App.GetService<ISysNotificationHub>(services);
                        var _notification_Base = App.GetService<IRepository<Notification_Base, TubroDbContextLocator>>(services);

                        //step1: 更新缓存（缓存中保存的所有通知）
                        _cacheService.Push<string>("_allNotifcations", notification.ID);

                        //step2: 通知客户端 （todo...后续按传入）
                        client.HandleNotificationChange(new NotificationModel());

                        //step3: 更新状态
                        notification.Status = 0;//已发送
                        _notification_Base.Update(notification);
                    });

                }, $"{notification.ID}");
            });
            
        }

        /// <summary>
        /// 移除定时消息相关的定时任务
        /// </summary>
        /// <param name="notification"></param>
        public static void RemoveNotifcationWorker(Notification_Base notification)
        {
            var worker = SpareTime.GetWorker(notification.ID);
            if (worker != null)//已存在一样的任务
                SpareTime.Cancel(notification.ID);
        }

        /// <summary>
        /// 即时通知客户端以及更新消息所有缓存
        /// </summary>
        /// <param name="notification"></param>
        public static void Notifcation(Notification_Base notification)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                Scoped.CreateUow((_, scope) =>
                {
                    var services = scope.ServiceProvider;

                    // 解析缓存服务
                    var _cacheService = App.GetService<ICacheService>(services);
                    var client = App.GetService<ISysNotificationHub>(services);
                    var _notification_Base = App.GetService<IRepository<Notification_Base, TubroDbContextLocator>>(services);

                    //step1: 更新缓存（缓存中保存的所有通知）

                    _cacheService.Push<string>("_allNotifcations", notification.ID);

                    //step2: 通知客户端(todo...后续按传入)
                    client.HandleNotificationChange(new NotificationModel());

                    //step3: 更新状态
                    notification.Status = 0;//已发送
                    _notification_Base.Update(notification);
                });
            });

        } 
    }
}
