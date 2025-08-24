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
    public class ValidatorBuilder : BuilderBase
    {

        /// <summary>
        /// 生成验证类的文件
        /// </summary>
        /// <param name="_tableinfo"></param>
        public ValidatorBuilder(TableInfoModel _tableinfo) : base(_tableinfo)
        {

        }

        /// <summary>
        /// 创建代码
        /// </summary>
        /// <returns></returns>
        public override void CreateCode()
        {
            string fileFloder = base.StudioFloder + base.Tableinfo.ValidatorClassPath; //完整路径
            if (!Directory.Exists(fileFloder)) Directory.CreateDirectory(fileFloder);

            string filePath = fileFloder +"\\"+ base.Tableinfo.ValidatorClassName + ".cs"; //文件完整路径
            if (File.Exists(filePath)) File.Delete(filePath);

            //生成代码
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using FluentValidation;");
            strclass.AppendLine("using "+base.Tableinfo.ModelNameSpace+";");
            strclass.AppendLine("using ManageSystem.Framework.Validators;");
            strclass.AppendLine("");
            strclass.AppendLine("namespace " + base.Tableinfo.ValidatorNameSpace+"");
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// 数据验证类 ，数据库表名：" + base.Tableinfo.Name + " ");
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "public partial class " + base.Tableinfo.ValidatorClassName + " : BaseValidator<"+base.Tableinfo.ModelClassName+">");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "public " + base.Tableinfo.ValidatorClassName + "()");
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

            foreach (var item in base.Tableinfo.ColumnInfoList)
            {
                if (!item.IsValidator) continue;
                if (!string.IsNullOrWhiteSpace(item.ValidateEmpty))
                    //非空验证
                    strclass.AppendSpaceLine(3, "RuleFor(x => x." + item.ColumnName + ").NotEmpty().WithMessage(\"" + item.ValidateEmpty+"\");");

                if (!string.IsNullOrWhiteSpace(item.ValidateOther))
                {
                    //其他验证，根据数据类型来操作
                    switch (item.DataType.ToLower())
                    {
                        case "int":
                        case "float":
                        case "double":
                        case "decimal":
                            strclass.AppendSpaceLine(3, "RuleFor(x => x." + item.ColumnName + ").NotEqual(0).WithMessage(\"" + item.ValidateOther + "\");");
                            break;
                        case "datetime":
                            break;
                        case "string":
                            if (!string.IsNullOrWhiteSpace(item.ValidateOtherValue) && item.ValidateOtherValue.Split('-').Count()>1)
                            {
                                var arrayTemp = item.ValidateOtherValue.Split('-');
                                strclass.AppendSpaceLine(3, "RuleFor(x => x." + item.ColumnName + ").Length("+ arrayTemp[0]+ ", " + arrayTemp[1] + ").WithMessage(\"" + item.ValidateOther + "\");");
                            }
                            break;
                    }
                }
            }

            return strclass.ToString();

        }
    }
}