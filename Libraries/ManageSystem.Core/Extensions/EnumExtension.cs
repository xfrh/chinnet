using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ManageSystem.Core.Extensions;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;

namespace ManageSystem.Core.Extensions
{
   public static class EnumExtension
    {
        /// <summary>
        /// 根据枚举转换成 SelectList ，用于绑定下拉列表
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumObj"></param>
        /// <param name="markCurrentAsSelected"></param>
        /// <param name="showAll"></param>
        /// <param name="valuesToExclude"></param>
        /// <returns></returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj,
            bool markCurrentAsSelected = true, bool showAll = true, int[] valuesToExclude = null) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("枚举类型不能为空", "enumObj");

            var values = from TEnum enumValue in System.Enum.GetValues(typeof(TEnum))
                         where valuesToExclude == null || !valuesToExclude.Contains(Convert.ToInt32(enumValue))
                         select new { ID = Convert.ToInt32(enumValue), Name = EnumExtension.GetDescription(enumValue) };

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in values)
                list.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });

            if (showAll)
                list.Insert(0, new SelectListItem() { Text = "全部", Value = "0" });

            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);
            return new SelectList(list, "Value", "Text", selectedValue);
        }



        public static ListItemCollection GetEnumDescriptions(object enumObject)
        {
            if (!enumObject.GetType().IsEnum)
            {
                throw new Exception("参数不是枚举对象");
            }

            ListItemCollection temStr = new ListItemCollection();
            Array enumArray = System.Enum.GetValues(enumObject.GetType());
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


        public static string GetDescription<T>(this T enumValue)
          where T : struct
        {
            return GetEnumDescription(enumValue);
        }


        /// <summary>
        /// 获取枚举的描述值
        /// </summary>
        /// <param name="enumItem"></param>
        /// <returns></returns>
        public static string GetEnumDescription(object enumItem)
        {
            string str = "";
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
            catch (Exception e) { }
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

        public static int GetIndex(List<string> collection, string value)
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
