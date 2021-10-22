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
using Api.Application.BuinessLayer;
using Api.Application.Dtos;
using Api.Core;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace Api.Web.Entry.Controller
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
    [ApiDescriptionSettings( "Shipment")]
    [AllowAnonymous]
    public class SSEDemoService : IDynamicApiController
    {
        // 接收浏览器请求，建立ServerSentEvents通道
        [HttpGet, Route("BuildingSse")]
        //public HttpResponseMessage BuildingSse(HttpRequestMessage message)
        public HttpResponseMessage BuildingSse()
        {
            HttpResponseMessage response = new HttpResponseMessage(); //message.CreateResponse(); //
            response.Content = new System.Net.Http.PushStreamContent((Action<Stream, HttpContent, TransportContext>)WriteToStream, new MediaTypeHeaderValue("text/event-stream"));
            return response;
        }

        private static readonly ConcurrentDictionary<StreamWriter, StreamWriter> _streammessage = new ConcurrentDictionary<StreamWriter, StreamWriter>();
        private void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            StreamWriter streamwriter = new StreamWriter(outputStream);
            _streammessage.TryAdd(streamwriter, streamwriter);
        }

        // 建立SSE通道后，其他Controller或程序调用此方法，可以向浏览器端主动推送消息
        public static void SendSseMsg(SseMessageObject sseMsg)
        {
            MessageCallback(sseMsg);
        }

        // 设置向浏览器推送的消息内容
        private static void MessageCallback(SseMessageObject sseMsg)
        {
            foreach (var subscriber in _streammessage.ToArray())
            {
                try
                {
                    subscriber.Value.WriteLine(string.Format("id: {0}\n", sseMsg.MsgId));
                    subscriber.Value.WriteLine(string.Format("data: {0}\n\n", sseMsg.MsgData));
                    subscriber.Value.Flush();
                }
                catch
                {
                    StreamWriter streamWriter;
                    _streammessage.TryRemove(subscriber.Value, out streamWriter);
                }
            }
        }

       

    }

    #region （服务器端）成功建立SSE通道后，向浏览器推送消息：
    // 服务端向网页端推送告警信息
//    var sseMsg = new SseMessageObject();
//    sseMsg.MsgId = "1101";
//sseMsg.MsgData = "自定义告警消息";
//ServerSentEventController.SendSseMsg(sseMsg);
    #endregion
}
