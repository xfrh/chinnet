using ManageSystem.Core.Domain.CRs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.CRs
{
    public partial class CRItemMap : ManageSystemEntityTypeConfiguration<CRItem>
    {
        public CRItemMap()
        {
            this.ToTable("CRItem");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Describe).IsMaxLength();

            this.Property(p => p.Name).HasMaxLength(1000);


        }
    }
}
