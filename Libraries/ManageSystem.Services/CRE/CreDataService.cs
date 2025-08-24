using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ManageSystem.Core.Domain.Cre;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Data;

namespace ManageSystem.Services.CRE
{
   public class CreDataService : ICreDataService
    {
        /// <summary>
        /// 第二级数据显示
        /// </summary>
        /// <returns></returns>
        public List<Toconfigure> CGetdata_Show(string year, string Germ_id)
        {
            var dateyear = DateTime.Now.Year.ToString();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                if (year == null && Germ_id == null)
                {
                    string sql = @"SELECT cred.germ_id,crep.Data_year,crec.Province,crec.Province_id,crec.City,crec.City_id, sum(Carbapenemase_kpc) as kpc,sum(Carbapenemase_ndm) as ndm,sum(Cr_detected) as cr,
round(cast(sum(Carbapenemase_kpc) as float)/cast(sum(Cr_detected) as float),4 ) *100 as ratekpc,
round(cast(sum(Carbapenemase_ndm) as float )/cast(sum(Cr_detected) as float),4 ) *100 as ratendm
FROM [dbo].[Cre_data] cred,
[dbo].[Cre_Contact] crec,
[dbo].[Cre_data_period] crep
where crec.Contact_id=cred.Contact_id and crep.Cre_id=cred.Cre_id and cred.germ_id=(select top(1) Germ_id from [dbo].[Cre_germ_types] where Allow_data_upload=1 and Ger_code='CR-KPN' order by Sortid) and cred.germ_detected>=cred.Cr_detected and cred.Cr_detected>0 and crep.Data_year=" + dateyear + " and crep.Isvalid=1 and crep.Isaudited=1 and crep.Allow_report_display=1 and(cred.Carbapenemase_kpc+cred.Carbapenemase_ipm+cred.Carbapenemase_ndm+cred.Carbapenemase_vim+cred.Carbapenemase_other+cred.Carbapenemase_oxa_48)<=cred.Cr_detected group by crep.Data_year,crec.Province,crec.Province_id,crec.City,crec.City_id,cred.germ_id having sum(Carbapenemase_kpc)<=sum(Cr_detected) and sum(Carbapenemase_ndm)<=sum(Cr_detected) order by crep.Data_year,crec.Province";
                    IEnumerable<Toconfigure> result = conn.Query<Toconfigure>(sql);
                    return result.ToList();
                }
                else
                {
                    string sql = string.Format(@"SELECT cred.germ_id,crep.Data_year,crec.Province,crec.Province_id,crec.City,crec.City_id, sum(Carbapenemase_kpc) as kpc,sum(Carbapenemase_ndm) as ndm,sum(Cr_detected) as cr,
round(cast(sum(Carbapenemase_kpc) as float)/cast(sum(Cr_detected) as float),4 ) * 100 as ratekpc,
round(cast(sum(Carbapenemase_ndm) as float )/cast(sum(Cr_detected) as float),4 )* 100 as ratendm
FROM [dbo].[Cre_data] cred,
[dbo].[Cre_Contact] crec,
[dbo].[Cre_data_period] crep
where crec.Contact_id=cred.Contact_id and crep.Cre_id=cred.Cre_id and cred.germ_id='{0}' and cred.germ_detected>=cred.Cr_detected and cred.Cr_detected>0 and crep.Data_year='{1}' and crep.Isvalid=1 and crep.Isaudited=1 and crep.Allow_report_display=1 and(cred.Carbapenemase_kpc+cred.Carbapenemase_ipm+cred.Carbapenemase_ndm+cred.Carbapenemase_vim+cred.Carbapenemase_other+cred.Carbapenemase_oxa_48)<=cred.Cr_detected group by crep.Data_year,crec.Province,crec.Province_id,crec.City,crec.City_id,cred.germ_id having sum(Carbapenemase_kpc)<=sum(Cr_detected) and sum(Carbapenemase_ndm)<=sum(Cr_detected) order by crep.Data_year,crec.Province", Germ_id, year);
                    IEnumerable<Toconfigure> result = conn.Query<Toconfigure>(sql);
                    return result.ToList();
                }
            }
        }

