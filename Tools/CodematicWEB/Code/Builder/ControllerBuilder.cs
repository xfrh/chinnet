using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodematicWEB.Code.Model;
using System.IO;
using CodematicWEB.Common;
using CodematicWEB.Code.Common;
using System.Threading;

namespace CodematicWEB.Code.Builder
{
    /// <summary>
    /// 生成控制的代码
    /// </summary>
    public class ControllerBuilder : BuilderBase
    {

        /// <summary>
        /// 生成实体类的文件
        /// </summary>
        /// <param name="_tableinfo"></param>
        public ControllerBuilder(TableInfoModel _tableinfo) : base(_tableinfo)
        {

        }

        /// <summary>
        /// 创建代码
        /// </summary>
        /// <returns></returns>
        public override void CreateCode()
        {
            string fileFloder = base.StudioFloder + base.Tableinfo.ControllerClassPath; //完整路径
            if (!Directory.Exists(fileFloder)) Directory.CreateDirectory(fileFloder);

            string filePath = fileFloder +"\\"+ base.Tableinfo.ControllerClassName + "_temp.cs"; //文件完整路径
            if (File.Exists(filePath)) File.Delete(filePath);

            //生成代码
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Linq;");
            strclass.AppendLine("using System.Text;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Collections.Generic;");
            strclass.AppendLine("using ManageSystem.Admin.Extensions;");
            strclass.AppendLine("using ManageSystem.Framework.Controllers;");
            strclass.AppendLine("using ManageSystem.Framework.Kendoui;");
            strclass.AppendLine("using System.Web;");
            strclass.AppendLine("using System.Web.Mvc;");
            strclass.AppendLine("using ManageSystem.Core;");
            strclass.AppendLine("using " + this.Tableinfo.ServicesNameSpace + ";");
            strclass.AppendLine("using " + this.Tableinfo.ModelNameSpace + ";");
            strclass.AppendLine("using "+this.Tableinfo.EntityNameSpace+";");
            strclass.AppendLine("using System.Linq.Expressions;");
            strclass.AppendLine("using ManageSystem.Services.Log;");
            strclass.AppendLine( "");
            strclass.AppendLine("namespace " + base.Tableinfo.ControllerNameSpace);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "");
            strclass.AppendSpaceLine(1, "public  class " + base.Tableinfo.ControllerClassName + " : AdminBaseController");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2,"private readonly I"+this.Tableinfo.ServicesClassName+" "+ StringUtil.ToTitleUpper( this.Tableinfo.ServicesClassName)+";");
            strclass.AppendSpaceLine(2,"  ");
            strclass.AppendSpaceLine(2,"public "+this.Tableinfo.ControllerClassName+ "( ");
            strclass.AppendSpaceLine(2,"    I" + this.Tableinfo.ServicesClassName + "  _" + StringUtil.ToTitleLower(this.Tableinfo.ServicesClassName) + "");
            strclass.AppendSpaceLine(2,")");
            strclass.AppendSpaceLine(2,"{");
            strclass.AppendSpaceLine(3,"this. " + StringUtil.ToTitleUpper(this.Tableinfo.ServicesClassName) + " =  _" + StringUtil.ToTitleLower(this.Tableinfo.ServicesClassName) + ";");
            strclass.AppendSpaceLine(2,"}");
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "#region " + this.Tableinfo.ShowName);
            strclass.AppendLine("");
            strclass.AppendLine(this.GetListCode());
            strclass.AppendLine("");
            strclass.AppendLine(this.GetCreateCode());
            strclass.AppendLine("");
            strclass.AppendLine(this.GetEditCode());
            strclass.AppendLine("");
            strclass.AppendLine(this.GetViewCode());
            strclass.AppendLine("");
            strclass.AppendLine(this.GetDeleteCode());
            strclass.AppendLine("");

            strclass.AppendSpaceLine(2, "#endregion");
            strclass.AppendLine("");
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");
            

            //将内容写入到文件
            FileUtil.WriteFile(filePath, strclass.ToString());
        }



        /// <summary>
        /// 获取List函数的代码
        /// </summary>
        /// <returns></returns>
        private string GetListCode()
        {
            StringPlus strclass = new StringPlus();

            strclass.AppendSpaceLine(2, " /// <summary>");
            strclass.AppendSpaceLine(2, " /// "+this.Tableinfo.ShowName+" 列表页面");
            strclass.AppendSpaceLine(2, " /// </summary>");
            strclass.AppendSpaceLine(2, " /// <returns></returns>");
            strclass.AppendSpaceLine(2, "public ActionResult " + StringUtil.ToTitleUpper(this.Tableinfo.Name) + "List()");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return View();");
            strclass.AppendSpaceLine(2, "}");
            strclass.AppendLine("");

            strclass.AppendSpaceLine(2, " /// <summary>");
            strclass.AppendSpaceLine(2, " /// " + this.Tableinfo.ShowName + " 获取数据");
            strclass.AppendSpaceLine(2, " /// </summary>");
            strclass.AppendSpaceLine(2, " /// <param name=\"command\">数据源对象，分页等数据</param>");
            strclass.AppendSpaceLine(2, " /// <param name=\"model\">查询数据对象</param>");
            strclass.AppendSpaceLine(2, " /// <returns></returns>");
            strclass.AppendSpaceLine(2, "[HttpPost]");
            strclass.AppendSpaceLine(2, "public ActionResult " + StringUtil.ToTitleUpper(this.Tableinfo.Name) + "List(DataSourceRequest command, "+this.Tableinfo.ModelClassName+" model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "");
            strclass.AppendSpaceLine(3, "//获得数据");
            strclass.AppendSpaceLine(3, "var list = this." + StringUtil.ToTitleUpper(this.Tableinfo.ServicesClassName) + ".QueryPage(command.Page - 1, command.PageSize);");
            strclass.AppendSpaceLine(3, "");
            strclass.AppendSpaceLine(3, "var gridModel = new DataSourceResult");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "Data = list.Select(x => x.ToModel()),");
            strclass.AppendSpaceLine(4, "Total = list.TotalCount");
            strclass.AppendSpaceLine(3, "};");
            strclass.AppendSpaceLine(3, "");
            strclass.AppendSpaceLine(3, "return new JsonResult { Data = gridModel };");
            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();
        }


        /// <summary>
        /// 获取添加函数的代码
        /// </summary>
        /// <returns></returns>
        private string GetCreateCode()
        {
            StringPlus strclass = new StringPlus();

            strclass.AppendSpaceLine(2, " /// <summary>");
            strclass.AppendSpaceLine(2, " /// " + this.Tableinfo.ShowName + " 设置创建页面的数据");
            strclass.AppendSpaceLine(2, " /// </summary>");
            strclass.AppendSpaceLine(2, " /// <returns></returns>");
            strclass.AppendSpaceLine(2, "public " + this.Tableinfo.ModelClassName + "  Set"+this.Tableinfo.Name+"CreateData()");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, this.Tableinfo.ModelClassName+ " model = new " + this.Tableinfo.ModelClassName+"() ;");
            strclass.AppendSpaceLine(3, "");
            strclass.AppendSpaceLine(3, "return model;");
            strclass.AppendSpaceLine(2, "}");
            strclass.AppendLine("");

            strclass.AppendSpaceLine(2, " /// <summary>");
            strclass.AppendSpaceLine(2, " /// " + this.Tableinfo.ShowName + " 创建");
            strclass.AppendSpaceLine(2, " /// </summary>");
            strclass.AppendSpaceLine(2, " /// <returns></returns>");
            strclass.AppendSpaceLine(2, "public ActionResult " + StringUtil.ToTitleUpper(this.Tableinfo.Name) + "Create()");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return View(this.Set" + this.Tableinfo.Name + "CreateData());");
            strclass.AppendSpaceLine(3, "");
            strclass.AppendSpaceLine(2, "}");
            strclass.AppendLine("");

            strclass.AppendSpaceLine(2, " /// <summary>");
            strclass.AppendSpaceLine(2, " /// " + this.Tableinfo.ShowName + " 保存数据");
            strclass.AppendSpaceLine(2, " /// </summary>");
            strclass.AppendSpaceLine(2, " /// <param name=\"model\">保存对象</param>");
            strclass.AppendSpaceLine(2, " /// <returns></returns>");
            strclass.AppendSpaceLine(2, "[ValidateAntiForgeryToken]");
            strclass.AppendSpaceLine(2, "[HttpPost]");
            strclass.AppendSpaceLine(2, "public ActionResult " +StringUtil.ToTitleUpper( this.Tableinfo.Name) + "Create(" + this.Tableinfo.ModelClassName + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "if (ModelState.IsValid) ");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "var entity = model.ToEntity();");
            strclass.AppendSpaceLine(4, "this." + StringUtil.ToTitleUpper(this.Tableinfo.ServicesClassName) + ".Insert(entity);");
            strclass.AppendSpaceLine(4, "");
            strclass.AppendSpaceLine(4, "string logContent = \"添加" + this.Tableinfo.ShowName + " \";");
            strclass.AppendSpaceLine(4, "base.InsetActionLog(ActionType.Create,logContent,entity.SerializeObject());");
            strclass.AppendSpaceLine(4, "base.SuccessNotification(logContent);");
            strclass.AppendSpaceLine(4, "");
            strclass.AppendSpaceLine(4, "return this.RedirectToAction(\"" + StringUtil.ToTitleUpper(this.Tableinfo.Name) + "Create\");");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "");
            strclass.AppendSpaceLine(3, "return View(this.Set" + this.Tableinfo.Name + "CreateData());");
            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();
        }

        

        /// <summary>
        /// 获取编辑函数的代码
        /// </summary>
        /// <returns></returns>
        private string GetEditCode()
        {
            StringPlus strclass = new StringPlus();

            strclass.AppendSpaceLine(2, " /// <summary>");
            strclass.AppendSpaceLine(2, " /// " + this.Tableinfo.ShowName + " 设置编辑页面的数据");
            strclass.AppendSpaceLine(2, " /// </summary>");
            strclass.AppendSpaceLine(2, " /// <returns></returns>");
            strclass.AppendSpaceLine(2, "public " + this.Tableinfo.ModelClassName + "  Set" + this.Tableinfo.Name + "EditData(long id)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "var entity = this." + StringUtil.ToTitleUpper(this.Tableinfo.ServicesClassName) + ".QueryEntity(id);");
            strclass.AppendSpaceLine(3, "");
            strclass.AppendSpaceLine(3, "var model = entity.ToModel();");
            strclass.AppendSpaceLine(3, "");
            strclass.AppendSpaceLine(3, "return model;");
            strclass.AppendSpaceLine(2, "}");
            strclass.AppendLine("");

            strclass.AppendSpaceLine(2, " /// <summary>");
            strclass.AppendSpaceLine(2, " /// " + this.Tableinfo.ShowName + " 编辑");
            strclass.AppendSpaceLine(2, " /// </summary>");
            strclass.AppendSpaceLine(2, " /// <returns></returns>");
            strclass.AppendSpaceLine(2, "public ActionResult " + StringUtil.ToTitleUpper(this.Tableinfo.Name) + "Edit(long id)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, " return this.View(this.Set" + this.Tableinfo.Name + "EditData(id));");
            strclass.AppendSpaceLine(2, "}");
            strclass.AppendLine("");

            strclass.AppendSpaceLine(2, " /// <summary>");
            strclass.AppendSpaceLine(2, " /// " + this.Tableinfo.ShowName + " 保存编辑数据");
            strclass.AppendSpaceLine(2, " /// </summary>");
            strclass.AppendSpaceLine(2, " /// <param name=\"model\">保存对象</param>");
            strclass.AppendSpaceLine(2, " /// <returns></returns>");
            strclass.AppendSpaceLine(2, "[ValidateAntiForgeryToken]");
            strclass.AppendSpaceLine(2, "[HttpPost]");
            strclass.AppendSpaceLine(2, "public ActionResult " + StringUtil.ToTitleUpper(this.Tableinfo.Name) + "Edit(" + this.Tableinfo.ModelClassName + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "if (ModelState.IsValid) ");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "var entity = this." + StringUtil.ToTitleUpper(this.Tableinfo.ServicesClassName) + ".QueryEntity(model.Id);");
            strclass.AppendSpaceLine(4, "base.SetDefaultValue(model, entity);");
            strclass.AppendSpaceLine(4, "entity = model.ToEntity(entity);");
            strclass.AppendSpaceLine(4, "");
            strclass.AppendSpaceLine(4, "this." + StringUtil.ToTitleUpper(this.Tableinfo.ServicesClassName) + ".Update(entity);");
            strclass.AppendSpaceLine(4, "");
            strclass.AppendSpaceLine(4, "string logContent = \"修改" + this.Tableinfo.ShowName + " \";");
            strclass.AppendSpaceLine(4, "base.InsetActionLog(ActionType.Edit,logContent,entity.SerializeObject());");
            strclass.AppendSpaceLine(4, "base.SuccessNotification(logContent);");
            strclass.AppendSpaceLine(4, "");
            strclass.AppendSpaceLine(4, "return this.RedirectToAction(\"" + StringUtil.ToTitleUpper(this.Tableinfo.Name) + "Edit\");");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return this.View(this.Set" + this.Tableinfo.Name + "EditData(model.Id));");
            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();
        }


        /// <summary>
        /// 获取查看函数的代码
        /// </summary>
        /// <returns></returns>
        private string GetViewCode()
        {
            StringPlus strclass = new StringPlus();

            strclass.AppendSpaceLine(2, " /// <summary>");
            strclass.AppendSpaceLine(2, " /// " + this.Tableinfo.ShowName + " 查看");
            strclass.AppendSpaceLine(2, " /// </summary>");
            strclass.AppendSpaceLine(2, " /// <returns></returns>");
            strclass.AppendSpaceLine(2, "public ActionResult " + StringUtil.ToTitleUpper(this.Tableinfo.Name) + "View(long id)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, " return this.View(this.Set" + this.Tableinfo.Name + "EditData(id));");
            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();
        }


        /// <summary>
        /// 获取删除函数的代码
        /// </summary>
        /// <returns></returns>
        private string GetDeleteCode()
        {
            StringPlus strclass = new StringPlus();
          
            strclass.AppendSpaceLine(2, " /// <summary>");
            strclass.AppendSpaceLine(2, " /// " + this.Tableinfo.ShowName + " 删除");
            strclass.AppendSpaceLine(2, " /// </summary>");
            strclass.AppendSpaceLine(2, " /// <param name=\"selectedIds\">需要删除的id集合，使用英文逗号分割</param>");
            strclass.AppendSpaceLine(2, " /// <returns></returns>");
            strclass.AppendSpaceLine(2, "[HttpPost]");
            strclass.AppendSpaceLine(2, "[ValidateAntiForgeryToken]");
            strclass.AppendSpaceLine(2, "public ActionResult " + StringUtil.ToTitleUpper(this.Tableinfo.Name) + "Delete(string selectedIds)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "if (string.IsNullOrWhiteSpace(selectedIds))");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "base.ErrorNotification(\"请选择需要删除的数据\");");
            strclass.AppendSpaceLine(4, "return this.RedirectToAction(\""+ StringUtil.ToTitleUpper(this.Tableinfo.Name) + "List\");");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "");
            strclass.AppendSpaceLine(3, "this." + StringUtil.ToTitleUpper(this.Tableinfo.ServicesClassName) + ".Delete(selectedIds);");
            strclass.AppendSpaceLine(3, "");
            strclass.AppendSpaceLine(3, "string logContent = \"删除" + this.Tableinfo.ShowName + "，删除的id集合:\"+ selectedIds ;");
            strclass.AppendSpaceLine(3, "base.InsetActionLog(ActionType.Delete,logContent,selectedIds.SerializeObject());");
            strclass.AppendSpaceLine(3, "base.SuccessNotification(logContent);");
            strclass.AppendSpaceLine(3, "");
            strclass.AppendSpaceLine(3, "return this.RedirectToAction(\"" + StringUtil.ToTitleUpper(this.Tableinfo.Name) + "List\");");
            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();
        }

        private string GetCode()
        {
            StringPlus strclass = new StringPlus();
            foreach (var item in base.Tableinfo.ColumnInfoList)
            {
                //生成所有的字段
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// "+item.ShowName);
                strclass.AppendSpaceLine(2, "/// <summary>");
                switch (item.DataType.ToLower())
                {
                    case "int": strclass.AppendSpaceLine(2, "public Int32 " + item.ColumnName + " { get; set; }"); break;
                    case "decimal": strclass.AppendSpaceLine(2, "public Decimal " + item.ColumnName + " { get; set; }"); break;
                    case "datetime": strclass.AppendSpaceLine(2, "public DateTime " + item.ColumnName + " { get; set; }"); break;
                    default: strclass.AppendSpaceLine(2, "public String " + item.ColumnName + " { get; set; }"); break;
                }
            }

            return strclass.ToString();

        }
    }
}