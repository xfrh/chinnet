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
    public class CreatePageBuilder : BuilderBase
    {

        /// <summary>
        /// 生成创建页面的文件
        /// </summary>
        /// <param name="_tableinfo"></param>
        public CreatePageBuilder(TableInfoModel _tableinfo) : base(_tableinfo)
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

            string filePath = fileFloder +"\\"+ base.Tableinfo.EntityClassName + "Create.cshtml"; //文件完整路径
            if (File.Exists(filePath)) File.Delete(filePath);


            //模版文件地址
            string tempPath = HttpContext.Current.Server.MapPath("~/File/Template/Test002/Create.cshtml.t");
            //替换内容
            string htmlString = "";
            using (StreamReader sr = new StreamReader(tempPath, Encoding.UTF8))
            {
                htmlString = sr.ReadToEnd();
                sr.Close();
            }

            //@model ManageSystem.Admin.Models.Log.SystemLogModel
            htmlString = htmlString.Replace("<$$ModelPath$$>", base.Tableinfo.ModelNameSpace+"." +base.Tableinfo.ModelClassName);
            htmlString = htmlString.Replace("<$$TableTitle$$>", "添加"+base.Tableinfo.ShowName);
            htmlString = htmlString.Replace("<$$TableName$$>", base.Tableinfo.Name);

            //将内容写入到文件
            FileUtil.WriteFile(filePath, htmlString.ToString());

            this.CreateOrUpdate();
        }

        #region _CreateOrUpdate
        private void CreateOrUpdate()
        {
            string fileFloder = base.StudioFloder + base.Tableinfo.PagePath; //完整路径
            if (!Directory.Exists(fileFloder)) Directory.CreateDirectory(fileFloder);

            string filePath = fileFloder + "\\_CreateOrUpdate" + base.Tableinfo.EntityClassName + ".cshtml"; //文件完整路径
            if (File.Exists(filePath)) File.Delete(filePath);


            //模版文件地址
            string tempPath = HttpContext.Current.Server.MapPath("~/File/Template/Test002/_CreateOrUpdate.cshtml.t");
            //替换内容
            string htmlString = "";
            using (StreamReader sr = new StreamReader(tempPath, Encoding.UTF8))
            {
                htmlString = sr.ReadToEnd();
                sr.Close();
            }

            StringPlus htmlContent = new StringPlus();
            StringPlus jsContent = new StringPlus();

            this.GetHtmlContent(htmlContent, jsContent);
            htmlString = htmlString.Replace("<$$ModelPath$$>", base.Tableinfo.ModelNameSpace + "." + base.Tableinfo.ModelClassName);
            htmlString = htmlString.Replace("<$$HtmlContent$$>", htmlContent.ToString());
            htmlString = htmlString.Replace("<$$JSContent$$>", jsContent.ToString());

            //将内容写入到文件
            FileUtil.WriteFile(filePath, htmlString.ToString());
        }

        private void GetHtmlContent(StringPlus htmlContent, StringPlus jsContent)
        {
            htmlContent.AppendSpaceLine(2,"<table class=\"adminContent\">");

            foreach (var item in base.Tableinfo.ColumnInfoList)
            {
                //生成所有的字段
               htmlContent.AppendSpaceLine(2, "<tr>");
               htmlContent.AppendSpaceLine(3, "<td class=\"adminTitle\">@Html.NopLabelFor(m => m."+item.ColumnName+")：</td>");
               htmlContent.AppendSpaceLine(4, "<td class=\"adminData\">");
                switch (item.DataType.ToLower())
                {
                    case "datetime": ;
                        jsContent.AppendSpaceLine(4, "$(\"#"+item.ColumnName+"\").kendoDatePicker({ value: new Date(), format: \"yyyy-MM-dd HH:mm\" });");
                        htmlContent.AppendSpaceLine(4, "@Html.EditorFor(m => m." + item.ColumnName + ")"); 
                        break;
                    default: htmlContent.AppendSpaceLine(4, "@Html.EditorFor(m => m." + item.ColumnName + ")"); break;
                }

                htmlContent.AppendSpaceLine(4, "@Html.ValidationMessageFor(m => m." + item.ColumnName + ")");
                htmlContent.AppendSpaceLine(3, "</td>");
                htmlContent.AppendSpaceLine(3, "</tr>");
            }

            htmlContent.AppendSpaceLine(2, "</table>");

        }



        #endregion






    }
}