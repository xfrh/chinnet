using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core;
using ManageSystem.Services.Security;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Services.SystemSet;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Data;
using ManageSystem.Core.Infrastructure;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using Dapper;
using ManageSystem.Services.Log;
using System.IO;
using OfficeOpenXml;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Medicine;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Log;
using System.Data.SqlClient;

namespace ManageSystem.Services.Members
{
    /// <summary>
    /// 操作类 ，数据库表名：Userinfo 
    /// </summary>
    public partial class MemberService : BaseService<Member>, IMemberService
    {
        private readonly IEncryptionService encryptionService;
        private ISystemLogService SystemLogService;
        private IHospitalService _hospitalService;

        public MemberService(IRepository<Member> repository,
             IEncryptionService _encryptionService,
               ISystemLogService systemLogService,
               IHospitalService hospitalService
            ) : base(repository)
        {
            this.encryptionService = _encryptionService;
            this.SystemLogService = systemLogService;
            this._hospitalService = hospitalService;
        }

        /// <summary>
        /// 检查OpendId是否已经存在，保证唯一
        /// </summary>
        /// <param name="openId"></param>
        /// <returns>true 表示可用，false 你可用</returns>
        public bool CheckOpenId(string openId)
        {
            if (string.IsNullOrWhiteSpace(openId)) throw new Exception("opendId 不能为空！");

            return this.Query(m => m.OpenId.Equals(openId) && m.Mark > 0).Count() <= 0;
        }

        public override void Delete(long id)
        {
            Member entity = this.QueryEntity(id);

            entity.Mark = 0;
            entity.DeleteTime = DateTime.Now;
            entity.LoginId = entity.LoginId + "_DELETE";
            if (!string.IsNullOrWhiteSpace(entity.OpenId))
            {
                entity.OpenId += "_DELETE";
            }

            if (!string.IsNullOrWhiteSpace(entity.Phone))
            {
                entity.Phone += "_DELETE";
            }

            this._repository.Update(entity);

        }

        public override void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return;

            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    ids = ids.TrimEnd(',');

                    string[] cities = ids.Split(',');

                    foreach (var item in cities)
                    {
                        long temp = long.Parse(item);
                        this.Delete(temp);
                    }

                    tran.Commit();
                }

