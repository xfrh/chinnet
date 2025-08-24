using ManageSystem.Core.Domain.Medicine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Members
{
    public class CenterMedicineProjectManageItemModel
    {
        public long Id { get; set; }
        public string HospitalName { get; set; }
        public int ProjectItem { get; set; }
        public string Project
        {
            get
            {
                switch (ProjectItem)
                {
                    case 1:
                        return "CHINET中国细菌耐药监测网";
                    case 2:
                        return "上海市细菌真菌耐药监测网";
                    case 3:
                        return "其他监测数据";
                    case 4:
                        return "浙江省细菌耐药监测网";
                    case 5:
                        return "河南省细菌耐药监测网";
                    default:
                        return "-";
                }
            }
        }
        public string UploadMessage { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }
        public string YearForQuarter
        {
            get
            {
                switch (Quarter)
                {
                    case 1:
                        return $"{Year}年第一季度";
                    case 2:
                        return $"{Year}年第二季度";
                    case 3:
                        return $"{Year}年第三季度";
                    case 4:
                        return $"{Year}年第四季度";
                    case 5:
                        return $"{Year}年全年";
                    case 6:
                        return $"{Year}年上半年";
                    case 7:
                        return $"{Year}年下半年";
                    default:
                        return "-";
                }
            }
        }
        public DateTime InsertTime { get; set; }
        public MedicalDataStatusEnum Status { get; set; }
    }
}