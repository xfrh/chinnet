using CodematicWEB.Code.Model;
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
    public abstract class BuilderBase
    {
        /// <summary>
        /// 项目根目录绝对文件夹地址 ，格式：
        /// </summary>
        protected  string StudioFloder = AppDomain.CurrentDomain.BaseDirectory.Replace("\\Tools\\CodematicWEB", "").TrimEnd('\\');

        ///// <summary>
        ///// 系统字段，不在前台显示
        ///// </summary>
        //protected string[] SystemColumn = { "", "", "", "", "", "" };

        protected TableInfoModel Tableinfo;
        public BuilderBase(TableInfoModel _tableinfo)
        {
            this.Tableinfo = _tableinfo;
        }



        /// <summary>
        /// 创建代码
        /// </summary>
        /// <returns></returns>
        public abstract void CreateCode();

    }
}