using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using CodematicWEB.Common;


namespace CodematicCodematicWEB.Common
{

    /// <summary>
    /// 对 asp.net 服务器控件 进行操作
    /// </summary>
    public class WebUtil
    {

        #region 获取相关数据

        /// <summary>
        /// 获取repeater控件选中id，
        /// 注意：  checkboxId 可为空，默认chkBox
        /// </summary>
        /// <param name="repeater"></param>
        /// <param name="checkboxId">CheckBox id</param>
        /// <returns></returns>
        public static string GetRepeatSelectId(Repeater repeater)
        {
            return GetRepeatSelectId(repeater, "chkBox");
        }


        /// <summary>
        /// 获取repeater控件选中id，
        /// 注意：  checkboxId 可为空，默认chkBox
        /// </summary>
        /// <param name="repeater"></param>
        /// <param name="checkboxId">CheckBox id</param>
        /// <returns></returns>
        public static string GetRepeatSelectId(Repeater repeater, string checkboxId)
        {
            string value = "";
            if (string.IsNullOrEmpty(checkboxId)) checkboxId = "chkBox";

            try
            {
                StringList sl = new StringList();
                foreach (RepeaterItem rItem in repeater.Items)
                {
                    HtmlInputCheckBox chk = rItem.FindControl(checkboxId) as HtmlInputCheckBox;
                    if (chk == null) break;

                    if (chk.Checked)
                    {
                        sl.Add(chk.Value);
                    }
                }
                if (sl.Count > 0)
                {
                    value = sl.GetString();
                }
            }
            catch (SystemException ex)
            {
                throw ex;
            }

            if (!string.IsNullOrEmpty(value)) value = StringUtil.DeleteLastChar(value);

            return value;
        }


        #endregion


        #region 使用枚举来绑定下拉列表框

        /// <summary>
        /// 使用枚举来绑定下拉列表框
        /// </summary>
        /// <param name="ddl">DropDownList 控件</param>
        /// <param name="enumObj">枚举</param>
        /// <param name="topText">放在第一行的 显示文本</param>
        /// <param name="topValue">放在第一行的 显示值</param>
        public static void BindDropDownListByEnum(DropDownList ddl, object enumObj, string topText, string topValue)
        {
            ListItemCollection list = EnumUtil.GetEnumDescriptions(enumObj);
            BindData(ddl, list, "text", "value", topText, topValue);

        }

        /// <summary>
        /// 使用枚举来绑定下拉列表框
        /// </summary>
        /// <param name="ddl">DropDownList 控件</param>
        /// <param name="enumObj">枚举</param>
        /// <param name="topText">放在第一行的 显示文本</param>
        /// <param name="topValue">放在第一行的 显示值</param>
        public static void BindDropDownListByEnum(DropDownList ddl, object enumObj)
        {
            ListItemCollection list = EnumUtil.GetEnumDescriptions(enumObj);
            BindData(ddl, list, "text", "value");

        }

        /// <summary>
        /// 使用枚举来绑定下拉列表框
        /// </summary>
        /// <param name="ddl">DropDownList 控件</param>
        /// <param name="enumObj">枚举</param>
        public static void BindDataByEnum(DropDownList ddl, object enumObj)
        {
            ListItemCollection list = EnumUtil.GetEnumDescriptions(enumObj);
            BindData(ddl, list, "text", "value");
        }

        #endregion


        #region web 服务器控件绑定数据


        public static void BindData(object control, string sql, string text, string value, string tipText, string tipValue)
        {
            DataTable table = DBClass.GetDataTable(sql);
            BindData(control, table, text, value, tipText, tipValue);
        }

        public static void BindData(object control, string sql, string text, string value)
        {
            DataTable table = DBClass.GetDataTable(sql);
            BindData(control, table, text, value, null, null);
        }

        public static void BindData(object control, string sql)
        {
            DataTable table = DBClass.GetDataTable(sql);
            BindData(control, table, null, null, null, null);
        }

        public static void BindData(object control, object source, string text, string value)
        {
            BindData(control, source, text, value, null, null);
        }

        public static void BindData(object control, object source)
        {

            BindData(control, source, null, null, null, null);
        }

