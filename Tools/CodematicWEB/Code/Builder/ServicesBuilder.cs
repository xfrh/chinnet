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
    /// 生成Services操类
    /// </summary>
    public class ServicesBuilder : BuilderBase
    {
        public ServicesBuilder(TableInfoModel _tableinfo) : base(_tableinfo)
        {

        }

        public override void CreateCode()
        {

            //创建接口类
            this.CreateInterface();

            //创建继承接口的具体类
            this.Create();

        }


        /// <summary>
        /// 创建接口类
        /// </summary>
        private void CreateInterface()
        {
            string fileFloder = base.StudioFloder + base.Tableinfo.ServicesClassPath; //完整路径
            if (!Directory.Exists(fileFloder)) Directory.CreateDirectory(fileFloder);

            string filePath = fileFloder + "\\I" + base.Tableinfo.ServicesClassName + ".cs"; //文件完整路径
            if (File.Exists(filePath)) File.Delete(filePath);

            //生成代码
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Linq;");
            strclass.AppendLine("using System.Text;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Collections.Generic;");
            strclass.AppendLine("using ManageSystem.Core.Data;");
            strclass.AppendLine("using ManageSystem.Core.Domain.Articles;");
            strclass.AppendLine("using ManageSystem.Core;");
            strclass.AppendLine("using "+base.Tableinfo.EntityNameSpace+";");
            strclass.AppendLine("");
            strclass.AppendLine("namespace " + base.Tableinfo.ServicesNameSpace);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// 操作接口类 ，数据库表名：" + base.Tableinfo.Name + " ");
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "public  partial interface I" + base.Tableinfo.ServicesClassName + " : IBaseService<" + base.Tableinfo.EntityClassName + ">");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendLine("");
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");

            //将内容写入到文件
            FileUtil.WriteFile(filePath, strclass.ToString());
        }

        /// <summary>
        /// 创建继承接口的具体类
        /// </summary>
        private void Create()
        {
            string fileFloder = base.StudioFloder + base.Tableinfo.ServicesClassPath; //完整路径
            if (!Directory.Exists(fileFloder)) Directory.CreateDirectory(fileFloder);

            string filePath = fileFloder + "\\" + base.Tableinfo.ServicesClassName + ".cs"; //文件完整路径
            if (File.Exists(filePath)) File.Delete(filePath);

            //生成代码
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Linq;");
            strclass.AppendLine("using System.Text;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Collections.Generic;");
            strclass.AppendLine("using ManageSystem.Core.Data;");
            strclass.AppendLine("using ManageSystem.Core.Domain.Articles;");
            strclass.AppendLine("using " + base.Tableinfo.EntityNameSpace + ";");
            strclass.AppendLine("");
            strclass.AppendLine("namespace " + base.Tableinfo.ServicesNameSpace);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// 操作类 ，数据库表名：" + base.Tableinfo.Name + " ");
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "public partial class " + base.Tableinfo.ServicesClassName + " :  BaseService<" + base.Tableinfo.EntityClassName + ">, I"+base.Tableinfo.ServicesClassName+"");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "public " + base.Tableinfo.ServicesClassName + "(IRepository<" + base.Tableinfo.EntityClassName + "> repository): base(repository)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "");
            strclass.AppendSpaceLine(2, "}");
            strclass.AppendLine("");
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");

            //将内容写入到文件
            FileUtil.WriteFile(filePath, strclass.ToString());
        }

    }
}