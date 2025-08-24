using FluentValidation.Attributes;
using ManageSystem.Admin.Validators;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Satellite
{
    [Validator(typeof(SatelliteRoleValidator))]
    public class SatelliteRoleModel : BaseEntityModel
    { /// <summary>
	  /// 角色id
	  /// <summary>
		public long RoleId { get; set; }
		/// <summary>
		/// 用户id
		/// <summary>
		public long SatelliteUserId { get; set; }
	}
}