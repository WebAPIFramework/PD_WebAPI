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
    public partial class Account : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<Account, TubroDbContextLocator>
    {
    
        public string Id { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public int AccountType { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string Creator { get; set; }
        public long? CreationTime { get; set; }
        public string Modifier { get; set; }
        public long? ModificationTime { get; set; }
        public string DeletedBy { get; set; }
        public long? DeletedTime { get; set; }
        public int Deleted { get; set; }
    
        public void Configure(EntityTypeBuilder<Account> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => e.Id)
                    .HasName("PK_ACCOUNT")
                    .IsClustered(false);

                entityBuilder.HasComment("账户信息表");

                entityBuilder.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .HasComment("账户唯一ID")
                    .ValueGeneratedOnAdd();//手工添加，针对主键新增时自动增加;

            entityBuilder.Property(e => e.AccountType)
                    .HasColumnName("Account_Type")
                    .HasComment("0 本地用户；1： LDAP 用户；2 微信用户；3 钉钉用户；-1 因致账户");

                entityBuilder.Property(e => e.CompanyId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("Company_Id")
                    .HasComment("企业id,多租户考虑");

                entityBuilder.Property(e => e.CreationTime)
                    .HasColumnName("creation_time")
                    .HasComment("创建时间");

                entityBuilder.Property(e => e.Creator)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("creator")
                    .HasComment("创建人");

                entityBuilder.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasComment("删除标记");

                entityBuilder.Property(e => e.DeletedBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("deleted_by")
                    .HasComment("删除人");

                entityBuilder.Property(e => e.DeletedTime)
                    .HasColumnName("deleted_time")
                    .HasComment("删除时间");

                entityBuilder.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasComment("登录账号（同企业内不允许重复）");

                entityBuilder.Property(e => e.ModificationTime)
                    .HasColumnName("modification_time")
                    .HasComment("修改时间");

                entityBuilder.Property(e => e.Modifier)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("modifier")
                    .HasComment("修改人");

                entityBuilder.Property(e => e.Password)
                    .HasMaxLength(128)
                    .HasComment("密码");

                entityBuilder.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("User_Id")
                    .HasComment("所属用户Id");
        }

    }
}
