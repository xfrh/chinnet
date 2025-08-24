using ManageSystem.Core.Domain.CRs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.CRs
{
    public partial class CRLogMap : ManageSystemEntityTypeConfiguration<CRLog>
    {
        public CRLogMap()
        {
            this.ToTable("CRLog");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


        }
    }
}
