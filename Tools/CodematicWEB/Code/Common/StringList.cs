using System;
using System.Collections;
using System.Collections.Generic;

namespace CodematicWEB.Common
{
    /// <summary>
    /// StringList 的摘要说明。
    /// </summary>
    public class StringList : ArrayList
    {
        public StringList()
        {
        }

        /// <summary>
        /// 剔除重复项
        /// </summary>
        /// <param name="oldFuncs"></param>
        public StringList(string oldFuncs)
        {
            if (oldFuncs.IndexOf(',') > 0)
            {
                string[] _funcs = oldFuncs.Split(',');
                foreach (string func in _funcs)
                {
                    if (!this.Contains(func))
                    {
                        this.Add(func);
                    }
                }
            }
            else
            {
                this.Add(oldFuncs);
            }
        }

        public string[] GetArray()
        {
            string[] Arr = new string[this.Count];
            this.CopyTo(Arr, 0);
            return Arr;
        }
        /// <summary>
        /// 获取逗号间隔字符串
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string[] Arr = GetArray();
            return String.Join(",", Arr);
        }
    }
}
