using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.CRs
{
   public class CRviews
    {
        /// <summary>
        /// 会员Id
        /// <summary>
        public long MemberId { get; set; }

        /// <summary>
        /// 上传会员的姓名
        /// <summary>
        public String MemberName { get; set; }

        /// <summary>
        /// 医院Id
        /// <summary>
        public long HospitalId { get; set; }

        /// <summary>
        /// 医院名称
        /// <summary>
        public String HospitalName { get; set; }

        /// <summary>
        /// 医院等级
        /// <summary>
        public String HospitalGrade { get; set; }

        /// <summary>
        /// 微生物负责人
        /// <summary>
        public String Name { get; set; }

        /// <summary>
        /// 联系电话
        /// <summary>
        public String Phone { get; set; }

        /// <summary>
        /// 邮件地址
        /// <summary>
        public String Email { get; set; }

        /// <summary>
        /// 省ID
        /// <summary>
        public long ProvinceId { get; set; }

        /// <summary>
        /// 市ID
        /// <summary>
        public long CityId { get; set; }

        /// <summary>
        /// 区ID
        /// <summary>
        public long DistrictsId { get; set; }
        /// <summary>
        /// 省
        /// <summary>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 省市区，例如：河北省 沧州市 沧县
        /// <summary>
        public String Area { get; set; }

        /// <summary>
        /// 详细地址，例如：江苏路135号903
        /// 
        /// <summary>
        public String Address { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public string Quarter { get; set; }
        /// <summary>
        /// 上年CRE平均检出率
        /// <summary>
        public String CREDetectionRate { get; set; }

        /// <summary>
        /// 上年CRAB平均检出率
        /// <summary>
        public String CREDrugRate { get; set; }

        /// <summary>
        /// MH平板来源厂家
        /// </summary>
        public string MHSource { get; set; }

        /// <summary>
        /// 上传文件名称
        /// <summary>
        public String FileName { get; set; }

        /// <summary>
        /// 上传数据的文件路径
        /// <summary>
        public String UploadFilePath { get; set; }

        /// <summary>
        /// 邮件状态 0：未发送，1已发送，2已确认
        /// <summary>
        public int EmailStatus { get; set; }

        /// <summary>
        /// 发送时间
        /// <summary>
        public DateTime EmailTime { get; set; }

        /// <summary>
        /// 确认时间
        /// <summary>
        public DateTime HospitalTime { get; set; }

        /// <summary>
        /// 邮件确认加密
        /// </summary>
        public string EmailStr { set; get; }

        public float cre { get; set; }

        public float crab { get; set; }
    }
}
