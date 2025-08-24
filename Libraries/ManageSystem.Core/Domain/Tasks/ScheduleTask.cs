using System;

namespace ManageSystem.Core.Domain.Tasks
{
    /// <summary>
    /// 安排的任务
    /// </summary>
    public partial class ScheduleTask : BaseEntity
    {
        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///获取或设置运行时间(以秒为单位)
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// 获取或设置适当的ITask类的类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置值指示是否启用了一个任务
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 获取或设置值指示一个任务是否应该停止一些错误
        /// </summary>
        public bool StopOnError { get; set; }


        /// <summary>
        /// 获取或设置计算机名称(实例),租赁这一任务。时使用运行在web农场(确保任务只运行在一台机器上)。它可以是零不是运行在web农场。
        /// </summary>
        public string LeasedByMachineName { get; set; }

        /// <summary>
        /// 获取或设置datetime,直到任务由一些机器租赁(实例)。时使用运行在web农场(确保任务只运行在一台机器上)。
        /// </summary>
        public DateTime? LeasedUntilUtc { get; set; }

        /// <summary>
        ///获取或设置datetime时开始最后一次
        /// </summary>
        public DateTime? LastStartUtc { get; set; }

        /// <summary>
        /// 获取或设置datetime时完成最后一次(不管成功失败)
        /// </summary>
        public DateTime? LastEndUtc { get; set; }

        /// <summary>
        /// 获取或设置datetime时成功地完成了最后一次
        /// </summary>
        public DateTime? LastSuccessUtc { get; set; }

    }
}
