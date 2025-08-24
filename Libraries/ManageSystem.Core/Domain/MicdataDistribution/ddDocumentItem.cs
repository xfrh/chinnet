using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.MicdataDistribution
{
   public class ddDocumentItem
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long tem_id{get;set;}
        /// <summary>
        /// 文件id
        /// </summary>
      public long document_id{get;set; }
        /// <summary>
        /// 病历号
        /// </summary>
       public string patinet_id{get;set; }
        /// <summary>
        /// 科室
        /// </summary>
       public string ward{get;set; }
        /// <summary>
        /// 
        /// </summary>
       public string specimem{get;set; }
        /// <summary>
        /// 
        /// </summary>
       public string specimem_date{get;set; }
        /// <summary>
        /// 
        /// </summary>
       public string Specimen_Type{get;set; }
        /// <summary>
        /// 
        /// </summary>
       public string organism{get;set; }
        /// <summary>
        /// 
        /// </summary>
       public string organism_Type{get;set; }
        /// <summary>
        /// 抗生素
        /// </summary>
        public string antibiotics { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string antibiotics_value{get;set;}
        /// <summary>
        /// 知否有效
        /// </summary>
        public bool isvalid{get;set;}
    }
}
