using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Common
{
    /// <summary>
    /// 错误异常信息类
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// ErrorCode，默认用EnumErrorCode，也可以自定义
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 异常额外信息
        /// </summary>
        public string Additional { get; set; }

        public ErrorMessage()
        {
        }

        /// <summary>
        /// 直接返回错误
        /// </summary>
        /// <returns></returns>
        public static ErrorMessage Failed()
        {
            return new ErrorMessage
            {
                Code = "-1",
                Message = "Failed"
            };
        }

        /// <summary>
        /// 直接返回成功
        /// </summary>
        /// <returns></returns>
        public static ErrorMessage Success()

        {
            return new ErrorMessage
            {
                Code = "0",
                Message = "Success"
            };
        }
    }
}
