// -----------------------------------------------------------------------------
// Generate By Furion Tools v2.11.1                            
// -----------------------------------------------------------------------------

using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using Api.Core;

namespace Api.Core
{
    public partial class Notification_Base : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<Notification_Base, TubroDbContextLocator>
    {
    
        public string ID { get; set; }
        public int PublishType { get; set; }
        public DateTime PublishTime { get; set; }
        public int NotficationType { get; set; }
        public int Status { get; set; }
        public string creator { get; set; }
        public long? creation_time { get; set; }
        public string modifier { get; set; }
        public long? modification_time { get; set; }
        public string deleted_by { get; set; }
        public long? deleted_time { get; set; }
        public int deleted { get; set; }
    
        public void Configure(EntityTypeBuilder<Notification_Base> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => e.ID)
                    .HasName("PK_NOTIFICATION_BASE")
                    .IsClustered(false);

                entityBuilder.HasComment("消息通知基本信息表");

                entityBuilder.Property(e => e.ID)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("角色唯一编号")
                    .ValueGeneratedOnAdd();//手工添加，针对主键新增时自动增加;

            entityBuilder.Property(e => e.NotficationType).HasComment("系统消息类型 （系统默认支持 0 其他；1 发布；2 维护）");

                entityBuilder.Property(e => e.PublishTime)
                    .HasColumnType("datetime")
                    .HasComment("发布时间 立即发布是当前创建时间； 定时发布时，是指定的时间");

                entityBuilder.Property(e => e.PublishType).HasComment("发布方式 0 立即发布 1 定时发布");

                entityBuilder.Property(e => e.Status).HasComment("-1 初始（未发送）； 0 已发送； 1： 撤回");

                entityBuilder.Property(e => e.creation_time).HasComment("创建时间");

                entityBuilder.Property(e => e.creator)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("创建人");

                entityBuilder.Property(e => e.deleted).HasComment("删除标记");

                entityBuilder.Property(e => e.deleted_by)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("删除人");

                entityBuilder.Property(e => e.deleted_time).HasComment("删除时间");

                entityBuilder.Property(e => e.modification_time).HasComment("修改时间");

                entityBuilder.Property(e => e.modifier)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("修改人");
        }

    }
}
