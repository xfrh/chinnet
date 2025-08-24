using FluentValidation;
using ManageSystem.Admin.Models.Organism;
using ManageSystem.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Validators.Organism
{
    /// <summary>
    /// 数据验证类 ，数据库表名：BacteriaDetailedData 
    /// </summary>
    public partial class BacteriaValidator : BaseValidator<BacteriaModel>
    {
        public BacteriaValidator()
        {
            RuleFor(x => x.MedicalOrganismId).NotEqual(0).WithMessage("请选择细菌！");
            RuleFor(x => x.DataFiles).NotNull().WithMessage("请选择数据文件！");
            

        }
    }
}