                con.Close();
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert(ex, Core.Domain.Log.SystemLogLevel.Error);
            }

        }

        public override void Insert(Member entity)
        {
            //entity.Password = this.encryptionService.EncryptText(entity.Password);

            base.Insert(entity);
        }

        /// <summary>
        /// 根据登录帐号获取一个用户
        /// </summary>
        /// <param name="loginId">登录帐号</param>
        /// <returns></returns>
        public Member QueryModelByLoginId(string loginId)
        {
            return this.QueryEntity(m => m.LoginId.Equals(loginId));
        }

        public IPagedList<Member> QueryPage(string name, string nickName,string LoginId, string phone, int state, int type, string hospital, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (!string.IsNullOrWhiteSpace(nickName))
                query = query.Where(m => m.NickName.Contains(nickName.Trim()));

            if (!string.IsNullOrWhiteSpace(LoginId))
                query = query.Where(m => m.LoginId.Contains(LoginId.Trim()));

            if (!string.IsNullOrWhiteSpace(phone))
                query = query.Where(m => m.Phone.Contains(phone.Trim()));

            if (state > 0)
                query = query.Where(m => m.Status == state);

            if (type > -1)
                query = query.Where(m => m.Type == type);

            if (!string.IsNullOrWhiteSpace(hospital))
            {
                using (var conn = DapperHelper.GetConnection())
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    List<long> hospitalIds = conn.Query<long>("SELECT Id FROM dbo.Hospital WHERE [Name] LIKE @HospitalName AND [Mark] > 0;", new { HospitalName = $"%{hospital}%" }).ToList();
                    query = query.Where(m => hospitalIds.Contains(m.HospitalId));
                }
            }
            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<Member>(query, pageIndex, pageSize);

        }
        
        public IPagedList<Member> QueryPage(int ProjectType, string name, string nickName, string LoginId, string phone, int state, int type, string hospital, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            //参照枚举值
            //0/Chient会员，1/SUGAG多中心，2/CRAB多中心，3/ERA多中心，4/CRE多中心
            if (ProjectType <= 0){ ProjectType = 0; }
            query = query.Where(m => m.ProjectType == ProjectType);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (!string.IsNullOrWhiteSpace(nickName))
                query = query.Where(m => m.NickName.Contains(nickName.Trim()));

            if (!string.IsNullOrWhiteSpace(LoginId))
                query = query.Where(m => m.LoginId.Contains(LoginId.Trim()));

            if (!string.IsNullOrWhiteSpace(phone))
                query = query.Where(m => m.Phone.Contains(phone.Trim()));

            if (state > 0)
                query = query.Where(m => m.Status == state);

            if (type > -1)
                query = query.Where(m => m.Type == type);

            if (!string.IsNullOrWhiteSpace(hospital))
            {
                using (var conn = DapperHelper.GetConnection())
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    List<long> hospitalIds = conn.Query<long>("SELECT Id FROM dbo.Hospital WHERE [Name] LIKE @HospitalName AND [Mark] > 0;", new { HospitalName = $"%{hospital}%" }).ToList();
                    query = query.Where(m => hospitalIds.Contains(m.HospitalId));
                }
            }
            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<Member>(query, pageIndex, pageSize);

        }

        /// <summary>
        /// 通过Excel文件导入会员
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public string ImportExcel(string filePath, ref int successCount)
        {
            try
            {
                FileInfo existingFile = new FileInfo(filePath);

                ExcelPackage package = new ExcelPackage(existingFile);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];//选定 指定页
                int maxRowNum = worksheet.Dimension.End.Row;//最小行

                StringBuilder sb = new StringBuilder();
                int columnIndex = 1;
                successCount = 0; //导入成功的总行数

                for (int n = 2; n <= maxRowNum; n++)
                {
                    try
                    {
                        string name = GetExcelCellValue(worksheet, n, columnIndex++);  //真实姓名
                        string typeName = GetExcelCellValue(worksheet, n, columnIndex++);  //帐号类型
                        string phone = GetExcelCellValue(worksheet, n, columnIndex++);  //手机号码

                        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(typeName) || string.IsNullOrWhiteSpace(phone)) continue;

                        if (this.Count(m => m.Phone.Equals(phone)) > 0) throw new Exception("第" + n + "行，真实姓名：" + name + "，手机号码：" + phone + " 已经存在");
                        if (!this.CheckPhone(phone)) throw new Exception("第" + n + "行，真实姓名：" + name + "，手机号码：" + phone + " 格式不正确");

                        Member model = new Member();
                        model.Name = name;
                        model.Phone = phone;
                        model.Password = "888888";
                        model.Type = (int)this.GetMemberType(typeName);
                        model.Email = GetExcelCellValue(worksheet, n, columnIndex++);  //邮箱地址
                        model.Describe = GetExcelCellValue(worksheet, n, columnIndex++);  //描述信息
                        model.Birthday = DateHelper.DefaultValue();
                        model.HeadImage = "";
                        model.LoginId = "";
                        model.NickName = "";
                        model.Sex = (int)MemberSex.Unknown;
                        model.Status = (int)MemberStatus.WaitCheck;
                        model.OpenId = "";

                        this.Insert(model);
                        successCount++;

                    }
                    catch (Exception ex)
                    {
                        sb.Append(ex.Message + " <br />");
                    }

                    columnIndex = 1;
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "导入会员失败，" + ex.Message;
            }
        }

        /// <summary>
        /// 获取Excel中的值
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        private string GetExcelCellValue(ExcelWorksheet worksheet, int rowIndex, int cellIndex)
        {
            try
            {
                return worksheet.Cells[rowIndex, cellIndex++].Value.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 获取用户的类型
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private MemberType GetMemberType(string typeName)
        {
            if (typeName.Equals("医生")) return MemberType.Doctor;

            if (typeName.Equals("主任")) return MemberType.Director;

            return MemberType.Authentication;
        }

        /// <summary>
        /// 检查导入的手机号码是否符合要求
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private bool CheckPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone) || !phone.IsLong() || phone.Length != 11) return false;

            return true;
        }

        /// <summary>
        /// 会员中心 分页查询  邀请会员，根据指定的邀请码查询被邀请的会员  
        /// </summary>
        /// <param name="inviteCode">会员邀请码</param>
        /// <returns></returns>
        public IQueryable<Member> QueryByInviteCode(string inviteCode)
        {
            return this._repository.Table.Where(m => m.Mark > 0 && m.InputInviteCode.Equals(inviteCode)).OrderByDescending(m => m.InsertTime);
        }

        /// <summary>
        /// 根据手机号码获取一个用户
        /// </summary>
        /// <param name="loginId">手机号码</param>
        /// <returns></returns>
        public Member QueryModelByPhone(string phone)
        {
            return this.QueryEntity(m => m.Phone.Equals(phone));
        }

        /// <summary>
        /// 根据邮箱号获取一个用户
        /// </summary>
        /// <param name="email">手机号码</param>
        /// <returns></returns>
        public Member QueryModelByEmail(string email)
        {
            return this.QueryEntity(m => m.Email.Equals(email));
        }

        /// <summary>
        /// 用户注册，底层函数
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="password">密码</param>
        /// <param name="password2">确认密码</param>
        /// <param name="provinceId">所属省份Id</param>
        /// <param name="hospital">医院名称</param>
        /// <param name="source">操作来源，1表示web 2表示移动端</param>
        /// <param name="inputCode">输入的邀请码</param>
        /// <returns>成功返回新添加的用户</returns>
        public Member Regist(string phone, string password, string password2, long provinceId, string hospital, int source, string inputCode = "")
        {
            #region 验证数据

            if (string.IsNullOrWhiteSpace(phone) || !RegexHelper.IsPhone(phone))
                throw new ManageSystemException("手机号码格式不正确");

            if (string.IsNullOrWhiteSpace(password) || password.Trim().Length < 6)
                throw new ManageSystemException("密码长度必须大于等于6位");

            //if (string.IsNullOrWhiteSpace(password2) || password2.Trim().Length < 6)
            //    throw new ManageSystemException("确认密码长度必须大于等于6位");

            //if (!password.Equals(password2))
            //    throw new ManageSystemException("2次密码不一致");

            if (provinceId <= 0)
                throw new ManageSystemException("请选择所属省份");

            if (this.Count(m => m.Mark > 0 && m.Phone.Equals(phone)) > 0)
                throw new ManageSystemException("手机号码已经存在，请更换");

            if (string.IsNullOrWhiteSpace(hospital))
                throw new ManageSystemException("请输入所属医院名称");

            IAreaService areaServic = EngineContext.Current.Resolve<IAreaService>();
            var areaEntity = areaServic.QueryEntity(provinceId);
            if (areaEntity == null || areaEntity.Id < 0)
                throw new ManageSystemException("选择的所属省份不存在");

            #endregion

            //查询填写的医院和相同的省份是否存在，不存在则创建新的医院，存在则直接复制医院的Id
            IHospitalService hospitalService = EngineContext.Current.Resolve<IHospitalService>();
            var hospitalEntity = hospitalService.QueryEntity(m => m.ProvinceId == provinceId && m.Name.Equals(hospital.Trim()));
            if (hospitalEntity == null || hospitalEntity.Id <= 0)
            {
                //填写的医院不存在，创建新的医院
                hospitalEntity = new Hospital()
                {
                    Id = CommonHelper.GuidToLongID,
                    Address = "",
                    Code = "",
                    ContactsTel = "",
                    ContactsUser = "",
                    Content = "",
                    MemberId = 0,
                    Name = hospital.Trim(),
                    ProvinceId = areaEntity.Id,
                    ProvinceName = areaEntity.Name,
                    Sort = 10000,
                    State = true,
                    Describe = "",
                    IsTeam = false
                };

                hospitalService.Insert(hospitalEntity);
            }

            //保存用户数据
            Member entity = new Member()
            {
                Id = CommonHelper.GuidToLongID,
                AreaId = areaEntity.Id,
                Birthday = DateHelper.DefaultValue(),
                Describe = "",
                DoctorTitleId = 0,
                Phone = phone,
                Email = "",
                HeadImage = "",
                HospitalDepartmentId = 0,
                HospitalId = hospitalEntity.Id,
                InputInviteCode = inputCode,
                IntegralAmount = 0,
                InviteCode = MemberExtensions.GetInviteCode(),
                LoginId = phone,
                Name = "",
                NickName = "匿名",
                OpenId = "",
                Password = password,
                Sex = (int)MemberSex.Unknown,
                Status = (int)MemberStatus.Normal,
                Type = (int)MemberType.Doctor,
                OtherHospital = "",
                MedicineEmail = "",
                ProjectItem = "",
                Address = "",
                CityId = 0,
                DistrictsId = 0,
                Area = "",
                ProvinceId = areaEntity.Id,
                IsTest = false
            };
            this.Insert(entity);

            //添加日志
            IActionLogService actionLogService = EngineContext.Current.Resolve<IActionLogService>();
            actionLogService.Insert(ActionType.Create, source == 1 ? ActionSource.Web : ActionSource.Mobile, entity.Id, entity.Name + "（" + entity.LoginId + "）", "用户注册成功", entity.SerializeObject());

            return entity;
        }

        /// <summary>
        /// 用户注册，底层函数
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="password">密码</param>
        /// <param name="password2">确认密码</param>
        /// <param name="source">操作来源，1表示web 2表示移动端</param>
        /// <param name="inputCode">输入的邀请码</param>
        /// <returns>成功返回新添加的用户</returns>
        public Member Regist(string phone, string password, string password2, int source, string inputCode = "")
        {
            #region 验证数据

            if (string.IsNullOrWhiteSpace(phone) || !RegexHelper.IsPhone(phone))
                throw new ManageSystemException("手机号码格式不正确");

            if (string.IsNullOrWhiteSpace(password) || password.Trim().Length < 6)
                throw new ManageSystemException("密码长度必须大于等于6位");

            //if (string.IsNullOrWhiteSpace(password2) || password2.Trim().Length < 6)
            //    throw new ManageSystemException("确认密码长度必须大于等于6位");

            //if (!password.Equals(password2))
            //    throw new ManageSystemException("2次密码不一致");

            #endregion

            //保存用户数据
            Member entity = new Member()
            {
                Id = CommonHelper.GuidToLongID,
                AreaId = 0,
                Birthday = DateHelper.DefaultValue(),
                Describe = "",
                DoctorTitleId = 0,
                Phone = phone,
                Email = "",
                HeadImage = "",
                HospitalDepartmentId = 0,
                HospitalId = 0,
                InputInviteCode = inputCode,
                IntegralAmount = 0,
                InviteCode = MemberExtensions.GetInviteCode(),
                LoginId = phone,
                Name = "",
                NickName = "匿名",
                OpenId = "",
                Password = password,
                Sex = (int)MemberSex.Unknown,
                Status = (int)MemberStatus.Normal,
                Type = (int)MemberType.Doctor,
                OtherHospital = "",
                MedicineEmail = "",
                ProjectItem = "",
                IsTest = false,
                Address = "",
                CityId = 0,
                DistrictsId = 0,
                Area = "",
                ProvinceId = 0
            };
            this.Insert(entity);

            //添加日志
            IActionLogService actionLogService = EngineContext.Current.Resolve<IActionLogService>();
            actionLogService.Insert(ActionType.Create, source == 1 ? ActionSource.Web : ActionSource.Mobile, entity.Id, entity.Name + "（" + entity.LoginId + "）", "用户注册成功", entity.SerializeObject());

            return entity;
        }

        /// <summary>
        /// 用户注册，底层函数
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="phone">手机号码</param>
        /// <param name="password">密码</param>
        /// <param name="password2">确认密码</param>
        /// <param name="source">操作来源，1表示web 2表示移动端</param>
        /// <returns></returns>
        public Member Regist(string loginId, string name, string phone, string password, string password2, int source)
        {
            #region 验证数据
            if (string.IsNullOrWhiteSpace(loginId))
                throw new ManageSystemException("登录用户名不能为空");

            loginId = loginId.Trim();
            if (base.Count(r => r.LoginId == loginId) > 0)
            {
                throw new ManageSystemException("登录用户名已经存在");
            }

            //if (string.IsNullOrWhiteSpace(name))
            //    throw new ManageSystemException("用户姓名不能为空");

            //2022/2/24 根据胡主任要求修改 注册改为 手机号+用户名 填写一个即可
            //if (string.IsNullOrWhiteSpace(phone) || !RegexHelper.IsPhone(phone))
            //    throw new ManageSystemException("手机号码格式不正确");

            if (string.IsNullOrWhiteSpace(password) || password.Trim().Length < 6)
                throw new ManageSystemException("密码长度必须大于等于6位");

            //if (string.IsNullOrWhiteSpace(password2) || password2.Trim().Length < 6)
            //    throw new ManageSystemException("确认密码长度必须大于等于6位");

            //if (!password.Equals(password2))
            //    throw new ManageSystemException("2次密码不一致");

            if (Count(r => r.LoginId == phone && r.Mark != 0) > 0)
                throw new ManageSystemException("此手机号已经注册过了");

            #endregion

            //保存用户数据
            Member entity = new Member()
            {
                Id = CommonHelper.GuidToLongID,
                AreaId = 0,
                Birthday = DateHelper.DefaultValue(),
                Describe = "",
                DoctorTitleId = 0,
                Phone = phone,
                Email = "",
                HeadImage = "",
                HospitalDepartmentId = 0,
                HospitalId = 0,
                InputInviteCode = null,
                IntegralAmount = 0,
                InviteCode = MemberExtensions.GetInviteCode(),
                LoginId = loginId,
                Name = name,
                NickName = name,
                OpenId = "",
                Password = password,
                Sex = (int)MemberSex.Unknown,
                Status = (int)MemberStatus.Normal,
                Type = (int)MemberType.Doctor,
                OtherHospital = "",
                MedicineEmail = "",
                ProjectItem = "",
                IsTest = false,
                IsSettingPassword = true,
                Address = "",
                CityId = 0,
                DistrictsId = 0,
                Area = "",
                ProvinceId = 0,
                Median = ""
            };
            this.Insert(entity);

            //添加日志
            IActionLogService actionLogService = EngineContext.Current.Resolve<IActionLogService>();
            actionLogService.Insert(ActionType.Create, source == 1 ? ActionSource.Web : ActionSource.Mobile, entity.Id, entity.Name + "（" + entity.LoginId + "）", "用户注册成功", entity.SerializeObject());

            return entity;
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool UpdatePassword(long id, string password)
        {
            var entity = QueryEntity(id);
            entity.Password = password;
            entity.IsSettingPassword = true;
            Update(entity);
            //添加日志
            IActionLogService actionLogService = EngineContext.Current.Resolve<IActionLogService>();
            actionLogService.Insert(ActionType.Edit, ActionSource.Web, entity.Id, entity.Name + "（" + entity.LoginId + "）", "用户更新登录密码", entity.SerializeObject());
            return true;
        }


        #region 批量刷用户所属医院的数据和上传数据是否显示的数据，请勿随意删除和调用

        public void UpdateMe()
        {
            //查询所有用户医院是“其他医院的”
            var memberList = this.Query(m => m.HospitalId == 5434766963748420189);
            if (memberList == null || !memberList.Any())
                throw new Exception("没有查询到数据");

            //查询医院该用户所属的医院是否存在，不存在，则创建，存在则更新用户的医院数据
            IAreaService areaServic = EngineContext.Current.Resolve<IAreaService>();
            IHospitalService hospitalService = EngineContext.Current.Resolve<IHospitalService>();

            foreach (var item in memberList)
            {
                if (string.IsNullOrWhiteSpace(item.OtherHospital))
                    continue;

                //1、更新用户的用户的所属医院数据
                var hospitalEntity = hospitalService.QueryEntity(m => m.ProvinceId == item.AreaId && m.Name.Equals(item.OtherHospital.Trim()));
                if (hospitalEntity == null || hospitalEntity.Id <= 0)
                {
                    var areaEntity = areaServic.QueryEntity(item.AreaId);
                    if (areaEntity == null || areaEntity.Id < 0)
                        throw new ManageSystemException("选择的所属省份不存在");

                    //填写的医院不存在，创建新的医院
                    hospitalEntity = new Hospital()
                    {
                        Id = CommonHelper.GuidToLongID,
                        Address = "",
                        Code = "",
                        ContactsTel = "",
                        ContactsUser = "",
                        Content = "",
                        MemberId = 0,
                        Name = item.OtherHospital.Trim(),
                        ProvinceId = areaEntity.Id,
                        ProvinceName = areaEntity.Name,
                        Sort = 10000,
                        State = true,
                        Describe = "",
                        IsTeam = false
                    };
                    hospitalService.Insert(hospitalEntity);
                }

                item.HospitalId = hospitalEntity.Id;
                item.OtherHospital = "";
                this.Update(item);

                //2、修改用户上传的医学数据所属医院信息，直接使用SQL搞定
            }

        }

        /// <summary>
        /// 修改所有上传数据的显示状态，批量刷数据使用
        /// 规则：一个医院同一年，同一季度，同一项目，最后上传的显示出来，之前的则不显示出来
        /// </summary>
        public void UpdateMe2()
        {
            //查询出所有的医院

            //查询指定医院的所有上传数据，
            /*
              规则：一个医院同一年，同一季度，同一项目，最后上传的显示出来，之前的则不显示出来
             */

            IMedicalDataService md = EngineContext.Current.Resolve<IMedicalDataService>();
            IHospitalService hospitalService = EngineContext.Current.Resolve<IHospitalService>();
            var hospitalList = hospitalService.Query(m => m.Mark > 0).ToList();

            if (hospitalList == null || !hospitalList.Any())
                throw new Exception("未查询到数据");

            var yearList = new List<int>() { 2018, 2019, 2020 }; //年份
            var quartyList = new List<int>() { 5, 6, 7 };//季度
            var typeList = new List<int>() { 1, 2, 3, 4 }; //项目

            foreach (var item in hospitalList)
            {
                var dataList = md.Query(m => m.HospitalId == item.Id).ToList();
                if (dataList == null || !dataList.Any()) continue;

                foreach (var yearNode in yearList)
                {
                    foreach (var quartyNode in quartyList)
                    {
                        foreach (var typeNode in typeList)
                        {
                            this.UpdateMe3(md, dataList, yearNode, quartyNode, typeNode);
                        }
                    }
                }
            }
        }

        private void UpdateMe3(IMedicalDataService md, List<MedicalData> list, int year, int quart, int type)
        {
            var dataList = list.Where(m => m.Year == year && m.Quarter == quart && m.ProjectType == type).ToList();
            if (dataList == null || !dataList.Any()) return;

            // 规则：一个医院同一年，同一季度，同一项目，最后上传的显示出来，之前的则不显示出来
            dataList = dataList.OrderByDescending(m => m.InsertTime).ToList(); //最后上传的显示出来，之前的不显示出来
            int index = 0;
            foreach (var item in dataList)
            {
                item.Display = false; //默认都不显示
                if (index == 0)
                {
                    item.Display = true; //第一个显示出来
                    index = 1;
                }
                md.Update(item);
            }
        }

        public IPagedList<Member> QueryPage(string name, string phone, int? state, List<long> hospital, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => (m.Name.Contains(name) || m.NickName.Contains(name) || m.Phone.Contains(name)));

            //if (!string.IsNullOrWhiteSpace(phone))
            //    query = query.Where(m => m.Phone.Contains(phone));

            if (state != null)
                query = query.Where(m => m.Status == state.Value);

            if (hospital != null && hospital.Count > 0)
            {
                query = query.Where(m => hospital.Contains(m.HospitalId));
            }
            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<Member>(query, pageIndex, pageSize);
        }
        #endregion
    }
}
