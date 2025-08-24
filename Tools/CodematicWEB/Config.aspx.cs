using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

using CodematicCodematicWEB.Common;
using CodematicWEB.Code.Model;
using CodematicWEB.Code.Manage;
using CodematicWEB.Common;

namespace CodematicWEB
{
    public partial class Config : System.Web.UI.Page
    {

       private  TemplateManage templateManage = new TemplateManage();

        protected string TemplateId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.Bind();
            }
        }

        /// <summary>
        /// 如果已经设置过了，这里方向绑定数据
        /// </summary>
        private void Bind()
        {
            //其他设置项
            SettingModel model = new SettingManage().GetSetting();
            if (model == null || string.IsNullOrEmpty(model.DatabaseName))
            {
                //绑定模版数据
                WebUtil.BindData(this.rptTemplateList, templateManage.GetListByXML());
            }
            else
            {
                if (!string.IsNullOrEmpty(model.Connection))
                {
                    DataTable dataBaseTable = new TableInfoManage().GetDatabaseTable(model.Connection);
                    if (dataBaseTable != null) WebUtil.BindData(this.ddlDatabase, dataBaseTable, "Name", "Name");

                    this.ddlDatabase.SelectedValue = model.DatabaseName;
                }

                this.txtConnection.Value = model.Connection;
                this.hfTemplateId.Value = model.TemplateId;
                this.TemplateId = model.TemplateId;
                this.ddlDatabaseVersion.SelectedValue = model.DatabaseVersion;
                this.txtSolutionName.Value = model.SolutionName;
                this.txtFromId.Value = model.FormId;
                this.txtControlId.Value = model.ControlId;

                //绑定模版数据
                WebUtil.BindData(this.rptTemplateList, templateManage.GetListByXML());
            }
        }

        /// <summary>
        /// 加载数据库列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnLoadDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = this.txtConnection.Value;
                string version = this.ddlDatabaseVersion.SelectedValue;

                //非空检查
                if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(version))
                {
                    this.SetDatabaseError.InnerHtml = this.GetErrorHtml("数据库链接字符串和数据库版本不能为空！");
                    return;
                }

                //获取数据库信息并绑定
                DataTable dataBaseTable = new TableInfoManage().GetDatabaseTable(connectionString);
                if (dataBaseTable == null) throw new Exception();
                WebUtil.BindData(this.ddlDatabase, dataBaseTable, "Name", "Name");
            }
            catch (Exception)
            {
                this.SetDatabaseError.InnerHtml = this.GetErrorHtml("获取数据库列表失败，请检查！");
            }
        }

        /// <summary>
        /// 获取错误提示的 HTML 代码
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected string GetErrorHtml(string errorMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"form-group\">");
            sb.Append("<div class=\"col-sm-offset-2 col-sm-8\">");
            sb.Append(" <div class=\"alert alert-danger fade in radius-bordered alert-shadowed\">");
            sb.Append("  <strong>错误：</strong> " + errorMessage);
            sb.Append(" </div>");
            sb.Append("  </div>");
            sb.Append("  </div>");
            return sb.ToString();
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSaveSetting_Click(object sender, EventArgs e)
        {
            SettingModel model = new SettingModel();
            model.TemplateId = this.hfTemplateId.Value;
            model.State = "true";
            model.Connection = this.txtConnection.Value;
            model.DatabaseName = this.ddlDatabase.SelectedValue;
            model.DatabaseVersion = this.ddlDatabaseVersion.SelectedValue;
            model.SolutionName = this.txtSolutionName.Value;
            model.FormId = this.txtFromId.Value;
            model.ControlId = this.txtControlId.Value;

            if (new SettingManage().SaveSetting(model))
            {
                JSUtil.Alert("保存设置成功！", "Index.aspx", this);
                return;
            }

            JSUtil.Alert("保存设置失败，请检查重试！", this);
        }

        /// <summary>
        /// 清除设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnClearSetting_Click(object sender, EventArgs e)
        {
            if (new SettingManage().ClearSetting())
            {
                JSUtil.Alert("清除设置成功！", this.Request.Url.ToString(), this);
                return;
            }

            JSUtil.Alert("清除设置失败，请检查重试！", this);
        }

    }
}