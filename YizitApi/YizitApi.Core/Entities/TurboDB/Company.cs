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
    public partial class Company : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<Company, TubroDbContextLocator>
    {
    
        public string Id { get; set; }
        public string Name { get; set; }
        public string No { get; set; }
        public string Desc { get; set; }
        public string TimeZoneId { get; set; }
        public string Manager { get; set; }
        public string CompanySchema { get; set; }
        public string CompanyDbConnection { get; set; }
        public string Creator { get; set; }
        public long? CreationTime { get; set; }
        public string Modifier { get; set; }
        public long? ModificationTime { get; set; }
        public string DeletedBy { get; set; }
        public long? DeletedTime { get; set; }
        public int Deleted { get; set; }
    
        public void Configure(EntityTypeBuilder<Company> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => e.Id)
                    .HasName("PK_COMPANY")
                    .IsClustered(false);

                entityBuilder.HasComment("��ҵ��Ϣ��");

                entityBuilder.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .HasComment("��ҵΨһ���")
                    .ValueGeneratedOnAdd();//�ֹ���ӣ������������ʱ�Զ�����;

            entityBuilder.Property(e => e.CompanyDbConnection)
                    .HasMaxLength(128)
                    .HasColumnName("Company_DbConnection")
                    .HasComment("��ҵ���ӿ���Ϣ�����⻧����-����databaseʱ��");

                entityBuilder.Property(e => e.CompanySchema)
                    .HasMaxLength(128)
                    .HasColumnName("Company_Schema")
                    .HasComment("��ҵSchema�����⻧����-����database������schemaʱ��");

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

                entityBuilder.Property(e => e.Desc)
                    .HasMaxLength(1024)
                    .HasComment("��ҵ����");

                entityBuilder.Property(e => e.Manager)
                    .HasMaxLength(128)
                    .HasComment("��ҵ����Ա����");

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
                    .HasMaxLength(512)
                    .HasComment("��ҵ����");

                entityBuilder.Property(e => e.No)
                    .HasMaxLength(128)
                    .HasComment("��ҵ���");

                entityBuilder.Property(e => e.TimeZoneId)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("TimeZone_Id")
                    .HasComment("ʱ��id");
        }

    }
}
