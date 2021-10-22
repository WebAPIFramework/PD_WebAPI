using Core.Infrastructure.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Infrastructure.Common
{

    #region 分页基础DTO
    /// <summary>
    /// 基础分页类
    /// </summary>
    public  class BasePagination
    {
        /// <summary>
        /// 分页条数
        /// </summary>
        public int PageSize { get; set; } = 20;
        /// <summary>
        /// 当前页
        /// </summary>
        public int Current { get; set; } = 1;

        public Dictionary<string, EnumSortType> Sorter { get; set; }
       
        public  Dictionary<string,List<string>> Filters { get; set; }


    }

    //public class Sorter
    //{
    //    public string SortField { get; set; }
    //    public EnumSortType SortWay { get; set; }
    //}

    //public class Filter<T>
    //{
    //    public string FilterField { get; set; }
    //    public List<T> FilterCondition { get; set; }
    //}
   
    #endregion


    #region 查询参数


    /// <summary>
    /// 查询参数
    /// </summary>
    public class QueryParameter
    {
        public BasePagination PaginationInfo { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        public List<QueryField> QueryFields { get; set; }
        /// <summary>
        /// 表头过滤条件
        /// </summary>
        public List<FilterField> HeaderFilters { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public SortField SortField { get; set; }
    }


    /// <summary>
    /// 排序字段
    /// </summary>
    public class SortField
    {
        public SortField()
        {
            SortType = EnumSortType.Ascend;
        }

        public string Name { get; set; }
        public EnumSortType SortType { get; set; }
    }


    #endregion


    #region 从dto接收后用于 封装的查询字段

    /// <summary>
    /// 查询字段
    /// </summary>
    public class FilterField<T>
    {
        public FilterField()
        {
            Operator = EnumQueryOperator.Contains;
        }

        public string Name { get; set; }
        public List<T>  Value { get; set; }
        public EnumQueryOperator Operator { get; set; }
    }

    public  class FilterField
    {
        public FilterField()
        {
            Operator = EnumQueryOperator.Equals;
        }

        public string Name { get; set; }
        public List<string>  Value { get; set; }
        public EnumQueryOperator Operator { get; set; }
    }

    /// <summary>
    /// 查询字段
    /// </summary>
    public class QueryField<T>:QueryField
    {
        public QueryField()
        {
            Operator = EnumQueryOperator.Equals;
        }

        public new string Name { get; set; }
        public new T Value { get; set; }
        public new EnumQueryOperator Operator { get; set; }
    }

    /// <summary>
    /// 查询字段
    /// </summary>
    public class QueryField
    {
        public QueryField()
        {
            Operator = EnumQueryOperator.Equals;
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public EnumQueryOperator Operator { get; set; }
    }

    /// <summary>
    /// 查询操作的枚举
    /// </summary>
    public enum EnumQueryOperator
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equals,

        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan,

        /// <summary>
        /// 大于或等于
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// 小于
        /// </summary>
        LessThan,

        /// <summary>
        /// 小于或等于
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// 开始于
        /// </summary>
        StartsWith,

        /// <summary>
        /// 结束于
        /// </summary>
        EndsWith,

        /// <summary>
        /// 包含
        /// </summary>
        Contains,

        /// <summary>
        /// 不等于
        /// </summary>
        NotEquals,
        /// <summary>
        /// 交集
        /// </summary>
        Intersect
    }

    #endregion

}
