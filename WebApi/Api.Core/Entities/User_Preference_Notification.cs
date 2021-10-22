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
    public partial class User_Preference_Notification : IEntity<TubroDbContextLocator>, IEntityTypeBuilder<User_Preference_Notification, TubroDbContextLocator>
    {
    
        public string User_Id { get; set; }
        public string Notification_Id { get; set; }
        public long creation_time { get; set; }
        public long? deleted_time { get; set; }
        public int deleted { get; set; }
    
        public void Configure(EntityTypeBuilder<User_Preference_Notification> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
                entityBuilder.HasKey(e => new { e.User_Id, e.Notification_Id });

                entityBuilder.Property(e => e.User_Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("�û�id");

                entityBuilder.Property(e => e.Notification_Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasComment("֪ͨid");

                entityBuilder.Property(e => e.creation_time).HasComment("��¼�Ѷ�ʱ��");

                entityBuilder.Property(e => e.deleted).HasComment("ɾ����ǣ������û������ٽ��ո�֪ͨ��");

                entityBuilder.Property(e => e.deleted_time).HasComment("ɾ��ʱ��");
        }

    }
}
