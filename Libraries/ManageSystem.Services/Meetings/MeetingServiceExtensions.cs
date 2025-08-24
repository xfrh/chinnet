using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Core;
using ManageSystem.Core.Utility;

namespace ManageSystem.Services.Meetings
{
    /// <summary>
    /// 操作类 ，数据库表名：Meeting 
    /// </summary>
    public  static class MeetingServiceExtensions 
    {
        /// <summary>
        /// 获取文章封面图片的完整路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetCoverImage(string path)
        {
            if (!string.IsNullOrWhiteSpace(path)) return path;

            return @"\Content\Upload\Meetings\default.jpg";
        }

        /// <summary>
        /// 根据日期转成中文周
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetWeekName(DateTime time)
        {
            return DateHelper.GetWeekName(time);
        }
    }
}
