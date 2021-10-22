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
    public partial class RolePrivileges : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<RolePrivileges, TubroDbContextLocator>
    {
    
        public string RoleId { get; set; }
        public string PrivilegeCode { get; set; }
        public int CodeType { get; set; }
        public string CompanyId { get; set; }
    
        public void Configure(EntityTypeBuilder<RolePrivileges> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => new { e.RoleId, e.PrivilegeCode, e.CodeType })
                    .HasName("PK_ROLE_PRIVILEGES")
                    .IsClustered(false);

                entityBuilder.ToTable("Role_Privileges");

                entityBuilder.HasComment("角色权限表");

                entityBuilder.Property(e => e.RoleId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("Role_Id")
                    .HasComment("角色id");

                entityBuilder.Property(e => e.PrivilegeCode)
                    .HasMaxLength(256)
                    .HasColumnName("Privilege_Code")
                    .HasComment("权限唯一Code");

                entityBuilder.Property(e => e.CodeType)
                    .HasColumnName("Code_Type")
                    .HasComment("code所属，0：menus；1：entities");

                entityBuilder.Property(e => e.CompanyId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("Company_Id")
                    .HasComment("所属企业id,多租户考虑");
        }

    }
}
