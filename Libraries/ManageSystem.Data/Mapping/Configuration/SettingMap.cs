using ManageSystem.Core.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.Configuration
{
    public partial class SettingMap : ManageSystemEntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            this.ToTable("Setting");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(s => s.Name).IsRequired().HasMaxLength(200);
            this.Property(s => s.Title).IsRequired().HasMaxLength(1000);
            this.Property(s => s.Type).HasMaxLength(1000);

        }
    }
}
