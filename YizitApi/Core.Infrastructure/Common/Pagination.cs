using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Common
{
    /// <summary>
    /// 分页对象
    /// </summary>
    /// <typeparam name="T">实体类</typeparam>
    public class Pagination<T> where  T:  new()
    {
        public int Total { get; set; }
        public int Current { get; set; }
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页集合
        /// </summary>
        public IEnumerable<T> Data { get; set; }
        public Pagination<TN> ReplaceItems<TN>(List<TN> items) where TN:new ()
        {
            return new Pagination<TN>
            {
                Total= Total,
                Current = Current,
                PageSize= PageSize,
                //TotalCount = TotalCount,
                //TotalPages= TotalPages,
                //HasPrevPages= HasPrevPages,
                //HasNextPages= HasNextPages,
                Data = items
            };
        }
    }


}