        /// <summary>
        /// 柱状图抗药物分析
        /// </summary>
        /// <param name="field"></param>
        /// <param name="Germ_id"></param>
        /// <returns></returns>
        public List<string> Drugrate(string field, long? Germ_id)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                List<Cre_data> result = null;
                if (field == "大肠埃希菌CR-ECO")
                {
                    //大肠埃希菌CR-ECO
                    string sql = string.Format("SELECT P.Supplier,P.Drugrate FROM(select ROUND(cast(SUM(a.Antibiotic_polymyxin) as float) / SUM(a.Cr_detected), 2) Strainrate5,ROUND(cast(SUM(a.Antibiotic_ceftazidime) as float) / SUM(a.Cr_detected), 2) Strainrate4,ROUND(cast(SUM(a.Antibiotic_chloramphenicol) as float) / SUM(a.Cr_detected), 2) Strainrate3,ROUND(cast(SUM(a.Antibiotic_drugtest) as float) / SUM(a.Cr_detected), 2) Strainrate2,ROUND(cast(SUM(a.Antibiotic_fosfomycin) as float) / SUM(a.Cr_detected), 2) Strainrate1,ROUND(cast(SUM(a.Antibiotic_tegafycline) as float) / SUM(a.Cr_detected), 2) Strainrate from[Cre_data] a where a.Germ_id = '{0}' and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected)T UNPIVOT(Drugrate FOR Supplier IN(Strainrate5, Strainrate4, Strainrate3, Strainrate2, Strainrate1, Strainrate)) P", Germ_id);
                    result = conn.Query<Cre_data>(sql).ToList();
                }
                else if (field == "肺炎克雷伯菌CR-KPN")
                {
                    //肺炎克雷伯菌CR-KPN 
                    string sql = string.Format("SELECT P.Supplier,P.Drugrate FROM(select ROUND(cast(SUM(a.Antibiotic_polymyxin) as float) / SUM(a.Cr_detected), 2) Strainrate5,ROUND(cast(SUM(a.Antibiotic_ceftazidime) as float) / SUM(a.Cr_detected), 2) Strainrate4,ROUND(cast(SUM(a.Antibiotic_chloramphenicol) as float) / SUM(a.Cr_detected), 2) Strainrate3,ROUND(cast(SUM(a.Antibiotic_drugtest) as float) / SUM(a.Cr_detected), 2) Strainrate2,ROUND(cast(SUM(a.Antibiotic_fosfomycin) as float) / SUM(a.Cr_detected), 2) Strainrate1,ROUND(cast(SUM(a.Antibiotic_tegafycline) as float) / SUM(a.Cr_detected), 2) Strainrate from[Cre_data] a where a.Germ_id = '{0}' and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected)T UNPIVOT(Drugrate FOR Supplier IN(Strainrate5, Strainrate4, Strainrate3, Strainrate2, Strainrate1, Strainrate)) P", Germ_id);
                    result = conn.Query<Cre_data>(sql).ToList();
                }
                else if (field == "阴沟肠杆菌CR-ECL")
                {
                    //阴沟肠杆菌CR-ECL
                    string sql = string.Format("SELECT P.Supplier,P.Drugrate FROM(select ROUND(cast(SUM(a.Antibiotic_polymyxin) as float) / SUM(a.Cr_detected), 2) Strainrate5,ROUND(cast(SUM(a.Antibiotic_ceftazidime) as float) / SUM(a.Cr_detected), 2) Strainrate4,ROUND(cast(SUM(a.Antibiotic_chloramphenicol) as float) / SUM(a.Cr_detected), 2) Strainrate3,ROUND(cast(SUM(a.Antibiotic_drugtest) as float) / SUM(a.Cr_detected), 2) Strainrate2,ROUND(cast(SUM(a.Antibiotic_fosfomycin) as float) / SUM(a.Cr_detected), 2) Strainrate1,ROUND(cast(SUM(a.Antibiotic_tegafycline) as float) / SUM(a.Cr_detected), 2) Strainrate from[Cre_data] a where a.Germ_id = '{0}' and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected)T UNPIVOT(Drugrate FOR Supplier IN(Strainrate5, Strainrate4, Strainrate3, Strainrate2, Strainrate1, Strainrate)) P", Germ_id);
                    result = conn.Query<Cre_data>(sql).ToList();
                }
                else if (field == "粘质沙雷菌CR-SMA")
                {
                    //黏质沙雷菌CR-SMA
                    string sql = string.Format("SELECT P.Supplier,P.Drugrate FROM(select ROUND(cast(SUM(a.Antibiotic_polymyxin) as float) / SUM(a.Cr_detected), 2) Strainrate5,ROUND(cast(SUM(a.Antibiotic_ceftazidime) as float) / SUM(a.Cr_detected), 2) Strainrate4,ROUND(cast(SUM(a.Antibiotic_chloramphenicol) as float) / SUM(a.Cr_detected), 2) Strainrate3,ROUND(cast(SUM(a.Antibiotic_drugtest) as float) / SUM(a.Cr_detected), 2) Strainrate2,ROUND(cast(SUM(a.Antibiotic_fosfomycin) as float) / SUM(a.Cr_detected), 2) Strainrate1,ROUND(cast(SUM(a.Antibiotic_tegafycline) as float) / SUM(a.Cr_detected), 2) Strainrate from[Cre_data] a where a.Germ_id = '{0}' and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected)T UNPIVOT(Drugrate FOR Supplier IN(Strainrate5, Strainrate4, Strainrate3, Strainrate2, Strainrate1, Strainrate)) P", Germ_id);
                    result = conn.Query<Cre_data>(sql).ToList();
                }
                string supplier1 = ""; string supplier2 = ""; string supplier3 = ""; string supplier4 = ""; string supplier5 = ""; string supplier6 = "";
                string Drugrate1 = ""; string Drugrate2 = ""; string Drugrate3 = ""; string Drugrate4 = ""; string Drugrate5 = ""; string Drugrate6 = "";
                for (int i = 0; i < result.Count; i++)
                {
                    if (i == 0)
                    {
                        supplier1 = result[i].Supplier;
                        Drugrate1 = result[i].Drugrate.ToString();
                    }
                    else if (i == 1)
                    {
                        supplier2 = result[i].Supplier;
                        Drugrate2 = result[i].Drugrate.ToString();
                    }
                    else if (i == 2)
                    {
                        supplier3 = result[i].Supplier;
                        Drugrate3 = result[i].Drugrate.ToString();
                    }
                    else if (i == 3)
                    {
                        supplier4 = result[i].Supplier;
                        Drugrate4 = result[i].Drugrate.ToString();
                    }
                    else if (i == 4)
                    {
                        supplier5 = result[i].Supplier;
                        Drugrate5 = result[i].Drugrate.ToString();
                    }
                    else if (i == 5)
                    {
                        supplier6 = result[i].Supplier;
                        Drugrate6 = result[i].Drugrate.ToString();
                    }

                }
                string[] supplier = new string[] { "Strainrate5", "Strainrate4", "Strainrate3", "Strainrate2", "Strainrate1", "Strainrate" }; //不定长   
                string[] Drugrate = new string[] { "0", "0", "0", "0", "0", "0" };
                for (int i = 0; i < supplier.Length; i++)
                {
                    if (supplier1 == supplier[i])
                    {
                        Drugrate[i] = Drugrate1;
                        continue;
                    }
                    else if (supplier2 == supplier[i])
                    {
                        Drugrate[i] = Drugrate2;
                        continue;
                    }
                    else if (supplier3 == supplier[i])
                    {
                        Drugrate[i] = Drugrate3;
                        continue;
                    }
                    else if (supplier4 == supplier[i])
                    {
                        Drugrate[i] = Drugrate4;
                        continue;
                    }
                    else if (supplier5 == supplier[i])
                    {
                        Drugrate[i] = Drugrate5;
                        continue;
                    }
                    else if (supplier6 == supplier[i])
                    {
                        Drugrate[i] = Drugrate6;
                        continue;
                    }

                }
                return Drugrate.ToList();
            }
        }
        /// <summary>
        /// 柱状图阴沟肠杆菌
        /// </summary>
        /// <param name="field"></param>
        /// <param name="Germ_id"></param>
        /// <returns></returns>
        public List<string> Enterobacter(string field, long? Germ_id)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                List<Cre_data> result = null;

                if (field == "s痰")
                {
                    string sq1 = string.Format("select Specimen_tan as xAxis,COUNT(Specimen_tan) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_tan=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_tan,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s血液")
                {
                    string sq1 = string.Format("select Specimen_blood as xAxis,COUNT(Specimen_blood) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_blood=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_blood,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s粪便")
                {
                    string sq1 = string.Format("select Specimen_faeces as xAxis,COUNT(Specimen_faeces) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_faeces=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_faeces,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s肺泡灌洗液")
                {
                    string sq1 = string.Format("select Specimen_alveolar as xAxis,COUNT(Specimen_alveolar) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_alveolar=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_alveolar,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s脓液/伤口")
                {
                    string sq1 = string.Format("select Specimen_woundpus as xAxis,COUNT(Specimen_woundpus) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_woundpus=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_woundpus,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s尿道")
                {
                    string sq1 = string.Format("select Specimen_urethra as xAxis,COUNT(Specimen_urethra) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_urethra=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_urethra,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s中心静脉导管")
                {
                    string sq1 = string.Format("select Specimen_venous as xAxis,COUNT(Specimen_venous) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_venous=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_venous,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s穿刺液")
                {
                    string sq1 = string.Format("select Specimen_puncture as xAxis,COUNT(Specimen_puncture) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_puncture=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_puncture,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s其他")
                {
                    string sq1 = string.Format("select Specimen_other as xAxis,COUNT(Specimen_other) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_other=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_other,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d血液科")
                {
                    string sq1 = string.Format("select Department_bloodsection as xAxis,COUNT(Department_bloodsection) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_bloodsection=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_bloodsection,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "dICU")
                {
                    string sq1 = string.Format("select Department_icu as xAxis,COUNT(Department_icu) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_icu=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_icu,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d呼吸科")
                {
                    string sq1 = string.Format("select Department_breathing as xAxis,COUNT(Department_breathing) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_breathing=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_breathing,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d感染科")
                {
                    string sq1 = string.Format("select Department_Infected as xAxis,COUNT(Department_Infected) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_Infected=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_Infected,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d移植科")
                {
                    string sq1 = string.Format("select Department_transplant as xAxis,COUNT(Department_transplant) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_transplant=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_transplant,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d其他")
                {
                    string sq1 = string.Format("select Department_other as xAxis,COUNT(Department_other) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_other=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_other,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else
                {
                    string sq1 = string.Format("select Specimen_tan as xAxis,COUNT(Specimen_tan) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_tan=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_tan,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                string xAxi1 = ""; string xAxi2 = ""; string xAxi3 = ""; string xAxi4 = ""; string xAxi5 = ""; string xAxi6 = ""; string xAxi7 = ""; string xAxi8 = "";
                string sum1 = ""; string sum2 = ""; string sum3 = ""; string sum4 = ""; string sum5 = ""; string sum6 = ""; string sum7 = ""; string sum8 = "";
                for (int i = 0; i < result.Count; i++)
                {
                    if (i == 0)
                    {
                        xAxi1 = result[i].xAxis;
                        sum1 = result[i].sums.ToString();
                    }
                    else if (i == 1)
                    {
                        xAxi2 = result[i].xAxis;
                        sum2 = result[i].sums.ToString();
                    }
                    else if (i == 2)
                    {
                        xAxi3 = result[i].xAxis;
                        sum3 = result[i].sums.ToString();
                    }
                    else if (i == 3)
                    {
                        xAxi4 = result[i].xAxis;
                        sum4 = result[i].sums.ToString();
                    }
                    else if (i == 4)
                    {
                        xAxi5 = result[i].xAxis;
                        sum5 = result[i].sums.ToString();
                    }
                    else if (i == 5)
                    {
                        xAxi6 = result[i].xAxis;
                        sum6 = result[i].sums.ToString();
                    }
                    else if (i == 6)
                    {
                        xAxi7 = result[i].xAxis;
                        sum7 = result[i].sums.ToString();
                    }
                    else if (i == 7)
                    {
                        xAxi8 = result[i].xAxis;
                        sum8 = result[i].sums.ToString();
                    }
                }
                string[] xAxis = new string[] { "0-5%", "6%-10%", "11%-15%", "16%-20%", "21%-30%", "31%-40%", "41%-50%", "50%以上" }; //不定长   
                                                                                                                                    // List<string> sums = new List<string>();
                string[] sums = new string[] { "0", "0", "0", "0", "0", "0", "0", "0" };
                for (int i = 0; i < xAxis.Length; i++)
                {
                    if (xAxi1 == xAxis[i])
                    {
                        sums[i] = sum1;
                        continue;
                    }
                    else if (xAxi2 == xAxis[i])
                    {
                        sums[i] = sum2;
                        continue;
                    }
                    else if (xAxi3 == xAxis[i])
                    {
                        sums[i] = sum3;
                        continue;
                    }
                    else if (xAxi4 == xAxis[i])
                    {
                        sums[i] = sum4;
                        continue;
                    }
                    else if (xAxi5 == xAxis[i])
                    {
                        sums[i] = sum5;
                        continue;
                    }
                    else if (xAxi6 == xAxis[i])
                    {
                        sums[i] = sum6;
                        continue;
                    }
                    else if (xAxi7 == xAxis[i])
                    {
                        sums[i] = sum7;
                        continue;
                    }
                    else if (xAxi8 == xAxis[i])
                    {
                        sums[i] = sum8;
                        continue;
                    }

                }
                return sums.ToList();
            }
        }

        /// <summary>
        /// 柱状图大肠埃希菌
        /// </summary>
        /// <param name="field"></param>
        /// <param name="Germ_id"></param>
        /// <returns></returns>
        public List<string> Escherichiacoli(string field, long? Germ_id)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                List<Cre_data> result = null;

                if (field == "s痰")
                {
                    string sq1 = string.Format("select Specimen_tan as xAxis,COUNT(Specimen_tan) sums from [dbo].[Cre_data] a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_tan=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_tan,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s血液")
                {
                    string sq1 = string.Format("select Specimen_blood as xAxis,COUNT(Specimen_blood) sums from [dbo].[Cre_data] a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_blood=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_blood,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s粪便")
                {
                    string sq1 = string.Format("select Specimen_faeces as xAxis,COUNT(Specimen_faeces) sums from [dbo].[Cre_data] a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_faeces=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_faeces,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s肺泡灌洗液")
                {
                    string sq1 = string.Format("select Specimen_alveolar as xAxis,COUNT(Specimen_alveolar) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_alveolar=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_alveolar,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s脓液/伤口")
                {
                    string sq1 = string.Format("select Specimen_woundpus as xAxis,COUNT(Specimen_woundpus) sums from [dbo].[Cre_data] a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_woundpus=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_woundpus,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s尿道")
                {
                    string sq1 = string.Format("select Specimen_urethra as xAxis,COUNT(Specimen_urethra) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_urethra=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_urethra,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s中心静脉导管")
                {
                    string sq1 = string.Format("select Specimen_venous as xAxis,COUNT(Specimen_venous) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_venous=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_venous,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s穿刺液")
                {
                    string sq1 = string.Format("select Specimen_puncture as xAxis,COUNT(Specimen_puncture) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_puncture=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_puncture,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s其他")
                {
                    string sq1 = string.Format("select Specimen_other as xAxis,COUNT(Specimen_other) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_other=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_other,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d血液科")
                {
                    string sq1 = string.Format("select Department_bloodsection as xAxis,COUNT(Department_bloodsection) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_bloodsection=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_bloodsection,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "dICU")
                {
                    string sq1 = string.Format("select Department_icu as xAxis,COUNT(Department_icu) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_icu=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_icu,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d呼吸科")
                {
                    string sq1 = string.Format("select Department_breathing as xAxis,COUNT(Department_breathing) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_breathing=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_breathing,b.sortid order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d感染科")
                {
                    string sq1 = string.Format("select Department_Infected as xAxis,COUNT(Department_Infected) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_Infected=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_Infected,b.sortid order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d移植科")
                {
                    string sq1 = string.Format("select Department_transplant as xAxis,COUNT(Department_transplant) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_transplant=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_transplant,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d其他")
                {
                    string sq1 = string.Format("select Department_other as xAxis,COUNT(Department_other) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_other=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_other,b.sortid order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else
                {
                    string sq1 = string.Format("select Specimen_tan as xAxis,COUNT(Specimen_tan) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_tan=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_tan,b.sortid order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                string xAxi1 = ""; string xAxi2 = ""; string xAxi3 = ""; string xAxi4 = ""; string xAxi5 = ""; string xAxi6 = ""; string xAxi7 = ""; string xAxi8 = "";
                string sum1 = ""; string sum2 = ""; string sum3 = ""; string sum4 = ""; string sum5 = ""; string sum6 = ""; string sum7 = ""; string sum8 = "";
                for (int i = 0; i < result.Count; i++)
                {
                    if (i == 0)
                    {
                        xAxi1 = result[i].xAxis;
                        sum1 = result[i].sums.ToString();
                    }
                    else if (i == 1)
                    {
                        xAxi2 = result[i].xAxis;
                        sum2 = result[i].sums.ToString();
                    }
                    else if (i == 2)
                    {
                        xAxi3 = result[i].xAxis;
                        sum3 = result[i].sums.ToString();
                    }
                    else if (i == 3)
                    {
                        xAxi4 = result[i].xAxis;
                        sum4 = result[i].sums.ToString();
                    }
                    else if (i == 4)
                    {
                        xAxi5 = result[i].xAxis;
                        sum5 = result[i].sums.ToString();
                    }
                    else if (i == 5)
                    {
                        xAxi6 = result[i].xAxis;
                        sum6 = result[i].sums.ToString();
                    }
                    else if (i == 6)
                    {
                        xAxi7 = result[i].xAxis;
                        sum7 = result[i].sums.ToString();
                    }
                    else if (i == 7)
                    {
                        xAxi8 = result[i].xAxis;
                        sum8 = result[i].sums.ToString();
                    }
                }
                string[] xAxis = new string[] { "0-5%", "6%-10%", "11%-15%", "16%-20%", "21%-30%", "31%-40%", "41%-50%", "50%以上" }; //不定长   
                //List<string> sums = new List<string>();
                string[] sums = new string[] { "0", "0", "0", "0", "0", "0", "0", "0" };
                for (int i = 0; i < xAxis.Length; i++)
                {
                    if (xAxi1 == xAxis[i])
                    {
                        sums[i] = sum1;
                        continue;
                    }
                    else if (xAxi2 == xAxis[i])
                    {
                        sums[i] = sum2;
                        continue;
                    }
                    else if (xAxi3 == xAxis[i])
                    {
                        sums[i] = sum3;
                        continue;
                    }
                    else if (xAxi4 == xAxis[i])
                    {
                        sums[i] = sum4;
                        continue;
                    }
                    else if (xAxi5 == xAxis[i])
                    {
                        sums[i] = sum5;
                        continue;
                    }
                    else if (xAxi6 == xAxis[i])
                    {
                        sums[i] = sum6;
                        continue;
                    }
                    else if (xAxi7 == xAxis[i])
                    {
                        sums[i] = sum7;
                        continue;
                    }
                    else if (xAxi8 == xAxis[i])
                    {
                        sums[i] = sum8;
                        continue;
                    }

                }
                return sums.ToList();
            }
        }

        public Cre_Colormatching GetColormatchings(string year, long? type)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select Content_value from Cre_Colormatching where Data_year=@Data_year and Data_id=@Data_id");
                Cre_Colormatching colo = conn.Query<Cre_Colormatching>(sql, new { Data_year = year, Data_id = type })?.ToList().FirstOrDefault();
                return colo;
            }
        }

        public List<Member> GetMember(long? id)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select  * from  Member where Id='{0}'",id);
                IEnumerable<Member>result = conn.Query<Member>(sql);
                return result.ToList();
            }
        }

        public string Get_Data_Periods()
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"select top(1) Data_year from Cre_data_period where Germ_id=(select top(1) Germ_id from [dbo].[Cre_germ_types] where Allow_data_upload=1 order by Sortid) order by Data_year desc";
                Cre_data_period result = conn.Query<Cre_data_period>(sql).ToList().FirstOrDefault();
                if (result == null)
                {
                    return null;
                }
                else
                {
                    return result.Data_year;
                }
            }
        }

        /// <summary>
        /// 查询可上传数据的细菌
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
        /// 查询有效细菌
        /// </summary>
        /// <returns></returns>
        public List<Cre_germ_types> Get_Type()
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from [dbo].[Cre_germ_types] where Allow_report_display=1");
                IEnumerable<Cre_germ_types> result = conn.Query<Cre_germ_types>(sql).ToList();
                return result.ToList();
            }
        }

        /// <summary>
        /// 查询默认显示细菌id
        /// </summary>
        /// <returns></returns>
        public Cre_germ_types Get_Typeshou()
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select top(1) Germ_id from [dbo].[Cre_germ_types] where Allow_data_upload=1 and Ger_code='CR-KPN' order by Sortid");
                Cre_germ_types result = conn.Query<Cre_germ_types>(sql)?.ToList().FirstOrDefault();
                return result;
            }
        }
        /// <summary>
        /// 查询有效年份
        /// </summary>
        /// <returns></returns>
        public List<Cre_data_period> Get_Year()
        {
            IEnumerable<Cre_data_period> result = null;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string str = String.Format("select DISTINCT a.Data_year,a.Germ_id from cre_data_period a,[dbo].[Cre_germ_types] b where  a.Germ_id=b.Germ_id and b.Allow_report_display=1");
                result = conn.Query<Cre_data_period>(str, null);
            }
            return result.ToList();
        }
        /// <summary>
        /// 柱状图肺炎克雷伯菌
        /// </summary>
        /// <param name="field"></param>
        /// <param name="Germ_id"></param>
        /// <returns></returns>
        public List<string> klebsiella(string field, long? Germ_id)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                List<Cre_data> result = null;

                if (field == "s痰")
                {
                    string sq1 = string.Format("select Specimen_tan as xAxis,COUNT(Specimen_tan) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_tan=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_tan,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s血液")
                {
                    string sq1 = string.Format("select Specimen_blood as xAxis,COUNT(Specimen_blood) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_blood=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_blood,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s粪便")
                {
                    string sq1 = string.Format("select Specimen_faeces as xAxis,COUNT(Specimen_faeces) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_faeces=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_faeces,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s肺泡灌洗液")
                {
                    string sq1 = string.Format("select Specimen_alveolar as xAxis,COUNT(Specimen_alveolar) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_alveolar=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_alveolar,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s脓液/伤口")
                {
                    string sq1 = string.Format("select Specimen_woundpus as xAxis,COUNT(Specimen_woundpus) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_woundpus=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_woundpus,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s尿道")
                {
                    string sq1 = string.Format("select Specimen_urethra as xAxis,COUNT(Specimen_urethra) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_urethra=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_urethra,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s中心静脉导管")
                {
                    string sq1 = string.Format("select Specimen_venous as xAxis,COUNT(Specimen_venous) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_venous=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_venous,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s穿刺液")
                {
                    string sq1 = string.Format("select Specimen_puncture as xAxis,COUNT(Specimen_puncture) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_puncture=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_puncture,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s其他")
                {
                    string sq1 = string.Format("select Specimen_other as xAxis,COUNT(Specimen_other) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_other=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_other,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d血液科")
                {
                    string sq1 = string.Format("select Department_bloodsection as xAxis,COUNT(Department_bloodsection) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_bloodsection=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_bloodsection,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "dICU")
                {
                    string sq1 = string.Format("select Department_icu as xAxis,COUNT(Department_icu) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_icu=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_icu,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d呼吸科")
                {
                    string sq1 = string.Format("select Department_breathing as xAxis,COUNT(Department_breathing) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_breathing=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_breathing,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d感染科")
                {
                    string sq1 = string.Format("select Department_Infected as xAxis,COUNT(Department_Infected) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_Infected=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_Infected,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d移植科")
                {
                    string sq1 = string.Format("select Department_transplant as xAxis,COUNT(Department_transplant) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_transplant=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_transplant,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d其他")
                {
                    string sq1 = string.Format("select Department_other as xAxis,COUNT(Department_other) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_other=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_other,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else
                {
                    string sq1 = string.Format("select Specimen_tan as xAxis,COUNT(Specimen_tan) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_tan=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_tan,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                string[] sums = new string[] { "0", "0", "0", "0", "0", "0", "0", "0" };
                string xAxi1 = ""; string xAxi2 = ""; string xAxi3 = ""; string xAxi4 = ""; string xAxi5 = ""; string xAxi6 = ""; string xAxi7 = ""; string xAxi8 = "";
                string sum1 = ""; string sum2 = ""; string sum3 = ""; string sum4 = ""; string sum5 = ""; string sum6 = ""; string sum7 = ""; string sum8 = "";
                for (int i = 0; i < result.Count; i++)
                {
                    if (i == 0)
                    {
                        xAxi1 = result[i].xAxis;
                        sum1 = result[i].sums.ToString();
                    }
                    else if (i == 1)
                    {
                        xAxi2 = result[i].xAxis;
                        sum2 = result[i].sums.ToString();
                    }
                    else if (i == 2)
                    {
                        xAxi3 = result[i].xAxis;
                        sum3 = result[i].sums.ToString();
                    }
                    else if (i == 3)
                    {
                        xAxi4 = result[i].xAxis;
                        sum4 = result[i].sums.ToString();
                    }
                    else if (i == 4)
                    {
                        xAxi5 = result[i].xAxis;
                        sum5 = result[i].sums.ToString();
                    }
                    else if (i == 5)
                    {
                        xAxi6 = result[i].xAxis;
                        sum6 = result[i].sums.ToString();
                    }
                    else if (i == 6)
                    {
                        xAxi7 = result[i].xAxis;
                        sum7 = result[i].sums.ToString();
                    }
                    else if (i == 7)
                    {
                        xAxi8 = result[i].xAxis;
                        sum8 = result[i].sums.ToString();
                    }
                }
                string[] xAxis = new string[] { "0-5%", "6%-10%", "11%-15%", "16%-20%", "21%-30%", "31%-40%", "41%-50%", "50%以上" }; //不定长   
                //List<string> sums = new List<string>();
                for (int i = 0; i < xAxis.Length; i++)
                {
                    if (xAxi1 == xAxis[i])
                    {
                        sums[i] = sum1;
                        continue;
                    }
                    else if (xAxi2 == xAxis[i])
                    {
                        sums[i] = sum2;
                        continue;
                    }
                    else if (xAxi3 == xAxis[i])
                    {
                        sums[i] = sum3;
                        continue;
                    }
                    else if (xAxi4 == xAxis[i])
                    {
                        sums[i] = sum4;
                        continue;
                    }
                    else if (xAxi5 == xAxis[i])
                    {
                        sums[i] = sum5;
                        continue;
                    }
                    else if (xAxi6 == xAxis[i])
                    {
                        sums[i] = sum6;
                        continue;
                    }
                    else if (xAxi7 == xAxis[i])
                    {
                        sums[i] = sum7;
                        continue;
                    }
                    else if (xAxi8 == xAxis[i])
                    {
                        sums[i] = sum8;
                        continue;
                    }

                }

                return sums.ToList();
            }
        }

        /// <summary>
        /// 柱状图黏质沙雷菌
        /// </summary>
        /// <param name="field"></param>
        /// <param name="Germ_id"></param>
        /// <returns></returns>
        public List<string> serratia(string field, long? Germ_id)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                List<Cre_data> result = null;

                if (field == "s痰")
                {
                    string sq1 = string.Format("select Specimen_tan as xAxis,COUNT(Specimen_tan) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_tan=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected  group by Specimen_tan,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s血液")
                {
                    string sq1 = string.Format("select Specimen_blood as xAxis,COUNT(Specimen_blood) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_blood=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_blood,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s粪便")
                {
                    string sq1 = string.Format("select Specimen_faeces as xAxis,COUNT(Specimen_faeces) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_faeces=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_faeces,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s肺泡灌洗液")
                {
                    string sq1 = string.Format("select Specimen_alveolar as xAxis,COUNT(Specimen_alveolar) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_alveolar=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_alveolar,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s脓液/伤口")
                {
                    string sq1 = string.Format("select Specimen_woundpus as xAxis,COUNT(Specimen_woundpus) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_woundpus=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_woundpus,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s尿道")
                {
                    string sq1 = string.Format("select Specimen_urethra as xAxis,COUNT(Specimen_urethra) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_urethra=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_urethra,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s中心静脉导管")
                {
                    string sq1 = string.Format("select Specimen_venous as xAxis,COUNT(Specimen_venous) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_venous=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_venous,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s穿刺液")
                {
                    string sq1 = string.Format("select Specimen_puncture as xAxis,COUNT(Specimen_puncture) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_puncture=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_puncture,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "s其他")
                {
                    string sq1 = string.Format("select Specimen_other as xAxis,COUNT(Specimen_other) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_other=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_other,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d血液科")
                {
                    string sq1 = string.Format("select Department_bloodsection as xAxis,COUNT(Department_bloodsection) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_bloodsection=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_bloodsection,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "dICU")
                {
                    string sq1 = string.Format("select Department_icu as xAxis,COUNT(Department_icu) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_icu=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected  group by Department_icu,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d呼吸科")
                {
                    string sq1 = string.Format("select Department_breathing as xAxis,COUNT(Department_breathing) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_breathing=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_breathing,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d感染科")
                {
                    string sq1 = string.Format("select Department_Infected as xAxis,COUNT(Department_Infected) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_Infected=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_Infected,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d移植科")
                {
                    string sq1 = string.Format("select Department_transplant as xAxis,COUNT(Department_transplant) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_transplant=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_transplant,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else if (field == "d其他")
                {
                    string sq1 = string.Format("select Department_other as xAxis,COUNT(Department_other) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Department_other=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Department_other,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                else
                {
                    string sq1 = string.Format("select Specimen_tan as xAxis,COUNT(Specimen_tan) sums from [dbo].[Cre_data]  a,Cre_data_percentage b where  a.Germ_id='{0}' and a.Specimen_tan=b.percentages and a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by Specimen_tan,b.sortid  order by b.sortid", Germ_id);
                    result = conn.Query<Cre_data>(sq1).ToList();
                }
                string xAxi1 = ""; string xAxi2 = ""; string xAxi3 = ""; string xAxi4 = ""; string xAxi5 = ""; string xAxi6 = ""; string xAxi7 = ""; string xAxi8 = "";
                string sum1 = ""; string sum2 = ""; string sum3 = ""; string sum4 = ""; string sum5 = ""; string sum6 = ""; string sum7 = ""; string sum8 = "";
                for (int i = 0; i < result.Count; i++)
                {
                    if (i == 0)
                    {
                        xAxi1 = result[i].xAxis;
                        sum1 = result[i].sums.ToString();
                    }
                    else if (i == 1)
                    {
                        xAxi2 = result[i].xAxis;
                        sum2 = result[i].sums.ToString();
                    }
                    else if (i == 2)
                    {
                        xAxi3 = result[i].xAxis;
                        sum3 = result[i].sums.ToString();
                    }
                    else if (i == 3)
                    {
                        xAxi4 = result[i].xAxis;
                        sum4 = result[i].sums.ToString();
                    }
                    else if (i == 4)
                    {
                        xAxi5 = result[i].xAxis;
                        sum5 = result[i].sums.ToString();
                    }
                    else if (i == 5)
                    {
                        xAxi6 = result[i].xAxis;
                        sum6 = result[i].sums.ToString();
                    }
                    else if (i == 6)
                    {
                        xAxi7 = result[i].xAxis;
                        sum7 = result[i].sums.ToString();
                    }
                    else if (i == 7)
                    {
                        xAxi8 = result[i].xAxis;
                        sum8 = result[i].sums.ToString();
                    }
                }
                string[] xAxis = new string[] { "0-5%", "6%-10%", "11%-15%", "16%-20%", "21%-30%", "31%-40%", "41%-50%", "50%以上" }; //不定长   
                //List<string> sums = new List<string>();
                string[] sums = new string[] { "0", "0", "0", "0", "0", "0", "0", "0" };
                for (int i = 0; i < xAxis.Length; i++)
                {
                    if (xAxi1 == xAxis[i])
                    {
                        sums[i] = sum1;
                        continue;
                    }
                    else if (xAxi2 == xAxis[i])
                    {
                        sums[i] = sum2;
                        continue;
                    }
                    else if (xAxi3 == xAxis[i])
                    {
                        sums[i] = sum3;
                        continue;
                    }
                    else if (xAxi4 == xAxis[i])
                    {
                        sums[i] = sum4;
                        continue;
                    }
                    else if (xAxi5 == xAxis[i])
                    {
                        sums[i] = sum5;
                        continue;
                    }
                    else if (xAxi6 == xAxis[i])
                    {
                        sums[i] = sum6;
                        continue;
                    }
                    else if (xAxi7 == xAxis[i])
                    {
                        sums[i] = sum7;
                        continue;
                    }
                    else if (xAxi8 == xAxis[i])
                    {
                        sums[i] = sum8;
                        continue;
                    }

                }
                return sums.ToList();
            }
        }

        /// <summary>
        /// 第一级数据显示
        /// </summary>
        /// <returns></returns>
        public List<Toconfigure> SGetdata_Show(string year, string Germ_id)
        {
            var dateyear = DateTime.Now.Year.ToString();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                if (year == null && Germ_id == null)
                {
                    string sql = @"SELECT cred.germ_id,crep.Data_year,crec.Province,crec.Province_id, sum(Carbapenemase_kpc) as kpc,sum(Carbapenemase_ndm) as ndm,sum(Cr_detected) as cr,
round(cast(sum(Carbapenemase_kpc) as float)/cast(sum(Cr_detected) as float),4 ) *100 as ratekpc,
round(cast(sum(Carbapenemase_ndm) as float )/cast(sum(Cr_detected) as float),4 ) *100 as ratendm
FROM [dbo].[Cre_data] cred,
[dbo].[Cre_Contact] crec,
[dbo].[Cre_data_period] crep
where crec.Contact_id=cred.Contact_id and crep.Cre_id=cred.Cre_id and cred.germ_id=(select top(1) Germ_id from [dbo].[Cre_germ_types] where Allow_data_upload=1 and Ger_code='CR-KPN' order by Sortid) and cred.germ_detected>=cred.Cr_detected and cred.Cr_detected>0  and crep.Data_year=" + dateyear + " and crep.Isvalid=1 and crep.Isaudited=1 and crep.Allow_report_display=1 and(cred.Carbapenemase_kpc+cred.Carbapenemase_ipm+cred.Carbapenemase_ndm+cred.Carbapenemase_vim+cred.Carbapenemase_other+cred.Carbapenemase_oxa_48)<=cred.Cr_detected group by crep.Data_year,crec.Province,crec.Province_id,cred.germ_id having sum(Carbapenemase_kpc)<=sum(Cr_detected) and sum(Carbapenemase_ndm)<=sum(Cr_detected) order by crep.Data_year,crec.Province";
                    IEnumerable<Toconfigure> result = conn.Query<Toconfigure>(sql);
                    return result.ToList();
                }
                else
                {
                    string sql = string.Format(@"SELECT cred.germ_id,crep.Data_year,crec.Province,crec.Province_id, sum(Carbapenemase_kpc) as kpc,sum(Carbapenemase_ndm) as ndm,sum(Cr_detected) as cr,
round(cast(sum(Carbapenemase_kpc) as float)/cast(sum(Cr_detected) as float),4 ) *100 as ratekpc,
round(cast(sum(Carbapenemase_ndm) as float )/cast(sum(Cr_detected) as float),4 ) *100 as ratendm
FROM [dbo].[Cre_data] cred,
[dbo].[Cre_Contact] crec,
[dbo].[Cre_data_period] crep
where crec.Contact_id=cred.Contact_id and crep.Cre_id=cred.Cre_id and cred.germ_id='{0}' and cred.germ_detected>=cred.Cr_detected and cred.Cr_detected>0  and crep.Data_year='{1}' and
 crep.Isvalid=1 and crep.Isaudited=1 and crep.Allow_report_display=1 and (cred.Carbapenemase_kpc+cred.Carbapenemase_ipm+cred.Carbapenemase_ndm+cred.Carbapenemase_vim+cred.Carbapenemase_other+cred.Carbapenemase_oxa_48)<=cred.Cr_detected group by crep.Data_year,crec.Province,crec.Province_id,cred.germ_id having sum(Carbapenemase_kpc)<=sum(Cr_detected) and sum(Carbapenemase_ndm)<=sum(Cr_detected) order by crep.Data_year,crec.Province", Germ_id, year);
                    IEnumerable<Toconfigure> result = conn.Query<Toconfigure>(sql);
                    return result.ToList();
                }
            }
        }
        /// <summary>
        /// 耐药分析查询
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public List<string> Strainrate(string field)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                List<Cre_data> result = null;
                if (field == "多粘菌素")
                {
                    //多黏菌素
                    string sql = string.Format("select a.Germ_id,a.Germ, ROUND(cast(SUM(a.Antibiotic_polymyxin)as float)/SUM(a.Cr_detected),2) Strainrate from  [Cre_data] a where a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected  group by a.Germ_id,a.Germ");
                    result = conn.Query<Cre_data>(sql).ToList();
                }
                else if (field == "替加环素")
                {
                    //替加环素
                    string sql = string.Format("select a.Germ_id, a.Germ, ROUND(cast(SUM(a.Antibiotic_tegafycline)as float)/SUM(a.Cr_detected),2) Strainrate from  [Cre_data] a where a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by a.Germ_id,a.Germ");
                    result = conn.Query<Cre_data>(sql).ToList();
                }
                else if (field == "阿维巴坦")
                {
                    //头孢他啶-阿维巴坦
                    string sql = string.Format("select a.Germ_id, a.Germ, ROUND(cast(SUM(a.Antibiotic_ceftazidime)as float)/SUM(a.Cr_detected),2) Strainrate from  [Cre_data] a where a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by a.Germ_id,a.Germ");
                    result = conn.Query<Cre_data>(sql).ToList();
                }
                else if (field == "磷霉素")
                {
                    //磷霉素
                    string sql = string.Format("select a.Germ_id, a.Germ, ROUND(cast(SUM(a.Antibiotic_fosfomycin)as float)/SUM(a.Cr_detected),2) Strainrate from  [Cre_data] a where a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by a.Germ_id,a.Germ");
                    result = conn.Query<Cre_data>(sql).ToList();
                }
                else if (field == "氯霉素")
                {
                    //氯霉素
                    string sql = string.Format("select a.Germ_id, a.Germ, ROUND(cast(SUM(a.Antibiotic_chloramphenicol)as float)/SUM(a.Cr_detected),2) Strainrate from  [Cre_data] a where  a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by a.Germ_id,a.Germ");
                    result = conn.Query<Cre_data>(sql).ToList();
                }
                else if (field == "联合药敏试验")
                {
                    //联合药敏试验
                    string sql = string.Format("select a.Germ_id, a.Germ, ROUND(cast(SUM(a.Antibiotic_drugtest)as float)/SUM(a.Cr_detected),2) Strainrate from  [Cre_data] a  where  a.Cr_detected>0 and a.germ_detected>=a.Cr_detected and(a.Carbapenemase_kpc+a.Carbapenemase_ipm+a.Carbapenemase_ndm+a.Carbapenemase_vim+a.Carbapenemase_other+a.Carbapenemase_oxa_48)<=a.Cr_detected group by a.Germ_id,a.Germ");
                    result = conn.Query<Cre_data>(sql).ToList();
                }
                string Germ1 = ""; string Germ2 = ""; string Germ3 = ""; string Germ4 = "";
                string Strainrate1 = ""; string Strainrate2 = ""; string Strainrate3 = ""; string Strainrate4 = "";
                for (int i = 0; i < result.Count; i++)
                {
                    if (i == 0)
                    {
                        Germ1 = result[i].Germ;
                        Strainrate1 = result[i].Strainrate.ToString();
                    }
                    else if (i == 1)
                    {
                        Germ2 = result[i].Germ;
                        Strainrate2 = result[i].Strainrate.ToString();
                    }
                    else if (i == 2)
                    {
                        Germ3 = result[i].Germ;
                        Strainrate3 = result[i].Strainrate.ToString();
                    }
                    else if (i == 3)
                    {
                        Germ4 = result[i].Germ;
                        Strainrate4 = result[i].Strainrate.ToString();
                    }

                }
                string[] Germ = new string[] { "大肠埃希菌CR-ECO", "肺炎克雷伯菌CR-KPN", "阴沟肠杆菌CR-ECL", "黏质沙雷菌CR-SMA" }; //不定长   
                string[] Strainrate = new string[] { "0", "0", "0", "0" };
                for (int i = 0; i < Germ.Length; i++)
                {
                    if (Germ1 == Germ[i])
                    {
                        Strainrate[i] = Strainrate1;
                        continue;
                    }
                    else if (Germ2 == Germ[i])
                    {
                        Strainrate[i] = Strainrate2;
                        continue;
                    }
                    else if (Germ3 == Germ[i])
                    {
                        Strainrate[i] = Strainrate3;
                        continue;
                    }
                    else if (Germ4 == Germ[i])
                    {
                        Strainrate[i] = Strainrate4;
                        continue;
                    }

                }
                return Strainrate.ToList();
            }
        }
    }
}
