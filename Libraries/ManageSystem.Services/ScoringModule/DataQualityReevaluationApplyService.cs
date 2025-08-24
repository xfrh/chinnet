using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.ScoringModule;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public class DataQualityReevaluationApplyService : BaseService<DataQuality_Reevaluation_Apply>, IDataQualityReevaluationApplyService
    {
        private IDataQualityJuryService _juryService;
        private IDataQualityScoreService _scoreService;
        private IDataQualityHospitalService _dqhospitalService;
        private IMemberService _memberService;
        private IHospitalService _hospitalService;
        public DataQualityReevaluationApplyService(IRepository<DataQuality_Reevaluation_Apply> repository, IDataQualityJuryService juryService, IDataQualityScoreService scoreService, IDataQualityHospitalService dqhospitalService, IMemberService memberService, IHospitalService hospitalService) : base(repository)
        {
            _juryService = juryService;
            _scoreService = scoreService;
            _dqhospitalService = dqhospitalService;
            _memberService = memberService;
            _hospitalService = hospitalService;
        }

        public IPagedList<DataQuality_Reevaluation_Apply> QueryPage(string title, string status, string juryName, string hospital, int page, int pageSize)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0);
            if (!string.IsNullOrWhiteSpace(title))
            {
                var ids = _scoreService.Query(r => r.Title.Contains(title) && r.Mark > 0).Select(r => r.Id).ToList();
                query = query.Where(r => ids.Contains(r.DataQuality_Score_Id));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                switch (status)
                {
                    case "待处理":
                        query = query.Where(r => r.Status == 1);
                        break;
                    case "同意":
                        query = query.Where(r => r.Status == 2);
                        break;
                    case "取消":
                        query = query.Where(r => r.Status == 3);
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(juryName))
            {
                List<long> member_ids = _memberService.Query(r => r.Mark > 0 && (r.Name.Contains(juryName) || r.NickName.Contains(juryName) || r.Phone.Contains(juryName))).Select(r => r.Id).ToList();
                List<long> ids = _juryService.Query(r => member_ids.Contains(r.Jury_Id) && r.Mark > 0).Select(r => r.Id).ToList();
                query = query.Where(r => ids.Contains(r.DataQuality_Jury_Id));
            }

            if (!string.IsNullOrEmpty(hospital))
            {
                var hospital_ids = _hospitalService.Query(r => r.Name.Contains(hospital) && r.Mark > 0).Select(r => r.Id);
                var ids = _dqhospitalService.Query(r => hospital_ids.Contains(r.Hospital_Id) && r.Mark > 0).Select(r => r.Id).ToList();
                query = query.Where(r => ids.Contains(r.DataQuality_Hospital_Id));
            }

            query = query.OrderByDescending(m => m.InsertTime)
                 .ThenByDescending(r => r.UpdateTime)
                 .ThenBy(r => r.Id);

            var list = new PagedList<DataQuality_Reevaluation_Apply>(query.ToList(), page, pageSize);

            return list;
        }
    }
}
