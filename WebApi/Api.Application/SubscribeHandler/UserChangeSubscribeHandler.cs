using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Core;

namespace Api.Application.SubscribeHandler
{
    /// <summary>
    /// 模拟订阅处理程序类
    /// 在 Furion 框架中，事件总线是不支持构造函数注入的，而且构造函数也只会执行一次。所以需要用到服务，应该通过静态类解析，App.GetService<xx>() 或 Db.GetRepository<XX>()
    /// </summary>
    public class UserChangeSubscribeHandler : ISubscribeHandler
    {
        // 定义一条消息
        [SubscribeMessage("create:user")]
        public void CreateUser(string eventId, object payload)
        {
            App.PrintToMiniProfiler("UserChangeSubscribeHandler", "新增用户", "CreateUser 方法被调用了,新增了一个用户");
            Scoped.Create((_, scope) =>
            {
                var services = scope.ServiceProvider;

                //var repository = Db.GetRepository<SLogPrintT>(services);
                //var someService = App.GetService<ISomeService>(services);   // services 传递进去
                //var otherService = services.GetService<IOtherService>();    // 直接用 services 解析
                //var data = repository.AsQueryable().FirstOrDefault();
                App.PrintToMiniProfiler("UserChangeSubscribeHandler", "新增用户", "CreateUser方法被调用了,新增了一个用户");
            });
        }

        // 多条消息共用同一个处理程序
        [SubscribeMessage("delete:user")]
        [SubscribeMessage("remove:user")]
        public void RemoveUser(string eventId, object payload)
        {
            App.PrintToMiniProfiler("UserChangeSubscribeHandler", "删除用户", "RemoveUser 方法被调用了,删除了一个用户");
            //Console.WriteLine("我是" + eventId);
        }

     
    }
}
