using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Web.Validators.Meetings;
using System.Web.Mvc;

namespace ManageSystem.Web.Models.Meetings
{
	/// <summary>
	/// 模型类 ，数据库表名：MeetingType 
	/// </summary>
	public partial class MeetingTypeModel : BaseEntityModel
	{

        public MeetingTypeModel() {
            MeetingTypeList = new List<SelectListItem>();
        }
      
		/// <summary>
		/// 会议类型名称
		/// <summary>
		[HtmlDisplayAttribute("会议类型名称","会议类型名称",true)]
		public String Name { get; set; }

		/// <summary>
		/// 排序编号
		/// <summary>
		[HtmlDisplayAttribute("排序编号","排序编号", true)]
		public Int32 Sort { get; set; }

		/// <summary>
		/// 上级分类
		/// <summary>
		[HtmlDisplayAttribute("上级分类","上级分类")]
		public long MeetingTypeId { get; set; }


        /// <summary>
        /// 所有会议类型
        /// </summary>
        public IList<SelectListItem> MeetingTypeList { get; set; }


    }
}
