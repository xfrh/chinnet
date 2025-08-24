using ManageSystem.Core.Domain.Medicine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.Medicine
{
    public partial class HospitalWardLocationMap : ManageSystemEntityTypeConfiguration<HospitalWardLocation>
    {
        public HospitalWardLocationMap()
        {
            this.ToTable("HospitalWardLocation");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Name).IsMaxLength();
            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.Ward).HasMaxLength(255);
            this.Property(p => p.Department_CN).HasMaxLength(255);
            this.Property(p => p.Department_EN).IsMaxLength();
            this.Property(p => p.Location).HasMaxLength(500);
            this.Property(p => p.Location_Type).HasMaxLength(500);
        }
    }
}
