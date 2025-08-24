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
    ///  生成注册AutoMapper代码
    /// </summary>
    public class AutoMapperBuilder : BuilderBase
    {

        public AutoMapperBuilder(TableInfoModel _tableinfo) : base(_tableinfo)
        {

        }

        /// <summary>
        /// 创建代码
        /// </summary>
        /// <returns></returns>
        public override void CreateCode()
        {
            //AutoMapperAdminRegistrar
            this.GetCode1();

            //MappingExtensions
            this.GetCode2();

        }


        private void GetCode1()
        {
            StreamReader sr = null;
            try
            {
                string fileFloder = base.StudioFloder + @"\Presentation\ManageSystem.Admin\Extensions"; //完整路径

                string filePath = fileFloder + "\\AutoMapperAdminRegistrar.cs"; //文件完整路径
                                                                                //生成代码
                StringPlus strclass = new StringPlus();
                strclass.AppendSpaceLine(2, "Mapper.CreateMap<" + this.Tableinfo.EntityClassName + ", " + this.Tableinfo.ModelClassName + ">(); ");
                strclass.AppendSpaceLine(2, "Mapper.CreateMap<" + this.Tableinfo.ModelClassName + ", " + this.Tableinfo.EntityClassName + ">(); ");
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
            finally
            {
                if (sr != null) sr.Close();
            }
        }

        private void GetCode2()
        {
            StreamReader sr = null;
            try
            {
                string fileFloder = base.StudioFloder + @"\Presentation\ManageSystem.Admin\Extensions"; //完整路径

                string filePath = fileFloder + "\\MappingExtensions.cs"; //文件完整路径

                //生成代码
                string e = this.Tableinfo.EntityClassName;
                string m = this.Tableinfo.ModelClassName;

                StringPlus strclass = new StringPlus();

                strclass.AppendSpaceLine(2, "#region "+this.Tableinfo.ShowName);
                strclass.AppendSpaceLine(2, "");
                strclass.AppendSpaceLine(2, "public static "+m+" ToModel(this "+e+" entity) ");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "return entity.MapTo<"+e+", "+m+">();");
                strclass.AppendSpaceLine(2, "}");
                strclass.AppendSpaceLine(2, "");
                strclass.AppendSpaceLine(2, "public static "+e+" ToEntity(this "+m+" model)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "return model.MapTo<"+m+", "+e+">();");
                strclass.AppendSpaceLine(2, "}");
                strclass.AppendSpaceLine(2, "");
                strclass.AppendSpaceLine(2, "public static " + e + " ToEntity(this " + m+" model, "+e+" destination)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "return model.MapTo(destination);");
                strclass.AppendSpaceLine(2, "}");
                strclass.AppendSpaceLine(2, "");
                strclass.AppendSpaceLine(2, "#endregion");

                strclass.AppendSpaceLine(2, "//自动生成代码标识位");
                strclass.AppendSpaceLine(2, "");

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
            finally
            {
                if (sr != null) sr.Close();
            }
        }
    }
}