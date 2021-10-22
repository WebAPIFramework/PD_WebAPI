using Core.Infrastructure.Common;
using Core.Infrastructure.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Infrastructure.Extensions
{
    /// <summary>
    /// 分页参数整理帮助类
    /// </summary>
    public static class PaginationHelper
    {
        /// <summary>
        /// 根据外部传入的接收的列头部过滤条件整理出有效的过滤条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<FilterField> ConvertHeaderFilterConditions<T>(this Dictionary<string, List<string>> filters, T t)
        {
            if (filters == null) return new List<FilterField>();

            Type obj = t.GetType();//获得该类的Type
            if (t == null) return new List<FilterField>();
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0) return new List<FilterField>();
            List<FilterField> headerFilters = new List<FilterField>();
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string propertyName = item.Name;
                var propertyType = item.PropertyType;

                //首字母小写

                if (filters.Keys.Contains(propertyName, StringComparer.OrdinalIgnoreCase))
                    headerFilters.Add(new FilterField { Name = propertyName, Value = filters.FirstOrDefault(x => x.Key.ToLower() == propertyName.ToLower()).Value });

            }

            return headerFilters;
        }

        /// <summary>
        /// 根据外部接收指定的排序条件，整理出排序条件
        /// </summary>
        /// <param name="sorter"></param>
        /// <returns></returns>
        public static SortField ConvertSortField(this Dictionary<string, EnumSortType> sorter)
        {
           
            if (sorter == null || sorter.Count == 0) return new SortField();
            SortField res = new SortField 
            {
                Name=sorter.FirstOrDefault().Key,
                SortType=sorter.FirstOrDefault().Value
            };

            return res;
        }

        /// <summary>
        /// 分页拓展
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="pageIndex">页码，必须大于0</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static Pagination<TEntity> ToPagination<TEntity>(this IQueryable<TEntity> entities, int pageIndex = 1, int pageSize = 20)
            where TEntity : new()
        {
            if (pageIndex <= 0) throw new InvalidOperationException($"{nameof(pageIndex)} must be a positive integer greater than 0.");

            var totalCount = entities.Count();
            var items = entities.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new Pagination<TEntity>
            {
                Total= totalCount,
                Data = items,
                Current = pageIndex,
                PageSize = pageSize,
                //TotalCount = totalCount,
                //TotalPages = totalPages,
                //HasNextPages = pageIndex < totalPages,
                //HasPrevPages = pageIndex - 1 > 0
            };
        }
    }
}
