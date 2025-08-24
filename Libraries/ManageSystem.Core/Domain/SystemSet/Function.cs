using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.SystemSet
{
    /// <summary>
    /// 实体类 ，数据库表名：Function 
    /// </summary>
    public partial class Function : BaseEntity
    {

        /// <summary>
        /// 功能名称
        /// <summary>
        public String Name { get; set; }
        /// <summary>
        /// 排序编号
        /// <summary>
        public int Sort { get; set; }
        /// <summary>
        /// 页面地址
        /// <summary>
        public String Url { get; set; }

        /// <summary>
        /// 功能类型
        /// <summary>
        public int Type { get; set; }

        /// <summary>
        /// 功能类型
        /// <summary>
        public FunctionType FunctionType
        {
            get
            {
                return (FunctionType)this.Type;
            }
            set
            {
                this.Type = (int)value;
            }
        }

        /// <summary>
        /// 控件Id
        /// <summary>
        public String ControlId { get; set; }
        /// <summary>
        /// 图标
        /// <summary>
        public String Image { get; set; }
        /// <summary>
        /// 自定义页面class
        /// <summary>
        public String CustomClass { get; set; }
        /// <summary>
        /// 上级功能id
        /// <summary>
        public long FunctionId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// <summary>
        public String CreateName { get; set; }

        /// <summary>
        /// 是否菜单，如果是将在左边显示菜单中
        /// </summary>
        public bool IsMenu { get; set; }

    }
}
