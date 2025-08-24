using ManageSystem.Core.Domain.CRs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.CRs
{
    public partial class CRMap : ManageSystemEntityTypeConfiguration<CRData>
    {
        public CRMap()
        {
            this.ToTable("CRData");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.HospitalName).HasMaxLength(1000);
            this.Property(p => p.HospitalGrade).HasMaxLength(100);
            this.Property(p => p.Name).HasMaxLength(100);
            this.Property(p => p.UploadFilePath).HasMaxLength(1000);
            this.Property(p => p.Email).HasMaxLength(500);
            this.Property(p => p.Area).HasMaxLength(1000);
            this.Property(p => p.Address).HasMaxLength(1000);

            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.FileName).HasMaxLength(1000);


        }
    }
}
