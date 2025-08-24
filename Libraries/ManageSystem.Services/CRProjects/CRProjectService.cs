using Dapper;
using ICSharpCode.SharpZipLib.Zip;
using ManageSystem.Core;
using ManageSystem.Core.Caching;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Domain.CRProjects;
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

namespace ManageSystem.Services.CRProjects
{
    /// <summary>
    /// 操作类 ，数据库表名：MedicalData 
    /// </summary>
    public partial class CRProjectService : BaseService<CRProject>, ICRProjectService
    {
        private readonly ICRProjectItemService ProjectItemService;
        private readonly ICacheManager CacheManager;
        private readonly ICRProjectLogService ProjectLogService;
        private readonly ISystemLogService SystemLogService;
        private readonly IActionLogService ActionLogService;
        private readonly IMemberService MemberService;
        private readonly IHospitalService HospitalService;

        public CRProjectService(IRepository<CRProject> repository,
                ICRProjectItemService projectItemService,
                  ICacheManager cacheManager,
                  ICRProjectLogService projectLogService,
                   ISystemLogService systemLogService,
                  IActionLogService actionLogService,
                  IMemberService memberService,
                  IHospitalService hospitalService
            ) : base(repository)
        {

            this.ProjectItemService = projectItemService;
            this.CacheManager = cacheManager;
            this.ProjectLogService = projectLogService;
            this.SystemLogService = systemLogService;
            this.ActionLogService = actionLogService;
            this.MemberService = memberService;
            this.HospitalService = hospitalService;

        }

        public IPagedList<CRProject> QueryPage(string hospitalName, long hospitalId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(hospitalName))
                query = query.Where(m => m.HospitalName.Contains(hospitalName));

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

            var list = new PagedList<CRProject>(query, pageIndex, pageSize);

            return list;
        }

        /// <summary>
        /// 单个获取原始文件的相对地址
        /// </summary>
        /// <param name="id">上传的数据Id</param>
        /// <returns></returns>
        public string DownloadOriginalData(long id)
        {
            var entity = this.QueryEntity(m => m.Id == id && m.Mark > 0);
            if (string.IsNullOrWhiteSpace(entity.UploadFilePath))
                throw new Exception("原始数据文件丢失");

            return entity.UploadFilePath;
        }

        /// <summary>
        /// 单个获取容错文件的相对地址
        /// </summary>
        /// <param name="id">上传的数据Id</param>
        /// <returns></returns>
        public string DownloadNewData(long id)
        {
            var entity = this.QueryEntity(m => m.Id == id && m.Mark > 0);
            if (entity == null || entity.Id <= 0)
                throw new Exception("数据不存在或者错误");


            string fileExtension = Path.GetExtension(entity.UploadFilePath).ToUpperInvariant();

            return "";
        }

