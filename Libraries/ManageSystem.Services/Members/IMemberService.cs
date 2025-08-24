using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Domain.Members;

namespace ManageSystem.Services.Members
{
    /// <summary>
    /// 操作接口类 ，数据库表名：Member 
    /// </summary>
    public partial interface IMemberService : IBaseService<Member>
    {



        /// <summary>
        /// 根据登录帐号获取一个用户
        /// </summary>
        /// <param name="loginId">登录帐号</param>
        /// <returns></returns>
        Member QueryModelByLoginId(string loginId);

        /// <summary>
        /// 根据手机号码获取一个用户
        /// </summary>
        /// <param name="loginId">手机号码</param>
        /// <returns></returns>
        Member QueryModelByPhone(string phone);

        /// <summary>
        /// 根据邮箱号获取一个用户
        /// </summary>
        /// <param name="email">手机号码</param>
        /// <returns></returns>
        Member QueryModelByEmail(string email);


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nickName"></param>
        /// <param name="phone"></param>
        /// <param name="state"></param>
        /// <param name="type"></param>
        /// <param name="hospital"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Member> QueryPage(string name, string nickName,string LoginId, string phone, int state, int type, string hospital, int pageIndex = 0, int pageSize = int.MaxValue);

        IPagedList<Member> QueryPage(int ProjectType, string name, string nickName, string LoginId, string phone, int state, int type, string hospital, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="state"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Member> QueryPage(string name, string phone, int? state, List<long> hospital, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 检查OpendId是否已经存在，保证唯一
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        bool CheckOpenId(string openId);

        /// <summary>
        /// 通过Excel文件导入会员
        /// </summary>
        /// <param name="fielPath">文件的绝对路径</param>
        /// <returns></returns>
        string ImportExcel(string filePath, ref int successCount);


        /// <summary>
        /// 会员中心 分页查询  邀请会员，根据指定的邀请码查询被邀请的会员  
        /// </summary>
        /// <param name="inviteCode">会员邀请码</param>
        /// <returns></returns>
        IQueryable<Member> QueryByInviteCode(string inviteCode);

        /// <summary>
        /// 用户注册，底层函数
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="password">密码</param>
        /// <param name="password2">确认密码</param>
        /// <param name="provinceId">所属省份Id</param>
        /// <param name="hospital">医院名称</param>
        /// <param name="source">操作来源，1表示web 2表示移动端</param>
        /// <param name="inputCode">输入的邀请码</param>
        /// <returns>成功返回新添加的用户</returns>
        Member Regist(string phone, string password, string password2, long provinceId, string hospital, int source, string inputCode = "");

        /// <summary>
        /// 用户注册，底层函数
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="password">密码</param>
        /// <param name="password2">确认密码</param>
        /// <param name="source">操作来源，1表示web 2表示移动端</param>
        /// <param name="inputCode">输入的邀请码</param>
        /// <returns>成功返回新添加的用户</returns>
        Member Regist(string phone, string password, string password2, int source, string inputCode = "");

        /// <summary>
        /// 用户注册，底层函数
        /// </summary>
        /// <param name="loginId">登录用户名</param>
        /// <param name="name">姓名</param>
        /// <param name="phone">手机号码</param>
        /// <param name="password">密码</param>
        /// <param name="password2">确认密码</param>
        /// <param name="source">操作来源，1表示web 2表示移动端</param>
        /// <returns></returns>
        Member Regist(string loginId, string name, string phone, string password, string password2, int source);
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool UpdatePassword(long id, string password);

        #region 批量刷用户所属医院的数据和上传数据是否显示的数据，请勿随意删除和调用

        void UpdateMe();

        void UpdateMe2();

        #endregion
   
    }
}
