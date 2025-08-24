using ManageSystem.Core.Domain.Sate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping
{
    public partial class SatelliteUserMap : ManageSystemEntityTypeConfiguration<SatelliteUser>
    {
        public SatelliteUserMap()
        {
            this.ToTable("SatelliteUser");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Ignore(p => p.FunctionList);
            this.Ignore(p => p.RoleList);
        }
    }
}
