using FluentValidation.Attributes;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Admin.Validators;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Satellite
{
    [Validator(typeof(SatelliteUserValidator))]
    public class SatelliteUserModel : BaseEntityModel
    { /// <summary>
      /// 用户名
      /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String PassWord { get; set; }

        /// <summary>
        /// SatelliteId
        /// </summary>
        public long SatelliteId { get; set; }


        /// <summary>
        /// 状态
        /// <summary>
        [HtmlDisplayAttribute("用户状态", "用户状态")]
        public int State { get; set; }

        /// <summary>
        /// 状态枚举列表
        /// </summary>
        [HtmlDisplayAttribute("用户状态", "用户状态")]
        public IList<SelectListItem> UserinfoStateList { get; set; }

        /// <summary>
        /// 用户可用功能
        /// <summary>
        [HtmlDisplayAttribute("用户可用功能", "用户可用功能集合，在登录的时候填充数据")]
        public List<FunctionModel> FunctionList { get; set; }

    }
}