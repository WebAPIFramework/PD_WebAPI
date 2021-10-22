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
    public partial class NotficationScope : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<NotficationScope, TubroDbContextLocator>
    {
    
        public string Notification_Id { get; set; }
        public int Scope_Type { get; set; }
        public string Scope_Id { get; set; }
    
        public void Configure(EntityTypeBuilder<NotficationScope> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => new { e.Notification_Id, e.Scope_Type, e.Scope_Id })
                    .HasName("PK_NOTFICATIONSCOPE")
                    .IsClustered(false);

                entityBuilder.HasComment("��Ϣ֪ͨ��Χ");

                entityBuilder.Property(e => e.Notification_Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("֪ͨid");

                entityBuilder.Property(e => e.Scope_Type).HasComment("ͨ����Χ���� ��0�� ��ҵ�� 1�� ��ɫ�� 2�� �û���");

                entityBuilder.Property(e => e.Scope_Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("�������ͣ��ֱ�����ҵid����ɫid���û�id");
        }

    }
}
