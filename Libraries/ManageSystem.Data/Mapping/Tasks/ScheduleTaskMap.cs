using ManageSystem.Core.Domain.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageSystem.Data.Mapping.Tasks
{
    public partial class ScheduleTaskMap : ManageSystemEntityTypeConfiguration<ScheduleTask>
    {
        public ScheduleTaskMap()
        {
            this.ToTable("ScheduleTask");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name).IsRequired();
            this.Property(t => t.Type).IsRequired();
            this.Property(t => t.Seconds).IsRequired();
            this.Property(t => t.Describe).IsMaxLength();
        }
    }
}