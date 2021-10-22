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
    public partial class UserRoles : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<UserRoles, TubroDbContextLocator>
    {
    
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string CompanyId { get; set; }
    
        public void Configure(EntityTypeBuilder<UserRoles> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_USERROLES")
                    .IsClustered(false);

                entityBuilder.HasComment("�û���ɫ��ϵ��");

                entityBuilder.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("User_Id")
                    .HasComment("�û�d");

                entityBuilder.Property(e => e.RoleId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("Role_Id")
                    .HasComment("������ɫId");

                entityBuilder.Property(e => e.CompanyId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("Company_Id")
                    .HasComment("������ҵid,���⻧����");
        }

    }
}
