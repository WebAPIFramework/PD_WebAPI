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

                entityBuilder.HasComment("�˻���Ϣ��");

                entityBuilder.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .HasComment("�˻�ΨһID")
                    .ValueGeneratedOnAdd();//�ֹ���ӣ������������ʱ�Զ�����;

            entityBuilder.Property(e => e.AccountType)
                    .HasColumnName("Account_Type")
                    .HasComment("0 �����û���1�� LDAP �û���2 ΢���û���3 �����û���-1 �����˻�");

                entityBuilder.Property(e => e.CompanyId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("Company_Id")
                    .HasComment("��ҵid,���⻧����");

                entityBuilder.Property(e => e.CreationTime)
                    .HasColumnName("creation_time")
                    .HasComment("����ʱ��");

                entityBuilder.Property(e => e.Creator)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("creator")
                    .HasComment("������");

                entityBuilder.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasComment("ɾ�����");

                entityBuilder.Property(e => e.DeletedBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("deleted_by")
                    .HasComment("ɾ����");

                entityBuilder.Property(e => e.DeletedTime)
                    .HasColumnName("deleted_time")
                    .HasComment("ɾ��ʱ��");

                entityBuilder.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasComment("��¼�˺ţ�ͬ��ҵ�ڲ������ظ���");

                entityBuilder.Property(e => e.ModificationTime)
                    .HasColumnName("modification_time")
                    .HasComment("�޸�ʱ��");

                entityBuilder.Property(e => e.Modifier)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("modifier")
                    .HasComment("�޸���");

                entityBuilder.Property(e => e.Password)
                    .HasMaxLength(128)
                    .HasComment("����");

                entityBuilder.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("User_Id")
                    .HasComment("�����û�Id");
        }

    }
}
