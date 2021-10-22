using Furion;
using Furion.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.AOP
{
    /// <summary>
    /// AOP注册拦截
    /// 创建代理类实现 AspectDispatchProxy, IDispatchProxy或IGlobalDispatchProxy
    /// </summary>
    public class LogDispatchProxy : AspectDispatchProxy, IGlobalDispatchProxy
    {
        /// <summary>
        /// 当前服务实例
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// 服务提供器，可以用来解析服务，如：Services.GetService()
        /// </summary>
        public IServiceProvider Services { get; set; }
        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override object Invoke(MethodInfo method, object[] args)
        {
            
            //App.PrintToMiniProfiler("PrintLog", "状态", "SayHello 方法被调用了");
            var result = method.Invoke(Target, args);

            //App.PrintToMiniProfiler("PrintLog", "状态", "SayHello 方法调用完成");

            return result;
        }
        /// <summary>
        /// 异步无返回值
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override async Task InvokeAsync(MethodInfo method, object[] args)
        {
            //App.PrintToMiniProfiler("PrintLog", "状态", "SayHello 方法被调用了");

            var task = method.Invoke(Target, args) as Task;
            await task;
            //App.PrintToMiniProfiler("PrintLog", "状态", "SayHello 方法调用完成");
        }

        /// <summary>
        /// 异步带返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override async Task<T> InvokeAsyncT<T>(MethodInfo method, object[] args)
        {
            //App.PrintToMiniProfiler("PrintLog", "状态", "SayHello 方法被调用了");

            var taskT = method.Invoke(Target, args) as Task<T>;
            var result = await taskT;

            //App.PrintToMiniProfiler("PrintLog", "状态", "SayHello 方法调用完成");

            return result;
        }
    }
}