        /// <summary>
        /// 后台，打包下载文件
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="downloadEnum"></param>
        /// <returns></returns>
        public string Download(IEnumerable<long> ids, CRProjectDownloadEnum downloadEnum)
        {
            try
            {
                var data = Query(r => ids.Contains(r.Id) && r.Mark > 0);
                IEnumerable<string> filesPath = null;
                switch (downloadEnum)
                {
                    case CRProjectDownloadEnum.Original:
                        filesPath = data.Where(r => !string.IsNullOrWhiteSpace(r.UploadFilePath)).Select(r => r.UploadFilePath);
                        break;
                    case CRProjectDownloadEnum.FaultTolerant:
                        //     filesPath = data.Where(r => !string.IsNullOrWhiteSpace(r.DisposeFilePath)).Select(r => r.DisposeFilePath);
                        break;
                    default:
                        break;
                }

                if (filesPath != null && filesPath.Count() > 0)
                {
                    string outPath = $"/Content/Download/MedicalData/{Guid.NewGuid().ToString("N")}.zip";

                    if (CompressFile(filesPath, System.Web.HttpContext.Current.Server.MapPath(outPath)))
                    {
                        return outPath;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 压缩文件

        /// <summary>
        /// 压缩多个文件/文件夹
        /// </summary>
        /// <param name="sourceList">源文件/文件夹路径列表</param>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="comment">注释信息</param>
        /// <param name="password">压缩密码</param>
        /// <param name="compressionLevel">压缩等级，范围从0到9，可选，默认为6</param>
        /// <returns></returns>
        private bool CompressFile(IEnumerable<string> sourceList, string zipFilePath,
             string comment = null, string password = null, int compressionLevel = 6)
        {
            bool result = false;

            try
            {
                //检测目标文件所属的文件夹是否存在，如果不存在则建立
                string zipFileDirectory = Path.GetDirectoryName(zipFilePath);
                if (!Directory.Exists(zipFileDirectory))
                {
                    Directory.CreateDirectory(zipFileDirectory);
                }

                Dictionary<string, string> dictionaryList = PrepareFileSystementities(sourceList);

                using (ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipFilePath)))
                {
                    zipStream.Password = password;//设置密码
                    zipStream.SetComment(comment);//添加注释
                    zipStream.SetLevel(compressionLevel);//设置压缩等级

                    foreach (string key in dictionaryList.Keys)//从字典取文件添加到压缩文件
                    {
                        if (File.Exists(key))//判断是文件还是文件夹
                        {
                            FileInfo fileItem = new FileInfo(key);

                            using (FileStream readStream = fileItem.Open(FileMode.Open,
                                FileAccess.Read, FileShare.Read))
                            {
                                ZipEntry zipEntry = new ZipEntry(dictionaryList[key]);
                                zipEntry.DateTime = fileItem.LastWriteTime;
                                zipEntry.Size = readStream.Length;
                                zipStream.PutNextEntry(zipEntry);
                                int readLength = 0;
                                byte[] buffer = new byte[8192];

                                do
                                {
                                    readLength = readStream.Read(buffer, 0, 8192);
                                    zipStream.Write(buffer, 0, readLength);
                                } while (readLength == 8192);

                                readStream.Close();
                            }
                        }
                        else//对文件夹的处理
                        {
                            ZipEntry zipEntry = new ZipEntry(dictionaryList[key] + "/");
                            zipStream.PutNextEntry(zipEntry);
                        }
                    }

                    zipStream.Flush();
                    zipStream.Finish();
                    zipStream.Close();
                }

                result = true;
            }
            catch (System.Exception ex)
            {
                throw new Exception("压缩文件失败", ex);
            }

            return result;
        }

        /// <summary>
        /// 为压缩准备文件系统对象
        /// </summary>
        /// <param name="sourceFileEntityPathList"></param>
        /// <returns></returns>
        private Dictionary<string, string> PrepareFileSystementities(IEnumerable<string> sourceFileEntityPathList)
        {
            Dictionary<string, string> fileEntityDictionary = new Dictionary<string, string>();//文件字典
            string parentDirectoryPath = "";
            foreach (string fileEntityPath in sourceFileEntityPathList)
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(fileEntityPath);
                //保证传入的文件夹也被压缩进文件
                if (path.EndsWith(@"\"))
                {
                    path = path.Remove(path.LastIndexOf(@"\"));
                }

                parentDirectoryPath = Path.GetDirectoryName(path) + @"\";

                if (parentDirectoryPath.EndsWith(@":\\"))//防止根目录下把盘符压入的错误
                {
                    parentDirectoryPath = parentDirectoryPath.Replace(@"\\", @"\");
                }

                //获取目录中所有的文件系统对象
                Dictionary<string, string> subDictionary = GetAllFileSystemEntities(path, parentDirectoryPath);

                //将文件系统对象添加到总的文件字典中
                foreach (string key in subDictionary.Keys)
                {
                    if (!fileEntityDictionary.ContainsKey(key))//检测重复项
                    {
                        fileEntityDictionary.Add(key, subDictionary[key]);
                    }
                }
            }
            return fileEntityDictionary;
        }

        /// <summary>
        /// 获取所有文件系统对象
        /// </summary>
        /// <param name="source">源路径</param>
        /// <param name="topDirectory">顶级文件夹</param>
        /// <returns>字典中Key为完整路径，Value为文件(夹)名称</returns>
        private Dictionary<string, string> GetAllFileSystemEntities(string source, string topDirectory)
        {
            Dictionary<string, string> entitiesDictionary = new Dictionary<string, string>();
            entitiesDictionary.Add(source, source.Replace(topDirectory, ""));

            if (Directory.Exists(source))
            {
                //一次性获取下级所有目录，避免递归
                string[] directories = Directory.GetDirectories(source, "*.*", SearchOption.AllDirectories);
                foreach (string directory in directories)
                {
                    entitiesDictionary.Add(directory, directory.Replace(topDirectory, ""));
                }

                string[] files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    entitiesDictionary.Add(file, file.Replace(topDirectory, ""));
                }
            }

            return entitiesDictionary;
        }

        #endregion

        /// <summary>
        /// 会员中心，CR复敏信息管理的分页数据
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public IQueryable<CRProject> Query(long memberId)
        {
            var data = base._repository.Table.Where(m => m.Mark > 0 && m.MemberId == memberId);

            return data.OrderByDescending(m => m.InsertTime);
        }

        /// <summary>
        ///  上传数据 提交
        /// </summary>
        /// <param name="id">CR 的Id</param>
        /// <param name="filePath">上传成功的文件，相对路径</param>
        /// <param name="oldFileName">原始文件名称</param>
        /// <param name="member">当前登录用户</param>
        /// <returns>SUCCESS 表示成功，其他则是错误信息</returns>
        public string Upload(long menberid, string filePath, string oldFileName, Member member)
        {
            try
            {
                #region 数据验证

                //文件的完整地址
                string fullFilePath = System.Web.HttpContext.Current.Server.MapPath(filePath);

                var entity = this.Query().Where(x => x.MemberId == menberid).FirstOrDefault();

                if (entity == null || entity.Id <= 0 || entity.MemberId != member.Id)
                    throw new Exception("数据不存在");

                System.IO.FileInfo fileInfo = new FileInfo(fullFilePath);
                if (!fileInfo.Exists)
                    throw new Exception("未能读取到数据文件，请重新上传文件");

                DataTable table = null;
                switch (fileInfo.Extension.ToUpperInvariant())
                {
                    case ".XLS":
                        table = Core.Utility.Excel.ImportDataTable.ExcelToDataTable(fileInfo.FullName, true, 3);
                        break;
                    case ".XLSX":
                        table = EPPlusHelper.WorksheetToTable(fileInfo.FullName, 4);
                        break;
                    default:
                        throw new Exception("文件格式不支持");
                }

                if (table == null || table.Rows.Count <= 0)
                    throw new Exception("从上传的文件中未能读取到数据");

                string systemGerm = ConfigHelper.GetConfigString("cr.upload.excel.germ");
                if (string.IsNullOrWhiteSpace(systemGerm))
                    throw new Exception("请配置系统支持的细菌名称");
                systemGerm = "," + systemGerm + ",";//前后加逗号方便下面判断用的

                #endregion

                #region  业务处理

                List<CRProjectItem> list = new List<CRProjectItem>();
                for (int n = 0; n < table.Rows.Count; n++)
                {
                    //编号和细菌名称不能为空
                    var item = table.Rows[n];
                    string no = item["编号"].ToString();
                    string germ = item["细菌名称"].ToString();
                    string imipenemNumber = (item["亚胺培南MIC值(ug/ml)"] ?? "").ToString().Trim();
                    string imineNumber = (item["亚胺培南抑菌圈(mm)"] ?? "").ToString().Trim();
                    string tegacyclineNumber = (item["替加环素抑菌圈直径(mm)"] ?? "").ToString().Trim();
                    string bacteriostasisNumber = (item["替加环素复敏抑菌圈直径(mm)"] ?? "").ToString().Trim();
                    string recheckNumber = (item["肉汤法MIC值(ug/ml)"] ?? "").ToString().Trim();

                    if (string.IsNullOrWhiteSpace(no) || string.IsNullOrWhiteSpace(germ)
                        /*|| string.IsNullOrWhiteSpace(imipenemNumber)
                        || string.IsNullOrWhiteSpace(imineNumber)
                        || string.IsNullOrWhiteSpace(tegacyclineNumber)
                        || string.IsNullOrWhiteSpace(bacteriostasisNumber)
                        || string.IsNullOrWhiteSpace(recheckNumber)*/)
                        continue; //该行没有数据

                    if (string.IsNullOrWhiteSpace(no) || string.IsNullOrWhiteSpace(germ))
                        throw new Exception("第 " + n + " 行中的编号或细菌名称不能为空");

                    //细菌名称必须是在指定的范围内
                    //if (!systemGerm.Contains("," + germ.Trim() + ","))
                    //    throw new Exception("第 " + n + " 行中的细菌名称系统不支持");

                    list.Add(new CRProjectItem()
                    {
                        Id = CommonHelper.GuidToLongID,
                        Number = no.Trim(),
                        Name = germ.Trim(),
                        ProjectId = entity.Id,
                        MIC_Imipenem = imipenemNumber,
                        ImineNumber = imineNumber,
                        TegacyclineNumber = tegacyclineNumber,
                        BacteriostasisNumber = bacteriostasisNumber,
                        RecheckNumber = recheckNumber
                    });
                }

                if (list == null || !list.Any())
                    throw new Exception("从Excel的文件中未能读取到数据");

                //1、保存主表中的上传文件
                entity.UploadFilePath = filePath;
                entity.FileName = oldFileName;
                this.Update(entity);

                //2、删除之前上传的数据
                this.ProjectItemService.DeleteByProject(entity.Id);

                //3、保存明细数据到数据库
                this.ProjectItemService.Insert(list);

                //4、添加CR日志
                this.ProjectLogService.Insert(ActionType.Create, ActionSource.Web, entity.Id, member.Id, member.Name, "上传Excel数据", "上传Excel数据");

                //5、添加操作日志
                this.ActionLogService.Insert(ActionType.Edit, ActionSource.Web, member.Id, member.Name, "上传CR项目Excel数据", entity.SerializeObject());

                //6、返回
                return "SUCCESS";

                #endregion
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert(ex, SystemLogLevel.Error);
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取指定用户最后的一次提交数据
        /// </summary>
        /// <param name="memberId">所属用户Id</param>
        /// <returns></returns>
        public CRProject QueryEntityByMember(long memberId)
        {
            var entity = this.Query(m => m.Mark > 0 && m.MemberId == memberId).OrderByDescending(m => m.InsertTime).FirstOrDefault();
            return entity;
        }

        /// <summary>
        /// 修改CR数据
        /// </summary>
        /// <param name="entity">CR数据</param>
        /// <param name="member">当前登录用户</param>
        /// <returns>SUCCESS 表示成功，其他则是错误信息</returns>
        public string Update(CRProject entity, Member member)
        {
            #region 业务处理

            try
            {
                //修改CR项目主表的数据
                this.Update(entity);

                //修改用户的省市区地址信息
                var memberEntity = this.MemberService.QueryEntity(member.Id);
                if (memberEntity == null || memberEntity.Id <= 0)
                    return "用户信息不存在";
                memberEntity.ProvinceId = entity.ProvinceId;
                memberEntity.CityId = entity.CityId;
                memberEntity.DistrictsId = entity.DistrictsId;
                memberEntity.Address = entity.Address;
                memberEntity.Area = entity.Area;
                this.MemberService.Update(memberEntity);

                //添加cr日志
                this.ProjectLogService.Insert(ActionType.Create, ActionSource.Web, entity.Id, member.Id, member.Name, "修改CR数据", "修改CR数据");

                //5、添加操作日志
                this.ActionLogService.Insert(ActionType.Edit, ActionSource.Web, member.Id, member.Name, "修改CR项目Excel数据", entity.SerializeObject());

                //6、返回
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert(ex, SystemLogLevel.Error);
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
            List<CRProject> list = new List<CRProject>();
            if (ids != null && ids.Any())
            {
                //勾选了数据则导出指定的数据
                list = this.Query(r => ids.Contains(r.Id) && r.Mark > 0).ToList() ?? new List<CRProject>();
            }
            else
            {
                //如果没有选择数据则默认导出全部的数据，根据excel格式，前面显示医院，如果医院没有上传数据的则姓名等数据为空
                var allProject = this.Query(m => m.Mark > 0).ToList() ?? new List<CRProject>(); //查询所有的项目信息
                var allHospital = this.HospitalService.QueryListCR() ?? new List<Hospital>(); //查询所有参与cr项目的医院

                list.AddRange(allProject);

                allHospital = allHospital.Where(r => !allProject.Any(m => m.HospitalId == r.Id)).ToList();
                allHospital.ForEach(item =>
                {
                    list.Add(new CRProject
                    {
                        HospitalId = item.Id,
                        HospitalName = item.Name
                    });
                });


                //foreach (var item in allHospital)
                //{
                //    var mi = allProject.Where(m => m.HospitalId == item.Id).FirstOrDefault();
                //    if (mi != null && mi.Id > 0)
                //        list.Add(mi);
                //    else
                //        list.Add(new CRProject()
                //        {
                //            HospitalId = item.Id,
                //            HospitalName = item.Name,
                //        });
                //}
            }

            list = list.OrderBy(m => m.Id).ToList() ?? new List<CRProject>();

            //获取导出的明细数据
            var itemList = this.ProjectItemService.Query().Where(p => list.Any(p2 => p2.Id == p.ProjectId)).OrderBy(p => p.ProjectId).ToList() ?? new List<CRProjectItem>();

            //1、第一个sheet是医院上传的明细数据
            this.ExportProjectItem(itemList, list, ep);

            //2、第二个sheet是医院主表数据
            this.ExportProject(list, ep);

            return "CR项目导出数据_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        }

        /// <summary>
        /// 导出项目主表的医院相关信息
        /// </summary>
        /// <param name="list">本次导出的项目数据</param>
        /// <param name="ep"></param>
        private void ExportProject(List<CRProject> list, ExcelPackage ep)
        {
            //写入表头
            ExcelWorksheet ws = ep.Workbook.Worksheets.Add("医院及相关信息");
            ws.Cells[1, 1].Value = "医院名称";
            ws.Cells[1, 2].Value = "上年CRE平均检出率";
            ws.Cells[1, 3].Value = "上年CRAB平均检出率";
            ws.Cells[1, 4].Value = "MH平板来源厂家";
            ws.Cells[1, 5].Value = "联系人";
            ws.Cells[1, 6].Value = "手机";
            ws.Cells[1, 7].Value = "邮箱";
            ws.Cells[1, 8].Value = "地址";
            ws.Cells[1, 9].Value = "邮件确认时间";

            int index = 2;
            foreach (var item in list)
            {
                ws.Cells[index, 1].Value = item.HospitalName;
                ws.Cells[index, 2].Value = item.CREDetectionRate;
                ws.Cells[index, 3].Value = item.CREDrugRate;
                ws.Cells[index, 4].Value = item.MHSource;
                ws.Cells[index, 5].Value = item.Name;
                ws.Cells[index, 6].Value = item.Phone;
                ws.Cells[index, 7].Value = item.Email;
                ws.Cells[index, 8].Value = item.Area + item.Address;
                ws.Cells[index, 9].Value = item.HospitalTime.GetNormalString("L");
                index++;
            }
        }

        /// <summary>
        /// 导出CR项目明细表中的数据
        /// </summary>
        /// <param name="list">本次导出的明细数据</param>
        /// <param name="projectList">本次导出的项目数据</param>
        /// <param name="ep"></param>
        private void ExportProjectItem(List<CRProjectItem> list, List<CRProject> projectList, ExcelPackage ep)
        {
            //写入表头
            ExcelWorksheet ws = ep.Workbook.Worksheets.Add("CR");
            ws.Cells[1, 1].Value = "编号";
            ws.Cells[1, 2].Value = "细菌名称";
            ws.Cells[1, 3].Value = "亚胺培南MIC值(ug/ml)";
            ws.Cells[1, 4].Value = "亚胺培南抑菌圈(mm)";
            ws.Cells[1, 5].Value = "替加环素抑菌圈直径(mm)";
            ws.Cells[1, 6].Value = "替加环素复敏抑菌圈直径(mm)";
            ws.Cells[1, 7].Value = "肉汤法MIC值(ug/ml)";
            ws.Cells[1, 8].Value = "医院";

            int index = 2;
            foreach (var item in list)
            {
                ws.Cells[index, 1].Value = item.Number;
                ws.Cells[index, 2].Value = item.Name;
                ws.Cells[index, 3].Value = item.MIC_Imipenem;
                ws.Cells[index, 4].Value = item.ImineNumber;
                ws.Cells[index, 5].Value = item.TegacyclineNumber;
                ws.Cells[index, 6].Value = item.BacteriostasisNumber;
                ws.Cells[index, 7].Value = item.RecheckNumber;
                ws.Cells[index, 8].Value = this.GetHospitalName(projectList, item.ProjectId);
                index++;
            }
        }

        /// <summary>
        /// 根据项目Id获取项目所对应的医院名称
        /// </summary>
        /// <param name="projectList"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private string GetHospitalName(List<CRProject> projectList, long projectId)
        {
            var entity = projectList.Where(d => d.Id == projectId).FirstOrDefault();
            return (entity == null || entity.Id <= 0) ? "" : entity.HospitalName;
        }

    }
}
