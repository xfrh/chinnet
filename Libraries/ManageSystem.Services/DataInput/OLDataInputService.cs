using ManageSystem.Core;
using ManageSystem.Core.Domain.DataInput;
using ManageSystem.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using OfficeOpenXml;
using ManageSystem.Core.Utility.FastDBF;
using System.IO;
using System.Data;
using ManageSystem.Core.Utility.Excel;
using ManageSystem.Core.Utility;

namespace ManageSystem.Services.DataInput
{
    public partial class OLDataInputService : IOLDataInputService
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddOLDataInput(OLDataInput model)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("insert into [dbo].[OLdataInput] values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}') ", model.Id, model.projectid,model.templateId, model.experimenttime,model.experimenter,model.jobnumber,model.germnumber,model.germname,model.auditor,model.auditortime,model.datevalue,model.created_byId, DateTime.Now, DateTime.Now);
                result = conn.Execute(sql);
            }
            return result;
        }

        //public int BatchOLDataInput(List<OLDataInput> modelList)
        //{
        //    int result = 0;
        //    using (IDbConnection conn = DapperHelper.GetConnection())
        //    {
        //        string sql = @"insert into [dbo].[OLdataInput] (Id,projectid,templateId,experimenttime,experimenter,jobnumber,germnumber,germname,auditor,auditortime,datevalue,created_byId,InsertTime,updatetime) values(@Id, @projectid, @templateId,@experimenttime,@experimenter, @jobnumber, @germnumber, @germname, @auditor, @auditortime, @datevalue, @created_byId,@InsertTime,@updatetime)";
        //        result = conn.Execute(sql, modelList);
        //    }
        //    return result;
        //}
        /// <summary>
        /// 审核数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AuditorOLDataInput(OLDataInput model,string Ids)
        {
            Ids = Ids.TrimEnd(',');
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("Update [dbo].[OLdataInput] set auditor='{0}',auditortime='{1}'  where Id in({2})", model.auditor, model.auditortime,Ids);
                result = conn.Execute(sql);
            }
            return result;
        }

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int delete(long Id)
        {
            int result;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {          
                    string sql = string.Format("delete  from  [dbo].[OLdataInput] where Id='{0}'", Id);
                    result = conn.Execute(sql);               
            }
            return result;
        }

        public List<OLDataInput> QueryList(string[] ids)
        {
            List<OLDataInput> resultList = new List<OLDataInput>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from [dbo].[OLdataInput] where id in ({0})", String.Join(",",ids));
                resultList = conn.Query<OLDataInput>(sql).ToList();
            }


            return resultList;
        }

        #region 弃用方法
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="ep"></param>
        /// <returns></returns>
        //public string Export(string ids, ExcelPackage ep)
        //{
        //    string sheetname = "";
        //    if (ids != null && ids.Any())
        //    {
        //        ids = ids.TrimEnd(',');
        //        string [] id = ids.Split(',');              
        //        for (int i = 0; i < id.Length; i++)
        //        {
        //            List<OLFieldmodel> list = new List<OLFieldmodel>();
        //            List<OLDataInput> Datalist = new List<OLDataInput>();
        //            //勾选了数据则导出指定的数据        
        //            using (SqlConnection conn = DapperHelper.GetConnection())
        //            {
        //                string sql = string.Format("select * from [dbo].[OLfieldmodel] where templateId =(select templateId from OLdataInput where projectid='{0}' group by templateId)", id[i]);
        //                IEnumerable<OLFieldmodel> result = conn.Query<OLFieldmodel>(sql);
        //                list = result.ToList();
        //            }

        //            using (SqlConnection conn = DapperHelper.GetConnection())
        //            {
        //                string sql = string.Format("select b.project_name,a.* from [OLdataInput] a left join [dbo].[OLproject] b on a.projectid=b.Id  where projectid='{0}'", id[i]);
        //                IEnumerable<OLDataInput> result = conn.Query<OLDataInput>(sql);
        //                Datalist = result.ToList();
        //            }

        //            using (SqlConnection conn = DapperHelper.GetConnection())
        //            {
        //                string sql = string.Format("select * from [dbo].[OLproject] where Id='{0}'", id[i]);
        //                sheetname = conn.Query<OLProject>(sql).FirstOrDefault().project_name;
        //            }

        //            this.ExportProject(Datalist, list, ep, sheetname);
        //        }
        //    }
        //    return "折点项目导出数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        //}

        ///// <summary>
        ///// 导出医院相关信息
        ///// </summary>
        ///// <param name="list">本次导出的项目数据</param>
        ///// <param name="ep"></param>
        //private void ExportProject(List<OLDataInput> list, List<OLFieldmodel> OLDataheade, ExcelPackage ep,string sheetname)
        //{
        //    //写入表头
        //    ExcelWorksheet ws = ep.Workbook.Worksheets.Add(sheetname);
        //    ws.Cells[1, 1].Value = "项目名称";
        //    ws.Cells[1, 2].Value = "实验人";
        //    ws.Cells[1, 3].Value = "实验时间";
        //    ws.Cells[1, 4].Value = "工作编号";
        //    ws.Cells[1, 5].Value = "菌种库号";
        //    ws.Cells[1, 6].Value = "细菌名称";
        //    int n = 7;
        //    for (int i = 0; i < OLDataheade.Count; i++)
        //    {
        //        ws.Cells[1, n].Value = OLDataheade[i].field_name;
        //        n++;
        //    }          
        //    int index = 2;
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        ws.Cells[index, 1].Value = list[i].project_name;
        //        ws.Cells[index, 2].Value = list[i].experimenter;
        //        ws.Cells[index, 3].Value = list[i].experimenttime;
        //        ws.Cells[index, 4].Value = list[i].jobnumber;
        //        ws.Cells[index, 5].Value = list[i].germnumber;
        //        ws.Cells[index, 6].Value = list[i].germname;;
        //        int m = 7;
        //        string[] value = list[i].datevalue.Split(',');
        //        for (int j = 0; j < value.Length; j++)
        //        {
        //            ws.Cells[index, m].Value = value[j].ToString();
        //            m++;
        //        }
        //        index++;
        //    }
        //    //foreach (var item in list)
        //    //{
        //    //    ws.Cells[index, 1].Value = item.Hospital;
        //    //    ws.Cells[index, 2].Value = item.Province;
        //    //    ws.Cells[index, 3].Value = item.City;
        //    //    ws.Cells[index, 4].Value = item.District;
        //    //    ws.Cells[index, 5].Value = item.Detailedaddress;
        //    //    ws.Cells[index, 6].Value = item.Contact;
        //    //    ws.Cells[index, 7].Value = item.Mobile;
        //    //    ws.Cells[index, 8].Value = item.Email;
        //    //    
        //    //}
        //}
        #endregion
        /// <summary>
        /// 根据Id查询数据表头
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public List<OLFieldmodel> GetOLDataheade(long projectid)
        {
            List<OLFieldmodel> result = new List<OLFieldmodel>();

            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select a.project_name,b.* from OLproject a left join  [dbo].[OLfieldmodel] b on a.templateId=b.templateId where b.templateId =(select templateId from OLproject where Id='{0}' group by templateId) and a.Id='{1}'", projectid,projectid);
                result = conn.Query<OLFieldmodel>(sql).ToList();
            }
            return result;
        }

        /// <summary>
        /// 分页查询详情数据
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<OLDataInput> QueryPage(long Id,long create_byId ,string number,string ischeck,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string ischecksql = "";
                if (!string.IsNullOrEmpty(ischeck))
                    ischecksql = ischeck == "1" ? " and auditor is not null " : " and auditor is null ";
                string sql = @"select b.project_name,a.* from [OLdataInput] a left join [dbo].[OLproject] b on a.projectid=b.Id  where projectid=" + Id + "and a.created_byId="+ create_byId + (!string.IsNullOrEmpty(number)? " and jobnumber like'%" + number + "%'":"")+ ischecksql + " order by a.InsertTime desc,a.jobnumber";
                List<OLDataInput> result = conn.Query<OLDataInput>(sql.ToString()).ToList();              
                return new PagedList<OLDataInput>(result, pageIndex, pageSize);
            }
        }

        public IPagedList<OLDataInput> QuerPageByCondition(string number, string ischeck, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"select b.project_name,a.* from [OLdataInput] a left join [dbo].[OLproject] b on a.projectid=b.Id  where jobnumber like'%"+number+"%' "+(ischeck=="1"? "and auditor is not null " : "and auditor is null") +"order by a.InsertTime";
                List<OLDataInput> result = conn.Query<OLDataInput>(sql.ToString()).ToList();
                return new PagedList<OLDataInput>(result, pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int updateOLDataInput(OLDataInput model)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("Update [dbo].[OLdataInput] set experimenttime='{0}',experimenter='{1}',datevalue='{2}',updatetime='{3}' where Id='{4}'",model.experimenttime, model.experimenter, model.datevalue, model.Updatetime, model.Id);
                result = conn.Execute(sql);
            }
            return result;
        }
        /// <summary>
        /// 单挑数据编辑返填
        /// </summary>
        /// <param name="dataid"></param>
        /// <returns></returns>
        public List<OLDataInput> updateOLDataheade(long dataid)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from [dbo].[OLdataInput] a  left join [dbo].[OLproject] b  on a.projectid=b.Id  where a.Id='{0}'", dataid);
                IEnumerable<OLDataInput> result = conn.Query<OLDataInput>(sql);
                var  Datalist = result.ToList();
                return Datalist;
            }
        }
        /// <summary>
        /// 配置编辑列表
        /// </summary>
        /// <param name="dataid"></param>
        /// <returns></returns>
        public List<OLFieldmodel> GetOLDataheadelist(long dataid)
        {
            List<OLFieldmodel> result = new List<OLFieldmodel>();

            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("  select * from [dbo].[OLfieldmodel] where templateId =(select templateId from OLdataInput where Id = '{0}')", dataid);
                result = conn.Query<OLFieldmodel>(sql).ToList();
            }
            return result;
        }
        /// <summary>
        /// 管理员的数据列表页
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<OLDataInput> QueryPagelist(long Id, long createdid, string number,string ischeck,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string ischecksql = "";
                if (!string.IsNullOrEmpty(ischeck))
                    ischecksql = ischeck == "1" ? " and auditor !='' " : " and auditor ='' ";
                string sql = @"select b.project_name,a.* from [OLdataInput] a left join [dbo].[OLproject] b on a.projectid=b.Id  where projectid=" + Id + "and a.created_byId=" + createdid + (!string.IsNullOrEmpty(number) ? " and jobnumber like'%" + number + "%'" : "") + ischecksql + " order by a.InsertTime desc,a.jobnumber";

                List<OLDataInput> result = conn.Query<OLDataInput>(sql.ToString()).ToList();
                return new PagedList<OLDataInput>(result, pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 根据项目id查询最后这个项目最后插入的一条数据
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public OLDataInput GetMaxDatalist(long projectid,long userId)
        {         

            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from  OLdataInput where jobnumber=(select  MAX(jobnumber) from OLdataInput where projectid='{0}' and created_byId='{1}') and  projectid='{2}'", projectid,userId, projectid);
                var result = conn.Query<OLDataInput>(sql).FirstOrDefault();
                return result;
            }
           
        }
        /// <summary>
        /// 导出项目DBF文件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetDBF(long Id)
        {
            try
            {
                List<OLDataInput> Datalist = new List<OLDataInput>();
                using (SqlConnection conn = DapperHelper.GetConnection())
                {
                    string sql = string.Format("select b.project_name,a.* from [OLdataInput] a left join [dbo].[OLproject] b on a.projectid=b.Id  where projectid='{0}' order by a.jobnumber", Id);
                    IEnumerable<OLDataInput> result = conn.Query<OLDataInput>(sql);
                    Datalist = result.ToList();
                }
                List<OLFieldmodel> list = new List<OLFieldmodel>();
                //勾选了数据则导出指定的数据        
                using (SqlConnection conn = DapperHelper.GetConnection())
                {
                    string sql = string.Format("select field_name,field_code from [dbo].[OLfieldmodel] where templateId =(select templateId from OLproject where Id='{0}' group by templateId)", Id);
                    IEnumerable<OLFieldmodel> result = conn.Query<OLFieldmodel>(sql);
                    list = result.ToList();
                }
                // 容错文件路径
                string path ="~/Areas/DataInput/Content/File/折点项目导出数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".dbf";

                string mapPath = System.Web.HttpContext.Current.Server.MapPath(path);
                if (mapPath.ToUpper().StartsWith("C:"))
                {
                    mapPath = System.Web.Hosting.HostingEnvironment.MapPath(path);
                }
                //#region 创建dbf
                var odbf = new DbfFile(Encoding.GetEncoding("GB2312"));
                odbf.Open(mapPath, FileMode.Create);
                #region 设置表头                
                odbf.Header.AddColumn(new DbfColumn("LSAT_NAME", DbfColumn.DbfColumnType.Character, 30, 0));
                odbf.Header.AddColumn(new DbfColumn("FIRST_NAME", DbfColumn.DbfColumnType.Character, 30, 0));
                odbf.Header.AddColumn(new DbfColumn("SPEC_DATE", DbfColumn.DbfColumnType.Character, 30, 0));
                odbf.Header.AddColumn(new DbfColumn("SPEC_NUM", DbfColumn.DbfColumnType.Character, 30, 0));
                //odbf.Header.AddColumn(new DbfColumn("PATIENT_ID", DbfColumn.DbfColumnType.Character, 30, 0));
                odbf.Header.AddColumn(new DbfColumn("ORGANISM", DbfColumn.DbfColumnType.Character, 30, 0));
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        odbf.Header.AddColumn(new DbfColumn(item.field_code, DbfColumn.DbfColumnType.Character, 30, 0));
                    }
                }
                            
                #endregion

                #region 填充数据
                var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                foreach (var item in Datalist)
                {                   
                    orec["LSAT_NAME"] = !string.IsNullOrWhiteSpace(item.project_name) ? item.project_name : "";
                    orec["FIRST_NAME"] = !string.IsNullOrWhiteSpace(item.experimenter) ? item.experimenter : "";
                    orec["SPEC_DATE"] = !string.IsNullOrWhiteSpace(item.experimenttime) ? DateTime.Parse(item.experimenttime).ToString("yyyy/MM/dd") : "";
                    orec["SPEC_NUM"] = !string.IsNullOrWhiteSpace(item.jobnumber) ? item.jobnumber : "";
                    //orec["PATIENT_ID"] = !string.IsNullOrWhiteSpace(item.germnumber) ? item.germnumber : "";
                    string _organism = "";
                    switch (item.germname)
                    {
                        case "大肠埃希菌":
                            _organism = "eco";
                            break;
                        case "肺炎克雷伯菌":
                            _organism = "kpn";
                            break;
                        case "卡他莫拉菌":
                            _organism = "bca";
                            break;
                        case "流感嗜血杆菌":
                            _organism = "hin";
                            break;
                        case "化脓链球菌":
                            _organism = "spy";
                            break;
                        case "无乳链球菌":
                            _organism = "sgc";
                            break;
                        case "草绿色链球菌":
                            _organism = "svi";
                            break;
                        case "肺炎链球菌":
                            _organism = "spn";
                            break;
                        default:
                            _organism = item.germname;
                            break;
                    }
                    orec["ORGANISM"] = !string.IsNullOrWhiteSpace(item.germname) ? _organism : "";
                    if (list.Count > 0)
                    {
                        string[] value = item.datevalue.Split(',');
                        for (int j = 0; j < value.Length; j++)
                        {
                            orec[list[j].field_code.ToString()] = !string.IsNullOrWhiteSpace(value[j].ToString()) ? value[j].ToString() : "";
                        }
                    }
                                 
                    odbf.Write(orec, true);
                }
                #endregion

                odbf.WriteHeader();
                odbf.Close();
                return path;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 导出上传模板excel文件
        /// </summary>
        /// <param name="Id">本次导出的项目Id</param>
        /// <param name="ep"></param>
        public string GetMoban(long Id, ExcelPackage ep)
        {        
            
                List<OLFieldmodel> title = new List<OLFieldmodel>();
                using (SqlConnection conn = DapperHelper.GetConnection())
                {
                    string sql = string.Format("select field_name from [dbo].[OLfieldmodel] where templateId =(select templateId from [OLproject] where Id ='{0}')",Id);
                    IEnumerable<OLFieldmodel> result = conn.Query<OLFieldmodel>(sql);
                    title = result.ToList();
                }              
                ExportProject(title,ep);
            
            return "折点项目上传模板.xlsx";
        }

        /// <summary>
        /// 导出上传模板excel文件
        /// </summary>
        /// <param name="title">本次导出的项目数据</param>
        /// <param name="ep"></param>
        private void ExportProject(List<OLFieldmodel> title, ExcelPackage ep)
        {
            //写入表头
            ExcelWorksheet ws = ep.Workbook.Worksheets.Add("折点项目数据");           
            ws.Cells[1, 1].Value = "工作编号";
            ws.Cells[1, 2].Value = "细菌名称";
            int n = 3;
            for (int i = 0; i < title.Count; i++)
            {
                ws.Cells[1, n].Value = title[i].field_name;
                n++;
            }                       
        }
    }
}
