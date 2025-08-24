using ManageSystem.Core.Domain.MIC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.MICappiy
{
    public partial class MICapplyMap : ManageSystemEntityTypeConfiguration<MICPermissionapplication>
    {
        public MICapplyMap()
        {

            this.ToTable("MICPermissionapplication");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.MID);
            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.Phone).HasMaxLength(1000);
            this.Property(p => p.Email).HasMaxLength(1000);

            this.Property(p => p.CompanyName).HasMaxLength(1000);
            this.Property(p => p.Department).HasMaxLength(1000);
            this.Property(p => p.Position).HasMaxLength(1000);
            this.Property(p => p.Applicationstate);
        }
    }
}
