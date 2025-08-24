using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ManageSystem.Core.Domain.Cre;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Data;

namespace ManageSystem.Services.CRE
{
    public class CreUploatDataService : ICreUploatDataService
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="toconfigure"></param>
        /// <returns></returns>
        public int AddCre_Data(Toconfigure toconfigure)
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
                    if (toconfigure.Germ_id == null)
                    {
                        toconfigure.Germ_id = Get_Typeshou().Germ_id;
                    }
                    ///添加时间段
                    Cre_data_period _Data_Period = new Cre_data_period();
                    _Data_Period.Germ_id = toconfigure.Germ_id;
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
                    string sql1 = string.Format("insert into Cre_data_period values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}');SELECT @@identity;", _Data_Period.Germ_id, _Data_Period.Data_year, _Data_Period.Data_season, _Data_Period.Allow_report_display, _Data_Period.Isvalid, _Data_Period.Isaudited, _Data_Period.Audited_by, _Data_Period.Audited_time, _Data_Period.Created, _Data_Period.Created_by, _Data_Period.Modified_by, _Data_Period.Modified);
                    long? Cre_id = conn.ExecuteScalar<int>(sql1.ToString(), _Data_Period, sqlTransaction);

                    ///添加联系人
                    Cre_contact cre_Contact = new Cre_contact();
                    cre_Contact.Cre_id = Cre_id;
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
                    long? Contact_id = null;
                    if (Cre_id != null)
                    {
                        string sql2 = string.Format("insert into Cre_contact values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}');SELECT @@identity;", cre_Contact.Cre_id, cre_Contact.Hospital_id, cre_Contact.Hospital, cre_Contact.Province_id, cre_Contact.Province, cre_Contact.City_id, cre_Contact.City, cre_Contact.District_id, cre_Contact.District, cre_Contact.Detailedaddress, cre_Contact.Contact, cre_Contact.Email, cre_Contact.Mobile);
                        Contact_id = conn.ExecuteScalar<int>(sql2.ToString(), cre_Contact, sqlTransaction);
                    }
                    Cre_data cre_Data = new Cre_data();
                    cre_Data.Contact_id = Contact_id;
                    cre_Data.Cre_id = Cre_id;
                    cre_Data.Germ_id = toconfigure.Germ_id;
                    cre_Data.Hospital_id = toconfigure.Hospital_id;
                    cre_Data.Germ = GetCre_germ_types(toconfigure.Germ_id);
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
                    cre_Data.Created_by = toconfigure.Created_by;
                    if (cre_Data.Contact_id != 0 && cre_Data.Cre_id != 0)
                    {
                        string sql3 = string.Format("insert into Cre_data values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}')", cre_Data.Contact_id, cre_Data.Cre_id, cre_Data.Germ_id, cre_Data.Hospital_id, cre_Data.Germ, cre_Data.Germ_detected, cre_Data.Cr_detected, cre_Data.Specimen_tan, cre_Data.Specimen_blood, cre_Data.Specimen_faeces, cre_Data.Specimen_alveolar, cre_Data.Specimen_woundpus, cre_Data.Specimen_urethra, cre_Data.Specimen_venous, cre_Data.Specimen_puncture, cre_Data.Specimen_other, cre_Data.Department_bloodsection, cre_Data.Department_icu, cre_Data.Department_breathing, cre_Data.Department_Infected, cre_Data.Department_transplant, cre_Data.Department_other, cre_Data.Sensitive, cre_Data.Hodge, cre_Data.CarbaNP, cre_Data.MCIMandeCIM, cre_Data.EDTAandAPB, cre_Data.Goldlabeled, cre_Data.PCR, cre_Data.GeneXpert, cre_Data.Carbapenemase, cre_Data.General_report, cre_Data.Report_type, cre_Data.Carbapenemase_kpc, cre_Data.Carbapenemase_ndm, cre_Data.Carbapenemase_oxa_48, cre_Data.Carbapenemase_ipm, cre_Data.Carbapenemase_vim, cre_Data.Carbapenemase_other, cre_Data.Antibiotic_polymyxin, cre_Data.Antibiotic_tegafycline, cre_Data.Antibiotic_ceftazidime, cre_Data.Antibiotic_fosfomycin, cre_Data.Antibiotic_chloramphenicol, cre_Data.Antibiotic_drugtest, cre_Data.Created_by);
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

        public List<Toconfigure> Basicinformation(long loginid)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select a.Data_year,a.Data_season,b.* from cre_data_period a,cre_contact b   where b.Cre_id=(select Cre_id from cre_data_period where Cre_id=(select Cre_id from cre_data_period where Cre_id=(select MAX(Cre_id) from cre_data_period where Created_by='{0}'))) and a.Cre_id=(select Cre_id from cre_data_period where Cre_id=(select MAX(Cre_id) from cre_data_period where Created_by='{1}'))", loginid, loginid);
                IEnumerable<Toconfigure> result = conn.Query<Toconfigure>(sql);
                return result.ToList();
            }
        }

        /// <summary>
        /// 地区下拉框绑定
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<Area> GetareList(long parentId)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from [dbo].[Area] where ParentId=@ParentId");
                IEnumerable<Area> result = conn.Query<Area>(sql, new { ParentId = parentId }).ToList();
                return result.ToList();
            }
        }

        public string GetCre_germ_types(long? ger_id)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                if (ger_id == null)
                {
                    string sql = string.Format("select top(1) * from [dbo].[Cre_germ_types] order by Sortid");
                    Cre_germ_types result = conn.Query<Cre_germ_types>(sql)?.ToList().FirstOrDefault();
                    return result.Germ_name;
                }
                else
                {
                    string sql = string.Format("select  * from [dbo].[Cre_germ_types] where Germ_id='{0}'", ger_id);
                    Cre_germ_types result = conn.Query<Cre_germ_types>(sql)?.ToList().FirstOrDefault();
                    return result.Germ_name;
                }
            }
        }

        /// <summary>
        /// 查询细菌
        /// </summary>
        /// <returns></returns>
        public List<Cre_germ_types> Get_Germ_Type()
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = "select * from [dbo].[Cre_germ_types] where Allow_data_upload=1 order by Sortid";
                IEnumerable<Cre_germ_types> result = conn.Query<Cre_germ_types>(sql);
                return result.ToList();
            }
        }
        /// <summary>
        /// 查询细菌名称
        /// </summary>
        /// <param name="germ_id"></param>
        /// <returns></returns>
        public List<Cre_germ_types> Get_Germ_Types(long? germ_id)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {

                if (germ_id == null)
                {
                    string sql1 = "select top(1) * from [dbo].[Cre_germ_types] order by Sortid";
                    IEnumerable<Cre_germ_types> results = conn.Query<Cre_germ_types>(sql1);
                    return results.ToList();
                }
                string sql = "select * from [dbo].[Cre_germ_types] where Germ_id=@Germ_id";
                IEnumerable<Cre_germ_types> result = conn.Query<Cre_germ_types>(sql, new { Germ_id = germ_id });
                return result.ToList();
            }
        }

        public Cre_germ_types Get_Typeshou()
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select top(1) * from [dbo].[Cre_germ_types] order by Sortid");
                Cre_germ_types result = conn.Query<Cre_germ_types>(sql)?.ToList().FirstOrDefault();
                return result;
            }
        }

        /// <summary>
        /// 数据返填
        /// </summary>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <param name="Germ_ids"></param>
        /// <returns></returns>
        public List<Toconfigure> Getdata_show(string year, string season, long? Germ_ids, long? Created_by)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                if (Germ_ids == null)
                {
                    string sql1 = string.Format("SELECT * FROM cre_data_period a, [dbo].[Cre_data] b where a.Data_year='{0}' and  a.Data_season='{1}' and b.Germ_id=(select top(1) Germ_id from [dbo].[Cre_germ_types] where Allow_data_upload=1 order by Sortid) and b.Cre_id=a.Cre_id and a.Germ_id=b.Germ_id and b.Created_by='{2}'", year, season, Created_by);
                    IEnumerable<Toconfigure> result1 = conn.Query<Toconfigure>(sql1).ToList();
                    return result1.ToList();
                }
                else
                {
                    string sql = string.Format("SELECT * FROM cre_data_period a, [dbo].[Cre_data] b where a.Data_year='{0}' and  a.Data_season='{1}' and b.Germ_id='{2}' and b.Cre_id=a.Cre_id and a.Germ_id=b.Germ_id and b.Created_by='{3}'", year, season, Germ_ids, Created_by);
                    IEnumerable<Toconfigure> result = conn.Query<Toconfigure>(sql);
                    return result.ToList();
                }
            }
        }
        /// <summary>
        /// 修改数据
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
                    ///添加时间段
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
                    string sql1 = string.Format("update  cre_data_period set Modified_by='{0}',Modified='{1}' where Cre_id='{2}'", _Data_Period.Modified_by, _Data_Period.Modified,toconfigure.Cre_id);
                    int results = conn.Execute(sql1.ToString(), _Data_Period, sqlTransaction);
                    int resul = 0;

                    ///添加联系人
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
        /// 查询数据是否存在
        /// </summary>
        /// <param name="Germ_id"></param>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <param name="memberid"></param>
        /// <returns></returns>
        public Cre_data_period GetPeriod(long? Germ_id, string year, string season, long? memberid)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                    string sql = string.Format("select * from Cre_data_period where Germ_id='{0}'and Data_year='{1}'and Data_season='{2}' and Created_by='{3}'", Germ_id, year, season, memberid);
                    Cre_data_period result = conn.Query<Cre_data_period>(sql)?.ToList().FirstOrDefault();
                    return result;
            }
        }
        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="heatmp_Log"></param>
        public void AddCreProject_log(CreProject_log heatmp_Log)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("insert into CreProject_log values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", heatmp_Log.BrowserName, heatmp_Log.UserinfoId, heatmp_Log.UserinfoName, heatmp_Log.Content, heatmp_Log.Detail, heatmp_Log.Type, heatmp_Log.InsertTime, heatmp_Log.UpdateTime, heatmp_Log.DeleteTime, heatmp_Log.CREdataId);
                int result = conn.Execute(sql.ToString(), heatmp_Log);
            }
        }       
    }
}
