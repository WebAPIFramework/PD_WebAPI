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
    public partial class NT_Other : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<NT_Other, TubroDbContextLocator>
    {
    
        public string ID { get; set; }
        public string Notification_Id { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
    
        public void Configure(EntityTypeBuilder<NT_Other> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => e.ID)
                    .HasName("PK_NT_OTHER")
                    .IsClustered(false);

                entityBuilder.HasComment("通知内容-其他");

                entityBuilder.Property(e => e.ID)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("版本唯一id")
                    .ValueGeneratedOnAdd();//手工添加，针对主键新增时自动增加;

            entityBuilder.Property(e => e.Content)
                    .HasMaxLength(1024)
                    .HasComment("内容");

                entityBuilder.Property(e => e.Notification_Id)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("所属通知id");

                entityBuilder.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasComment("时间");

                entityBuilder.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasComment("标题");
        }

    }
}
