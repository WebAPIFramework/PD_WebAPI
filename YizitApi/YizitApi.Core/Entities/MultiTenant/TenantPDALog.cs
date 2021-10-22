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
    public partial class TenantPDALog : IEntity<MasterDbContextLocator>, IEntityTypeBuilder<TenantPDALog, MasterDbContextLocator>
    {
    
        public string ID { get; set; }
        public string User_Id { get; set; }
        public DateTime Log_Time { get; set; }
        public string Station_Id { get; set; }
        public string Action { get; set; }
        public string Content { get; set; }
        public string Result_Code { get; set; }
        public string Result_Message { get; set; }
        public string TenantId { get; set; }
    
        public void Configure(EntityTypeBuilder<TenantPDALog> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasNoKey();

                entityBuilder.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entityBuilder.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entityBuilder.Property(e => e.ID)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entityBuilder.Property(e => e.Log_Time).HasColumnType("datetime");

                entityBuilder.Property(e => e.Result_Code)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entityBuilder.Property(e => e.Result_Message).HasMaxLength(512);

                entityBuilder.Property(e => e.Station_Id)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entityBuilder.Property(e => e.TenantId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entityBuilder.Property(e => e.User_Id)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);
        }

    }
}
