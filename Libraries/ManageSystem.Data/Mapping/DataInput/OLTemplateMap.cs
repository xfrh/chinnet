using ManageSystem.Core.Domain.DataInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.DataInput
{
    public partial class OLTemplateMap : ManageSystemEntityTypeConfiguration<OLTemplate>
    {
        public OLTemplateMap()
        {
            this.ToTable("OLtemplate");
            this.HasKey(p => p.Id);
           


        }
    }
}
