using ManageSystem.Core.Domain.Media;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageSystem.Data.Mapping.Media
{
    public partial class DownloadMap : ManageSystemEntityTypeConfiguration<Download>
    {
        public DownloadMap()
        {
            this.ToTable("Download");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.DownloadBinary).IsMaxLength();
        }
    }
}