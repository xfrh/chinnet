using ManageSystem.Core.Domain.Teams;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.Teams
{
    public partial class Team_CREContentMap : ManageSystemEntityTypeConfiguration<Team_CREContent>
    {
        public Team_CREContentMap()
        {
            this.ToTable("Team_CREContent");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Content).IsRequired().IsMaxLength();
            this.Property(t => t.Describe).IsMaxLength();
        }
    }
}
