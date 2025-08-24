using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Cre;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Data;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.CRE
{
    public class CreBackstageService : ICreBackstageService
    {
        /// <summary>
        /// 导出execl
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="ep"></param>
        /// <returns></returns>
        public string Export(List<long> ids, ExcelPackage ep)
        {
                   
            List<Toconfigure> list = new List<Toconfigure>();
            if (ids != null && ids.Any())
            {
                string id = "";
                for (int i = 0; i < ids.Count; i++)
                {
                    id += ids[i] + ",";
                }
                id = id.TrimEnd(',');
                //勾选了数据则导出指定的数据        
                using (SqlConnection conn = DapperHelper.GetConnection())
                {               
                        string sql = string.Format("select * from  Cre_data_period a,Cre_contact b,Cre_data c,Cre_germ_types d where  c.Cre_id in({0}) and b.Cre_id in(select Cre_id from Cre_data where Cre_id in({1})) and a.Cre_id in(select Cre_id from Cre_data where Cre_id in({2})) and c.Germ_id=d.Germ_id and b.Cre_id=c.Cre_id and a.Cre_id=c.Cre_id",id,id,id);
                    IEnumerable<Toconfigure> result = conn.Query<Toconfigure>(sql);
                           list = result.ToList();
                }                     
            }
            else
            {
                using (SqlConnection conn = DapperHelper.GetConnection())
                {
                    string sql = string.Format("select * from  Cre_data_period a,Cre_contact b,Cre_data c,Cre_germ_types d where  c.Germ_id=d.Germ_id and b.Cre_id=c.Cre_id and a.Cre_id=c.Cre_id");
                    IEnumerable <Toconfigure> result = conn.Query<Toconfigure>(sql);
                    list = result.ToList();
                }                
            }
            List<Toconfigure> lists = new List<Toconfigure>();
            if (ids != null && ids.Any())
            {
                string id = "";
                for (int i = 0; i < ids.Count; i++)
                {
                    id += ids[i] + ",";
                }
                id = id.TrimEnd(',');

                //勾选了数据则导出指定的数据        
                using (SqlConnection conn = DapperHelper.GetConnection())
                {                  
                        string sql = string.Format(@"select b.Hospital,b.Province,b.City,b.District,b.Detailedaddress,b.Contact,b.Mobile,b.Email,a.Modified INTO #TMP from  Cre_data_period a,Cre_contact b where a.Cre_id=b.Cre_id and b.Cre_id in({0}) group by b.Hospital,b.Province,b.City,b.District,b.Detailedaddress,b.Contact,b.Mobile,b.Email,a.Modified,a.Cre_id order by a.Modified desc
select Hospital,Province,City,District,Detailedaddress,Contact,Mobile,Email from  #TMP group by Hospital,Province,City,District,Detailedaddress,Contact,Mobile,Email", id);
                    IEnumerable<Toconfigure> result = conn.Query<Toconfigure>(sql);
                    lists = result.ToList();
                }
            }
            else
            {
                using (SqlConnection conn = DapperHelper.GetConnection())
                {
                    string sql = string.Format(@"select b.Hospital,b.Province,b.City,b.District,b.Detailedaddress,b.Contact,b.Mobile,b.Email,a.Modified INTO #TMP from  Cre_data_period a,Cre_contact b where a.Cre_id=b.Cre_id group by b.Hospital,b.Province,b.City,b.District,b.Detailedaddress,b.Contact,b.Mobile,b.Email,a.Modified,a.Cre_id order by a.Modified desc
select Hospital, Province, City, District, Detailedaddress, Contact, Mobile, Email from  #TMP group by Hospital,Province,City,District,Detailedaddress,Contact,Mobile,Email");
                    IEnumerable<Toconfigure> result = conn.Query<Toconfigure>(sql);
                    lists = result.ToList();
                }
            }
            //查询医院上传清单
            List <Toconfigure> lis = new List<Toconfigure>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select a.Hospital,b.Data_year,b.Data_season,a.Province from Cre_contact a, Cre_data_period b where a.Cre_id=b.Cre_id group by b.Data_year,b.Data_season,a.Hospital,a.Province order by b.Data_year,b.Data_season");
                IEnumerable<Toconfigure> result = conn.Query<Toconfigure>(sql);
                lis = result.ToList();
            }

            //获取导出的明细数据
            //var itemList = this.ProjectItemService.Query().Where(p => list.Any(p2 => p2.Id == p.ProjectId)).OrderBy(p => p.ProjectId).ToList() ?? new List<CRProjectItem>();

            //1、第一个sheet是医院上传的明细数据
            this.ExportProjectItem(list, ep);

            //2、第二个sheet是医院联系信息数据
            this.ExportProject(lists, ep);
            //3、第三个sheet是统计上传医院的数量
            this.ExportItem(lis, ep);

            return "CRE检测数据_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        }

        /// <summary>
        /// 导出医院相关信息
        /// </summary>
        /// <param name="list">本次导出的项目数据</param>
        /// <param name="ep"></param>
        private void ExportProject(List<Toconfigure> list, ExcelPackage ep)
        {
            //写入表头
            ExcelWorksheet ws = ep.Workbook.Worksheets.Add("医院联系信息");
            ws.Cells[1, 1].Value = "医院名称";
            ws.Cells[1, 2].Value = "省份";
            ws.Cells[1, 3].Value = "城市";
            ws.Cells[1, 4].Value = "地区";
            ws.Cells[1, 5].Value = "详细地址";
            ws.Cells[1, 6].Value = "负责人";
            ws.Cells[1, 7].Value = "联系电话";
            ws.Cells[1, 8].Value = "邮箱地址";
            int index = 2;
            foreach (var item in list)
            {
                ws.Cells[index, 1].Value = item.Hospital;
                ws.Cells[index, 2].Value = item.Province;
                ws.Cells[index, 3].Value = item.City;
                ws.Cells[index, 4].Value = item.District;
                ws.Cells[index, 5].Value = item.Detailedaddress;
                ws.Cells[index, 6].Value = item.Contact;
                ws.Cells[index, 7].Value = item.Mobile;
                ws.Cells[index, 8].Value = item.Email;                
                index++;
            }
        }

        /// <summary>
        /// 导出CRE项目明细表中的数据
        /// </summary>
        /// <param name="list">本次导出的明细数据</param>       
        /// <param name="ep"></param>
        private void ExportProjectItem(List<Toconfigure> list, ExcelPackage ep)
        {
            //写入表头
            ExcelWorksheet ws = ep.Workbook.Worksheets.Add("CRE数据");
            ws.Cells[1, 1].Value = "上传时间";
            ws.Cells[1, 2].Value = "医院名称";
            ws.Cells[1, 3].Value = "年度";
            ws.Cells[1, 4].Value = "季度";
            ws.Cells[1, 5].Value = "细菌名称";
            ws.Cells[1, 6].Value = "检出株数";
            ws.Cells[1, 7].Value = "耐药株数";
            ws.Cells[1, 8].Value = "痰";
            ws.Cells[1, 9].Value = "血液";
            ws.Cells[1, 10].Value = "粪便";
            ws.Cells[1, 11].Value = "肺泡灌洗液";
            ws.Cells[1, 12].Value = "脓液/伤口";
            ws.Cells[1, 13].Value = "尿道";
            ws.Cells[1, 14].Value = "中心静脉导管";
            ws.Cells[1, 15].Value = "穿刺液";
            ws.Cells[1, 16].Value = "其他";
            ws.Cells[1, 17].Value = "血液科";
            ws.Cells[1, 18].Value = "ICU";
            ws.Cells[1, 19].Value = "呼吸科";
            ws.Cells[1, 20].Value = "感染科";
            ws.Cells[1, 21].Value = "移植科";
            ws.Cells[1, 22].Value = "其他";
            ws.Cells[1, 23].Value = "目前实验室检测碳青霉烯酶的方法";
            ws.Cells[1, 24].Value = "是否报告";
            ws.Cells[1, 25].Value = "报告方式";
            ws.Cells[1, 26].Value = "KPC";
            ws.Cells[1, 27].Value = "NDM";
            ws.Cells[1, 28].Value = "OXA-48";
            ws.Cells[1, 29].Value = "IPM";
            ws.Cells[1, 30].Value = "VIM";
            ws.Cells[1, 31].Value = "其他";
            ws.Cells[1, 32].Value = "(1) 多黏菌素";
            ws.Cells[1, 33].Value = "(2) 替加环素";
            ws.Cells[1, 34].Value = "(3) 头孢他啶-阿维巴坦";
            ws.Cells[1, 35].Value = "(4) 磷霉素";
            ws.Cells[1, 36].Value = "(5) 氯霉素";
            ws.Cells[1, 37].Value = "(6) 联合药敏试验";
            int index = 2;
            foreach (var item in list)
            {
                ws.Cells[index, 1].Value = item.Created.ToString();
                ws.Cells[index, 2].Value = item.Hospital;
                ws.Cells[index, 3].Value = item.Data_year;
                ws.Cells[index, 4].Value = item.Data_season;
                ws.Cells[index, 5].Value = item.Germ_upload_name;
                ws.Cells[index, 6].Value = item.Germ_detected;
                ws.Cells[index, 7].Value = item.Cr_detected;
                ws.Cells[index, 8].Value = item.Specimen_tan;
                ws.Cells[index, 9].Value = item.Specimen_blood;
                ws.Cells[index, 10].Value = item.Specimen_faeces;
                ws.Cells[index, 11].Value = item.Specimen_alveolar;
                ws.Cells[index, 12].Value = item.Specimen_woundpus;
                ws.Cells[index, 13].Value = item.Specimen_urethra;
                ws.Cells[index, 14].Value = item.Specimen_venous;
                ws.Cells[index, 15].Value = item.Specimen_puncture;
                ws.Cells[index, 16].Value = item.Specimen_other;
                ws.Cells[index, 17].Value = item.Department_bloodsection;
                ws.Cells[index, 18].Value = item.Department_icu;
                ws.Cells[index, 19].Value = item.Department_breathing;
                ws.Cells[index, 20].Value = item.Department_Infected;
                ws.Cells[index, 21].Value = item.Department_transplant;
                ws.Cells[index, 22].Value = item.Department_other;
                ws.Cells[index, 23].Value = item.Sensitive+item.Hodge+item.CarbaNP+item.MCIMandeCIM+item.EDTAandAPB+item.Goldlabeled+item.PCR+item.GeneXpert+item.Carbapenemase;
                if (item.GeneXpert == "1")
                {
                    ws.Cells[index, 24].Value = "是";
                } else
                {
                    ws.Cells[index, 24].Value = "否";
                }
                ws.Cells[index, 25].Value = item.Report_type;
                ws.Cells[index, 26].Value = item.Carbapenemase_kpc;
                ws.Cells[index, 27].Value = item.Carbapenemase_ndm;
                ws.Cells[index, 28].Value = item.Carbapenemase_oxa_48;
                ws.Cells[index, 29].Value = item.Carbapenemase_ipm;
                ws.Cells[index, 30].Value = item.Carbapenemase_vim;
                ws.Cells[index, 31].Value = item.Carbapenemase_other;
                ws.Cells[index, 32].Value = item.Antibiotic_polymyxin;
                ws.Cells[index, 33].Value = item.Antibiotic_tegafycline;
                ws.Cells[index, 34].Value = item.Antibiotic_ceftazidime;
                ws.Cells[index, 35].Value = item.Antibiotic_fosfomycin;
                ws.Cells[index, 36].Value = item.Antibiotic_chloramphenicol;
                ws.Cells[index, 37].Value = item.Antibiotic_drugtest;            
                index++;
            }
        }

        /// <summary>
        /// 导出上传医院清单
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ep"></param>
        private void ExportItem(List<Toconfigure> list, ExcelPackage ep)
        {
            //写入表头
            ExcelWorksheet ws = ep.Workbook.Worksheets.Add("已上传医院清单");
            ws.Cells[1, 1].Value = "医院名称";
            ws.Cells[1, 2].Value = "年份";
            ws.Cells[1, 3].Value = "季度";
            ws.Cells[1, 4].Value = "省份";
            int index = 2;
            foreach (var item in list)
            {
                ws.Cells[index, 1].Value = item.Hospital;
                ws.Cells[index, 2].Value = item.Data_year;
                ws.Cells[index, 3].Value = item.Data_season;
                ws.Cells[index, 4].Value = item.Province;
                index++;
            }
        }

        /// <summary>
        /// 查询百分比
        /// </summary>
        /// <returns></returns>
        public List<Cre_data_percentage> GetData_Percentages()
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select Percentages from Cre_data_percentage");
                List<Cre_data_percentage> result = conn.Query<Cre_data_percentage>(sql).ToList();                                            
                return result.ToList();
            }
        }

        /// <summary>
        /// 详情页数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Toconfigure GetToconfigure(long? id)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from  Cre_data_period a,Cre_contact b,Cre_data c,Cre_germ_types d where  c.Cre_id='{0}' and b.Cre_id=(select Cre_id from Cre_data where Cre_id='{1}') and a.Cre_id=(select Cre_id from Cre_data where Cre_id='{2}') and c.Germ_id=d.Germ_id", id,id,id);
                Toconfigure result = conn.Query<Toconfigure>(sql).FirstOrDefault();
                return result;
            }
        }

        /// <summary>
        /// 列表页数据
        /// </summary>
        /// <param name="Hospital"></param>
        /// <param name="Data_year"></param>
        /// <param name="Name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<Toconfigure> GetToconfigures(string Hospital, string Data_year, string Name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"SELECT c.Cre_id,c.Data_id,b.Hospital,a.Data_year,a.Data_season,c.Germ,c.Germ_detected,c.Cr_detected,c.Carbapenemase_kpc,c.Carbapenemase_ndm,a.Created,a.Modified,m.Name,a.Isvalid,a.Allow_report_display,a.Isaudited FROM  Cre_data_period a,Cre_contact b,Cre_data c,Member m  where a.Cre_id=c.Cre_id and b.Cre_id=c.Cre_id and a.Cre_id=b.Cre_id and a.Created_by=m.Id and a.Allow_report_display=1 and a.Isvalid=1 and a.Isaudited=1 order by a.Created desc";
                IEnumerable<Toconfigure> result = conn.Query<Toconfigure>(sql.ToString());
                IEnumerable<Toconfigure> query = from m in result
                                                  where m.Isvalid == 1
                                                  select m;
                if (!string.IsNullOrWhiteSpace(Hospital))
                {
                    query = from m in query
                            where m.Hospital.Contains(Hospital)
                            select m;
                }
                if (!string.IsNullOrWhiteSpace(Data_year))
                {
                    query = from m in query
                            where m.Data_year.Equals(Data_year)
                            select m;
                }
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    query = from m in query
                            where m.Name.Contains(Name)
                            select m;
                }
                return new PagedList<Toconfigure>(query.ToList(), pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="ids"></param>
        public virtual void Delete(string ids)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                if (string.IsNullOrEmpty(ids)) return;
                ids = ids.TrimEnd(',');
                var cities = ids.Split(',');
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                IDbTransaction sqlTransaction = conn.BeginTransaction();
                int result =0;
                try
                {
                    for (int i = 0; i < cities.Length; i++)
                    {
                        string sql = string.Format("DELETE FROM Cre_data_period WHERE Cre_id='{0}'", cities[i]);
                        result = conn.Execute(sql,null, sqlTransaction);                       
                        string sql1 = string.Format("DELETE FROM Cre_contact WHERE Cre_id='{0}'", cities[i]);
                        result = conn.Execute(sql1,null, sqlTransaction);                       
                        string sql2 = string.Format("DELETE FROM Cre_data WHERE Cre_id='{0}'", cities[i]);
                        result = conn.Execute(sql2,null,sqlTransaction);
                    }
                    sqlTransaction.Commit();//简化的转换写法
                }
                catch (Exception)
                {
                    sqlTransaction.Rollback();
                    throw;
                }               
            }
        }
        /// <summary>
        /// 查询细菌列表
        /// </summary>
        /// <returns></returns>
        public List<Cre_germ_types> Getgerm_types()
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select Germ_name from Cre_germ_types");
                IEnumerable<Cre_germ_types> result = conn.Query<Cre_germ_types>(sql);
                return result.ToList();
            }
        }
        /// <summary>
        /// 修改cre数据
        /// </summary>
        /// <param name="toconfigure"></param>
        /// <returns></returns>
        public int UpdateCre_data(Toconfigure toconfigure)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int result = 0;
                IDbTransaction sqlTransaction = conn.BeginTransaction();
                try
                {
                    ///修改时间段
                    Cre_data_period _Data_Period = new Cre_data_period();
                    _Data_Period.Data_year = toconfigure.Data_year;
                    _Data_Period.Data_season = toconfigure.Data_season;
                    _Data_Period.Allow_report_display = toconfigure.Allow_report_display;
                    _Data_Period.Isvalid = toconfigure.Isvalid;
                    _Data_Period.Isaudited = toconfigure.Isaudited;
                    _Data_Period.Audited_by = toconfigure.Audited_by;
                    _Data_Period.Audited_time = toconfigure.Audited_time;
                    _Data_Period.Created = toconfigure.Created;
                    _Data_Period.Created_by = toconfigure.Created_by;
                    _Data_Period.Modified_by = toconfigure.Modified_by;
                    _Data_Period.Modified = toconfigure.Modified;
                    string sql1 = string.Format("update  cre_data_period set Modified_by='{0}',Modified='{1}',Data_year='{2}',Data_season='{3}'where Cre_id='{4}'", _Data_Period.Modified_by, _Data_Period.Modified,_Data_Period.Data_year,_Data_Period.Data_season,toconfigure.Cre_id);
                    int results = conn.Execute(sql1.ToString(), _Data_Period, sqlTransaction);
                    int resul = 0;

                    ///修改联系人
                    Cre_contact cre_Contact = new Cre_contact();
                    cre_Contact.Hospital_id = toconfigure.Hospital_id;
                    cre_Contact.Hospital = toconfigure.Hospital;
                    cre_Contact.Province_id = toconfigure.Province_id;
                    string Province1 = string.Format("select * from [dbo].[Area] where Id=@Id");
                    Area Province = conn.Query<Area>(Province1, new { Id = toconfigure.Province_id }, sqlTransaction)?.ToList().FirstOrDefault();
                    cre_Contact.Province = Province.ShortName;
                    cre_Contact.City_id = toconfigure.City_id;
                    string City1 = string.Format("select * from [dbo].[Area] where Id=@Id");
                    Area City = conn.Query<Area>(City1, new { Id = toconfigure.City_id }, sqlTransaction)?.ToList().FirstOrDefault();
                    if (City.Name == "上海市" || City.Name == "天津市" || City.Name == "重庆市" || City.Name == "北京市")
                    {
                        string Dis = string.Format("select * from [dbo].[Area] where Id=@Id");
                        Area Dist = conn.Query<Area>(Dis, new { Id = toconfigure.District_id }, sqlTransaction)?.ToList().FirstOrDefault();
                        cre_Contact.City = Dist.Name;
                    }
                    else
                    {
                        cre_Contact.City = City.Name;
                    }
                    cre_Contact.District_id = toconfigure.District_id;
                    string District1 = string.Format("select * from [dbo].[Area] where Id=@Id");
                    Area District = conn.Query<Area>(District1, new { Id = toconfigure.District_id }, sqlTransaction)?.ToList().FirstOrDefault();
                    cre_Contact.District = District.Name;
                    cre_Contact.Detailedaddress = toconfigure.Detailedaddress;
                    cre_Contact.Contact = toconfigure.Contact;
                    cre_Contact.Email = toconfigure.Email;
                    cre_Contact.Mobile = toconfigure.Mobile;
                    if (results > 0)
                    {
                        string sql2 = string.Format("update  Cre_contact set Hospital_id='{0}', Hospital='{1}',Province_id='{2}',Province='{3}',City_id='{4}',City='{5}',District_id='{6}',District='{7}',Detailedaddress='{8}',Contact='{9}',Email='{10}',Mobile='{11}' where Cre_id='{12}'", cre_Contact.Hospital_id, cre_Contact.Hospital, cre_Contact.Province_id, cre_Contact.Province, cre_Contact.City_id, cre_Contact.City, cre_Contact.District_id, cre_Contact.District, cre_Contact.Detailedaddress, cre_Contact.Contact, cre_Contact.Email, cre_Contact.Mobile, toconfigure.Cre_id);
                        resul = conn.Execute(sql2.ToString(), cre_Contact, sqlTransaction);
                    }

                    Cre_data cre_Data = new Cre_data();
                    cre_Data.Germ_id = toconfigure.Germ_id;
                    cre_Data.Hospital_id = toconfigure.Hospital_id;
                    cre_Data.Germ_detected = toconfigure.Germ_detected;
                    cre_Data.Cr_detected = toconfigure.Cr_detected;
                    if (cre_Data.Cr_detected == 0)
                    {
                        cre_Data.Specimen_tan = null;
                        cre_Data.Specimen_blood = null;
                        cre_Data.Specimen_faeces = null;
                        cre_Data.Specimen_alveolar = null;
                        cre_Data.Specimen_woundpus = null;
                        cre_Data.Specimen_urethra = null;
                        cre_Data.Specimen_venous = null;
                        cre_Data.Specimen_puncture = null;
                        cre_Data.Specimen_other = null;
                        cre_Data.Department_bloodsection = null;
                        cre_Data.Department_icu = null;
                        cre_Data.Department_breathing = null;
                        cre_Data.Department_Infected = null;
                        cre_Data.Department_transplant = null;
                        cre_Data.Department_other = null;
                        cre_Data.Sensitive = null;
                        cre_Data.Hodge = null;
                        cre_Data.CarbaNP = null;
                        cre_Data.MCIMandeCIM = null;
                        cre_Data.EDTAandAPB = null;
                        cre_Data.Goldlabeled = null;
                        cre_Data.PCR = null;
                        cre_Data.GeneXpert = null;
                        cre_Data.Carbapenemase = null;
                        cre_Data.General_report = 0;
                        cre_Data.Report_type = null;
                        cre_Data.Carbapenemase_kpc = 0;
                        cre_Data.Carbapenemase_ndm = 0;
                        cre_Data.Carbapenemase_oxa_48 = 0;
                        cre_Data.Carbapenemase_ipm = 0;
                        cre_Data.Carbapenemase_vim = 0;
                        cre_Data.Carbapenemase_other = 0;
                        cre_Data.Antibiotic_polymyxin = 0;
                        cre_Data.Antibiotic_tegafycline = 0;
                        cre_Data.Antibiotic_ceftazidime = 0;
                        cre_Data.Antibiotic_fosfomycin = 0;
                        cre_Data.Antibiotic_chloramphenicol = 0;
                        cre_Data.Antibiotic_drugtest = 0;
                    }
                    else
                    {
                        cre_Data.Specimen_tan = toconfigure.Specimen_tan;
                        cre_Data.Specimen_blood = toconfigure.Specimen_blood;
                        cre_Data.Specimen_faeces = toconfigure.Specimen_faeces;
                        cre_Data.Specimen_alveolar = toconfigure.Specimen_alveolar;
                        cre_Data.Specimen_woundpus = toconfigure.Specimen_woundpus;
                        cre_Data.Specimen_urethra = toconfigure.Specimen_urethra;
                        cre_Data.Specimen_venous = toconfigure.Specimen_venous;
                        cre_Data.Specimen_puncture = toconfigure.Specimen_other;
                        cre_Data.Specimen_other = toconfigure.Specimen_other;
                        cre_Data.Department_bloodsection = toconfigure.Department_bloodsection;
                        cre_Data.Department_icu = toconfigure.Department_icu;
                        cre_Data.Department_breathing = toconfigure.Department_breathing;
                        cre_Data.Department_Infected = toconfigure.Department_Infected;
                        cre_Data.Department_transplant = toconfigure.Department_transplant;
                        cre_Data.Department_other = toconfigure.Department_other;
                        cre_Data.Sensitive = toconfigure.Sensitive;
                        cre_Data.Hodge = toconfigure.Hodge;
                        cre_Data.CarbaNP = toconfigure.CarbaNP;
                        cre_Data.MCIMandeCIM = toconfigure.MCIMandeCIM;
                        cre_Data.EDTAandAPB = toconfigure.EDTAandAPB;
                        cre_Data.Goldlabeled = toconfigure.Goldlabeled;
                        cre_Data.PCR = toconfigure.PCR;
                        cre_Data.GeneXpert = toconfigure.GeneXpert;
                        cre_Data.Carbapenemase = toconfigure.Carbapenemase;
                        cre_Data.General_report = toconfigure.General_report;
                        cre_Data.Report_type = toconfigure.Report_type;
                        cre_Data.Carbapenemase_kpc = toconfigure.Carbapenemase_kpc;
                        cre_Data.Carbapenemase_ndm = toconfigure.Carbapenemase_ndm;
                        cre_Data.Carbapenemase_oxa_48 = toconfigure.Carbapenemase_oxa_48;
                        cre_Data.Carbapenemase_ipm = toconfigure.Carbapenemase_ipm;
                        cre_Data.Carbapenemase_vim = toconfigure.Carbapenemase_vim;
                        cre_Data.Carbapenemase_other = toconfigure.Carbapenemase_other;
                        cre_Data.Antibiotic_polymyxin = toconfigure.Antibiotic_polymyxin;
                        cre_Data.Antibiotic_tegafycline = toconfigure.Antibiotic_tegafycline;
                        cre_Data.Antibiotic_ceftazidime = toconfigure.Antibiotic_ceftazidime;
                        cre_Data.Antibiotic_fosfomycin = toconfigure.Antibiotic_fosfomycin;
                        cre_Data.Antibiotic_chloramphenicol = toconfigure.Antibiotic_chloramphenicol;
                        cre_Data.Antibiotic_drugtest = toconfigure.Antibiotic_drugtest;
                    }
                    if (resul > 0 && results > 0)
                    {
                        string sql3 = string.Format("update Cre_data set Germ_detected='{0}',Cr_detected='{1}',Specimen_tan='{2}',Specimen_blood='{3}',Specimen_faeces='{4}',Specimen_alveolar='{5}',Specimen_woundpus='{6}',Specimen_urethra='{7}',Specimen_venous='{8}',Specimen_puncture='{9}',Specimen_other='{10}',Department_bloodsection='{11}',Department_icu='{12}',Department_breathing='{13}',Department_Infected='{14}',Department_transplant='{15}',Department_other='{16}',Sensitive='{17}',Hodge='{18}',CarbaNP='{19}',MCIMandeCIM='{20}',EDTAandAPB='{21}',Goldlabeled='{22}',PCR='{23}',GeneXpert='{24}',Carbapenemase='{25}',General_report='{26}',Report_type='{27}',Carbapenemase_kpc='{28}',Carbapenemase_ndm='{29}',Carbapenemase_oxa_48='{30}',Carbapenemase_ipm='{31}',Carbapenemase_vim='{32}',Carbapenemase_other='{33}',Antibiotic_polymyxin='{34}',Antibiotic_tegafycline='{35}',Antibiotic_ceftazidime='{36}',Antibiotic_fosfomycin='{37}',Antibiotic_chloramphenicol='{38}',Antibiotic_drugtest='{39}' where Cre_id='{40}'", cre_Data.Germ_detected, cre_Data.Cr_detected, cre_Data.Specimen_tan, cre_Data.Specimen_blood, cre_Data.Specimen_faeces, cre_Data.Specimen_alveolar, cre_Data.Specimen_woundpus, cre_Data.Specimen_urethra, cre_Data.Specimen_venous, cre_Data.Specimen_puncture, cre_Data.Specimen_other, cre_Data.Department_bloodsection, cre_Data.Department_icu, cre_Data.Department_breathing, cre_Data.Department_Infected, cre_Data.Department_transplant, cre_Data.Department_other, cre_Data.Sensitive, cre_Data.Hodge, cre_Data.CarbaNP, cre_Data.MCIMandeCIM, cre_Data.EDTAandAPB, cre_Data.Goldlabeled, cre_Data.PCR, cre_Data.GeneXpert, cre_Data.Carbapenemase, cre_Data.General_report, cre_Data.Report_type, cre_Data.Carbapenemase_kpc, cre_Data.Carbapenemase_ndm, cre_Data.Carbapenemase_oxa_48, cre_Data.Carbapenemase_ipm, cre_Data.Carbapenemase_vim, cre_Data.Carbapenemase_other, cre_Data.Antibiotic_polymyxin, cre_Data.Antibiotic_tegafycline, cre_Data.Antibiotic_ceftazidime, cre_Data.Antibiotic_fosfomycin, cre_Data.Antibiotic_chloramphenicol, cre_Data.Antibiotic_drugtest, toconfigure.Cre_id);
                        result = conn.Execute(sql3.ToString(), cre_Data, sqlTransaction);
                    }
                    sqlTransaction.Commit();//简化的转换写法
                }
                catch (Exception)
                {
                    sqlTransaction.Rollback();
                    throw;
                }

                return result;
            }
        }
        /// <summary>
        /// 细菌列表
        /// </summary>
        /// <param name="Germ_name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<Cre_germ_types> GetBacteriaList(string Germ_name,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"select * from Cre_germ_types";
                IEnumerable<Cre_germ_types> result = conn.Query<Cre_germ_types>(sql.ToString());
                IEnumerable<Cre_germ_types> query = from m in result
                                                 select m;
                if (!string.IsNullOrWhiteSpace(Germ_name))
                {
                    query = from m in query
                            where m.Germ_name.Contains(Germ_name)
                            select m;
                }               
                return new PagedList<Cre_germ_types>(query.ToList(), pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 查询最大排序号
        /// </summary>
        /// <returns></returns>
        public int GetMaxsort()
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"select MAX(Sortid) sort from Cre_germ_types";
                int result = conn.Query<Cre_germ_types>(sql.ToString()).FirstOrDefault().sort;
                return result;
            }
        }
        /// <summary>
        /// 添加细菌
        /// </summary>
        /// <param name="germ_Types"></param>
        /// <returns></returns>
        public int CreateBacteria(Cre_germ_types germ_Types)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("insert into Cre_germ_types values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",germ_Types.Ger_code,germ_Types.Germ_upload_name,germ_Types.Germ_name,germ_Types.Cr_name,germ_Types.Sortid,germ_Types.allow_edit,germ_Types.Allow_report_display,germ_Types.Allow_data_upload,germ_Types.Force_audit,germ_Types.Auditor_email,germ_Types.User_id,germ_Types.InsrtTime,germ_Types.UpdateTime);
                 int result = conn.Execute(sql.ToString());
                return result;
            }
        }
        /// <summary>
        /// 根据id查询细菌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cre_germ_types GetCre_germ_types(long? id)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select *  from Cre_germ_types where Germ_id='{0}'", id);
                Cre_germ_types result = conn.Query<Cre_germ_types>(sql).FirstOrDefault();
                return result;
            }
        }
        /// <summary>
        /// 删除细菌
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteBacteria(string ids)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                if (string.IsNullOrEmpty(ids)) return;
                ids = ids.TrimEnd(',');
                var cities = ids.Split(',');
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                IDbTransaction sqlTransaction = conn.BeginTransaction();
                int result = 0;
                try
                {
                    for (int i = 0; i < cities.Length; i++)
                    {                       
                        string sql = string.Format("update Cre_germ_types set State=0  WHERE Germ_id='{0}'", cities[i]);
                        result = conn.Execute(sql, null, sqlTransaction);
                        sqlTransaction.Commit();//简化的转换写法
                    }
                }
                catch (Exception)
                {
                    sqlTransaction.Rollback();
                    throw;
                }
            }
        }

        public int UpdateBacteria(Cre_germ_types germ_Types)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                IDbTransaction sqlTransaction = conn.BeginTransaction();
                try
                {
                    string sql = string.Format("update Cre_germ_types set Ger_code='{0}',Germ_upload_name='{1}',Germ_name='{2}',Cr_name='{3}',Sortid='{4}',allow_edit='{5}', Allow_data_upload='{6}',Allow_report_display='{7}',UpdateTime='{8}' WHERE Germ_id='{9}'", germ_Types.Ger_code, germ_Types.Germ_upload_name, germ_Types.Germ_name, germ_Types.Cr_name, germ_Types.Sortid, germ_Types.allow_edit, germ_Types.Allow_data_upload, germ_Types.Allow_report_display, germ_Types.UpdateTime, germ_Types.Germ_id);
                    int result = conn.Execute(sql, null, sqlTransaction);
                    sqlTransaction.Commit();//简化的转换写法
                    return result;
                }
                catch (Exception)
                {
                    sqlTransaction.Rollback();
                    throw;
                }
              
            }
        }

        public List<Cre_germ_types> GetSort(int Sort)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select *  from Cre_germ_types where [Sortid]='{0}'", Sort);
                IEnumerable<Cre_germ_types> result = conn.Query<Cre_germ_types>(sql);
                return result.ToList(); ;
            }
        }

        public int UpdateSort(int Sort)
        {
            string sql = string.Format("update Cre_germ_types set sort=sort+1 where sort>='{0}'", Sort);
            return DapperHelper.GetConnection().Execute(sql);
        }
    }
}
