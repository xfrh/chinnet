using ManageSystem.Core.Domain.Organism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.Organism
{
    /// <summary>
    /// 数据库操作类 ，数据库表名：BacteriaDetailedData
    /// </summary>
    public partial class BacteriaDetailedDataMap : ManageSystemEntityTypeConfiguration<BacteriaDetailedData>
    {

        public BacteriaDetailedDataMap()
        {
            this.ToTable("BacteriaDetailedData");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name).IsMaxLength();
            this.Property(c => c.OrganismTypeId).IsRequired();
            this.Property(c => c.V0002).HasMaxLength(1000);
            this.Property(c => c.V0004).HasMaxLength(1000);
            this.Property(c => c.V0008).HasMaxLength(1000);
            this.Property(c => c.V0016).HasMaxLength(1000);
            this.Property(c => c.V0032).HasMaxLength(1000);
            this.Property(c => c.V0064).HasMaxLength(1000);
            this.Property(c => c.V0125).HasMaxLength(1000);
            this.Property(c => c.V0025).HasMaxLength(1000);
            this.Property(c => c.V0005).HasMaxLength(1000);
            this.Property(c => c.V1001).HasMaxLength(1000);
            this.Property(c => c.V1002).HasMaxLength(1000);
            this.Property(c => c.V1004).HasMaxLength(1000);
            this.Property(c => c.V1008).HasMaxLength(1000);
            this.Property(c => c.V1016).HasMaxLength(1000);
            this.Property(c => c.V1032).HasMaxLength(1000);
            this.Property(c => c.V1064).HasMaxLength(1000);
            this.Property(c => c.V1128).HasMaxLength(1000);
            this.Property(c => c.V1256).HasMaxLength(1000);
            this.Property(c => c.V1512).HasMaxLength(1000);
            this.Property(c => c.Ecoff).HasMaxLength(1000);
            this.Property(c => c.Distributions).HasMaxLength(1000);
            this.Property(c => c.Observations).HasMaxLength(1000);

        }

    }
}
