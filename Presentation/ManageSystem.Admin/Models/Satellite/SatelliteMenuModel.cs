using FluentValidation.Attributes;
using ManageSystem.Admin.Validators;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Satellite
{
    [Validator(typeof(SatelliteMenuValidator))]
    public class SatelliteMenuModel : BaseEntityModel
    { /// <summary>
	  /// 角色id
	  /// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 用户id
		/// <summary>
		public long FatherId { get; set; }

		public String Func { get; set; }

		public String Controller { get; set; }
	}
}