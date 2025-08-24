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
    public class EditPageBuilder : BuilderBase
    {

        /// <summary>
        /// 生成编辑页面的文件
        /// </summary>
        /// <param name="_tableinfo"></param>
        public EditPageBuilder(TableInfoModel _tableinfo) : base(_tableinfo)
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

            string filePath = fileFloder +"\\"+ base.Tableinfo.EntityClassName + "Edit.cshtml"; //文件完整路径
            if (File.Exists(filePath)) File.Delete(filePath);


            //模版文件地址
            string tempPath = HttpContext.Current.Server.MapPath("~/File/Template/Test002/Edit.cshtml.t");
            //替换内容
            string htmlString = "";
            using (StreamReader sr = new StreamReader(tempPath, Encoding.UTF8))
            {
                htmlString = sr.ReadToEnd();
                sr.Close();
            }

            //@model ManageSystem.Admin.Models.Log.SystemLogModel
            htmlString = htmlString.Replace("<$$ModelPath$$>", base.Tableinfo.ModelNameSpace+"." +base.Tableinfo.ModelClassName);
            htmlString = htmlString.Replace("<$$TableTitle$$>", "编辑"+base.Tableinfo.ShowName);
            htmlString = htmlString.Replace("<$$TableName$$>", base.Tableinfo.Name);

            //将内容写入到文件
            FileUtil.WriteFile(filePath, htmlString.ToString());
        }

    }
}