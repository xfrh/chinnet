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
    public partial class SatelliteMenuRoleMap : ManageSystemEntityTypeConfiguration<SatelliteMenuRole>
    {
        public SatelliteMenuRoleMap()
        {
            this.ToTable("SatelliteMenuRole");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
