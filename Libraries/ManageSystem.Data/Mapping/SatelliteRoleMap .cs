using ManageSystem.Core.Domain.Sate;
using ManageSystem.Core.Domain.SystemSet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping
{
    public partial class SatelliteRoleMap : ManageSystemEntityTypeConfiguration<SatelliteRole>
    {
        public SatelliteRoleMap()
        {
            this.ToTable("SatelliteRole");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
