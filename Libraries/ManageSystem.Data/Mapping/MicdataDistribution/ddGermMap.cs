using ManageSystem.Core.Domain.MicdataDistribution;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.MicdataDistribution
{
    public partial class ddGermMap : ManageSystemEntityTypeConfiguration<ddGerm>
    {
        public ddGermMap()
        {
            this.ToTable("ddGerm");
            this.HasKey(p => p.germ_id);
            this.Property(p => p.group_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.title).HasMaxLength(1000);

        }
    }
}
