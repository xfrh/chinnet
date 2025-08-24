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
    public class EntityBuilder : BuilderBase
    {

        /// <summary>
        /// 生成实体类的文件
        /// </summary>
        /// <param name="_tableinfo"></param>
        public EntityBuilder(TableInfoModel _tableinfo) : base(_tableinfo)
        {

        }

        /// <summary>
        /// 创建代码
        /// </summary>
        /// <returns></returns>
        public override void CreateCode()
        {
            string fileFloder = base.StudioFloder + base.Tableinfo.EntityClassPath; //完整路径
            if (!Directory.Exists(fileFloder)) Directory.CreateDirectory(fileFloder);

            string filePath = fileFloder +"\\"+ base.Tableinfo.EntityClassName + ".cs"; //文件完整路径
            if (File.Exists(filePath)) File.Delete(filePath);

            //生成代码
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Linq;");
            strclass.AppendLine("using System.Text;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Collections.Generic;");
            strclass.AppendLine("");
            strclass.AppendLine("");
            strclass.AppendLine("namespace " + base.Tableinfo.EntityNameSpace);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// 实体类 ，数据库表名：" + base.Tableinfo.Name + " ");
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "public partial class " + base.Tableinfo.EntityClassName + " : BaseEntity");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendLine("");
            strclass.AppendLine(GetCode());
            strclass.AppendLine("");
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");

            //将内容写入到文件
            FileUtil.WriteFile(filePath, strclass.ToString());
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
                    case "long": strclass.AppendSpaceLine(2, "public long " + item.ColumnName + " { get; set; }"); break;
                    case "bool": strclass.AppendSpaceLine(2, "public bool " + item.ColumnName + " { get; set; }"); break;
                    case "double": strclass.AppendSpaceLine(2, "public double " + item.ColumnName + " { get; set; }"); break;
                    case "float": strclass.AppendSpaceLine(2, "public float " + item.ColumnName + " { get; set; }"); break;
                    case "decimal": strclass.AppendSpaceLine(2, "public Decimal " + item.ColumnName + " { get; set; }"); break;
                    case "datetime": strclass.AppendSpaceLine(2, "public DateTime " + item.ColumnName + " { get; set; }"); break;
                    default: strclass.AppendSpaceLine(2, "public String " + item.ColumnName + " { get; set; }"); break;
                }
            }

            return strclass.ToString();

        }
    }
}