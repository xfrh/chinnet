using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodematicWEB.Common;

namespace CodematicWEB.Code.Model
{

    /// <summary>
    /// 公用和静态变量存储类
    /// </summary>
    public class PublicCode
    {

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string SqlConnectionString = ConfigUtil.GetValueByKey("strconn");

        /// <summary>
        /// 实体类（Entity）中类名的追加名称
        /// </summary>
        public static string EntityClassNameExt = "";

        /// <summary>
        /// 业务逻辑处理（Serve）中类名的追加名称
        /// </summary>
        public static string ServeClassNameExt = "Serve";

        /// <summary>
        /// 生成的JS文件的追加名称
        /// </summary>
        public static string JSFileNameExt = "";

        /// <summary>
        /// JS验证代码中 form表单的验证名称，母版页的话这里注意修改
        /// </summary>
        public static string JSFormName = "#form1";

        /// <summary>
        /// JS验证代码中控件名称的前面追加， 例：如果是母版页前面加
        /// </summary>
        public static string JSControlNameExt = "ctl00$ContentPlaceHolder1$";
        
         /// <summary>
        /// 页面生成的模版文件文件夹
        /// </summary>
        public static string PageTemplatePath = "~/Template/Page/";

       

    }
}