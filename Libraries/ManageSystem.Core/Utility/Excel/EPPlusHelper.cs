using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPPlus.Extensions;
using OfficeOpenXml;
using System.IO;


namespace ManageSystem.Core.Utility
{
    /// <summary>
    /// 使用  EPPlus 第三方的组件读取Excel
    /// </summary>
    public class EPPlusHelper
    {
        private static string GetString(object obj)
        {
            //Peng-2024-01-17增加判断处理
            if(obj == null || string.IsNullOrWhiteSpace(obj.ToString()))
            {
                return string.Empty;
            }

            try
            {
                return obj.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        ///将指定的Excel的文件转换成DataTable
        /// </summary>
        /// <param name="fullFielPath">文件的绝对路径</param>
        /// <param name="startRow">从第几行开始读取数据，1表示第一行</param>
        /// <returns></returns>
        public static DataTable WorksheetToTable(string fullFielPath, int startRow = 1)
        {
            try
            {
                FileInfo existingFile = new FileInfo(fullFielPath);

                ExcelPackage package = new ExcelPackage(existingFile);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];//选定 指定页

                return WorksheetToTable(worksheet, startRow);
            }
            catch (Exception ex)
            {
                Log4Helper.Info($"读取Excel内容错误:{ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 将worksheet转成datatable
        /// </summary>
        /// <param name="worksheet">待处理的worksheet</param>
        ///  <param name="startRow">从第几行开始读取数据，1表示第一行</param>
        /// <returns>返回处理后的datatable</returns>
        public static DataTable WorksheetToTable(ExcelWorksheet worksheet, int startRow = 1)
        {
            //获取worksheet的行数
            int rows = worksheet.Dimension.End.Row;
            //获取worksheet的列数
            int cols = worksheet.Dimension.End.Column;
            try
            {
                DataTable dt = new DataTable(worksheet.Name);
                DataRow dr = null;
                for (int i = startRow; i <= rows; i++)
                {
                    if (i > 1)
                    {
                        dr = dt.Rows.Add();
                    }

                    for (int j = 1; j <= cols; j++)
                    {
                        //默认将第一行设置为datatable的标题
                        if (i == startRow)
                        {
                            dt.Columns.Add(GetString(worksheet.Cells[i, j].Value));
                        }
                        //剩下的写入datatable
                        else
                        {
                            dr[j - 1] = GetString(worksheet.Cells[i, j].Value);
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
