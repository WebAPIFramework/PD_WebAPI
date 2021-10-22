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
    public partial class Notification_Base : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<Notification_Base, TubroDbContextLocator>
    {
    
        public string ID { get; set; }
        public int PublishType { get; set; }
        public DateTime PublishTime { get; set; }
        public int NotficationType { get; set; }
        public int Status { get; set; }
        public string creator { get; set; }
        public long? creation_time { get; set; }
        public string modifier { get; set; }
        public long? modification_time { get; set; }
        public string deleted_by { get; set; }
        public long? deleted_time { get; set; }
        public int deleted { get; set; }
    
        public void Configure(EntityTypeBuilder<Notification_Base> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => e.ID)
                    .HasName("PK_NOTIFICATION_BASE")
                    .IsClustered(false);

                entityBuilder.HasComment("��Ϣ֪ͨ������Ϣ��");

                entityBuilder.Property(e => e.ID)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("��ɫΨһ���")
                    .ValueGeneratedOnAdd();//�ֹ���ӣ������������ʱ�Զ�����;

            entityBuilder.Property(e => e.NotficationType).HasComment("ϵͳ��Ϣ���� ��ϵͳĬ��֧�� 0 ������1 ������2 ά����");

                entityBuilder.Property(e => e.PublishTime)
                    .HasColumnType("datetime")
                    .HasComment("����ʱ�� ���������ǵ�ǰ����ʱ�䣻 ��ʱ����ʱ����ָ����ʱ��");

                entityBuilder.Property(e => e.PublishType).HasComment("������ʽ 0 �������� 1 ��ʱ����");

                entityBuilder.Property(e => e.Status).HasComment("-1 ��ʼ��δ���ͣ��� 0 �ѷ��ͣ� 1�� ����");

                entityBuilder.Property(e => e.creation_time).HasComment("����ʱ��");

                entityBuilder.Property(e => e.creator)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("������");

                entityBuilder.Property(e => e.deleted).HasComment("ɾ�����");

                entityBuilder.Property(e => e.deleted_by)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("ɾ����");

                entityBuilder.Property(e => e.deleted_time).HasComment("ɾ��ʱ��");

                entityBuilder.Property(e => e.modification_time).HasComment("�޸�ʱ��");

                entityBuilder.Property(e => e.modifier)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("�޸���");
        }

    }
}
