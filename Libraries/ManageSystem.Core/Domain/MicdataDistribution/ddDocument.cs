using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.MicdataDistribution
{
    public class ddDocument
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long document_id { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public long? hospital_id { get; set; }
        /// <summary>
        /// 类别id
        /// </summary>
        public long category_id { get; set; }
        /// <summary>
        /// 年份id
        /// </summary>
        public long year_id { get; set; }
        /// <summary>
        /// 细菌id
        /// </summary>
        public long? germ_id { get; set; }
        /// <summary>
        /// 抗生素id
        /// </summary>
        public long? antibiotic_id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string file_path { get; set; }
        /// <summary>
        /// 数据量
        /// </summary>
        public int datacount { get; set; }
        /// <summary>
        /// 细菌代码
        /// </summary>
        public string organism { get; set; }        
        /// <summary>
        /// 表头标签
        /// </summary>
        public string header{get;set;}
        /// <summary>
        /// 排序
        /// </summary>
      public int sortid{get;set;}
        /// <summary>
        /// 是否有效
        /// </summary>
      public bool isvalid{get;set;}
        /// <summary>
        /// 添加时间
        /// </summary>
      public DateTime created{get;set;}
        /// <summary>
        /// 创建者
        /// </summary>
      public long created_by{get;set;}
    }
}
