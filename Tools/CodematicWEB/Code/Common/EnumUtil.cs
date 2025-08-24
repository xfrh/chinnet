using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;
using System.Xml;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;

namespace CodematicWEB.Common
{
    public class EnumUtil
    {
        static string basicDir = AppDomain.CurrentDomain.BaseDirectory;

        public static ListItemCollection GetEnumDescriptions(object enumObject)
        {
            if (!enumObject.GetType().IsEnum)
            {
                throw new Exception("参数不是枚举对象");
            }
            ListItemCollection temStr = new ListItemCollection();
            Array enumArray = Enum.GetValues(enumObject.GetType());
            foreach (var ee in enumArray)
            {
                FieldInfo enumInfo = ee.GetType().GetField(ee.ToString());
                DescriptionAttribute[] des = (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                ListItem item = new ListItem();
                if (des != null && des.Length > 0)
                {
                    item.Text = des[0].Description;
                    item.Value = ((int)ee).ToString();
                }
                else
                {
                    item.Text = ee.ToString();
                    item.Value = ((int)ee).ToString();
                }
                temStr.Add(item);
            }
            return temStr;
        }


        public static List<string> GetProviderDistrict()
        {
            List<string> tempStr = new List<string>();
            if (!File.Exists(basicDir + "/basicRes/ProviderDistrict.xml"))
            {
                throw new Exception("文件不存在,ProviderDistrict.xml");
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(basicDir + "/basicRes/ProviderDistrict.xml");
            XmlElement root = doc.DocumentElement;
            foreach (XmlNode ee in root.ChildNodes)
            {
                tempStr.Add(ee.InnerText);
            }
            return tempStr;
        }



        public static string GetEnumDescription(object enumItem)
        {
            string str = string.Empty;
            try
            {
                DescriptionAttribute[] des = (DescriptionAttribute[])enumItem.GetType().GetField(enumItem.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (des != null && des.Count() > 0)
                {
                    str = des[0].Description;
                }
                else
                {
                    str = enumItem.ToString();
                }
            }
            catch {}
            return str;
        }

        public static int GetIndex(ListItemCollection collection, string value)
        {
            int result = 0;
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].Value == value)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        public static  int GetIndex(List<string> collection, string value)
        {
            int result = 0;
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i] == value)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        public static int GetIndex(DataTable table, string key)
        {
            int result = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["id"].ToString() == key)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
    }
}
