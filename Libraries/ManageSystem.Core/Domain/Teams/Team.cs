using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Teams
{
    /// <summary>
    /// 对应数据表：Team
    /// </summary>
    public partial class Team:BaseEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id{get;set;}
        /// <summary>
        /// 关联医院id
        /// </summary>
      public long Hospitalid {get;set;}
        /// <summary>
        /// 医院显示名称
        /// </summary>
      public string Title {get;set;}
        /// <summary>
        /// 负责人
        /// </summary>
      public string People {get;set;}
        /// <summary>
        /// 简介图片
        /// </summary>
      public string Images {get;set;}
        /// <summary>
        /// 是否显示简介
        /// </summary>
      public bool Displayimages {get;set; }

        /// <summary>
        /// 地区Id
        /// </summary>
      public long ProvinceId {get;set; }

        /// <summary>
        /// 地区名称
        /// </summary>
      public string ProvinceName { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public long classifyId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
      public DateTime InsertTime{get;set;}
        /// <summary>
        /// 创建人id
        /// </summary>
      public long Createby_Id{get;set;}
        /// <summary>
        /// 项目代码
        /// </summary>
      public string project_code { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
       public DateTime UpdateTime {get;set;}
    }
}
