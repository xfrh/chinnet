using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Services.Common;
using ManageSystem.Core.Infrastructure;

namespace ManageSystem.Services.Members
{
    /// <summary>
    /// 操作类 ，数据库表名：Member 
    /// </summary>
    public static partial class MemberExtensions
    {


        /// <summary>
        /// 获取用户头像图片的完整路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetHeadImage(string path)
        {
            if (!string.IsNullOrWhiteSpace(path)) return path;

            return @"/Content/Upload/Member/default.jpg";
        }


        /// <summary>
        /// 系统产生用户的邀请码
        /// </summary>
        /// <returns></returns>
        public static string GetInviteCode()
        {
            return Guid.NewGuid().ToString("N").ToLower();
        }


    }
}
