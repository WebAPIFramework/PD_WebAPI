using Core.Infrastructure.Common;
using Core.Infrastructure.Enum;
using Furion.FriendlyException;
using Furion.LinqBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Infrastructure.Extensions
{
    /// <summary>
    /// 业务常用的查询扩展
    /// </summary>
    public static class QueryableExtension
    {
        private static MethodInfo _whereMethod;

        //private static readonly ILog Log = LogManager.GetLogger(typeof(QueryableExtension));

        /// <summary>
        /// 根据字段的名称进行排序
        /// </summary>
        /// <param name="query"></param>
        /// <param name="field"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(
            this IQueryable<TSource> query, SortField field)
        {
            var entityType = typeof(TSource);
            if (null == field || field.Name.IsNullOrEmpty())
            {
                //Log.Error("排序字段为空");
                return (IOrderedQueryable<TSource>)query;
            }

            var flag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
            var propertyInfo = entityType.GetProperty(field.Name, flag);
            if (null == propertyInfo)
            {
               // Log.Info("未找到相应的属性");
                return (IOrderedQueryable<TSource>)query;
            }

            var arg = Expression.Parameter(entityType, "x");
            var property = Expression.Property(arg, field.Name);
            var selector = Expression.Lambda(property, arg);

            var enumType = typeof(Queryable);
            var method = EnumSortType.Ascend == field.SortType
                ? enumType.GetMethods()
                    .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                    .Where(m =>
                    {
                        var parameters = m.GetParameters().ToList();
                        return parameters.Count == 2;
                    }).Single()
                : enumType.GetMethods()
                    .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
                    .Where(m =>
                    {
                        var parameters = m.GetParameters().ToList();
                        return parameters.Count == 2;
                    }).Single();

            var genericMethod = method
                .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            return (IOrderedQueryable<TSource>)genericMethod
                .Invoke(genericMethod, new object[] { query, selector });
        }


        #region 表头查询

        ///// <summary>
        ///// 根据查询条件进行查询
        ///// </summary>
        ///// <typeparam name="TSource"></typeparam>
        ///// <param name="query"></param>
        ///// <param name="fields"></param>
        ///// <returns></returns>
        //public static IQueryable<TSource> Where<TSource>(
        //    this IQueryable<TSource> query, IEnumerable<FilterField> fields)
        //{
        //    var param = Expression.Parameter(typeof(TSource), "f");
        //    foreach (var field in fields)
        //    {
        //        query = GetFilterExpression<TSource>(query, field, param);
        //    }

        //    return query;
        //}


        //private static IQueryable<T> GetFilterExpression<T>(IQueryable query, FilterField field,
        //    ParameterExpression param)
        //{
        //    //检查操作符与数据类型
        //    var fieldProperty = typeof(T).GetProperty(field.Name);

        //    //非字符串类型的属性，如果传入值为空，抛出异常
        //    if (field.Value.IsNullOrEmpty() && "String" != fieldProperty.PropertyType.Name)
        //        throw Oops.Oh("{0}数据库获取出错", field.Name);

        //    //如果找不到对应的属性，抛出异常
        //    if (null == fieldProperty)
        //        throw Oops.Oh("{0}数据库获取出错", field.Name);

        //    var fieldExp = Expression.Property(param, fieldProperty);
        //    var expression = GetExpressionByFilterFieldInfo(fieldExp, field);
        //    var where = Expression.Lambda<Func<T, bool>>(expression, param);
        //    var genericMethod = GetQueryWhereMethod().MakeGenericMethod(typeof(T));
        //    return (IQueryable<T>)genericMethod.Invoke(genericMethod, parameters: new object[] { query, where });
        //}
        //private static Expression GetExpressionByFilterFieldInfo(Expression fieldExpression, FilterField queryField)
        //{
        //    try
        //    {
        //        var type = fieldExpression.Type;
        //        Expression fieldValue;
        //        switch (type.FullName)
        //        {
        //            case "System.String":
        //                {
        //                    fieldValue = Expression.Constant(queryField.Value);
        //                    break;
        //                }
        //            case "System.Int32":
        //            case
        //                "System.Nullable`1[[System.Int32, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
        //                :
        //                {
        //                    var tmp = int.Parse(queryField.Value);
        //                    fieldValue = Expression.Constant(tmp, type);
        //                    break;
        //                }
        //            case "System.Int64":
        //            case
        //                "System.Nullable`1[[System.Int64, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
        //                :
        //                {
        //                    var tmp = long.Parse(queryField.Value);
        //                    fieldValue = Expression.Constant(tmp, type);
        //                    break;
        //                }
        //            case "System.Double":
        //            case
        //                "System.Nullable`1[[System.Double, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
        //                :
        //                {
        //                    var tmp = double.Parse(queryField.Value);
        //                    fieldValue = Expression.Constant(tmp, type);
        //                    break;
        //                }
        //            case "System.DateTime":
        //            case
        //                "System.Nullable`1[[System.DateTime, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
        //                :
        //                {
        //                    /* .net core 2.2存在datetime转换成了datetime2类型的问题
        //                     * https://github.com/aspnet/EntityFrameworkCore/issues/14095
        //                     * issue显示该缺陷已在19.7.28合并到master，而2.2.6创建于19.7.9，故猜测官方只在3.0+后进行了修复
        //                     * 
        //                     * 目前的解决方案：
        //                     * 1.定义datetime/datetime?的参数时，配置Column特性标记如下
        //                     * 
        //                     * [Column("ENTRYDATE", TypeName = "datetime")]
        //                     * [DataType("datetime")]
        //                     * public DateTime? EntryDate { get; set; }
        //                     */
        //                    var tmp = DateTime.Parse(queryField.Value);
        //                    fieldValue = Expression.Constant(tmp, type);
        //                    break;
        //                }
        //            case "System.Boolean":
        //            case "System.Nullable`1[[System.Boolean, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]":
        //                {
        //                    var value = queryField.Value;
        //                    var tmp = (value == "0" || value == "1") ? (value != "0") : Convert.ToBoolean(value);
        //                    fieldValue = Expression.Constant(tmp, type);
        //                    break;
        //                }

        //            default:
        //                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        //        }

        //        return GetExpressByOperator(queryField.Operator, fieldExpression, fieldValue);
        //    }
        //    catch (Exception e)
        //    {
        //        throw Oops.Oh("{0}数据处理出错", e.Message);
        //        //throw CommonException.Builder().CreateException(EnumErrorCode.DBProcessError, e.Message);
        //    }
        //}
        #endregion


        /// <summary>
        /// 根据查询条件进行查询
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="query"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IQueryable<TSource> Where<TSource>(
            this IQueryable<TSource> query, IEnumerable<QueryField> fields)
        {
            var param = Expression.Parameter(typeof(TSource), "f");
            foreach (var field in fields)
            {
                query = GetQueryExpression<TSource>(query, field, param);
            }

            return query;
        }


        private static IQueryable<T> GetQueryExpression<T>(IQueryable query, QueryField field,
            ParameterExpression param)
        {
            //检查操作符与数据类型
            var fieldProperty = typeof(T).GetProperty(field.Name);

            //非字符串类型的属性，如果传入值为空，抛出异常
            if (field.Value.IsNullOrEmpty() && "String" != fieldProperty.PropertyType.Name)
                throw Oops.Oh("{0}数据库获取出错", field.Name);

            //如果找不到对应的属性，抛出异常
            if (null == fieldProperty)
                throw Oops.Oh("{0}数据库获取出错", field.Name);

            var fieldExp = Expression.Property(param, fieldProperty);
            var expression = GetExpressionByFieldInfo<T>(fieldExp, field);
            var where = Expression.Lambda<Func<T, bool>>(expression, param);
            var genericMethod = GetQueryWhereMethod().MakeGenericMethod(typeof(T));
            return (IQueryable<T>)genericMethod.Invoke(genericMethod, parameters: new object[] { query, where });
        }

        private static Expression GetExpressByOperator<T>(EnumQueryOperator queryOperator, Expression left,
            Expression right)
        {
            switch (queryOperator)
            {
                case EnumQueryOperator.Equals:
                    {
                        return Expression.Equal(left, right);
                    }
                case EnumQueryOperator.GreaterThan:
                    {
                        return Expression.GreaterThan(left, right);
                    }
                case EnumQueryOperator.GreaterThanOrEqual:
                    {
                        return Expression.GreaterThanOrEqual(left, right);
                    }
                case EnumQueryOperator.LessThan:
                    {
                        return Expression.LessThan(left, right);
                    }
                case EnumQueryOperator.LessThanOrEqual:
                    {
                        return Expression.LessThanOrEqual(left, right);
                    }
                case EnumQueryOperator.StartsWith:
                    {
                        var method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                        return null == method ? null : Expression.Call(left, method, right);
                    }
                case EnumQueryOperator.EndsWith:
                    {
                        var method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                        return null == method ? null : Expression.Call(left, method, right);
                    }
                case EnumQueryOperator.Contains:
                    {
                        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        return null == method ? null : Expression.Call(left, method, right);
                    }
                case EnumQueryOperator.NotEquals:
                    {
                        return Expression.NotEqual(left, right);
                    }
                case EnumQueryOperator.Intersect:
                    {
                        //todo...待完成交集的查询
                        return null;
                       
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(queryOperator), queryOperator, null);
            }
        }

        private static Expression GetExpressionByFieldInfo<T>(Expression fieldExpression, QueryField queryField)
        {
            try
            {
                var type = fieldExpression.Type;
                Expression fieldValue;
                switch (type.FullName)
                {
                    case "System.String":
                        {
                            fieldValue = Expression.Constant(queryField.Value);
                            break;
                        }
                    case "System.Int32":
                    case
                        "System.Nullable`1[[System.Int32, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
                        :
                        {
                            var tmp = int.Parse(queryField.Value);
                            fieldValue = Expression.Constant(tmp, type);
                            break;
                        }
                    case "System.Int64":
                    case
                        "System.Nullable`1[[System.Int64, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
                        :
                        {
                            var tmp = long.Parse(queryField.Value);
                            fieldValue = Expression.Constant(tmp, type);
                            break;
                        }
                    case "System.Double":
                    case
                        "System.Nullable`1[[System.Double, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
                        :
                        {
                            var tmp = double.Parse(queryField.Value);
                            fieldValue = Expression.Constant(tmp, type);
                            break;
                        }
                    case "System.DateTime":
                    case
                        "System.Nullable`1[[System.DateTime, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
                        :
                        {
                            /* .net core 2.2存在datetime转换成了datetime2类型的问题
                             * https://github.com/aspnet/EntityFrameworkCore/issues/14095
                             * issue显示该缺陷已在19.7.28合并到master，而2.2.6创建于19.7.9，故猜测官方只在3.0+后进行了修复
                             * 
                             * 目前的解决方案：
                             * 1.定义datetime/datetime?的参数时，配置Column特性标记如下
                             * 
                             * [Column("ENTRYDATE", TypeName = "datetime")]
                             * [DataType("datetime")]
                             * public DateTime? EntryDate { get; set; }
                             */
                            var tmp = DateTime.Parse(queryField.Value);
                            fieldValue = Expression.Constant(tmp, type);
                            break;
                        }
                    case "System.Boolean":
                    case "System.Nullable`1[[System.Boolean, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]":
                        {
                            var value = queryField.Value;
                            var tmp = (value == "0" || value == "1") ? (value != "0") : Convert.ToBoolean(value);
                            fieldValue = Expression.Constant(tmp, type);
                            break;
                        }
                    case "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]":
                        {
                            var value = queryField.Value.Split(",").ToList() ;
                            fieldValue = Expression.Constant(value, type);
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                return GetExpressByOperator<T>(queryField.Operator, fieldExpression, fieldValue);
            }
            catch (Exception e)
            {
                throw Oops.Oh("{0}数据处理出错", e.Message);
                //throw CommonException.Builder().CreateException(EnumErrorCode.DBProcessError, e.Message);
            }
        }

        private static MethodInfo GetQueryWhereMethod()
        {
            if (null == _whereMethod)
            {
                _whereMethod = GetMethod(typeof(Queryable), "Where", 2);
            }

            return _whereMethod;
        }

        private static MethodInfo GetMethod(Type type, string name, int parametersCount)
        {
            if (null == type) return null;
            var result = type.GetMethods().First(m =>
            {
                var parameters = m.GetParameters().ToList();
                return m.Name == name
                       && m.IsGenericMethodDefinition && parameters.Count == parametersCount;
            });
            return result;
        }
    }
}
