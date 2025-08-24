using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ManageSystem.Core.Utility
{
    /// <summary>
    /// 
    /// 使用Log4net 来记录文件日志，
    /// 需要配置 log4net.config，
    /// 注意日志文件的存放目录，目前为： 网站根目录/App_Data/Log/
    /// 
    /// </summary>
    public class Log4Helper
    {

        #region  错误日志

        /// <summary>
        /// 错误日志，根据类型和日志类容记录
        /// </summary>
        /// <param name="type">类型， 类使用：typeof(类名) 获取，函数使用：MethodBase.GetCurrentMethod().GetType() 获取</param>
        /// <param name="message">日志内容</param>
        public static void Error(Type type, string message)
        {
            Log4Helper.Create(type, message, Log4netLevel.Error);
        }

        /// <summary>
        ///错误日志，根据类型和日志类容记录  
        /// </summary>
        /// <param name="type">类型， 类使用：typeof(类名) 获取，函数使用：MethodBase.GetCurrentMethod().GetType() 获取</param>
        /// <param name="ex">异常对象</param>
        public static void Error(Type type, Exception ex)
        {
            Log4Helper.Create(type, ex.ToString(), Log4netLevel.Error);
        }

        /// <summary>
        /// 错误日志，根据类型和日志类容记录
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Error(string message)
        {
            Log4Helper.Create(MethodBase.GetCurrentMethod().GetType(), message, Log4netLevel.Error);
        }

        #endregion

        #region  警告日志

        /// <summary>
        /// 警告日志，根据类型和日志类容记录
        /// </summary>
        /// <param name="type">类型， 类使用：typeof(类名) 获取，函数使用：MethodBase.GetCurrentMethod().GetType() 获取</param>
        /// <param name="message">日志内容</param>
        public static void Warn(Type type, string message)
        {
            Log4Helper.Create(type, message, Log4netLevel.Warn);
        }

        /// <summary>
        ///警告日志，根据类型和日志类容记录  
        /// </summary>
        /// <param name="type">类型， 类使用：typeof(类名) 获取，函数使用：MethodBase.GetCurrentMethod().GetType() 获取</param>
        /// <param name="ex">异常对象</param>
        public static void Warn(Type type, Exception ex)
        {
            Log4Helper.Create(type, ex.ToString(), Log4netLevel.Warn);
        }

        /// <summary>
        /// 警告日志，根据类型和日志类容记录
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Warn(string message)
        {
            Log4Helper.Create(MethodBase.GetCurrentMethod().GetType(), message, Log4netLevel.Warn);
        }

        #endregion

        #region  信息日志

        /// <summary>
        /// 信息日志，根据类型和日志类容记录
        /// </summary>
        /// <param name="type">类型， 类使用：typeof(类名) 获取，函数使用：MethodBase.GetCurrentMethod().GetType() 获取</param>
        /// <param name="message">日志内容</param>
        public static void Info(Type type, string message)
        {
            Log4Helper.Create(type, message, Log4netLevel.Info);
        }

        /// <summary>
        ///信息日志，根据类型和日志类容记录  
        /// </summary>
        /// <param name="type">类型， 类使用：typeof(类名) 获取，函数使用：MethodBase.GetCurrentMethod().GetType() 获取</param>
        /// <param name="ex">异常对象</param>
        public static void Info(Type type, Exception ex)
        {
            Log4Helper.Create(type, ex.ToString(), Log4netLevel.Info);
        }

        /// <summary>
        /// 信息日志，根据类型和日志类容记录
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(string message)
        {
            Log4Helper.Create(MethodBase.GetCurrentMethod().GetType(), message, Log4netLevel.Info);
        }

        #endregion

        #region  Debug日志

        /// <summary>
        /// Debug日志，根据类型和日志类容记录
        /// </summary>
        /// <param name="type">类型， 类使用：typeof(类名) 获取，函数使用：MethodBase.GetCurrentMethod().GetType() 获取</param>
        /// <param name="message">日志内容</param>
        public static void Debug(Type type, string message)
        {
            Log4Helper.Create(type, message, Log4netLevel.Debug);
        }

        /// <summary>
        ///Debug日志，根据类型和日志类容记录  
        /// </summary>
        /// <param name="type">类型， 类使用：typeof(类名) 获取，函数使用：MethodBase.GetCurrentMethod().GetType() 获取</param>
        /// <param name="ex">异常对象</param>
        public static void Debug(Type type, Exception ex)
        {
            Log4Helper.Create(type, ex.ToString(), Log4netLevel.Debug);
        }

        /// <summary>
        /// Debug日志，根据类型和日志类容记录
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Debug(string message)
        {
            Log4Helper.Create(MethodBase.GetCurrentMethod().GetType(), message, Log4netLevel.Debug);
        }

        #endregion


        /// <summary>
        /// 记录日志完整函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public static void Create(Type type, string message, Log4netLevel level)
        {
            ILog log = log4net.LogManager.GetLogger(type);
            switch (level)
            {
                case Log4netLevel.Error:
                    log.Error(message);
                    break;
                case Log4netLevel.Warn:
                    log.Warn(message);
                    break;
                case Log4netLevel.Info:
                    log.Info(message);
                    break;
                case Log4netLevel.Debug:
                    log.Debug(message);
                    break;
            }
        }


    }


    /// <summary>
    /// Log4net  日志等级
    /// </summary>
    public enum Log4netLevel
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error = 1,

        /// <summary>
        /// 警告
        /// </summary>
        Warn = 2,

        /// <summary>
        /// 信息
        /// </summary>
        Info = 3,

        /// <summary>
        /// Debug
        /// </summary>
        Debug = 4

    }

}