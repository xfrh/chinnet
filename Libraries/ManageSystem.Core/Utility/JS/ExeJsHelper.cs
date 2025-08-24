using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;

namespace ManageSystem.Core.Utility.JS
{
    /// <summary>
    /// 用于执行JS的类
    /// </summary>
    public class ExeJsHelper
    {

      
        ///// <summary>
        ///// 密码加密
        ///// </summary>
        ///// <param name="pass"></param>
        ///// <returns></returns>
        //public string EncodePass(string pass)
        //{
        //    ScriptControlClass sc = new ScriptControlClass();
        //    sc.UseSafeSubset = true;
        //    sc.Language = "JScript";
        //    sc.AddCode(Properties.Resources.QQRsa);  //从资源中读取js内容,也可以写成Js文件神马的.
        //    string str = sc.Run("rsaEncrypt", new object[] { pass }).ToString();
        //    return str;
        //}
    }
}
