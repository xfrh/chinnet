using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.DynamicLinq;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 操作类 ，数据库表名：MedicalAntibioticRule 
    /// </summary>
    public partial class MedicalAntibioticRuleService : BaseService<MedicalAntibioticRule>, IMedicalAntibioticRuleService
    {

        private readonly IHospitalService HospitalService;
        private readonly ISpecimenService SpecimenService;
        private readonly IBacteriaTypeService BacteriaTypeService;
        private readonly IHospitalDepartmentService HospitalDepartmentService;
        private readonly IAutoCodeService AutoCodeService;
        private readonly IMedicalDataItemService MedicalDataItemService;
        private readonly IMedicalDataItemValidateService MedicalDataItemValidateService;
        private readonly IMedicalDataWardTypeService MedicalDataWardTypeService;
        private readonly IMedicalDepartmentTypeService MedicalDepartmentTypeService;
        private readonly IMedicalSpecTypeService MedicalSpecTypeService;
        private readonly IMedicalOrganismTypeService MedicalOrganismTypeService;
        private readonly IMedicalOrganismService MedicalOrganismService;
        private readonly IAuthenticationService AuthenticationService;
        private readonly IMedicalAntibioticService MedicalAntibioticService;
        private readonly IMedicalDataProjectService MedicalDataProjectService;

        private int DataSort = 0; //验证数据的排序

        public MedicalAntibioticRuleService(
            IRepository<MedicalAntibioticRule> repository,
                IHospitalService hospitalService,
                ISpecimenService specimenService,
                IBacteriaTypeService bacteriaTypeService,
                IHospitalDepartmentService hospitalDepartmentService,
                IAutoCodeService autoCodeService,
                IMedicalDataItemService medicalDataItemService,
                IMedicalDataItemValidateService medicalDataItemValidateService,
                IMedicalDataWardTypeService medicalDataWardTypeService,
                IMedicalDepartmentTypeService medicalDepartmentTypeService,
                IMedicalSpecTypeService medicalSpecTypeService,
                IMedicalOrganismTypeService medicalOrganismTypeService,
                IAuthenticationService authenticationService,
                IMedicalOrganismService medicalOrganismService,
                IMedicalAntibioticService medicalAntibioticService,
                IMedicalDataProjectService medicalDataProjectService
            ) : base(repository)
        {
            this.HospitalService = hospitalService;
            this.SpecimenService = specimenService;
            this.BacteriaTypeService = bacteriaTypeService;
            this.HospitalDepartmentService = hospitalDepartmentService;
            this.AutoCodeService = autoCodeService;
            this.MedicalDataItemService = medicalDataItemService;
            this.MedicalDataItemValidateService = medicalDataItemValidateService;
            this.MedicalDataWardTypeService = medicalDataWardTypeService;
            this.MedicalDepartmentTypeService = medicalDepartmentTypeService;
            this.MedicalSpecTypeService = medicalSpecTypeService;
            this.MedicalOrganismTypeService = medicalOrganismTypeService;
            this.AuthenticationService = authenticationService;
            this.MedicalOrganismService = medicalOrganismService;
            this.MedicalAntibioticService = medicalAntibioticService;
            this.MedicalDataProjectService = medicalDataProjectService;
        }


        #region 验证导入的数据

        /// <summary>
        /// 根据传入的数据验证规则
        /// </summary>
        /// <param name="uploadModel"></param>
        public void ValidateUploadData(UploadMedicalResult uploadModel)
        {
            try
            {
                #region 获取用于验证的基础数据

                // 使用SPEC_DATE排序，用于后面的剔除重复菌株，无其他用途
                // 日期格式已经在读取数据的时候验证并格式化 yyyy-MM-dd HH:mm:ss 2020-01-13 新加
                var orderBySpecDateList = uploadModel.ExcelItemList.OrderBy(m => m.SPEC_DATE);

                #endregion

                #region 定义相关临时变量

                //没有找到字段的提示
                string warning = "【{0}】字段未填写值，系统已经自动设置为：{1}";
                string warning1 = "【{0}】填写值不正确，系统已经自动设置为：{1}";
                string notFind = "数据文件中未发现字段【{0}】，系统已自动生成，并可根据其它字段推断出此字段的值，在分析数据文件时相关记录会因此产生提示信息，这是正常的";
                string error = "【{0}】字段为必须填写";
                string errorLength = "【{0}】字段的值长度不能{1}";
                string error1 = "【{0}】填写值不正确";

                List<string> bl_sf = new List<string> { "bl", "sf" };
                #endregion

                foreach (var item in uploadModel.ExcelItemList)
                {
                    if (item == null || item.Id <= 0) continue;

                    item.IsValid = false;
                    item.OrganismId = 0;
                    item.OrganismName = "";

                    #region 验证医院代码 INSTITUT

                    //使用当前登录用户所属的医院编码，其他的则不验证
                    if (!string.IsNullOrWhiteSpace(uploadModel.HospitalEntity.Code))
                    {
                        if (string.IsNullOrWhiteSpace(item.INSTITUT) || !item.INSTITUT.ToLower().Equals(uploadModel.HospitalEntity.Code.ToLower()))
                        {
                            item.INSTITUT = uploadModel.HospitalEntity.Code;
                            this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "INSTITUT", "", string.Format(warning1, "INSTITUT", uploadModel.HospitalEntity.Code));
                        }

                        if (string.IsNullOrWhiteSpace(item.LABORATORY) || !item.LABORATORY.ToLower().Equals(uploadModel.HospitalEntity.Code.ToLower()))
                        {
                            item.LABORATORY = uploadModel.HospitalEntity.Code;
                            this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "LABORATORY", "", string.Format(warning1, "LABORATORY", uploadModel.HospitalEntity.Code));
                        }
                    }
                    #endregion

                    #region 自动剔除重复菌株数据：除血液（bl）和脑脊液（sf）外，其他标本只保留第一株细菌
                    /**
                     * 自动剔除重复菌株数据：除血液（bl）和脑脊液（sf）外，其他标本只保留第一株细菌（按日期字段[SPEC_DATE]先后顺序），然后提供合并后的数据下载。
                     * a)	剔除重复菌株原则：国家代码（COUNTRY_A）、实验室代码（LABORATORY）和住院号（PATIENT_ID）一致者
                     * b)	门诊患者（住院号PATIENT_ID一栏为空白者）分离菌株无需剔除重复菌株
                     */
                    if (!bl_sf.Contains(item.SPEC_TYPE.ToLower()) && !string.IsNullOrWhiteSpace(item.PATIENT_ID))
                    {
                        Expression<Func<MedicalDataItem, bool>> predicate = m => !bl_sf.Contains(m.SPEC_TYPE.ToLower());
                        predicate = predicate.And(m => m.COUNTRY_A.ToLower().Equals(item.COUNTRY_A.ToLower()));
                        predicate = predicate.And(m => m.LABORATORY.ToLower().Equals(item.LABORATORY.ToLower()));
                        predicate = predicate.And(m => m.SPEC_TYPE.ToLower().Equals(item.SPEC_TYPE.ToLower()));
                        predicate = predicate.And(m => m.ORGANISM.ToLower().Equals(item.ORGANISM.ToLower()));
                        predicate = predicate.And(m => m.PATIENT_ID.ToLower().Equals(item.PATIENT_ID.ToLower()));
                        Func<MedicalDataItem, bool> itemListWhere = predicate.Compile();

                        // uploadModel.ItemList.Where(m => !bl_sf.Contains(m.SPEC_TYPE.ToLower()) && m.COUNTRY_A.ToLower().Equals(item.COUNTRY_A.ToLower()) && m.LABORATORY.ToLower().Equals(item.LABORATORY.ToLower()) && m.PATIENT_ID.ToLower().Equals(item.PATIENT_ID.ToLower())).Count() <= 0
                        // 判断数据中有无对应的数据
                        if (uploadModel.ItemList.Where(itemListWhere).Count() <= 0)
                        {
                            /**
                             * 数据中还没有对应的数据
                             * 保存顺序的第一个
                             * 在上传数据中查询对应数据的第一条数据(按SPEC_DATE升序) 此处日期格式已经在读取数据时验证并格式化
                             */
                            //var tempValue = orderBySpecDateList.Where(m => m.COUNTRY_A.ToLower().Equals(item.COUNTRY_A.ToLower()) && m.LABORATORY.ToLower().Equals(item.LABORATORY.ToLower()) && m.PATIENT_ID.ToLower().Equals(item.PATIENT_ID.ToLower())).OrderBy(r => r.SPEC_DATE).FirstOrDefault();
                            var tempValue = orderBySpecDateList.Where(itemListWhere).OrderBy(r => r.SPEC_DATE).FirstOrDefault();
                            if (tempValue != null && tempValue.Id != item.Id)
                            {
                                // 该行数据是错误的，有重复，且不是第一个，所以剔除掉
                                this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "SPEC_TYPE", "", "自动剔除重复菌株数据：除血液（bl）和脑脊液（sf）外，其他标本只保留第一株细菌");
                                continue;
                            }
                        }
                        else
                        {
                            // 该行数据已存在/重复，所以剔除掉
                            this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "SPEC_TYPE", "", "自动剔除重复菌株数据：除血液（bl）和脑脊液（sf）外，其他标本只保留第一株细菌");
                            continue;
                        }
                    }
                    #endregion

                    #region   验证细菌  ORGANISM  注意必须要放在最前面，因为填写的细菌不再范围则直接不进行后面验证
                    /**
                     * 100%必填字段，标准3位英文字符代码，此代码为WHONET 5.6标准细菌代码；注意！系统仅接收革兰阳性菌与革兰阴性菌，见附表（4），
                     * 不接受真菌，支原体，厌氧菌和分枝杆菌等。当发现在附表（4）所规定的标准代码外的数据时，系统会给出提示并自动忽略该条数据。
                     */
                    long checkMedicalDataProjectID = (MedicalDataProjectService.QueryEntity(r => r.Name == "浙江省细菌耐药监测网" && r.Mark > 0)?.Id) ?? 0;
                    // 本行数据所对应的细菌
                    MedicalOrganism organismTemp = null;
                    // 必填且长度为3
                    if (string.IsNullOrWhiteSpace(item.ORGANISM) || item.ORGANISM.Length != 3)
                    {
                        this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, "ORGANISM", "", "【ORGANISM】字段的值长度必须等于3位");
                    }
                    else
                    {
                        item.ORGANISM = item.ORGANISM.ToLower();

                        // 本行数据所对应的细菌
                        organismTemp = uploadModel.OrganismList.Where(m => m.Code.ToLower().Equals(item.ORGANISM.ToLower())).FirstOrDefault();

                        if (checkMedicalDataProjectID != uploadModel.MedicalEntity.ProjectType && (organismTemp == null || organismTemp.Id <= 0))
                        {
                            this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "ORGANISM", "", "【ORGANISM】字段的值系统无法识别，自动忽略该条数据");
                            continue;
                        }
                    }

                    if (checkMedicalDataProjectID != uploadModel.MedicalEntity.ProjectType && (organismTemp == null || organismTemp.Id <= 0))
                    {
                        this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "ORGANISM", "", "ORGANISM ，自动忽略该条数据");
                        continue;
                    }

                    item.OrganismName = (organismTemp?.Name) ?? ""; // 只有MedicalEntity.ProjectType=浙江省耐药监测时可能为null
                    item.OrganismId = (organismTemp?.Id) ?? 0; // 只有MedicalEntity.ProjectType=浙江省耐药监测时可能为null
                    #endregion

                    #region 验证来源 ORIGIN

                    item.ORIGIN = "h"; //统一值，不用导入

                    #endregion

                    #region  验证性别 SEX

                    //此字段要求必填，最长1个字，WHONET要求只能填写“f”或者“m”，系统提供了相关的容错处理机制，允许填写“男”和“女”，
                    //系统会自动将“男”替换成“m”，将“女”替换成“f”。

                    item.SEX = item.SEX.ToLower();
                    if (string.IsNullOrWhiteSpace(item.SEX) || item.SEX.Length > 1)
                        this.AddValidateItem(uploadModel.ValidateList, 0, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, "SEX", "", "【SEX】字段为必须填写，且只能是“f”、“m”或者 男”和“女”");

                    if (!string.IsNullOrWhiteSpace(item.SEX) && !item.SEX.Equals("男") && !item.SEX.Equals("女") && !item.SEX.Equals("m") && !item.SEX.Equals("f"))
                        this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, "SEX", "", "【SEX】字段无法识别，且只能是“f”、“m”或者 男”和“女”");

                    if (item.SEX.Equals("男"))
                    {
                        item.SEX = "m";
                        this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "SEX", "", string.Format(warning1, "SEX", "m"));
                    }

                    else if (item.SEX.Equals("女"))
                    {
                        item.SEX = "f";
                        this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "SEX", "", string.Format(warning1, "SEX", "f"));
                    }


                    #endregion

                    #region 验证生日 DATE_BIRTH

                    if (item.DATE_BIRTH == null || !item.DATE_BIRTH.IsDateTime() || item.DATE_BIRTH.GetDateTime().Year <= 1900)
                        item.DATE_BIRTH = DateHelper.DefaultValue().ToString("yyyy-MM-dd HH:mm:ss");

                    #endregion

                    #region 验证科室类别  WARD_TYPE

                    //为非必填项，最大长度为3个字。如果不填写，系统不进行校验，如果填写，则必须与WHONET内置的代码要求一致，见附表1。

                    if (!string.IsNullOrWhiteSpace(item.WARD_TYPE))
                    {
                        if (item.WARD_TYPE.Length > 3)
                            this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, "WARD_TYPE", "", string.Format(errorLength, "WARD_TYPE", "大于3个字符"));

                        var tTemp = uploadModel.HospitalWardList.Where(m => m.Location_Type.ToLowerInvariant().Equals(item.WARD_TYPE.ToLowerInvariant())).FirstOrDefault();
                        if (tTemp == null || tTemp.Id <= 0)
                            this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, "WARD_TYPE", "", "【WARD_TYPE】字段的值【" + item.WARD_TYPE + "】系统无法识别");
                    }

                    #endregion

                    #region 验证专业类别 DEPARTMENT
                    /*
                    为100%必填项，最大长度为3个英文字符，此字段所填写的值，必须为系统所规定的标准专业类别代码，见附表2。
                    未在该附表中的值为非法值，无法通过系统校验。系统提供了容错处理，可直接通过WARD字段自动推算此列的值。因此，此字段其实可以不填写。
                    */
                    if (string.IsNullOrEmpty(item.DEPARTMENT) || item.DEPARTMENT.Length > 3)
                    {
                        //有错误需要容错
                        var wardType = uploadModel.DepartmentTypeList.Where(m => m.Code.Equals(item.WARD) || m.Name.Equals(item.WARD)).FirstOrDefault();
                        if (wardType != null)
                        {
                            this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "DEPARTMENT", "", string.Format(warning1, "DEPARTMENT", wardType.Code));
                            item.DEPARTMENT = wardType.Code;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(item.DEPARTMENT) && string.IsNullOrWhiteSpace(item.WARD))
                    {
                        this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, "DEPARTMENT", "", string.Format(error, "DEPARTMENT"));
                    }

                    #endregion

                    #region 验证标本类型英文代码 SPEC_TYPE

                    /*
                   “标本类型（SPEC_TYPE）”和标本代码“（SPEC_CODE）”对应验证，错误者以“标本类型”一栏名称自动纠正标本代码；对应代码和名称见表1。
                    */

                    if (!string.IsNullOrWhiteSpace(item.SPEC_TYPE))
                    {
                        //非空的数据在前面已经删除了，这里只处理非空的情况
                        var speTemp = uploadModel.SpecTypeList.Where(m => m.Code.ToLower().Equals(item.SPEC_TYPE.ToLower())).FirstOrDefault();
                        if (speTemp == null || speTemp.Id <= 0)
                        {
                            this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "SPEC_TYPE", "", "【SPEC_TYPE】字段的值【" + item.SPEC_TYPE + "】系统无法识别");
                        }
                        else
                        {
                            // 2019-02-21 新规则：SPEC_TYPE没填写则删除改行数据，SPEC_CODE的值则根据SPEC_TYPE来获取设置
                            item.SPEC_CODE = speTemp.Number.ToString();
                        }
                    }

                    #endregion

                    #region   验证细菌类别  ORG_TYPE

                    /*系统仅接受革兰阳性菌“+”和革兰阴性菌代码“-”，最大允许1字符，此字段可不填写。当系统发现未填写时会自动根据ORGANISM字段的值推导出来
                    ，当系统发现本字段中填写的值与系统所推导的合法值不匹配的时候，会自动将其替换成合法值。
                    */
                    if (!string.IsNullOrWhiteSpace(item.ORGANISM))
                    {
                        item.ORGANISM = item.ORGANISM.ToLower();
                        if (organismTemp != null && organismTemp.Id > 0)
                        {
                            item.ORG_TYPE = organismTemp.Type;
                            if (!string.IsNullOrWhiteSpace(item.ORG_TYPE) && !item.ORG_TYPE.ToLower().Equals(organismTemp.Type))
                            {
                                this.AddValidateItem(uploadModel.ValidateList, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "ORG_TYPE", "", string.Format(warning1, "ORG_TYPE", item.ORG_TYPE));
                            }
                        }
                    }

                    #endregion

                    #region 验证抗生素

                    item.AMK_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AMK_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AMK_ND30");
                    item.AMC_ND20 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AMC_ND20, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AMC_ND20");
                    item.AZM_ND15 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AZM_ND15, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AZM_ND15");
                    item.AMP_ND10 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AMP_ND10, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AMP_ND10");
                    item.SAM_ND10 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.SAM_ND10, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "SAM_ND10");
                    item.ATM_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.ATM_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "ATM_ND30");
                    item.OXA_ND1 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.OXA_ND1, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "OXA_ND1");
                    item.POL_ND300 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.POL_ND300, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "POL_ND300");
                    item.NIT_ND300 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.NIT_ND300, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "NIT_ND300");
                    item.SXT_ND1_2 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.SXT_ND1_2, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "SXT_ND1_2");
                    item.STH_ND300 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.STH_ND300, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "STH_ND300");
                    item.GEH_ND120 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.GEH_ND120, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "GEH_ND120");
                    item.ERY_ND15 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.ERY_ND15, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "ERY_ND15");
                    item.CIP_ND5 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CIP_ND5, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CIP_ND5");
                    item.CLI_ND2 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CLI_ND2, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CLI_ND2");
                    item.RIF_ND5 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.RIF_ND5, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "RIF_ND5");
                    item.LNZ_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.LNZ_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "LNZ_ND30");
                    item.STR_ND10 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.STR_ND10, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "STR_ND10");
                    item.FOS_ND200 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.FOS_ND200, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "FOS_ND200");
                    item.CHL_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CHL_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CHL_ND30");
                    item.MEM_ND10 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.MEM_ND10, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "MEM_ND10");
                    item.MNO_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.MNO_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "MNO_ND30");
                    item.MFX_ND5 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.MFX_ND5, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "MFX_ND5");
                    item.PIP_ND100 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.PIP_ND100, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "PIP_ND100");
                    item.TZP_ND100 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TZP_ND100, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TZP_ND100");
                    item.PEN_ND10 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.PEN_ND10, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "PEN_ND10");
                    item.GEN_ND10 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.GEN_ND10, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "GEN_ND10");
                    item.TCY_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TCY_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TCY_ND30");
                    item.TCC_ND75 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TCC_ND75, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TCC_ND75");
                    item.TIC_ND75 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TIC_ND75, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TIC_ND75");
                    item.TEC_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TEC_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TEC_ND30");
                    item.TGC_ND15 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TGC_ND15, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TGC_ND15");
                    item.FEP_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.FEP_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "FEP_ND30");
                    item.CXM_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CXM_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CXM_ND30");
                    item.CEC_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CEC_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CEC_ND30");
                    item.CFP_ND75 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CFP_ND75, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CFP_ND75");
                    item.CSL_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CSL_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CSL_ND30");
                    item.CRO_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CRO_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CRO_ND30");
                    item.CTX_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CTX_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CTX_ND30");
                    item.CAZ_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CAZ_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CAZ_ND30");
                    item.FOX_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.FOX_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "FOX_ND30");
                    item.CZO_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CZO_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CZO_ND30");
                    item.TOB_ND10 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TOB_ND10, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TOB_ND10");
                    item.VAN_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.VAN_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "VAN_ND30");
                    item.IPM_ND10 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.IPM_ND10, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "IPM_ND10");
                    item.LVX_ND5 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.LVX_ND5, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "LVX_ND5");
                    item.OFX_ND5 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.OFX_ND5, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "OFX_ND5");
                    item.DOX_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.DOX_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "DOX_ND30");
                    item.AMK_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AMK_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AMK_NM");
                    item.AMC_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AMC_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AMC_NM");
                    item.AZM_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AZM_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AZM_NM");
                    item.AMP_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AMP_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AMP_NM");
                    item.SAM_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.SAM_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "SAM_NM");
                    item.ATM_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.ATM_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "ATM_NM");
                    item.OXA_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.OXA_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "OXA_NM");
                    item.POL_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.POL_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "POL_NM");
                    item.NIT_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.NIT_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "NIT_NM");
                    item.SXT_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.SXT_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "SXT_NM");
                    item.STH_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.STH_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "STH_NM");
                    item.GEH_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.GEH_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "GEH_NM");
                    item.ERY_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.ERY_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "ERY_NM");
                    item.CIP_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CIP_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CIP_NM");
                    item.CLI_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CLI_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CLI_NM");
                    item.RIF_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.RIF_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "RIF_NM");
                    item.LNZ_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.LNZ_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "LNZ_NM");
                    item.STR_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.STR_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "STR_NM");
                    item.FOS_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.FOS_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "FOS_NM");
                    item.CHL_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CHL_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CHL_NM");
                    item.MEM_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.MEM_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "MEM_NM");
                    item.MNO_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.MNO_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "MNO_NM");
                    item.MFX_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.MFX_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "MFX_NM");
                    item.PIP_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.PIP_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "PIP_NM");
                    item.TZP_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TZP_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TZP_NM");
                    item.PEN_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.PEN_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "PEN_NM");
                    item.GEN_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.GEN_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "GEN_NM");
                    item.PEN_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.PEN_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "PEN_NE");
                    item.TCY_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TCY_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TCY_NM");
                    item.TCC_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TCC_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TCC_NM");
                    item.TIC_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TIC_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TIC_NM");
                    item.TEC_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TEC_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TEC_NM");
                    item.TGC_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TGC_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TGC_NM");
                    item.FEP_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.FEP_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "FEP_NM");
                    item.CXM_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CXM_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CXM_NM");
                    item.CFP_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CFP_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CFP_NM");
                    item.CSL_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CSL_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CSL_NM");
                    item.CRO_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CRO_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CRO_NM");
                    item.CTX_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CTX_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CTX_NM");
                    item.CAZ_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CAZ_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CAZ_NM");
                    item.FOX_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.FOX_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "FOX_NM");
                    item.CZO_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CZO_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CZO_NM");
                    item.TOB_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TOB_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TOB_NM");
                    item.VAN_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.VAN_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "VAN_NM");
                    item.VAN_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.VAN_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "VAN_NE");
                    item.IPM_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.IPM_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "IPM_NM");
                    item.LVX_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.LVX_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "LVX_NM");
                    item.CTX_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CTX_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CTX_NE");
                    item.CSL_ND75 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CSL_ND75, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CSL_ND75");
                    item.ETP_ND10 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.ETP_ND10, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "ETP_ND10");
                    item.ETP_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.ETP_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "ETP_NM");
                    item.CTT_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CTT_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CTT_ND30");
                    item.CTT_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CTT_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CTT_NM");
                    item.DOR_ND10 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.DOR_ND10, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "DOR_ND10");
                    item.DOR_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.DOR_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "DOR_NM");
                    item.NET_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.NET_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "NET_ND30");
                    item.NET_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.NET_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "NET_NM");
                    item.QDA_ND15 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.QDA_ND15, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "QDA_ND15");
                    item.QDA_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.QDA_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "QDA_NM");
                    item.CPT_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CPT_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CPT_ND30");
                    item.CPT_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CPT_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CPT_NM");
                    item.CPT_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CPT_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CPT_NE");
                    item.CZA_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CZA_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CZA_ND30");
                    item.CZA_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CZA_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CZA_NM");
                    item.CZA_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CZA_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CZA_NE");
                    item.AZA_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AZA_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AZA_ND30");
                    item.AZA_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AZA_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AZA_NM");
                    item.AZA_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AZA_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AZA_NE");
                    item.CZT_ND30 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CZT_ND30, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CZT_ND30");
                    item.CZT_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CZT_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CZT_NM");
                    item.CZT_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CZT_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CZT_NE"); 
                    //item.TGC_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TGC_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TGC_NE");//新增字段
                    item.DOX_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.DOX_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "DOX_NM");//新增字段
                    item.COL_ND10 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.COL_ND10, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "COL_ND10");//新增字段
                    item.COL_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.COL_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "COL_NM");//新增字段
                    item.COL_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.COL_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "COL_NE");//新增字段
                    item.AMC_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AMC_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AMC_NE");//新增字段
                    item.AMK_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AMK_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AMK_NE");//新增字段
                    item.AMP_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AMP_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AMP_NE");//新增字段
                    item.ATM_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.ATM_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "ATM_NE");//新增字段
                    item.AZM_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.AZM_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "AZM_NE");//新增字段
                    item.CAZ_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CAZ_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CAZ_NE");//新增字段
                    item.CEC_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CEC_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CEC_NE");//新增字段
                    item.CFP_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CFP_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CFP_NE");//新增字段
                    item.CHL_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CHL_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CHL_NE");//新增字段
                    item.CIP_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CIP_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CIP_NE");//新增字段
                    item.CLI_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CLI_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CLI_NE");//新增字段
                    item.CRO_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CRO_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CRO_NE");//新增字段
                    item.CTT_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CTT_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CTT_NE");//新增字段
                    item.CXM_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CXM_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CXM_NE");//新增字段
                    item.CZO_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.CZO_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "CZO_NE");//新增字段
                    item.DOR_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.DOR_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "DOR_NE");//新增字段
                    item.DOX_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.DOX_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "DOX_NE");//新增字段
                    item.ERY_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.ERY_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "ERY_NE");//新增字段
                    item.ETP_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.ETP_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "ETP_NE");//新增字段
                    item.FEP_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.FEP_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "FEP_NE");//新增字段
                    item.FOS_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.FOS_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "FOS_NE");//新增字段
                    item.FOX_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.FOX_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "FOX_NE");//新增字段
                    item.GEN_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.GEN_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "GEN_NE");//新增字段
                    item.IPM_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.IPM_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "IPM_NE");//新增字段
                    item.LNZ_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.LNZ_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "LNZ_NE");//新增字段
                    item.LVX_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.LVX_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "LVX_NE");//新增字段
                    item.MEM_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.MEM_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "MEM_NE");//新增字段
                    item.MFX_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.MFX_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "MFX_NE");//新增字段
                    item.MNO_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.MNO_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "MNO_NE");//新增字段
                    item.NET_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.NET_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "NET_NE");//新增字段
                    item.NIT_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.NIT_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "NIT_NE");//新增字段
                    item.OXA_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.OXA_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "OXA_NE");//新增字段
                    item.PIP_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.PIP_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "PIP_NE");//新增字段
                    item.POL_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.POL_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "POL_NE");//新增字段
                    item.QDA_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.QDA_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "QDA_NE");//新增字段
                    item.RIF_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.RIF_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "RIF_NE");//新增字段
                    item.SAM_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.SAM_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "SAM_NE");//新增字段
                    item.STH_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.STH_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "STH_NE");//新增字段
                    item.STR_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.STR_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "STR_NE");//新增字段
                    item.SXT_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.SXT_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "SXT_NE");//新增字段
                    item.TCC_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TCC_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TCC_NE");//新增字段
                    item.TCY_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TCY_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TCY_NE");//新增字段
                    item.TEC_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TEC_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TEC_NE");//新增字段
                    item.TGC_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TGC_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TGC_NE");//新增字段
                    item.TIC_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TIC_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TIC_NE");//新增字段
                    item.TOB_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TOB_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TOB_NE");//新增字段
                    item.TZP_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.TZP_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "TZP_NE");//新增字段

                    //2025-04-22 Gerry 新增加字段：依拉环素ERV_NM，ERV_ND20, ERV_NE
                    item.ERV_NM = this.CheckAntibiotic(uploadModel.ValidateList, item, item.ERV_NM, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "ERV_NM");//新增字段
                    item.ERV_ND20 = this.CheckAntibiotic(uploadModel.ValidateList, item, item.ERV_ND20, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "ERV_ND20");//新增字段
                    item.ERV_NE = this.CheckAntibiotic(uploadModel.ValidateList, item, item.ERV_NE, uploadModel.AntibioticList, uploadModel.AntibioticRuleList, item.ORGANISM, "ERV_NE");//新增字段

                    #endregion

                    //如果该行数据验证中没有错误的，那么这个则是有效的
                    item.IsValid = uploadModel.ValidateList.Where(m => m.MedicalDataItemId == item.Id && m.Level == (int)MedicalDataItemValidateLevelEnum.Error).Count() <= 0;

                    uploadModel.ItemList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Log4Helper.Info($"{uploadModel.MedicalEntity.Id}： 验证数据发生异常");
                Log4Helper.Error(typeof(MedicalAntibioticRuleService), ex);
                throw;
            }
        }

        /// <summary>
        ///  获取验证对象数据
        /// </summary>
        private void AddValidateItem(List<MedicalDataItemValidate> list, long id, int rowIndex, MedicalDataItemValidateLevelEnum level, string keyName, string errorName, string content)
        {
            list.Add(new MedicalDataItemValidate()
            {
                Sort = ++DataSort,
                UploadRowIndex = rowIndex,
                KeyName = keyName,
                Content = content,
                ErrorName = errorName,
                Level = (int)level,
                MedicalDataItemId = id
            });
        }

        /// <summary>
        /// 按照规则验证日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ValidateDateTime(string value)
        {
            //支持的格式：
            /*
            2014 11 05、2014 11 5、2014-11-05、2014-11-5、2014年11月05日、2014年11月5日、
            Nov - 25-2014、Nov/05/2016、05 Nov 2014、05-Nov-2014、05/Nov/2014、
             2014 05 Nov、2014-05-Nov、2014/05/Nov、2014 Nov 05、2014-Nov-05、2014/Nov/05
            */

            if (value.IsDateTime()) return value;

            //20141105（如果没有分隔符，那么长度必须为8位）
            if (value.IsLong() && value.Length == 8) return value;

            throw new Exception("格式不正确");
        }

        /// <summary>
        /// 验证抗生素
        /// </summary>
        /// <param name="list">记录验证信息列表</param>
        /// <param name="item">当前数据项</param>
        /// <param name="value">当前项的Excel值</param>
        /// <param name="antibioticList">抗生素集合</param>
        /// <param name="ruleList">验证规则集合</param>
        /// <param name="germCode">细菌编码</param>
        /// <param name="keyName">当前验证的字段名称</param>
        private string CheckAntibiotic(List<MedicalDataItemValidate> list, MedicalDataItem item, string value, List<MedicalAntibiotic> antibioticList, List<MedicalAntibioticRule> ruleList, string germCode, string keyName)
        {
            try
            {
                //目前的规则，所有的抗生素都可以不必填
                if (list == null || item == null ||
                    antibioticList == null || !antibioticList.Any() ||
                    ruleList == null || !ruleList.Any() ||
                    string.IsNullOrWhiteSpace(value) ||
                    string.IsNullOrWhiteSpace(germCode) ||
                    string.IsNullOrWhiteSpace(keyName) ||
                    value.Equals("0"))
                {
                    return value;
                }

                #region 简单处理填写的数值，如删除符号，小数点删掉或补全等
                string oldValue = value; // 原始的数据值，容错以后特殊情况除外，其他的保留原始用户上传的值
                germCode = germCode.ToLower();
                keyName = keyName.ToUpper();

                value = value.DeleteSymbol(); //删除值中的各种符号，包括大于等于等等，这个值只是用于数据验证，不做其他的处理
                value = value.TrimEnd('.'); //删除值中最后的一个点，例如：128.  则替换成128

                //处理错误数据 空或者是 . 的情况
                if (string.IsNullOrWhiteSpace(value) || value.Trim().Equals("."))
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", $"【{keyName}】字段值${oldValue}$系统无法识别");
                    item.IsValid = false;
                    return oldValue;
                }

                //处理小数带0的情况，例如：2.0 系统替换成2，2.3则不替换
                if (value.Contains("."))
                {
                    value = Regex.Replace(value, "0+?$", "");
                    value = Regex.Replace(value, "[.]$", "");
                }
                //if (value.LastIndexOf(".") > 0 && (value.Substring(value.LastIndexOf(".")).Equals(".0") || value.Substring(value.LastIndexOf(".")).Equals(".00")))
                //    value = value.Substring(0, value.LastIndexOf("."));

                //处理前面带.的情况，例如： <=.12，因为在上面已经删除符号了，所以得到的是 .12  等等情况，前面追加一个0
                if (value.StartsWith("."))
                {
                    value = $"0{value}";
                }
                #endregion

                #region 验证规则说明

                /*
                    所有抗生素代码采用Whonet 5.6标准代码。抗生素代码见附表（5）。
                    纸片法（K-b法）：_ND
                    不要求强制填写
                    如果填写结果只能为6至80之间的整数，否则视为填写错误，系统会将该值忽略，这会影响该列的通过率。对应的字段名应为：抗生素英文代码+“_ND”+抗生素含量，例如：AMK_ND30、AMX_ND25。切记：不能为（S、I、R）、不能有小数、不能带任何符号（.><=等等）。

                    MIC法：_NM
                    不要求强制填写
                    对于除高浓度庆大霉素和高浓度链霉素以外的MIC法字段，
                    1. 系统要求只能为小数或者1、2、4、8、16、32、64、128、256、512，不在这些数字范围内的，均为非法值;
                    2. 可以有前导【>、<、=】符号;
                       2.1. 合法情况举例：
                            2.1.1. 等于纯整数数字或者小数数字：4、4.0；
                            2.1.2. 大于或者大于等于上述两类数字：>4、>=4、>4.0、>=4.0；
                            2.1.3. 小于或者小于等于上述两类数字：<4、<=4、<4.0、<=4.0；
                    3. 对应的字段名应为：抗生素英文代码+“_NM”；
                       3.1. 例如：AMK_NM、AMX_NM；
                    4. 切记：不能为（S、I、R）、不能为非2的平方数如6、7、99、100等等。

                    MIC法特殊字段-【GEH_NM】高浓度庆大霉素的填写规则：
                    以下特殊规则将直接替代普通MIC法的校验规则，具体如下：
                    1、仅允许出现如下值：250，256，500，512，513，1000，1024，1025，不在这些数字范围内的，均为非法值;
                    2、可以有前导【>、<、=】符号;
                    3、如果填写了【SYN-S】，系统将自动替换为【<=500】;
                    4、如果填写了【SYN-R】，则将自动替换为【>=1000】。

                    MIC法特殊字段-【STH_NM】，高浓度链霉素的填写规则：
                    以下特殊规则直接替代普通MIC法的校验规则，具体如下：
                    1、仅允许出现如下值：500，512，1000，1024，2000，2048，2049，不在这些数字范围内的，均视为非法值;
                    2、可以有前导【>、<、=】符号;
                    3、如果填写了【SYN-S】，系统将自动替换为【<=1000】;
                    4、如果填写了【SYN-R】则将自动替换为【>=2000】。

                    MIC法特殊字段-【SXT_NM】复方新诺明的填写规则：
                    该结果上传后系统自动将数据为320的字符替换为16，160替换为8，80替换4，40替换为2，20替换为1，10替换为0.5，其余字符保持原始数据。
                    A.系统会将数据中出现的一些特殊字符替换为合法字符，具体如下：
                    a)全角“≧”或“≥” 将替换为半角“>=”
                    b)全角“≦”、“≤” 将替换为半角“<=”
                    c)全角“＞”将替换为半角“>”
                    d)全角“＜”将替换为半角“<”
                    e)全角“＝”将替换为半角“=”
                    f)全角“．”与“。”将替换为半角“.”
                    在数据中若出现未在附表（5）中的其它抗生素字段，系统会自动忽略该字段的数据，不予保存。
                    本系统目前抗菌药物敏感性执行标准为CLSI M100-S23，上传数据如果有未达到该折点标准的数据，系统会将该组数据中的未达到CLSI M100-S23标准的抗生素结果忽略。如某一抗生素，CLSI M100-S23折点为S:<=2，I:4-8，R:>=16，测定结果为<=8，这样的数据系统无法判断是敏感还是中介，系统在统计的时候会自动将该组数据中的这个抗生素结果给忽略掉，不予统计。
                */

                #endregion

                #region 根据字段的名称获取抗生素的对象

                // 获取抗生素信息  例如：AMK_ND30
                if (keyName.IndexOf("_") <= 0)
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, keyName, "", "【" + keyName + "】字段系统无法识别，自动忽略该条数据");
                    item.IsValid = false;
                    return oldValue;
                }

                // 例如：AMK
                string antibioticCode = keyName.ToUpperInvariant().Substring(0, keyName.IndexOf("_"));

                // 根据抗生素Code查询抗生素实体
                MedicalAntibiotic antibioticEntity = antibioticList.Where(m => m.Mark > 0 && m.Code.ToUpperInvariant().Equals(antibioticCode)).FirstOrDefault();
                if (antibioticEntity == null || antibioticEntity.Id <= 0)
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, keyName, "", "【" + keyName + "】字段系统无法识别，自动忽略该条数据");
                    item.IsValid = false;
                    return oldValue;
                }

                #endregion

                //验证KB法
                if (keyName.Contains("_ND") && !string.IsNullOrWhiteSpace(value))
                    return this.CheckAntibioticKB(value, oldValue, keyName, list, item);

                //验证MIC法
                if (keyName.Contains("_NM") && !string.IsNullOrWhiteSpace(value))
                    return this.CheckAntibioticMIC(value, oldValue, keyName, list, item);

                return oldValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证抗生素，KB法
        /// </summary>
        /// <param name="value">去除符号等处理以后的值</param>
        /// <param name="oldValue">原始的excel值</param>
        /// <param name="keyName">当前验证的字段名称</param>
        /// <param name="list">记录验证信息列表</param>
        /// <param name="item">当前数据项</param>
        /// <returns></returns>
        private string CheckAntibioticKB(string value, string oldValue, string keyName, List<MedicalDataItemValidate> list, MedicalDataItem item)
        {
            /**
             * 纸片法(KB法)规则验证
             * 1. 所有的抗生素名称中带有“_ND”的则是纸片法（KB法）；
             * 2. 不要求强制填写；
             * 3. 如果有值且只能是[6,80]范围的整数;
             * 4. 不能为（S、I、R）、不能有小数、不能带任何符号（.><=等等）;
             * 5. 对于填写错误的则提示错误，系统自动删除错误的值。 
             */

            if (!keyName.Contains("_ND") || string.IsNullOrWhiteSpace(value))
                return oldValue;

            //处理2种极端情况
            if (oldValue.ReplaceSymbol().Equals(">80") || oldValue.ReplaceSymbol().Equals("<6"))
            {
                this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，纸片法（K-b法）填写结果只能为6至80之间的整数");
                item.IsValid = false;
                return oldValue;
            }

            string tempValue = value.Replace("mm", "").Replace("MM", "");
            if (!tempValue.IsInt() || tempValue.GetInt() < 6 || tempValue.GetInt() > 80)
            {
                this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", $"【{keyName}】字段值系统无法识别，填写的值${oldValue}$超出了整数范围[6-80]");
                item.IsValid = false;
                return oldValue;
            }

            return oldValue;
        }

        /// <summary>
        /// 验证抗生素，MIC法
        /// </summary>
        /// <param name="value">去除符号等处理以后的值</param>
        /// <param name="oldValue">原始的excel值</param>
        /// <param name="keyName">当前验证的字段名称</param>
        /// <param name="list">记录验证信息列表</param>
        /// <param name="item">当前数据项</param>
        /// <returns></returns>
        private string CheckAntibioticMIC(string value, string oldValue, string keyName, List<MedicalDataItemValidate> list, MedicalDataItem item)
        {
            /**
             * 常规MIC法规则验证
             * 1. 不要求强制填写；
             * 2. 如果有值只能是【0.03,0.06,0.12,0.125,0.25,0.5,1,2,4,8,16,32,64,128,256,512,1024,2048】；
             * 3. 可以有前导【>、<、=】符号;
             * 4. 不能为（S、I、R）、不能为非2的平方数如6、7、99、100等等。
             */
            if (!keyName.Contains("_NM") || string.IsNullOrWhiteSpace(value))
            {
                return oldValue;
            }
            else if(keyName.Contains("_NM"))
            {
                /**
                 * 验证字段名称包含_NM的值含有/的情况下，只获取/之前的值，如：>=2/40, 只获取>=2
                 */
                if (value.Contains("/"))
                {
                    string[] value2 = value.Split('/');
                    value = value2[0].ToString();
                }
            }

            //处理2种极端情况
            if (oldValue.ReplaceSymbol().Equals(">6000") || oldValue.ReplaceSymbol().Equals("<=0"))
            {
                this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", $"【{keyName}】字段值系统无法识别，填写的值${oldValue}$超出了范围(0, 6000]");
                item.IsValid = false;
                return oldValue;
            }

            switch (keyName.ToUpperInvariant().Trim())
            {
                case "GEH_NM":
                    {
                        #region MIC法特殊字段-【GEH_NM】高浓度庆大霉素

                        /**
                        * MIC法特殊字段-【GEH_NM】高浓度庆大霉素的填写规则：
                        * 以下特殊规则将直接替代普通MIC法的校验规则，具体如下：
                        * 1、仅允许出现如下值：250，256，500，512，513，1000，1024，1025，不在这些数字范围内的，均为非法值;
                        * 2、可以有前导【>、<、=】符号;
                        * 3、如果填写了【SYN-S】，系统将自动替换为【<=500】;
                        * 4、如果填写了【SYN-R】，则将自动替换为【>=1000】。
                        */
                        if (oldValue.ToUpperInvariant().Equals("SYN-S") || oldValue.ToUpperInvariant().Equals("S"))
                        {
                            this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, keyName, "", $"系统自动将填写的值“{oldValue}”转换成“<=500”");
                            oldValue = "<=500";
                            return oldValue;
                        }
                        else if (oldValue.ToUpperInvariant().Equals("SYN-R") || oldValue.ToUpperInvariant().Equals("R"))
                        {
                            this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, keyName, "", $"系统自动将填写的值“{oldValue}”转换成“>=1000”");
                            oldValue = ">=1000";
                            return oldValue;
                        }

                        string tempValue = value.Replace("mm", "").Replace("MM", "");

                        if (!tempValue.IsInt())
                        {
                            this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，必须为纯数字或带前导【>、<、=】符号");
                            item.IsValid = false;
                            return oldValue;
                        }

                        int intValue = tempValue.GetInt();

                        List<int> gen_value_support = new List<int> { 250, 256, 500, 512, 513, 1000, 1024, 1025 };

                        if (!gen_value_support.Contains(intValue))
                        {
                            this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，仅允许出现如下值：250，256，500，512，513，1000，1024，1025");
                            item.IsValid = false;
                            return oldValue;
                        }

                        return oldValue;

                        #endregion
                    }
                case "STH_NM":
                    {
                        #region MIC法特殊字段-【STH_NM】高浓度链霉素

                        /**
                        *  MIC法特殊字段-【STH_NM】高浓度链霉素的填写规则：
                        * 以下特殊规则直接替代普通MIC法的校验规则，具体如下：
                        * 1、仅允许出现如下值：500，512，1000，1024，2000，2048，2049，不在这些数字范围内的，均视为非法值;
                        * 2、可以有前导【>、<、=】符号;
                        * 3、如果填写了【SYN-S】，系统将自动替换为【<=1000】;
                        * 4、如果填写了【SYN-R】则将自动替换为【>=2000】。
                        */
                        if (oldValue.ToUpperInvariant().Equals("SYN-S") || oldValue.ToUpperInvariant().Equals("S"))
                        {
                            this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, keyName, "", "系统自动将填写的值“" + oldValue + "”转换成“<=1000”");
                            oldValue = "<=1000";
                            return oldValue;
                        }
                        else if (oldValue.ToUpperInvariant().Equals("SYN-R") || oldValue.ToUpperInvariant().Equals("R"))
                        {
                            this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, keyName, "", "系统自动将填写的值“" + oldValue + "”转换成“>=2000”");
                            oldValue = ">=2000";
                            return oldValue;
                        }

                        string tempValue = value.Replace("mm", "").Replace("MM", "");

                        if (!tempValue.IsInt())
                        {
                            this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，必须为纯数字或带前导【>、<、=】符号");
                            item.IsValid = false;
                            return oldValue;
                        }

                        int intValue = tempValue.GetInt();

                        List<int> sth_value_support = new List<int> { 500, 512, 1000, 1024, 2000, 2048, 2049 };

                        if (!sth_value_support.Contains(intValue))
                        {
                            this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，仅允许出现如下值：500，512，1000，1024，2000，2048，2049");
                            item.IsValid = false;
                            return oldValue;
                        }
                        return oldValue;

                        #endregion
                    }
                case "SXT_NM":
                    {
                        #region MIC法特殊字段-【SXT_NM】复方新诺明

                        /**
                        * MIC法特殊字段-【SXT_NM】复方新诺明的填写规则：
                        * 该结果上传后系统自动将数据为320的字符替换为16，160替换为8，80替换4，40替换为2，20替换为1，10替换为0.5，其余字符保持原始数据。
                        * A.系统会将数据中出现的一些特殊字符替换为合法字符，具体如下：
                        * a)全角“≧”或“≥” 将替换为半角“>=”
                        * b)全角“≦”、“≤” 将替换为半角“<=”
                        * c)全角“＞”将替换为半角“>”
                        * d)全角“＜”将替换为半角“<”
                        * e)全角“＝”将替换为半角“=”
                        * f)全角“．”与“。”将替换为半角“.”
                        */

                        // 抗菌药物“SXT_NM”或“SXT_NE”一栏若数值为10、20、40、80、160、320、640、1280、2560、5120，自动将这些数值除以  20.

                        string tempValue = value.Replace("mm", "").Replace("MM", "");

                        /**
                         * SXT_NM的值含有/的情况下，只获取/之前的值，如：>=2/40, 只获取>=2
                         */
                        //if (tempValue.Contains("/"))
                        //{
                        //    string[] tempValue2 = tempValue.Split('/');
                        //    tempValue = tempValue2[0].ToString();
                        //}

                        if (!tempValue.IsNumeric())
                        {
                            this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，必须为纯数字或带前导【>、<、=】符号");
                            item.IsValid = false;
                            return oldValue;
                        }

                        //if (!tempValue.IsInt())
                        //{
                        //    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，必须为纯数字或带前导【>、<、=】符号");
                        //    item.IsValid = false;
                        //    return oldValue;
                        //}

                        double newValue = 0;
                        double doubleValue = 0;
                        if (!(tempValue.Contains(".") && tempValue.IsDouble(out doubleValue)))
                        {
                            int intValue = tempValue.GetInt();
                            List<int> sxt_value_support = new List<int> { 10, 20, 40, 80, 160, 320, 640, 1280, 2560, 5120 };
                            if (sxt_value_support.Contains(intValue))
                            {
                                newValue = (double)intValue / 20.0;
                                this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, "SXT_NM", "", $"【SXT_NM】字段的值被修正，除以20，原始值【{intValue}】");
                            }
                            else
                            {
                                newValue = intValue;
                            }
                        }
                        else
                        {
                            newValue = doubleValue;
                        }


                        oldValue = oldValue
                            .Replace("≧", ">=")
                            .Replace("≥", ">=")
                            .Replace("≦", "<=")
                            .Replace("≤", ">=")
                            .Replace("＞", ">")
                            .Replace("＜", "<")
                            .Replace("＝", "=")
                            .Replace("．", ".")
                            .Replace("。", ".");

                        oldValue = oldValue.StartsWith(">=") ? $">={newValue}" :
                           oldValue.StartsWith("<=") ? $"<={newValue}" :
                           oldValue.StartsWith(">") ? $">{newValue}" :
                           oldValue.StartsWith("<") ? $"<{newValue}" :
                           newValue.ToString();

                        return oldValue;
                        #endregion
                    }
                default:
                    {
                        //string tempValue = value.Replace("mm", "").Replace("MM", "");

                        //if (!tempValue.IsFloat())
                        //{
                        //    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，填写的值不在指定范围内");
                        //    item.IsValid = false;
                        //    return oldValue;
                        //}

                        //float floatValue = tempValue.GetFloat();

                        //List<float> general_value_support = new List<float> { 0.03f, 0.06f, 0.12f, 0.125f, 0.25f, 0.5f, 1f, 2f, 4f, 8f, 16f, 32f, 64f, 128f, 256f, 512f, 1024f, 2048f };
                        //if (!general_value_support.Contains(floatValue))
                        //{
                        //    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，仅允许出现如下值：0.03,0.06,0.12,0.125,0.25,0.5,1,2,4,8,16,32,64,128,256,512,1024,2048");
                        //    item.IsValid = false;
                        //    return oldValue;
                        //}

                        if (!value.IsDecimal() || value.GetFloat() > 6000 || value.GetFloat() <= 0)
                        {
                            //错误的值
                            this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", $"【{keyName}】字段值${oldValue}$错误，系统无法识别，大于0且小于6000");
                            item.IsValid = false;
                            return oldValue;
                        }

                        return oldValue;
                    }
            }

            #region 弃用

            ////处理2种极端情况
            //if (oldValue.ReplaceSymbol().Equals(">6000") || oldValue.ReplaceSymbol().Equals("<=0"))
            //{
            //    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，填写的值超出了范围");
            //    item.IsValid = false;
            //    return oldValue;
            //}

            //if (keyName.ToUpper().Equals("GEH_NM"))
            //{
            //    #region MIC法特殊字段-【GEH_NM】高浓度庆大霉素的填写规则：
            //    /*
            //    以下特殊规则将直接替代普通MIC法的校验规则，具体如下：
            //    1、仅允许出现如下值：250，256，500，512，513，1000，1024，1025，不在这些数字范围内的，均为非法值，影响通过率。
            //    2、可以有前导【>、<、=】符号
            //    3、如果填写了【SYN-S】、【S】，系统将自动替换为【<=500】
            //    4、如果填写了【SYN-R】、【R】，则将自动替换为【>=1000】
            //    */

            //    //处理2种极端情况
            //    if (oldValue.ReplaceSymbol().Equals(">1025") || oldValue.ReplaceSymbol().Equals("<250"))
            //    {
            //        this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，填写的值超出了范围");
            //        item.IsValid = false;
            //        return oldValue;
            //    }

            //    if (value.ToUpper().Equals("SYN-S") || value.ToUpper().Equals("S"))
            //    {
            //        this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, keyName, "", "系统自动将填写的值“" + oldValue + "”转换成“<=500”");
            //        return "<=500";
            //    }

            //    if (value.ToUpper().Equals("SYN-R") || value.ToUpper().Equals("R"))
            //    {
            //        this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, keyName, "", "系统自动将填写的值“" + oldValue + "”转换成“>=1000”");
            //        return ">=1000";
            //    }

            //    int value2 = value.GetInt();
            //    if (value2 <= 0)
            //    {
            //        this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，必须为纯数字或带前导【>、<、=】符号");
            //        item.IsValid = false;
            //        return oldValue;
            //    }

            //    if (value2 != 250 && value2 != 256 && value2 != 500 && value2 != 512 && value2 != 513
            //        && value2 != 1000 && value2 != 1024 && value2 != 1025)
            //    {
            //        this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，仅允许出现如下值：250，256，500，512，513，1000，1024，1025");
            //        item.IsValid = false;
            //        return oldValue;
            //    }

            //    #endregion
            //}
            //else if (keyName.ToUpper().Equals("STH_NM"))
            //{
            //    #region MIC法特殊字段-【STH_NM】，高浓度链霉素的填写规则

            //    /*
            //   以下特殊规则直接替代普通MIC法的校验规则，具体如下：
            //    1、仅允许出现如下值：500，512，1000，1024，2000，2048，2049，不在这些数字范围内的，均视为非法值
            //    2、可以有前导【>、<、=】符号
            //    3、如果填写了【SYN-S】、【S】，系统将自动替换为【<=1000】
            //    4、如果填写了【SYN-R】、【R】，则将自动替换为【>=2000】
            //    */

            //    //处理2种极端情况
            //    if (oldValue.ReplaceSymbol().Equals(">2049") || oldValue.ReplaceSymbol().Equals("<500"))
            //    {
            //        this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，填写的值超出了范围");
            //        item.IsValid = false;
            //        return oldValue;
            //    }

            //    if (value.ToUpper().Equals("SYN-S") || value.ToUpper().Equals("S"))
            //    {
            //        this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, keyName, "", "系统自动将填写的值“" + oldValue + "”转换成“<=1000”");
            //        return "<=1000";
            //    }

            //    if (value.ToUpper().Equals("SYN-R") || value.ToUpper().Equals("R"))
            //    {
            //        this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, keyName, "", "系统自动将填写的值“" + oldValue + "”转换成“>=2000”");
            //        return ">=2000";
            //    }

            //    int value2 = value.GetInt();
            //    if (value2 <= 0)
            //    {
            //        this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，必须为纯数字或带前导【>、<、=】符号");
            //        item.IsValid = false;
            //        return oldValue;
            //    }

            //    if (value2 != 500 && value2 != 512 && value2 != 1000 && value2 != 1024
            //        && value2 != 2000 && value2 != 2048 && value2 != 2049)
            //    {
            //        this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，仅允许出现如下值：500，512，1000，1024，2000，2048，2049");
            //        item.IsValid = false;
            //        return oldValue;
            //    }

            //    #endregion
            //}
            //else
            //{
            //// string standardValue = ",0.03,0.06,0.12,0.125,0.25,0.5,1,2,4,8,16,32,64,128,256,512,1024,2048,";
            //// !standardValue.Contains("," + value + ",")

            //if (!value.IsDecimal() || value.GetFloat() > 6000 || value.GetFloat() <= 0)
            //{
            //    //错误的值
            //    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值错误，系统无法识别，大于0且小于6000");
            //    item.IsValid = false;
            //    return oldValue;
            //}
            //}

            //return oldValue;

            #endregion
        }

        /// <summary>
        /// 验证抗生素，E-Test法
        /// 
        /// 目前E-Test方法和MIC的一致
        /// 
        /// </summary>
        /// <param name="value">去除符号等处理以后的值</param>
        /// <param name="oldValue">原始的excel值</param>
        /// <param name="keyName">当前验证的字段名称</param>
        /// <param name="list">记录验证信息列表</param>
        /// <param name="item">当前数据项</param>
        /// <returns></returns>
        private string CheckAntibioticETest(string value, string oldValue, string keyName, List<MedicalDataItemValidate> list, MedicalDataItem item)
        {
            if (!keyName.Contains("_NT") || string.IsNullOrWhiteSpace(value))
                return oldValue;

            /*
              MIC法代码验证，以及数据合格性验证。数据值必须大于0，且小于等于6000
              所有的抗生素名称中带有 “_NM”的则是MIC法，其中的值要么不填写，填写了则必须是上面指定值中的一个。对于填写错误的则提示错误，系统自动删除错误的值
            */

            //处理2种极端情况
            if (oldValue.ReplaceSymbol().Equals(">6000") || oldValue.ReplaceSymbol().Equals("<=0"))
            {
                this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", $"【{keyName}】字段值系统无法识别，填写的值${oldValue}$超出了范围(0, 6000]");
                item.IsValid = false;
                return oldValue;
            }

            if (keyName.ToUpper().Equals("GEH_NM"))
            {
                #region E-Test法特殊字段-【GEH_NM】高浓度庆大霉素的填写规则：
                /*
                以下特殊规则将直接替代普通MIC法的校验规则，具体如下：
                1、仅允许出现如下值：250，256，500，512，513，1000，1024，1025，不在这些数字范围内的，均为非法值，影响通过率。
                2、可以有前导【>、<、=】符号
                3、如果填写了【SYN-S】、【S】，系统将自动替换为【<=500】
                4、如果填写了【SYN-R】、【R】，则将自动替换为【>=1000】
                */

                //处理2种极端情况
                if (oldValue.ReplaceSymbol().Equals(">1025") || oldValue.ReplaceSymbol().Equals("<250"))
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", $"【{keyName}】字段值系统无法识别，填写的值${oldValue}$超出了范围(250, 1025)");
                    item.IsValid = false;
                    return oldValue;
                }

                if (value.ToUpper().Equals("SYN-S") || value.ToUpper().Equals("S"))
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, keyName, "", $"系统自动将填写的值“{value}”转换成“<=500”");
                    return "<=500";
                }

                if (value.ToUpper().Equals("SYN-R") || value.ToUpper().Equals("R"))
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, keyName, "", $"系统自动将填写的值“{value}”转换成“>=1000”");
                    return ">=1000";
                }

                int value2 = value.GetInt();
                if (value2 <= 0)
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，必须为纯数字或带前导【>、<、=】符号");
                    item.IsValid = false;
                    return oldValue;
                }

                if (value2 != 250 && value2 != 256 && value2 != 500 && value2 != 512 && value2 != 513
                    && value2 != 1000 && value2 != 1024 && value2 != 1025)
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，仅允许出现如下值：250，256，500，512，513，1000，1024，1025");
                    item.IsValid = false;
                    return oldValue;
                }

                #endregion
            }
            else if (keyName.ToUpper().Equals("STH_NM"))
            {
                #region E-Test法特殊字段-【STH_NM】，高浓度链霉素的填写规则

                /*
               以下特殊规则直接替代普通MIC法的校验规则，具体如下：
                1、仅允许出现如下值：500，512，1000，1024，2000，2048，2049，不在这些数字范围内的，均视为非法值
                2、可以有前导【>、<、=】符号
                3、如果填写了【SYN-S】、【S】，系统将自动替换为【<=1000】
                4、如果填写了【SYN-R】、【R】，则将自动替换为【>=2000】
                */

                //处理2种极端情况
                if (oldValue.ReplaceSymbol().Equals(">2049") || oldValue.ReplaceSymbol().Equals("<500"))
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", $"【{keyName}】字段值系统无法识别，填写的值${oldValue}$超出了范围(500, 2049)");
                    item.IsValid = false;
                    return oldValue;
                }

                if (value.ToUpper().Equals("SYN-S") || value.ToUpper().Equals("S"))
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, keyName, "", $"系统自动将填写的值“{value}”转换成“<=1000”");
                    return "<=1000";
                }

                if (value.ToUpper().Equals("SYN-R") || value.ToUpper().Equals("R"))
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Hint, keyName, "", $"系统自动将填写的值“{value}”转换成“>=2000”");
                    return ">=2000";
                }

                int value2 = value.GetInt();
                if (value2 <= 0)
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，必须为纯数字或带前导【>、<、=】符号");
                    item.IsValid = false;
                    return oldValue;
                }

                if (value2 != 500 && value2 != 512 && value2 != 1000 && value2 != 1024
                    && value2 != 2000 && value2 != 2048 && value2 != 2049)
                {
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值系统无法识别，仅允许出现如下值：500，512，1000，1024，2000，2048，2049");
                    item.IsValid = false;
                    return oldValue;
                }

                #endregion
            }
            else
            {
                // string standardValue = ",0.03,0.06,0.12,0.125,0.25,0.5,1,2,4,8,16,32,64,128,256,512,1024,2048,";
                // !standardValue.Contains("," + value + ",")

                if (!value.IsDecimal() || value.GetFloat() > 6000 || value.GetFloat() <= 0)
                {
                    //错误的值
                    this.AddValidateItem(list, item.Id, item.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, keyName, "", "【" + keyName + "】字段值错误，系统无法识别，大于0且小于6000");
                    item.IsValid = false;
                    return oldValue;
                }
            }

            return oldValue;
        }



        #endregion

        /// <summary>
        /// 根据细菌编码和字段的名称获取一个唯一的规则
        /// </summary>
        /// <param name="organismCode">细菌的编码</param>
        /// <param name="cod">字段的名称</param>
        ///  <param name="list">验证规则的列表，如果没有则查询数据库</param>
        /// <returns></returns>
        public MedicalAntibioticRule QueryEntity(string organismCode, string code, List<MedicalAntibioticRule> list = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(organismCode) || string.IsNullOrWhiteSpace(code)) return null;

                if (list == null || !list.Any())
                    list = this.Query().ToList();

                return list.Where(m => m.OrganismCode.ToLower().Equals(organismCode.ToLower()) && m.Code.ToLower().Equals(code.ToLower())).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
