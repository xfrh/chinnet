using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System.Web.Mvc;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Admin.Validators.Project;

namespace ManageSystem.Admin.Models.Project
{
    /// <summary>
    /// 模型类 ，数据库表名：MedicalData 
    /// </summary>
    [Validator(typeof(ProjectDataItemValidator))]
    public partial class ProjectDataItemModel : BaseEntityModel
    {
        /// <summary>
        /// 编号
        /// <summary>
        [HtmlDisplayAttribute("编号", "编号", false)]
        public string Number { get; set; }

        /// <summary>
        /// 主表关联
        /// <summary>
        [HtmlDisplayAttribute("主表关联", "主表关联", false)]
        public long ProjectId { set; get; }

        /// <summary>
        /// 细菌名称
        /// <summary>
        [HtmlDisplayAttribute("细菌名称", "细菌名称", false)]
        public String Name { get; set; }

        /// <summary>
        /// 原始亚胺培南抑菌圈直径数值(mm)
        /// <summary>
        [HtmlDisplayAttribute("亚胺培南MIC值(ug/ml)", "亚胺培南MIC值(ug/ml)", false)]
        public decimal MIC_Imipenem { get; set; }

        /// <summary>
        /// 原始亚胺培南抑菌圈直径数值(mm)
        /// <summary>
        [HtmlDisplayAttribute("亚胺培南抑菌圈(mm)", "亚胺培南抑菌圈(mm)", false)]
        public decimal ImineNumber { get; set; }

        /// <summary>
        /// 替加环素纸片法抑菌圈直径数值(mm)
        /// <summary>
        [HtmlDisplayAttribute("替加环素抑菌圈直径(mm)", "替加环素抑菌圈直径(mm)", false)]
        public decimal TegacyclineNumber { get; set; }

        /// <summary>
        /// 替加环素纸片法复敏抑菌圈直径数值(mm)
        /// <summary>
        [HtmlDisplayAttribute("替加环素复敏抑菌圈直径(mm)", "替加环素复敏抑菌圈直径(mm)", false)]
        public decimal BacteriostasisNumber { get; set; }

        /// <summary>
        /// 肉汤法复核抑菌数值(ug/ml)
        /// <summary>
        [HtmlDisplayAttribute("肉汤法MIC值(ug/ml)", "肉汤法MIC值(ug/ml)", false)]
        public decimal RecheckNumber { get; set; }


    }
}
