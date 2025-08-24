using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Models.Regist
{
    /// <summary>
    ///注册数据封装
    /// </summary>
    public class RegistModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string LoginId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        public string Password2 { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 所属省份id
        /// </summary>
        public long AreaId { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 省份列表
        /// </summary>
        public List<SelectListItem> AreaList { get; set; }

        /// <summary>
        /// 如果医院下拉列表选择了其他，则填写其他医院的名称
        /// </summary>
        public string OtherHospital { get; set; }

        /// <summary>
        /// 注册来源，1：web   2：mobile
        /// </summary>
        public int Source { get; set; }
    }

}