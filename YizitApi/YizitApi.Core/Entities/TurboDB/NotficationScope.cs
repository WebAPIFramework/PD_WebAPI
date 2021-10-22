// -----------------------------------------------------------------------------
// Generate By Furion Tools v2.11.1                            
// -----------------------------------------------------------------------------

using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using YizitApi.Core;

namespace YizitApi.Core
{
    public partial class NotficationScope : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<NotficationScope, TubroDbContextLocator>
    {
    
        public string Notification_Id { get; set; }
        public int Scope_Type { get; set; }
        public string Scope_Id { get; set; }
    
        public void Configure(EntityTypeBuilder<NotficationScope> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => new { e.Notification_Id, e.Scope_Type, e.Scope_Id })
                    .HasName("PK_NOTFICATIONSCOPE")
                    .IsClustered(false);

                entityBuilder.HasComment("消息通知范围");

                entityBuilder.Property(e => e.Notification_Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("通知id");

                entityBuilder.Property(e => e.Scope_Type).HasComment("通过范围类型 （0： 企业； 1： 角色； 2： 用户）");

                entityBuilder.Property(e => e.Scope_Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("根据类型，分别是企业id，角色id，用户id");
        }

    }
}
