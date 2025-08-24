using ManageSystem.Core.Domain.Sate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping
{
    public partial class SatelliteMap: ManageSystemEntityTypeConfiguration<Satellite>
    {
        public SatelliteMap()
        {
            this.ToTable("Satellite");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
