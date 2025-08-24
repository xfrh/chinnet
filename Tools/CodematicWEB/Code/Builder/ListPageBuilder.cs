using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodematicWEB.Code.Model;
using System.IO;
using CodematicWEB.Common;
using CodematicWEB.Code.Common;
using System.Text;

namespace CodematicWEB.Code.Builder
{
    public class ListPageBuilder : BuilderBase
    {

        /// <summary>
        /// 生成列表页面的文件
        /// </summary>
        /// <param name="_tableinfo"></param>
        public ListPageBuilder(TableInfoModel _tableinfo) : base(_tableinfo)
        {

        }

        /// <summary>
        /// 创建代码
        /// </summary>
        /// <returns></returns>
        public override void CreateCode()
        {
            string fileFloder = base.StudioFloder + base.Tableinfo.PagePath; //完整路径
            if (!Directory.Exists(fileFloder)) Directory.CreateDirectory(fileFloder);

            string filePath = fileFloder +"\\"+ base.Tableinfo.EntityClassName + "List.cshtml"; //文件完整路径
            if (File.Exists(filePath)) File.Delete(filePath);


            //模版文件地址
            string tempPath = HttpContext.Current.Server.MapPath("~/File/Template/Test002/List.cshtml.t");
            //替换内容
            string htmlString = "";
            using (StreamReader sr = new StreamReader(tempPath, Encoding.UTF8))
            {
                htmlString = sr.ReadToEnd();
                sr.Close();
            }

            StringPlus htmlContent = new StringPlus();
            this.GetHtmlContent(htmlContent);

            //@model ManageSystem.Admin.Models.Log.SystemLogModel
            htmlString = htmlString.Replace("<$$ModelPath$$>", base.Tableinfo.ModelNameSpace+"." +base.Tableinfo.ModelClassName);
            htmlString = htmlString.Replace("<$$TableTitle$$>", base.Tableinfo.ShowName);
            htmlString = htmlString.Replace("<$$TableName$$>", base.Tableinfo.Name);

            htmlString = htmlString.Replace("<$$EntityClassName$$>", base.Tableinfo.EntityClassName);
            htmlString = htmlString.Replace("<$$ControllerName$$>", base.Tableinfo.ControllerClassName.Replace("Controller", ""));
            htmlString = htmlString.Replace("<$$HtmlContent$$>", htmlContent.ToString());


            //将内容写入到文件
            FileUtil.WriteFile(filePath, htmlString.ToString());
        }


        private void GetHtmlContent(StringPlus htmlContent)
        {

            foreach (var item in base.Tableinfo.ColumnInfoList)
            {
                //生成所有的字段
                htmlContent.AppendSpaceLine(2, "{");
                htmlContent.AppendSpaceLine(3, "field: \"" + item.ColumnName + "\",");
                htmlContent.AppendSpaceLine(3, "title: \"" + item.ShowName + "\",");
                switch (item.DataType.ToLower())
                {
                    case "datetime":
                        htmlContent.AppendSpaceLine(3, "type: \"date\",");
                        htmlContent.AppendSpaceLine(3, "format: \"{0:yyyy-MM-dd HH:mm}\",");
                        break;
                    default: break;
                }

                htmlContent.AppendSpaceLine(3, "width: 100");
                htmlContent.AppendSpaceLine(2, "},");
            }


        }


    }
}