using CodematicWEB.Code.Model;
using CodematicWEB.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;

namespace CodematicWEB.Code.Manage
{
    public class TableInfoManage
    {


        public DataTable GetDatabaseTable(string connectionString)
        {
            try
            {
                string sql = " SELECT dbid ,Name FROM sysdatabases WHERE NAME NOT IN ('master','tempdb','model','msdb') ";

                DBClass.ConnStr = connectionString;
                DataTable table = DBClass.GetDataTable(sql);
                return table;
            }
            catch (Exception)
            {
                return null;
            }

        }



        /// <summary>
        /// 通过Excel文件的绝对路径获取Tableinfo集合
        /// </summary>
        /// <param name="excelFilePath">excel文件的绝对路径</param>
        /// <returns></returns>
        public List<TableInfoModel> GetTableListByExcel(string excelFilePath)
        {

            HSSFWorkbook workbook = null;
            ISheet sheet = null;
            FileStream fs = null;
            FileStream templateFS = null;


            List<TableInfoModel> list = new List<TableInfoModel>();

            try
            {
                templateFS = new FileStream(excelFilePath, FileMode.Open);
                workbook = new HSSFWorkbook(templateFS);

                //获取所有的sheet
                List<ISheet> sheetList = new List<ISheet>();
                for (int i = 0; i < 100; i++)
                {
                    try
                    {
                        ISheet sheetItem = workbook.GetSheetAt(i);
                        if (sheetItem == null) continue;

                        TableInfoModel tableinfoItem = this.GetTableinfoBySheet(sheetItem);
                        if (tableinfoItem == null || string.IsNullOrEmpty(tableinfoItem.Name)) continue;

                        list.Add(tableinfoItem);

                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }


            }
            catch (Exception)
            {


                throw;
            }

            return list;

        }

        /// <summary>
        /// 获取单个 sheet 的表格数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private TableInfoModel GetTableinfoBySheet(ISheet sheet)
        {
            if (sheet == null) return null;

            //主表数据
            TableInfoModel model = new TableInfoModel();

            model.Name = sheet.GetRow(0).Cells[1].StringCellValue; //数据库表名
            model.ShowName = sheet.GetRow(1).Cells[1].StringCellValue;// 显示名称
            model.EntityClassPath = sheet.GetRow(2).Cells[1].StringCellValue;// Entity类生成路径
            model.DataClassPath = sheet.GetRow(3).Cells[1].StringCellValue;// Data类生成路径
            model.ServicesClassPath = sheet.GetRow(4).Cells[1].StringCellValue;// Services类生成路径
            model.ControllerClassPath = sheet.GetRow(5).Cells[1].StringCellValue;// Controllers类地址
            model.ModelClassPath = sheet.GetRow(6).Cells[1].StringCellValue;// Model类生成路径
            model.ValidatorClassPath = sheet.GetRow(7).Cells[1].StringCellValue;// Validator验证类地址
            model.PagePath = sheet.GetRow(8).Cells[1].StringCellValue;// View页面路径
            model.AddAutoMapper = sheet.GetRow(9).Cells[1].StringCellValue.Equals("是");// 添加AutoMapper映射 
            model.AddAutofac = sheet.GetRow(10).Cells[1].StringCellValue.Equals("是");// 注册Autofac

            model.EntityNameSpace = sheet.GetRow(2).Cells[5].StringCellValue; //Entity类命名空间
            model.DataNameSpace = sheet.GetRow(3).Cells[5].StringCellValue; //Data类命名空间
            model.ServicesNameSpace = sheet.GetRow(4).Cells[5].StringCellValue; //Services类命名空间
            model.ControllerNameSpace = sheet.GetRow(5).Cells[5].StringCellValue; //Controllers类命名空间
            model.ModelNameSpace = sheet.GetRow(6).Cells[5].StringCellValue; //Model类命名空间
            model.ValidatorNameSpace = sheet.GetRow(7).Cells[5].StringCellValue; //Validator类命名空间

            model.EntityClassName = sheet.GetRow(2).Cells[7].StringCellValue; //Entity类名
            model.DataClassName= sheet.GetRow(3).Cells[7].StringCellValue; //Data类名
            model.ServicesClassName= sheet.GetRow(4).Cells[7].StringCellValue; //Controllers类名
            model.ControllerClassName = sheet.GetRow(5).Cells[7].StringCellValue; //Validator类名
            model.ModelClassName = sheet.GetRow(6).Cells[7].StringCellValue; //Model类名
            model.ValidatorClassName = sheet.GetRow(7).Cells[7].StringCellValue; //Validator类名

            //设置表的字段
            List<ColumnInfo> list = new List<ColumnInfo>();
            int cell = 0;
            for (int i =13; i < 10000; i++)
            {
                cell = 0;
                IRow row = sheet.GetRow(i);
                if ( row ==null || row.Cells[0] ==null || string.IsNullOrWhiteSpace(row.Cells[0].StringCellValue)) break;

                ColumnInfo item = new ColumnInfo();
                item.ColumnName = row.Cells[cell++].StringCellValue;//字段名称
                item.ShowName = row.Cells[cell++].StringCellValue;//显示名称
                item.DataType = row.Cells[cell++].StringCellValue.ToLower(); //数据类型
                item.IsPK = row.Cells[cell++].StringCellValue.Equals("是"); //主键 
                item.IsValidator = row.Cells[cell++].StringCellValue.Equals("是"); //输入验证    
                item.ValidateEmpty = row.Cells[cell++].StringCellValue; //非空验证提示内容 
                item.ValidateOther = row.Cells[cell++].StringCellValue; //其他验证
                item.ValidateOtherValue = row.Cells[cell++].StringCellValue; //其他验证值

                list.Add(item);
            }

            model.ColumnInfoList = list;

            return model;

        }

    }
}