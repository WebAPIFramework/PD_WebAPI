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
    public partial class CompanyApplicablePrivileges : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<CompanyApplicablePrivileges, TubroDbContextLocator>
    {
    
        public string EnterpriseId { get; set; }
        public string PrivilegeCode { get; set; }
        public int CodeType { get; set; }
        public string Desc { get; set; }
    
        public void Configure(EntityTypeBuilder<CompanyApplicablePrivileges> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => new { e.EnterpriseId, e.PrivilegeCode, e.CodeType })
                    .HasName("PK_COMPANY_APPLICABLE_PRIVILEG")
                    .IsClustered(false);

                entityBuilder.ToTable("Company_Applicable_Privileges");

                entityBuilder.HasComment("��ҵ����Ȩ���б�");

                entityBuilder.Property(e => e.EnterpriseId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("Enterprise_Id")
                    .HasComment("��ҵid");

                entityBuilder.Property(e => e.PrivilegeCode)
                    .HasMaxLength(256)
                    .HasColumnName("Privilege_Code")
                    .HasComment("Ȩ��ΨһCode");

                entityBuilder.Property(e => e.CodeType)
                    .HasColumnName("Code_Type")
                    .HasComment("code������0��menus��1��entities");

                entityBuilder.Property(e => e.Desc)
                    .HasMaxLength(256)
                    .HasComment("Ȩ������");
        }

    }
}
