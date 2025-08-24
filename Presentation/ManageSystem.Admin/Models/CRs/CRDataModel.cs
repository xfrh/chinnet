using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.CRs;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.CRs
{
    [Validator(typeof(CRDataValidator))]
   public partial class CRDataModel : BaseEntityModel
    {
        /// <summary>
        /// 会员Id
        /// <summary>
        [HtmlDisplayAttribute("会员Id", "会员Id", false)]
        public long MemberId { get; set; }

        /// <summary>
        /// 上传会员的姓名
        /// <summary>
		[HtmlDisplayAttribute("会员姓名", "会员姓名", false)]
        public String MemberName { get; set; }

        /// <summary>
        /// 医院Id
        /// <summary>
        [HtmlDisplayAttribute("医院Id", "医院Id", false)]
        public long HospitalId { get; set; }

        /// <summary>
        /// 医院名称
        /// <summary>
        [HtmlDisplayAttribute("医院名称", "医院名称", false)]
        public String HospitalName { get; set; }

        /// <summary>
        /// 医院等级
        /// <summary>
        [HtmlDisplayAttribute("医院等级", "医院等级", false)]
        public String HospitalGrade { get; set; }

        /// <summary>
        /// 微生物负责人
        /// <summary>
        [HtmlDisplayAttribute("微生物负责人", "微生物负责人", false)]
        public String Name { get; set; }

        /// <summary>
        /// 联系电话
        /// <summary>
        [HtmlDisplayAttribute("联系电话", "联系电话", false)]
        public string Phone { get; set; }

        /// <summary>
        /// 邮件地址
        /// <summary>
        [HtmlDisplayAttribute("邮件地址", "邮件地址", false)]
        public String Email { get; set; }

        /// <summary>
        /// 省ID
        /// <summary>
        [HtmlDisplayAttribute("省ID", "省ID", false)]
        public long ProvinceId { get; set; }

        /// <summary>
        /// 市ID
        /// <summary>
        [HtmlDisplayAttribute("市ID", "市ID", false)]
        public long CityId { get; set; }

        /// <summary>
        /// 区ID
        /// <summary>
        [HtmlDisplayAttribute("区ID", "区ID", false)]
        public long DistrictsId { get; set; }

        /// <summary>
        /// 省市区，例如：河北省 沧州市 沧县
        /// <summary>
        [HtmlDisplayAttribute("省市区", "省市区", false)]
        public String Province { get; set; }
        public String City { get; set; }
        public String Area { get; set; }

        /// <summary>
        /// 详细地址，例如：江苏路135号903
        /// <summary>
        [HtmlDisplayAttribute("详细地址", "详细地址", false)]
        public String Address { get; set; }

        /// <summary>
        /// 上年CRE平均检出率
        /// <summary>
        [HtmlDisplayAttribute("上年CRE平均检出率", "上年CRE平均检出率", false)]
        public String CREDetectionRate { get; set; }

        /// <summary>
        /// 上年CRAB平均检出率
        /// <summary>
        [HtmlDisplayAttribute("上年CRAB平均检出率", "上年CRAB平均检出率", false)]
        public String CREDrugRate { get; set; }

        /// <summary>
        /// MH平板来源厂家
        /// </summary>
        [HtmlDisplayAttribute("MH平板来源厂家", "MH平板来源厂家", false)]
        public string MHSource { get; set; }

        /// <summary>
        /// 上传数据的文件路径
        /// <summary>
        [HtmlDisplayAttribute("上传文件名称", "上传文件名称", false)]
        public String UploadFilePath { get; set; }

        /// <summary>
        /// 处理过的Excel文件路径，相对路径
        /// </summary>
        [HtmlDisplayAttribute("上传文件名称", "上传文件名称", false)]
        public string DisposeFilePath { get; set; }

        /// <summary>
        /// 上传文件名称
        /// <summary>
        [HtmlDisplayAttribute("上传文件名称", "上传文件名称", false)]
        public String FileName { get; set; }

        /// <summary>
        /// 邮件状态 0：未发送，1已发送，2已确认
        /// <summary>
        [HtmlDisplayAttribute("邮件状态", "邮件状态", false)]
        public int EmailStatus { get; set; }
        /// <summary>
        /// 发送时间
        /// <summary>
        [HtmlDisplayAttribute("邮件发送时间", "邮件发送时间", false)]
        public DateTime EmailTime { get; set; }

        /// <summary>
        /// 确认时间
        /// <summary>
        [HtmlDisplayAttribute("医院邮件状态", "医院邮件状态", false)]
        public DateTime HospitalTime { get; set; }

        public string LoginId { set; get; }

        /// <summary>
        /// 处理过的Excel文件路径，相对路径
        /// </summary>
        [HtmlDisplayAttribute("年度", "年度", false)]
        public string Year { get; set; }

        /// <summary>
        /// 上传文件名称
        /// <summary>
        [HtmlDisplayAttribute("季度", "季度", false)]
        public String Quarter { get; set; }
    }
}