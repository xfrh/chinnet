using Dapper;
using ManageSystem.Core.Utility;
using ManageSystem.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Medicine
{
    public class MedicalDataHandleLockService : IMedicalDataHandleLockService
    {
        /// <summary>
        /// 状态更新完成
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool Complete(long medicalDataId, int status)
        {
            using (var conn = DapperHelper.GetConnection())
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                DateTime now = DateTime.Now;
                parameters.Add("@MedicalDataId", medicalDataId);
                parameters.Add("@Status", status);
                parameters.Add("@CompleteDate", now);
                parameters.Add("@Describe", $"\r\n{now.ToString("yyyy-MM-dd HH:mm:ss")}数据处理完成, Status: {status}");
                return conn.Execute("UPDATE [dbo].[MedicalDataHandleLock] SET [Status] = @Status, [CompleteDate] = @CompleteDate, [Describe] += @Describe, [Mark] = 2, [UpdateTime] = GETDATE() WHERE [MedicalDataId] = @MedicalDataId;", parameters) > 0;
            }
        }
        /// <summary>
        /// 获取状态,
        /// 不存在时返回0
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <returns></returns>
        public int GetStatusByMedicalDataId(long medicalDataId)
        {
            using (var conn = DapperHelper.GetConnection())
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MedicalDataId", medicalDataId);
                int count = conn.ExecuteScalar<int>("SELECT COUNT(1) FROM dbo.MedicalDataHandleLock WHERE [MedicalDataId] = @MedicalDataId AND [Mark] > 0;", parameters);
                if (count == 0)return 0;

                return conn.QueryFirstOrDefault<int>("SELECT TOP(1) [Status] FROM [dbo].[MedicalDataHandleLock] WHERE [MedicalDataId] = @MedicalDataId AND [Mark] > 0 ORDER BY [HandleDate] DESC;", parameters);
            }
        }

        /// <summary>
        /// 新增处理
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool Insert(long medicalDataId, int status)
        {
            using (var conn = DapperHelper.GetConnection())
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MedicalDataId", medicalDataId);
                DateTime now = DateTime.Now;
                parameters.Add("@Id", CommonHelper.GuidToLongID);
                parameters.Add("@Status", status);
                parameters.Add("@HandleDate", now);
                parameters.Add("@Describe", $"{now.ToString("yyyy-MM-dd HH:mm:ss")}开始处理数据，Status: {status}");
                return conn.Execute(@"INSERT INTO [dbo].[MedicalDataHandleLock] ( [Id], [MedicalDataId], [Status], [HandleDate], [CompleteDate], [InsertTime], [UpdateTime], [DeleteTime], [Version], [Mark], [Describe] ) VALUES ( @Id, @MedicalDataId, @Status, @HandleDate, NULL, @HandleDate, @HandleDate, '1900-01-01 00:00:00.000', 1, 1, @Describe );", parameters) > 0;
            }
        }

        /// <summary>
        /// 出现错误
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool Error(long medicalDataId, int status, string remark)
        {
            using (var conn = DapperHelper.GetConnection())
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MedicalDataId", medicalDataId);
                parameters.Add("@Status", status);
                parameters.Add("@Describe", remark);
                return conn.Execute("UPDATE [dbo].[MedicalDataHandleLock] SET [Status] = @Status, [Describe] += @Describe, [Mark] = 2, [UpdateTime] = GETDATE() WHERE [MedicalDataId] = @MedicalDataId;", parameters) > 0;
            }
        }
    }
}
