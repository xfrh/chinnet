using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;


namespace CodematicWEB.Common
{
    /// <summary>
    /// js页面弹出框和打开新窗口相关操作
    /// </summary>
    public class JSUtil
    {
        /// <summary>
        /// 弹出Alert窗口
        /// </summary>
        /// <param name="js">窗口信息</param>
        public static void Alert(string message, Page page)
        {
            string js = @"<script language='javascript'>
                    alert('" + message + "');</script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "alert"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "alert", js);
            }
        }

        public static void Alert(string message, string goUrl, Page page)
        {
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";

            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "alert"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "alert", string.Format(js, message, goUrl));
            }
        }

        /// <summary>
        /// 弹出提示框并跳转到制定页面
        /// </summary>
        /// <param name="message">弹出信息</param>
        /// <param name="goUrl">跳转Url地址</param>
        /// <param name="page"></param>
        public static void AlertAndRedirect(string message, string goUrl, Page page)
        {
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";

            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "AlertAndRedirect"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "AlertAndRedirect", string.Format(js, message, goUrl));
            }

        }


        /// <summary>
        /// 回到历史页面
        /// </summary>
        /// <param name="value">-1/1</param>    
        public static void History(int value, Page page)
        {
            string js = @"<script language='javascript'> history.go({0});  </script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "GoHistory"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "GoHistory", string.Format(js, value));
            }
        }

        /// <summary>
        /// 页面倒退一页
        /// </summary>
        /// <param name="page"></param>
        public static void HistoryBack(Page page)
        {
            History(-1, page);
        }

        /// <summary>
        /// 页面前进一页
        /// </summary>
        /// <param name="page"></param>
        public static void HistoryGo(Page page)
        {
            History(1, page);
        }


        /// <summary>
        /// 刷新父窗口
        /// </summary>
        /// <param name="url">父窗口地址</param>
        /// <param name="page"></param>
        public static void RefreshParent(string url, Page page)
        {

            string js = @"<script language='javascript'>
                    window.opener.location.href='" + url + "';window.close();</script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "RefreshParent"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "RefreshParent", js);
            }
        }

        /// <summary>
        /// 刷新当前页面
        /// </summary>  
        /// 
        public static void RefreshPage(Page page)
        {
            string js = @"<script language='javascript'>
                    opener.location.reload();
                  </script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "RefreshPage"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "RefreshPage", js);
            }
        }



        /// <summary>
        /// 打开指定大小的新窗体
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="width">宽</param>
        /// <param name="heigth">高</param>
        /// <param name="top">头位置</param>
        /// <param name="left">左位置</param>  
        public static void OpenWebFormSize(string url, int width, int heigth, int top, int left, Page page)
        {
            string js = @"<script language='javascript'>window.open('" + url + @"','','height=" + heigth + ",width=" + width + ",top=" + top + ",left=" + left + ",location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');</script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "OpenWebFormSize"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "OpenWebFormSize", js);
            }
        }


        /// <summary>
        /// 跳转到指定页面
        /// </summary>
        /// <param name="url">跳转到那里地址</param> 
        public static void LocationHref(string url, Page page)
        {
            string js = string.Format(@"<script language='javascript'>  window.location.href='{0}'; </script>", url);
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "LocationHref"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "LocationHref", js);
            }
        }

        /// <summary>
        /// 打开指定大小位置的模式对话框
        /// </summary>
        /// <param name="webFormUrl">连接地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="top">距离上位置</param>
        /// <param name="left">距离左位置</param> 
        /// 
        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left, Page page)
        {
            string features = "dialogWidth:" + width.ToString() + "px"
                + ";dialogHeight:" + height.ToString() + "px"
                + ";dialogLeft:" + left.ToString() + "px"
                + ";dialogTop:" + top.ToString() + "px"
                + ";center:yes;help=no;resizable:no;status:no;scroll=yes";
            ShowModalDialogWindow(webFormUrl, features, page);
        }
        /// <summary>
        /// 弹出模态窗口
        /// </summary>
        /// <param name="webFormUrl"></param>
        /// <param name="features"></param>   
        public static void ShowModalDialogWindow(string webFormUrl, string features, Page page)
        {
            string js = ShowModalDialogJavascript(webFormUrl, features);
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "ShowModalDialogWindow"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "ShowModalDialogWindow", js);
            }
        }


        /// <summary>
        /// 弹出模态窗口
        /// </summary>
        /// <param name="webFormUrl"></param>
        /// <param name="features"></param>
        /// <returns></returns>    
        public static string ShowModalDialogJavascript(string webFormUrl, string features)
        {
            string js = @"<script language=javascript>                            
        showModalDialog('" + webFormUrl + "','','" + features + "');</script>";
            return js;

        }

        /// <summary>
        /// 打开指定大小的新窗体
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="width">宽</param>
        /// <param name="heigth">高</param>
        /// <param name="top">头位置</param>
        /// <param name="left">左位置</param>  
        public static void OpenWebFormSize2(string url, string formName, int width, int heigth, int top, int left, Page page)
        {
            string js = @"<script language='javascript'>var tempwindow=window.open('_blank','" + formName + "','height=" + heigth + ",width=" + width + ",top=" + top + ",left=" + left + ",location=yes,menubar=yes,resizable=yes,scrollbars=yes,status=yes,titlebar=yes,toolbar=yes,directories=yes');tempwindow.location='" + url + "';</script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "OpenWebFormSize"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "OpenWebFormSize", js);
            }
        }


    }
}
