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
    public partial class Tennat : IEntity<MultiTenantDbContextLocator>, IEntityTypeBuilder<Tennat, MultiTenantDbContextLocator>
    {
    
        public string TenantId { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string TenantSchema { get; set; }
        public string ConnectionString { get; set; }
        public DateTime CreatedTime { get; set; }
    
        public void Configure(EntityTypeBuilder<Tennat> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => e.TenantId);

                entityBuilder.Property(e => e.TenantId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entityBuilder.Property(e => e.ConnectionString).HasMaxLength(256);

                entityBuilder.Property(e => e.CreatedTime).HasColumnType("datetime");

                entityBuilder.Property(e => e.EmailAddress).HasMaxLength(256);

                entityBuilder.Property(e => e.Host).HasMaxLength(256);

                entityBuilder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entityBuilder.Property(e => e.PhoneNumber).HasMaxLength(32);

                entityBuilder.Property(e => e.TenantSchema).HasMaxLength(32);
        }

    }
}
