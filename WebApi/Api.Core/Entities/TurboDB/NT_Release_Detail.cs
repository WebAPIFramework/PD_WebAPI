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
    public partial class NT_Release_Detail : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<NT_Release_Detail, TubroDbContextLocator>
    {

        public string ID { get; set; }
        public string Release_Id { get; set; }
        public string Notification_Id { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
    
        public void Configure(EntityTypeBuilder<NT_Release_Detail> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasKey(e => e.ID)
             .HasName("PK_NT_Release_Detail")
             .IsClustered(false);

            entityBuilder.HasComment("版本更新内容");

            entityBuilder.Property(e => e.ID)
                  .HasMaxLength(36)
                  .IsUnicode(false)
                  .HasComment("唯一id")
                  .ValueGeneratedOnAdd();//手工添加，针对主键新增时自动增加;

            entityBuilder.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .HasComment("内容");

                entityBuilder.Property(e => e.Notification_Id)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("所属通知id");

                entityBuilder.Property(e => e.Release_Id)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("所属版本更新id");

                entityBuilder.Property(e => e.Type).HasComment("更新类型 （0： fix; 1: feature；2: improvement; 3: design ; 4:doc）");
        }

    }
}
