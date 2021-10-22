using Furion;
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using Api.Core;

namespace Api.EntityFramework.Core
{
    /// <summary>
    /// 应用框架库数据库上下文
    /// </summary>
    //[AppDbContext("TurboDB", DbProvider.SqlServer)]
    public class TubroDbContext : AppDbContext<TubroDbContext, TubroDbContextLocator>,IMultiTenantOnTable,IModelBuilderFilter//, IMultiTenantOnDatabase
    {
        public TubroDbContext(DbContextOptions<TubroDbContext> options) : base(options)
        {
            //新增或更新时忽略空值
            InsertOrUpdateIgnoreNullValues = true;
            EnabledEntityStateTracked = false;
        }

        
        #region 支持多租户（按TenantId来区分，表中增加TenantId标记）

        public object GetTenantId()
        {
            //一般用户的token里带上所属的多租户id，针对超级管理员等属于多个企业的，如果没有固定的所属企业id，则要求传过来的httpcontext中带上TenantId
            //在超级管理员选择企业后，要求前端要把当前选择的企业的Tenantid存储并发送给后端
            var tokenCompanyId = App.User?.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(tokenCompanyId))
            {
                var headerCompanyId= App.HttpContext.Request.Headers["CompanyId"].ToString();
                return headerCompanyId;
            } 
            else return tokenCompanyId;
            //return App.User?.FindFirst("CompanyId")?.Value?? App.HttpContext.Request.Headers["CompanyId"];
        }

        public void OnCreating(ModelBuilder modelBuilder, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            var filter = BuildTenantQueryFilter(entityBuilder, dbContext);
            entityBuilder.HasQueryFilter(filter);
        }

        /// <summary>
        /// 重写SavingChangeEvent事件方法
        /// 实现新增数据：自动设置TenantId的值
        /// 实现更新数据：排除TenantId属性更新
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        protected override void SavingChangesEvent(DbContextEventData eventData, InterceptionResult<int> result)
        {
            // 获取当前事件对应上下文
            var dbContext = eventData.Context;

            // 获取所有新增、更新、删除的实体
            var entities = dbContext.ChangeTracker.Entries().Where(u => u.State == EntityState.Added || u.State == EntityState.Modified || u.State == EntityState.Deleted);

            foreach (var entity in entities)
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        if (entity.Metadata.FindProperty("Id") != null && string.IsNullOrEmpty(entity.Property("Id").CurrentValue.ToString()))
                            entity.Property("Id").CurrentValue = Guid.NewGuid().ToString();
                        // 自动设置租户Id
                        //if (entity.Metadata.FindProperty(nameof(Entity.TenantId))!=null && entity.Property(nameof(Entity.TenantId)).CurrentValue == null)
                        if (entity.Metadata.FindProperty("CompanyId") != null && string.IsNullOrEmpty(entity.Property("CompanyId")?.CurrentValue?.ToString()))
                            entity.Property("CompanyId").CurrentValue = GetTenantId();
                       
                        //自动填充创建人
                        if (entity.Metadata.FindProperty("Creator") != null)
                            entity.Property("Creator").CurrentValue = App.User?.FindFirst("UserId")?.Value;
                        //自动填充创建人
                        if (entity.Metadata.FindProperty("creator") != null)
                            entity.Property("creator").CurrentValue = App.User?.FindFirst("UserId")?.Value;
                       
                        //自动填充创建时间
                        if (entity.Metadata.FindProperty("CreationTime") != null)
                            entity.Property("CreationTime").CurrentValue = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        //自动填充创建时间
                        if (entity.Metadata.FindProperty("creation_time") != null)
                            entity.Property("creation_time").CurrentValue = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                       
                        //自动填充删除标记
                        if (entity.Metadata.FindProperty("Deleted") != null)
                            entity.Property("Deleted").CurrentValue = 0;
                        if (entity.Metadata.FindProperty("deleted") != null)
                            entity.Property("deleted").CurrentValue = 0;
                        break;
                    
                    case EntityState.Modified:
                        // 自动排除租户Id
                        if (entity.Metadata.FindProperty("CompanyId") != null)
                            entity.Property("CompanyId").IsModified = false;
                        //自动填充修改人
                        if (entity.Metadata.FindProperty("Modifier") != null)
                            entity.Property("Modifier").CurrentValue = App.User?.FindFirst("UserId")?.Value;
                        if (entity.Metadata.FindProperty("modifier") != null)
                            entity.Property("modifier").CurrentValue = App.User?.FindFirst("UserId")?.Value;

                        //自动填充修改时间
                        if (entity.Metadata.FindProperty("ModificationTime") != null)
                            entity.Property("ModificationTime").CurrentValue = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        if (entity.Metadata.FindProperty("modification_time") != null)
                            entity.Property("modification_time").CurrentValue = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        break;
                    // 删除处理
                    case EntityState.Deleted:
                        //自动填充删除标记
                        if (entity.Metadata.FindProperty("Deleted") != null)
                            entity.Property("Deleted").CurrentValue = 1;
                        if (entity.Metadata.FindProperty("deleted") != null)
                            entity.Property("deleted").CurrentValue = 1;

                        if (entity.Metadata.FindProperty("DeletedTime") != null)
                            entity.Property("DeletedTime").CurrentValue = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        if (entity.Metadata.FindProperty("deleted_time") != null)
                            entity.Property("deleted_time").CurrentValue = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                        if (entity.Metadata.FindProperty("DeletedBy") != null)
                            entity.Property("DeletedBy").CurrentValue = App.User?.FindFirst("UserId")?.Value;
                        if (entity.Metadata.FindProperty("deleted_by") != null)
                            entity.Property("deleted_by").CurrentValue = App.User?.FindFirst("UserId")?.Value;
                        break;
                }
            }
        }
        #endregion

        #region 支持多租户（基于独立Database方式）
        public string GetDatabaseConnectionString()
        {
           return App.User?.FindFirst("ConnectionString")?.Value?? App.Configuration["ConnectionStrings:TurboDB"];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetDatabaseConnectionString());

            base.OnConfiguring(optionsBuilder);
        }
        #endregion

    }
}