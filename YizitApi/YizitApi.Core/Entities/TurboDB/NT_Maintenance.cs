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
    public partial class NT_Maintenance : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<NT_Maintenance, TubroDbContextLocator>
    {
    
        public string ID { get; set; }
        public string Notification_Id { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }

        public void Configure(EntityTypeBuilder<NT_Maintenance> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => e.ID)
                    .HasName("PK_NT_MAINTENANCE")
                    .IsClustered(false);

                entityBuilder.HasComment("֪ͨ����-ά��");

                entityBuilder.Property(e => e.ID)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("�汾Ψһid")
                    .ValueGeneratedOnAdd();//�ֹ���ӣ������������ʱ�Զ�����;

            entityBuilder.Property(e => e.Content)
                    .HasMaxLength(1024)
                    .HasComment("ά������");

                entityBuilder.Property(e => e.Notification_Id)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("����֪ͨid");

                entityBuilder.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasComment("ά��ʱ��");

            entityBuilder.Property(e => e.Title)
                  .IsRequired()
                  .HasMaxLength(128)
                  .HasComment("����");
        }

    }
}
