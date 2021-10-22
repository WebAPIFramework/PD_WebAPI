using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DynamicApiController;
using Furion.EventBus;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YizitApi.Application.BuinessLayer;
using YizitApi.Application.Dtos;
using YizitApi.Core;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace YizitApi.Application
{
    /// <summary>
    /// 自定义的SSE消息对象实体
    /// </summary>
    public class SseMessageObject
    {
        public string MsgId { get; set; }
        public string MsgData { get; set; }
    }

    /// <summary>
    /// 模拟SSE（Server send events） 服务器往客户端推送
    /// </summary>
    [ApiDescriptionSettings("Turbo@2")]
    [AllowAnonymous]
    public class SSEDemoService : IDynamicApiController
    {

        //#region SSE 方案1
        //// 接收浏览器请求，建立ServerSentEvents通道
        ////[HttpGet, Route("BuildingSse")]
        ////public HttpResponseMessage BuildingSse(HttpRequestMessage message)
        //[HttpGet]
        //[NonUnify]
        //public HttpResponseMessage BuildingSse()
        //{
        //    var requestMessage = App.HttpContext.Items["MS_HttpRequestMessage"] as HttpRequestMessage ?? new HttpRequestMessage(); ;
        //    var response = requestMessage.CreateResponse();
        //    response.Content = new System.Net.Http.PushStreamContent(
        //        (Action<Stream, HttpContent, TransportContext>)WriteToStream, new MediaTypeHeaderValue("text/event-stream"));
        //    App.HttpContext.Response.Headers["Content-Type"] = "text/event-stream";
        //    App.HttpContext.Response.Headers["Cache-Control"] = "no-cache";
        //    App.HttpContext.Response.Headers["Connection"] = "keep-alive";

        //    return response;
        //}

        //private static readonly ConcurrentDictionary<StreamWriter, StreamWriter> _streammessage = new ConcurrentDictionary<StreamWriter, StreamWriter>();
        //private void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        //{
        //    StreamWriter streamwriter = new StreamWriter(outputStream);
        //    _streammessage.TryAdd(streamwriter, streamwriter);
        //}

        //// 建立SSE通道后，其他Controller或程序调用此方法，可以向浏览器端主动推送消息
        //public static void SendSseMsg(SseMessageObject sseMsg)
        //{
        //    MessageCallback(sseMsg);
        //}

        //// 设置向浏览器推送的消息内容
        //private static void MessageCallback(SseMessageObject sseMsg)
        //{
        //    foreach (var subscriber in _streammessage.ToArray())
        //    {
        //        try
        //        {
        //            subscriber.Value.WriteLine(string.Format("id: {0}\n", sseMsg.MsgId));
        //            subscriber.Value.WriteLine(string.Format("data: {0}\n\n", sseMsg.MsgData));
        //            subscriber.Value.Flush();
        //        }
        //        catch
        //        {
        //            StreamWriter streamWriter;
        //            _streammessage.TryRemove(subscriber.Value, out streamWriter);
        //        }
        //    }
        //}


        //#region （服务器端）成功建立SSE通道后，向浏览器推送消息：
        //// 服务端向网页端推送告警信息
        ////    var sseMsg = new SseMessageObject();
        ////    sseMsg.MsgId = "1101";
        ////sseMsg.MsgData = "自定义告警消息";
        ////ServerSentEventController.SendSseMsg(sseMsg);
        //#endregion
        //#endregion


        ///// <summary>
        ///// ...api/MyAPI/ServerSentEvents
        ///// </summary>
        ///// <returns></returns>
       
        //[HttpGet]
        //[NonUnify]
        //public void ServerSentEvents()
        //{
        //    string data = "";
        //    App.HttpContext.Response.Headers["Content-Type"] = "text/event-stream";
        //    App.HttpContext.Response.Headers["Cache-Control"] = "no-cache";
        //    App.HttpContext.Response.Headers["Connection"] = "keep-alive";
        //    //Response.Expires = -1
        //    //Response.Write("data: " & now())
        //    //Response.Flush()
        //    //Response.HttpContext.Response.Headers
        //    //唤醒默认的message
        //    data = ServerSentEventData(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), DateTime.Now.Ticks.ToString());

        //    App.HttpContext.Response.HttpContext.Response.Body.Write(System.Text.Encoding.Default.GetBytes(data), 0, data.Length);
        //    App.HttpContext.Response.HttpContext.Response.Body.Write(System.Text.Encoding.Default.GetBytes(""), 0, "".Length);

        //}

        //private string ServerSentEventData(string data, string id, string _event = "message", long retry = 1000)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendFormat("retry:{0}\n", retry);
        //    sb.AppendFormat("event:{0}\n", _event);
        //    sb.AppendFormat("id:{0}\n", id);
        //    sb.AppendFormat("data:{0}\n\n", data);
        //    return sb.ToString();
        //}


    }

}
