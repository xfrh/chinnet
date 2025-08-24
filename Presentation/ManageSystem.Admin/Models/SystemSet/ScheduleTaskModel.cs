using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.SystemSet;

namespace ManageSystem.Admin.Models.SystemSet
{
	/// <summary>
	/// 模型类 ，数据库表名：UserRole 
	/// </summary>
	 [Validator(typeof(ScheduleTaskValidator))]
	public partial class ScheduleTaskModel : BaseEntityModel
	{
        /// <summary>
        /// 获取或设置名称
        /// </summary>
        [HtmlDisplayAttribute("任务名称", "任务名称")]
        public string Name { get; set; }

        /// <summary>
        ///获取或设置运行时间(以秒为单位)
        /// </summary>
        [HtmlDisplayAttribute("运行时间（秒）", "运行时间(以秒为单位)")]
        public int Seconds { get; set; }

        /// <summary>
        /// 获取或设置适当的ITask类的类型
        /// </summary>
        [HtmlDisplayAttribute("ITask类的类型", "获取或设置适当的ITask类的类型，反射")]
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置值指示是否启用了一个任务
        /// </summary>
        [HtmlDisplayAttribute("是否启用", "获取或设置值指示是否启用了一个任务")]
        public bool Enabled { get; set; }

        /// <summary>
        /// 获取或设置值指示一个任务是否应该停止一些错误
        /// </summary>
        [HtmlDisplayAttribute("发送错误停止", "获取或设置值指示一个任务是否应该停止一些错误")]
        public bool StopOnError { get; set; }


        /// <summary>
        /// 获取或设置计算机名称(实例),租赁这一任务。时使用运行在web农场(确保任务只运行在一台机器上)。它可以是零不是运行在web农场。
        /// </summary>
        [HtmlDisplayAttribute("计算机名称", "执行任务的计算机名称")]
        public string LeasedByMachineName { get; set; }

        /// <summary>
        /// 获取或设置datetime,直到任务由一些机器租赁(实例)。时使用运行在web农场(确保任务只运行在一台机器上)。
        /// </summary>
        [HtmlDisplayAttribute("网站执行时间", "网站执行时间，确保任务只运行在一台机器上")]
        public DateTime? LeasedUntilUtc { get; set; }

        /// <summary>
        ///获取或设置datetime时开始最后一次
        /// </summary>
        [HtmlDisplayAttribute("最后开始时间", "最后一次执行开始的时间")]
        public DateTime? LastStartUtc { get; set; }

        /// <summary>
        /// 获取或设置datetime时完成最后一次(不管成功失败)
        /// </summary>
        [HtmlDisplayAttribute("最后完成时间", "最后一次执行的时间，不管成功或失败")]
        public DateTime? LastEndUtc { get; set; }

        /// <summary>
        /// 获取或设置datetime时成功地完成了最后一次
        /// </summary>
        [HtmlDisplayAttribute("最后成功时间", "最后一次执行成功的时间")]
        public DateTime? LastSuccessUtc { get; set; }


        /// <summary>
        /// 描述信息
        /// <summary>
        [HtmlDisplayAttribute("描述信息", "描述信息")]
        public String Describe { get; set; }


    }
}