        /// <summary>
        /// 绑定数据 支持控件类型：DataGrid、DataList、Repeater、DropDownList、RadioButtonList、CheckBoxList、
        /// </summary>
        /// <param name="control">控件对象</param>
        /// <param name="source">数据源 支持类型：List、DataTable</param>
        /// <param name="text">用于显示的值，注意部分控件不支持</param>
        /// <param name="value">用于绑定的值，注意部分控件不支持</param>
        /// <param name="tipText">用于显示在首行的值，注意部分控件不支持</param>
        /// <param name="tipValue">用于绑定在首行的值，注意部分控件不支持</param>
        public static void BindData(object control, object source, string text, string value, string tipText, string tipValue)
        {

            if (control == null || source == null) return;

            if (control is DataGrid)
            {
                DataGrid dg = (DataGrid)control;
                dg.DataSource = source;
                dg.DataBind();
            }
            if (control is DataList)
            {
                DataList dl = (DataList)control;
                dl.DataSource = source;
                dl.DataBind();
            }
            if (control is Repeater)
            {
                Repeater rp = (Repeater)control;
                rp.DataSource = source;
                rp.DataBind();
            }

            if (control is DropDownList)
            {
                DropDownList ddl = (DropDownList)control;
                ddl.DataSource = source;
                ddl.DataTextField = text;
                ddl.DataValueField = value;
                ddl.DataBind();
                if (!string.IsNullOrEmpty(tipText) || !string.IsNullOrEmpty(tipValue))
                    ddl.Items.Insert(0, new ListItem(tipText, tipValue));
            }

            if (control is RadioButtonList)
            {
                RadioButtonList rbl = (RadioButtonList)control;
                rbl.DataSource = source;
                rbl.DataTextField = text;
                rbl.DataValueField = value;
                rbl.DataBind();
                if (!string.IsNullOrEmpty(tipText) || !string.IsNullOrEmpty(tipValue))
                    rbl.Items.Insert(0, new ListItem(tipText, tipValue));
            }

            if (control is CheckBoxList)
            {
                CheckBoxList cbl = (CheckBoxList)control;
                cbl.DataSource = source;
                cbl.DataTextField = text;
                cbl.DataValueField = value;
                cbl.DataBind();
                if (!string.IsNullOrEmpty(tipText) || !string.IsNullOrEmpty(tipValue))
                    cbl.Items.Insert(0, new ListItem(tipText, tipValue));

            }

        }

        #endregion



        #region
        public static string Form(string key)
        {
            return HttpContext.Current.Request.Form[key];
        }

        public static string Query(string key)
        {
            if (HttpContext.Current.Request.QueryString[key] == null)
                return "";
            else
                return WebUtil.SafeData( HttpContext.Current.Request.QueryString[key].ToString());
        }

        public static string Query2(string key)
        {
            if (HttpContext.Current.Request.QueryString[key] == null)
                return "";
            else
                return HttpContext.Current.Request.QueryString[key].ToString();
        }


        /// <summary>
        /// 过滤危险字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SafeData(string str)//　数据过滤
        {
            string tempStr = str;
            if (tempStr != null)
            {
                tempStr = tempStr.Replace("<", "");
                tempStr = tempStr.Replace(">", "");
                tempStr = tempStr.Replace("'", "‘");
                tempStr = tempStr.Replace("\"", "");
                tempStr = tempStr.Replace("#", "＃");
                tempStr = tempStr.Replace(",", "，");
                tempStr = tempStr.Replace(";", "；");
            }
            return tempStr;
        }

        /// <summary>
        /// 过滤危险字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SafeData5(string str)//　数据过滤
        {
            string tempStr = str;
            if (tempStr != null)
            {
                tempStr = tempStr.Replace("eval", "").Replace("EVAL", "").Replace("Eval", "");
                tempStr = tempStr.Replace("exe", "");
                tempStr = tempStr.Replace("script", "").Replace("SCRIPT", "").Replace("Script", "");//有空在此将script标签所有内容过滤
                tempStr = tempStr.Replace("<", "");
                tempStr = tempStr.Replace(">", "");
                tempStr = tempStr.Replace("'", "‘");
                tempStr = tempStr.Replace("\"", "");
                tempStr = tempStr.Replace("#", "＃");
                tempStr = tempStr.Replace(",", "，");
                tempStr = tempStr.Replace(";", "；");
            }
            return tempStr;
        }

        public static string SafeDataHTML1(string str)//　数据过滤
        {
            string tempStr = str;
            if (tempStr != null)
            {
                tempStr = tempStr.Replace("<", "嘦");
                tempStr = tempStr.Replace(">", "㊣");
            }
            return tempStr;
        }
        public static string SafeDataHTML3(string str)//　数据过滤
        {
            string tempStr = str;
            if (tempStr != null)
            {
                tempStr = tempStr.Replace("'", "‘");
                tempStr = tempStr.Replace("\"", "");
                tempStr = tempStr.Replace("#", "＃");
                tempStr = tempStr.Replace(",", "，");
            }
            return tempStr;
        }

        public static string SafeDataHTML2(string str)//　数据过滤
        {
            string tempStr = str;
            if (tempStr != null)
            {
                tempStr = tempStr.Replace("嘦", "<");
                tempStr = tempStr.Replace("㊣", ">");
            }
            return tempStr;
        }
        #endregion

     
        
        
    }




}