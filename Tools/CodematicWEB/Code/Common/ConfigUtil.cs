using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Data.SqlClient;

namespace CodematicWEB.Common
{
    /// <summary>
    /// Sys 的摘要说明。
    /// </summary>
    public class ConfigUtil
    {
        public ConfigUtil()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }


        /// <summary>
        /// 根据key获取配置文件值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;

            return System.Configuration.ConfigurationManager.AppSettings[key];
        }


        public static string ConnStr
        {
            get
            {
                return ConfigurationSettings.AppSettings["strconn"];
            }
        }

        public static string WebTitle
        {
            get
            {
                return ConfigurationSettings.AppSettings["webTitle"];
            }
        }

        

        public static string StoreStr
        {
            get
            {
                return "?flag=" + HttpContext.Current.Request.QueryString["flag"];
            }
        }

        public static string StoreId
        {
            get
            {
                return HttpContext.Current.Request.QueryString["flag"];
            }
        }


        public static string Root
        {
            get
            {
                return ConfigurationSettings.AppSettings["Root"];
            }
        }


        public static string ImageServer
        {
            get
            {
                return ConfigurationSettings.AppSettings["Image_Server"];
            }
        }

        public static string ServerScoreImage
        {
            get
            {
                return ConfigurationSettings.AppSettings["Image_Server_Score"];
            }
        }


        /// <summary>
        /// 页面Title关键字
        /// </summary>
        public static string MetaTitle
        {
            get
            {
                return ConfigurationSettings.AppSettings["MetaTitle"].ToString();
            }
        }

        public static string CopyRight
        {
            get
            {
                return ConfigurationSettings.AppSettings["CopyRight"].ToString();
            }
        }

        public static string Mail_UserName
        {
            get
            {
                return ConfigurationSettings.AppSettings["Mail_UserName"].ToString();
            }
        }
        public static string Mail_PassWord
        {
            get
            {
                return ConfigurationSettings.AppSettings["Mail_PassWord"].ToString();
            }
        }
        public static string Mail_Server
        {
            get
            {
                return ConfigurationSettings.AppSettings["Mail_Server"].ToString();
            }
        }

        public static void SetCookie(string CookieName, string CookieValue)
        {
            HttpCookie myCookie = new HttpCookie(CookieName, CookieValue);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        public static string GetCookie(string CookieName)
        {
            return HttpContext.Current.Request.Cookies[CookieName].Value;
        }

        public static string LoginId
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session["LoginId"]);
            }
        }

        public static string RoleId
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session["Type"]);
            }
        }

        //对接礼品卡部分，可以使用的商品状态值，格式 例： 1,2,3
        public const string NORMAL_GOODS_STATE = "2";

    }
}
