using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Log
{
    /// <summary>
    /// 实体类 ，数据库表名：SystemLog 
    /// </summary>
    public partial class SystemLog : BaseEntity
    {

        /// <summary>
        /// 错误标题
        /// <summary>
        public String Title { get; set; }

        /// <summary>
        /// 日志等级
        /// <summary>
        public  int LevelId { get; set; }

        /// <summary>
        /// 日志等级
        /// <summary>
        public SystemLogLevel Level
        {
            get
            {
                return (SystemLogLevel)this.LevelId;
            }
            set
            {
                this.LevelId = (int)value;
            }
        }

        /// <summary>
        /// 出错类
        /// <summary>
        public String Logger { get; set; }

        /// <summary>
        /// 错误页面地址
        /// <summary>
        public String Url { get; set; }

        /// <summary>
        /// 错误详细内容
        /// <summary>
        public String Message { get; set; }

        /// <summary>
        /// 操作的IP地址
        /// <summary>
        public String IPAddress { get; set; }

    }
}
