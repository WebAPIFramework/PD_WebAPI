using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Common
{
    /// <summary>
    /// API返回格式
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class ResultSet<T>
    {
        public ErrorMessage ErrorMessage { get; set; }

        public T Data { get; set; }

        public ResultSet()
        {
            this.ErrorMessage = ErrorMessage.Success();
        }

        public static ResultSet<T> Success()
        {
            return new ResultSet<T> { ErrorMessage = ErrorMessage.Success() };
        }

        public static ResultSet<T> Success(T data)
        {
            return new ResultSet<T> { ErrorMessage = ErrorMessage.Success(), Data = data };
        }

        public static ResultSet<T> Failed(ErrorMessage errorMessage)
        {
            return new ResultSet<T> { ErrorMessage = errorMessage };
        }

        public static ResultSet<T> Failed()
        {
            return new ResultSet<T>
            {
                ErrorMessage = ErrorMessage.Failed()
            };
        }
    }
}
