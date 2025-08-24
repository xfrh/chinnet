using CodematicWEB.Code.Manage;
using CodematicWEB.Code.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodematicWEB.Common;
using CodematicWEB.Code.Builder;

namespace CodematicWEB
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string filePath = this.UploadExcelFile(this.FileUpload1);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                JSUtil.Alert("导入的Excel文件错误，请检查！",this);
                return;
            }
         
            List<TableInfoModel> tableList = new TableInfoManage().GetTableListByExcel(filePath);
            if (tableList == null || tableList.Count <= 0)
            {
                JSUtil.Alert("数据为空！", this);
                return;
            }

            //生成代码
            foreach (var item in tableList)
            {
               new EntityBuilder(item).CreateCode();
               new DataBuilder(item).CreateCode();
               new ServicesBuilder(item).CreateCode();
               new ModelBuilder(item).CreateCode();
               new ValidatorBuilder(item).CreateCode();
                new ControllerBuilder(item).CreateCode();
                new AutofacBuilder(item).CreateCode();
              new AutoMapperBuilder(item).CreateCode();
                new CreatePageBuilder(item).CreateCode();
                new EditPageBuilder(item).CreateCode();
                new ViewPageBuilder(item).CreateCode();
                new ListPageBuilder(item).CreateCode();

            }
        }


        /// <summary>
        /// 上传文件并返回绝对地址
        /// </summary>
        /// <param name="fup"></param>
        /// <returns></returns>
        private string UploadExcelFile(FileUpload fup)
        {
            try
            {

        
            string tempFileName = "";
            HttpPostedFile theFile = fup.PostedFile;
            if (theFile.ContentLength <= 0) return "";

            string fileName = theFile.FileName;
            int index = fileName.IndexOf('.');
            string strEx = fileName.Substring(index, fileName.Length- index);

            if (!strEx.ToLower().Equals(".xls")) return "";

            tempFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + strEx;
            string newPath =this.Server.MapPath(@"/File/UploadFile/" + tempFileName);

            if (File.Exists(newPath)) File.Delete(newPath);

            theFile.SaveAs(newPath);

            return newPath;
            }
            catch (Exception)
            {
              
            }

            return "";

        }
    }
}