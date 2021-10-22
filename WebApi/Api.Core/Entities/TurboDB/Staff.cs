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
    public partial class Staff : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<Staff, TubroDbContextLocator>
    {
    
        public string Id { get; set; }
        public string Name { get; set; }
        public string No { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string Creator { get; set; }
        public long? CreationTime { get; set; }
        public string Modifier { get; set; }
        public long? ModificationTime { get; set; }
        public string DeletedBy { get; set; }
        public long? DeletedTime { get; set; }
        public int Deleted { get; set; }
    
        public void Configure(EntityTypeBuilder<Staff> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => e.Id)
                    .HasName("PK_STAFF")
                    .IsClustered(false);

                entityBuilder.HasComment("Ա����Ϣ��");

                entityBuilder.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .HasComment("Ա��ΨһID")
                    .ValueGeneratedOnAdd();//�ֹ���ӣ������������ʱ�Զ�����;

            entityBuilder.Property(e => e.CompanyId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("Company_Id")
                    .HasComment("������ҵid,���⻧����");

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

                entityBuilder.Property(e => e.Email)
                    .HasMaxLength(128)
                    .HasComment("����");

                entityBuilder.Property(e => e.ModificationTime)
                    .HasColumnName("modification_time")
                    .HasComment("�޸�ʱ��");

                entityBuilder.Property(e => e.Modifier)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("modifier")
                    .HasComment("�޸���");

                entityBuilder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasComment("����");

                entityBuilder.Property(e => e.No)
                    .HasMaxLength(128)
                    .HasComment("Ա�����");

                entityBuilder.Property(e => e.Phone)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("�ֻ���");

                entityBuilder.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("User_Id")
                    .HasComment("�����û�ID");
        }

    }
}
