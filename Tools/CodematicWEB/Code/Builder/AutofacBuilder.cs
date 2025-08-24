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
    ///  生成注册Autofac代码
    /// </summary>
    public class AutofacBuilder : BuilderBase
    {

        public AutofacBuilder(TableInfoModel _tableinfo) : base(_tableinfo)
        {

        }

        /// <summary>
        /// 创建代码
        /// </summary>
        /// <returns></returns>
        public override void CreateCode()
        {
            StreamReader sr = null;
            try
            {
                string fileFloder = base.StudioFloder + @"\Presentation\ManageSystem.Admin\\Extensions"; //完整路径

                string filePath = fileFloder + "\\DependencyRegistrar.cs"; //文件完整路径
                 //生成代码
                StringPlus strclass = new StringPlus();
                strclass.AppendSpaceLine(2, "builder.RegisterType<" + this.Tableinfo.ServicesClassName + ">().As<I" + this.Tableinfo.ServicesClassName + ">().InstancePerLifetimeScope();");
                strclass.AppendSpaceLine(2, "//自动生成代码标识位");

                //读取文件，然后替换内容
                sr = new StreamReader(filePath);
                string value = sr.ReadToEnd();
                sr.Close();

                value = value.Replace(@"//自动生成代码标识位", strclass.ToString());
               
                //将内容写入到文件
                FileUtil.WriteFile(filePath, value);

            }
            catch (Exception)
            {

                throw;
            }
            finally {
                if (sr != null) sr.Close();
            }
        }

    }
}