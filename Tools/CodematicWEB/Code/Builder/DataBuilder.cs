using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodematicWEB.Code.Model;
using System.IO;
using CodematicWEB.Common;
using CodematicWEB.Code.Common;

namespace CodematicWEB.Code.Builder
{
    /// <summary>
    /// 生成data操作数据库类
    /// </summary>
    public class DataBuilder : BuilderBase
    {
        public DataBuilder(TableInfoModel _tableinfo) : base(_tableinfo)
        {

        }

        public override void CreateCode()
        {
            string fileFloder = base.StudioFloder + base.Tableinfo.DataClassPath; //完整路径
            if (!Directory.Exists(fileFloder)) Directory.CreateDirectory(fileFloder);

            string filePath = fileFloder + "\\" + base.Tableinfo.DataClassName + ".cs"; //文件完整路径
            if (File.Exists(filePath)) File.Delete(filePath);

            //生成代码
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Linq;");
            strclass.AppendLine("using System.Text;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Collections.Generic;");
            strclass.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            strclass.AppendLine("using "+base.Tableinfo.EntityNameSpace+";");
            strclass.AppendLine("");
            strclass.AppendLine("");
            strclass.AppendLine("namespace " + base.Tableinfo.DataNameSpace);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// 数据库操作类 ，数据库表名：" + base.Tableinfo.Name + " ");
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "public partial class " + base.Tableinfo.DataClassName + " : ManageSystemEntityTypeConfiguration<"+base.Tableinfo.EntityClassName+">");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "public "+ base.Tableinfo.DataClassName + "()");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendLine(GetCode());
            strclass.AppendSpaceLine(2, "}");
            strclass.AppendLine("");
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");

            //将内容写入到文件
            FileUtil.WriteFile(filePath, strclass.ToString());
        }

        private string GetCode()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(3, "this.ToTable(\""+base.Tableinfo.Name+"\");");
            strclass.AppendSpaceLine(3, "this.HasKey(p => p.Id);");
            strclass.AppendSpaceLine(3, "this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);");

            foreach (var item in base.Tableinfo.ColumnInfoList)
            {
             
            }

            return strclass.ToString();

        }
    }
}