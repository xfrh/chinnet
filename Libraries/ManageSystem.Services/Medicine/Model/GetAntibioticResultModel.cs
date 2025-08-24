using ManageSystem.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Medicine.Model
{
    /// <summary>
    /// 获取细菌和药物的耐药性结果
    /// </summary>
    public class GetAntibioticResultModel
    {
        /// <summary>
        /// 细菌Id
        /// </summary>
        public long OrganismId { get; set; }

        /// <summary>
        /// 细菌名称
        /// </summary>
        public string OrganismName { get; set; }

        /// <summary>
        /// 细菌编码
        /// </summary>
        public string OrganismCode { get; set; }

        /// <summary>
        /// 上传中该细菌的总数
        /// <summary>
        public int OrganismCount { get; set; }

        /// <summary>
        /// 抗生素Id
        /// </summary>
        public long AntibioticId { get; set; }

        /// <summary>
        /// 抗生素名称
        /// </summary>
        public string AntibioticName { get; set; }

        /// <summary>
        /// 抗生素编码
        /// </summary>
        public string AntibioticCode { get; set; }

        /// <summary>
        /// 敏感的数量
        /// </summary>
        public int SensitiveCount { get; set; }

        /// <summary>
        /// 敏感的总占比，单位%
        /// </summary>
        public decimal SensitiveRatio
        {
            get
            {
                if (DataCount > 0)
                {
                    return Math.Round((decimal)SensitiveCount * 100 / DataCount, 1, MidpointRounding.AwayFromZero);
                }

                return 0.0m;
            }
        }

        /// <summary>
        /// 中介的数量
        /// </summary>
        public int IntermediaryCount { get; set; }

        /// <summary>
        /// 中介的总占比，单位%
        /// </summary>
        public decimal IntermediaryRatio
        {
            get
            {
                decimal _value = 0.0m;

                if (DataCount > 0)
                {
                    _value = Math.Round((decimal)IntermediaryCount * 100 / DataCount, 1, MidpointRounding.AwayFromZero);
                }

                if (_value + ResistanceRatio + SensitiveRatio > 100.0m)
                {
                    _value = 100 - (ResistanceRatio + SensitiveRatio);
                }

                return _value;
            }
        }

        /// <summary>
        /// 耐药的数量
        /// </summary>
        public int ResistanceCount { get; set; }

        /// <summary>
        /// 耐药的总占比，单位%
        /// </summary>
        public decimal ResistanceRatio
        {
            get
            {
                if (DataCount > 0)
                {
                    return (ResistanceCount * 100.0m / DataCount).GetDecimal2(1);
                }

                return 0.0m;
            }
        }

        /// <summary>
        /// 敏感的数量 + 中介的数量 + 耐药的数量
        /// </summary>
        private int dataCount;

        public int DataCount
        {
            get
            {
                int total = SensitiveCount + IntermediaryCount + ResistanceCount;
                if (dataCount != total && dataCount > total)
                {
                    return dataCount;
                }

                if (dataCount != total && total > dataCount)
                {
                    return total;
                }

                return dataCount;
            }
            set { dataCount = value; }
        }

        //public int DataCount { get { return SensitiveCount + IntermediaryCount + ResistanceCount; } }
    }
}
