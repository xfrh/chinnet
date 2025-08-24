using Dapper;
using ICSharpCode.SharpZipLib.Zip;
using ManageSystem.Core;
using ManageSystem.Core.Caching;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Utility.Excel;
using ManageSystem.Data;
using ManageSystem.Services.Log;
using ManageSystem.Services.Members;
using ManageSystem.Services.Messages;
using ManageSystem.Services.SystemSet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using ManageSystem.Core.Domain.Members;
using OfficeOpenXml;
using ManageSystem.Services.Medicine;
using ManageSystem.Core.Domain.Survey;

namespace ManageSystem.Services.Survey
{
    /// <summary>
    /// 操作类 ，数据库表名：MedicalData 
    /// </summary>
    public partial class SurveySubjectService : BaseService<Survey_Subject>, ISurveySubjectService
    {

        public SurveySubjectService(IRepository<Survey_Subject> repository
            ) : base(repository)
        {
        }

        public IPagedList<Survey_Subject> QueryPage(string hospitalName, long hospitalId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

          

            //if (areaId > 0)
            //    query = query.Where(m => m.AreaId == areaId);

            //if (hospitalId > 0)
            //    query = query.Where(m => m.HospitalId == hospitalId);

            //if (year > 0)
            //    query = query.Where(m => m.Year == year);

            //if (quarter > 0)
            //    query = query.Where(m => m.Quarter == quarter);

            //if (projectType > 0)
            //{
            //    query = query.Where(r => r.ProjectType == projectType);
            //}
            query = query.OrderByDescending(m => m.InsertTime);

            var list = new PagedList<Survey_Subject>(query, pageIndex, pageSize);

            return list;
        }
        
        /// <summary>
        /// 会员中心，CR复敏信息管理的分页数据
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public IQueryable<Survey_Subject> Query(long memberId)
        {
            var data = base._repository.Table.Where(m => m.Mark > 0);

            return data.OrderByDescending(m => m.InsertTime);
        }
          
        /// <summary>
        /// 修改CR数据
        /// </summary>
        /// <param name="entity">CR数据</param>
        /// <param name="member">当前登录用户</param>
        /// <returns>SUCCESS 表示成功，其他则是错误信息</returns>
        public string Update(Survey_Subject entity, Member member)
        {
            #region 业务处理

            try
            {
                //修改CR项目主表的数据
                this.Update(entity);

                ////修改用户的省市区地址信息
                //var memberEntity = this.MemberService.QueryEntity(member.Id);
                //if (memberEntity == null || memberEntity.Id <= 0)
                //    return "用户信息不存在";
                //memberEntity.ProvinceId = entity.ProvinceId;
                //memberEntity.CityId = entity.CityId;
                //memberEntity.DistrictsId = entity.DistrictsId;
                //memberEntity.Address = entity.Address;
                //memberEntity.Area = entity.Area;
                //this.MemberService.Update(memberEntity);

                ////添加cr日志
                //this.ProjectLogService.Insert(ActionType.Create, ActionSource.Web, entity.Id, member.Id, member.Name, "修改CR数据", "修改CR数据");

                ////5、添加操作日志
                //this.ActionLogService.Insert(ActionType.Edit, ActionSource.Web, member.Id, member.Name, "修改CR项目Excel数据", entity.SerializeObject());

                //6、返回
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                //this.SystemLogService.Insert(ex, SystemLogLevel.Error);
                return ex.Message;
            }

            #endregion
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="ids">需要导出的项目id集合，为空则导出全部</param>
        /// <param name="ep">excel导出组建</param>
        /// <returns></returns>
        public string Export(List<long> ids, ExcelPackage ep)
        {
            List<Survey_Subject> list = new List<Survey_Subject>();
            if (ids != null && ids.Any())
            {
                //勾选了数据则导出指定的数据
                list = this.Query(r => ids.Contains(r.Id) && r.Mark > 0).ToList() ?? new List<Survey_Subject>();
            }
            else
            {
                //如果没有选择数据则默认导出全部的数据，根据excel格式，前面显示医院，如果医院没有上传数据的则姓名等数据为空
                var allProject = this.Query(m => m.Mark > 0).ToList() ?? new List<Survey_Subject>(); //查询所有的项目信息
                //var allHospital = this.HospitalService.QueryListCR() ?? new List<Hospital>(); //查询所有参与cr项目的医院

                list.AddRange(allProject);

                //allHospital = allHospital.Where(r => !allProject.Any(m => m.HospitalId == r.Id)).ToList();
                //allHospital.ForEach(item =>
                //{
                //    list.Add(new Survey_Subject
                //    {
                //        HospitalId = item.Id,
                //        HospitalName = item.Name
                //    });
                //});


                //foreach (var item in allHospital)
                //{
                //    var mi = allProject.Where(m => m.HospitalId == item.Id).FirstOrDefault();
                //    if (mi != null && mi.Id > 0)
                //        list.Add(mi);
                //    else
                //        list.Add(new Survey_Subject()
                //        {
                //            HospitalId = item.Id,
                //            HospitalName = item.Name,
                //        });
                //}
            }

            list = list.OrderBy(m => m.Id).ToList() ?? new List<Survey_Subject>();

            ////获取导出的明细数据
            //var itemList = this.ProjectItemService.Query().Where(p => list.Any(p2 => p2.Id == p.ProjectId)).OrderBy(p => p.ProjectId).ToList() ?? new List<CRProjectItem>();

            ////1、第一个sheet是医院上传的明细数据
            //this.ExportProjectItem(itemList, list, ep);

            ////2、第二个sheet是医院主表数据
            //this.ExportProject(list, ep);

            return "CR项目导出数据_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        }


    }
}
