using ManageSystem.Core.Domain.MicdataDistribution;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.MicdataDistribution
{
    public partial class ddYearMap : ManageSystemEntityTypeConfiguration<ddYear>
    {
        public ddYearMap()
        {
            this.ToTable("ddYear");
            this.HasKey(p => p.year_id);
            this.Property(p => p.year_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.title).HasMaxLength(1000);


        }
    }
}
