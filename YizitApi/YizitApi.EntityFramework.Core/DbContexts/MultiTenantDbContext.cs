using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using YizitApi.Core;

namespace YizitApi.EntityFramework.Core
{
    /// <summary>
    /// 多租户操作建议单独一个数据库上下文，而且需指定 MultiTenantDbContextLocator 数据库上下文定位器
    /// </summary>
    [AppDbContext("MultiTenantDB", DbProvider.SqlServer)]
    public class MultiTenantDbContext : AppDbContext<MultiTenantDbContext, MultiTenantDbContextLocator>
    {
        public MultiTenantDbContext(DbContextOptions<MultiTenantDbContext> options) : base(options)
        {
        }
    }
}