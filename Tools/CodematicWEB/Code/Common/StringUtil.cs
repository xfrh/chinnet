//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Hairihan TECH, Ltd. 
//-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;


namespace CodematicWEB.Common
{
    public class StringUtil
    {
        /// <summary>
        /// 获取子查询条件，这需要处理多个模糊匹配的字符
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="search">模糊查询</param>
        /// <returns>表达式</returns>
        public static string GetLike(string field, string search)
        {
            string returnValue = string.Empty;
            for (int i = 0; i < search.Length; i++)
            {
                returnValue += field + " LIKE '%" + search[i] + "%' AND ";
            }
            if (!string.IsNullOrEmpty(returnValue))
            {
                returnValue = returnValue.Substring(0, returnValue.Length - 5);
            }
            returnValue = "(" + returnValue + ")";
            return returnValue;
        }

     
        
     
        /// <summary>
        /// 判断字符串数组是否包含制定的值
        /// </summary>
        /// <param name="ids">数组</param>
        /// <param name="targetString">目标值</param>
        /// <returns>包含</returns>
        public static bool Exists(string[] ids, string targetString)
        {
            bool returnValue = false;
            if (ids != null && !string.IsNullOrEmpty(targetString))
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i].Equals(targetString))
                    {
                        returnValue = true;
                        break;
                    }
                }
            }
            return returnValue;
        }
     

        public static string[] Concat(string[] ids, string id)
        {
            return Concat(ids, new string[] { id });
        }

        
        /// <summary>
        /// 合并数组
        /// </summary>
        /// <param name="ids">数组</param>
        /// <returns>数组</returns>
        public static string[] Concat(params string[][] ids)
        {
            // 进行合并
            Hashtable hashValues = new Hashtable();
            if (ids != null)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i] != null)
                    {
                        for (int j = 0; j < ids[i].Length; j++)
                        {
                            if (ids[i][j] != null)
                            {
                                if (!hashValues.ContainsKey(ids[i][j]))
                                {
                                    hashValues.Add(ids[i][j], ids[i][j]);
                                }
                            }
                        }
                    }
                }
            }
            // 返回合并结果
            string[] returnValues = new string[hashValues.Count];
            IDictionaryEnumerator enumerator = hashValues.GetEnumerator();
            int key = 0;
            while (enumerator.MoveNext())
            {
                returnValues[key] = (string)(enumerator.Key.ToString());
                key++;
            }
            return returnValues;
        }
    
        /// <summary>
        /// 从目标数组中去除某个值
        /// </summary>
        /// <param name="ids">数组</param>
        /// <param name="id">目标值</param>
        /// <returns>数组</returns>
        public static string[] Remove(string[] ids, string id)
        {
            // 进行合并
            Hashtable hashValues = new Hashtable();
            if (ids != null)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i] != null && (!ids[i].Equals(id)))
                    {
                        if (!hashValues.ContainsKey(ids[i]))
                        {
                            hashValues.Add(ids[i], ids[i]);
                        }
                    }
                }
            }
            // 返回合并结果
            string[] returnValues = new string[hashValues.Count];
            IDictionaryEnumerator enumerator = hashValues.GetEnumerator();
            int key = 0;
            while (enumerator.MoveNext())
            {
                returnValues[key] = (string)(enumerator.Key.ToString());
                key++;
            }
            return returnValues;
        }
    


        /// <summary>
        /// 将传入的字符串数组转换成字符串
        /// 返回结果使用英文都好分割，例：1,3,4
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string ArrayToList(string[] ids)
        {
            return ArrayToList(ids, string.Empty);
        }

        public static string ArrayToList(string[] ids, string separativeSign)
        {
            int rowCount = 0;
            string returnValue = string.Empty;
            foreach (string id in ids)
            {
                rowCount++;
                returnValue += separativeSign + id + separativeSign + ",";
            }
            if (rowCount == 0)
            {
                returnValue = "";
            }
            else
            {
                returnValue = returnValue.TrimEnd(',');
            }
            return returnValue;
        }

        /// <summary>
        /// 重复字符串
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <param name="repeatCount">重复次数</param>
        /// <returns>结果字符串</returns>
        public static string RepeatString(string targetString, int repeatCount)
        {
            string returnValue = string.Empty;
            for (int i = 0; i < repeatCount; i++)
            {
                returnValue += targetString;
            }
            return returnValue;
        }

        /// <summary>
        /// 处理字符串最后一个字符 逗号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DeleteLastChar(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 1) return value;

            string temp = value.Substring(value.Length - 1, 1);
            if (temp.Equals(","))
            {
                return value.Substring(0, value.Length - 1);
            }

            return value;
        }


        /// <summary>
        /// 处理字符串第一位字符 逗号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DeleteFestChar(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 1) return value;

            string temp = value.Substring(0, 1);
            if (temp.Equals(","))
            {
                return value.Substring(1, value.Length - 1);
            }

            return value;
        }

  
        
        /// <summary>
        /// 删除不可见字符
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string DeleteUnVisibleChar(string sourceString)
        {
            var sBuilder = new StringBuilder(131);
            for (int i = 0; i < sourceString.Length; i++)
            {
                int Unicode = sourceString[i];
                if (Unicode >= 16)
                {
                    sBuilder.Append(sourceString[i].ToString());
                }
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 字符串剪切
        /// </summary>
        /// <param name="obj">字符串对象</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutString(object obj, int len)
        {
            if (null == obj) return "";
            char[] source = obj.ToString().ToCharArray();

            if (source.Length + System.Text.RegularExpressions.Regex.Matches(obj.ToString(), "[\u0080-\uffff]").Count <= len) return obj.ToString();

            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] > 127)
                    count++;
                count++;
                if (count >= len)
                {
                    string result = "";
                    for (int n = 0; n <= i; n++)
                    {
                        result += source[n];
                    }
                    return result;
                }
            }
            return obj.ToString();
        }



        /// <summary>
        /// 字符串首字母大写
        /// </summary>
        /// <param name="value">需要操作的值</param>
        /// <returns></returns>
        public static string ToTitleUpper(string value)
        {
            return value.Substring(0, 1).ToUpper() + value.Substring(1);
        }


        /// <summary>
        /// 字符串首字母小写
        /// </summary>
        /// <param name="value">需要操作的值</param>
        /// <returns></returns>
        public static string ToTitleLower(string value)
        {
            return value.Substring(0, 1).ToLower() + value.Substring(1);
        }


    }
}