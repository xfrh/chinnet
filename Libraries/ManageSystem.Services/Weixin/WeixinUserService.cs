using System;
using System.Linq;
using System.Data;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Weixin;
using ManageSystem.Core;

namespace ManageSystem.Services.Weixin
{
    /// <summary>
    /// 操作类 ，数据库表名：WeixinUser 
    /// </summary>
    public partial class WeixinUserService : BaseService<WeixinUser>, IWeixinUserService
    {

        public WeixinUserService(IRepository<WeixinUser> repository) : base(repository)
        {

        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="subscribe"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<WeixinUser> QueryPage(string openid, string subscribeValue,string startTime,string endTime, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0); ;
            if (!string.IsNullOrWhiteSpace(openid))
                query = query.Where(m => m.Openid.Contains(openid));

            if (!string.IsNullOrWhiteSpace(subscribeValue))
            {
                bool subscribeTemp = subscribeValue.Equals("1");
                query = query.Where(m => m.Subscribe == subscribeTemp);
            }

            if (!string.IsNullOrWhiteSpace(startTime) && IsDateTime(startTime))
            {
                DateTime temp = DateTime.Parse(startTime);
                query = query.Where(m => m.SubscribeTime >= temp);
            }

            if (!string.IsNullOrWhiteSpace(endTime) && IsDateTime(endTime))
            {
                DateTime temp = DateTime.Parse(endTime);
                query = query.Where(m => m.SubscribeTime <= temp);
            }

            query = query.OrderByDescending(m => m.InsertTime);

            var list = new PagedList<WeixinUser>(query.ToList(), pageIndex, pageSize);

            return list;
        }

        /// <summary>
        /// 用户关注以后 添加或者修改微信用户记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UserSubscribe(WeixinUser newEntity)
        {
            if (string.IsNullOrWhiteSpace(newEntity.Openid)) return false;

            try
            {
                var entity = this.QueryEntity(m => m.Openid.ToLower().Equals(newEntity.Openid.ToLower()));
                if (entity == null || string.IsNullOrWhiteSpace(entity.Openid))
                {
                    //第一次关注
                    this.Insert(newEntity);
                }
                else
                {
                    //非第一次关注
                  entity.Subscribe = true;
                  entity.SubscribeTime = DateTime.Now;
                  entity.Name = newEntity.Name;
                  entity.Sex = newEntity.Sex;
                  entity.Area = newEntity.Area;

                  this.Update(entity);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }


        /// <summary>
        /// 用户取消关注以后 修改微信用户记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UserUnsubscribe(WeixinUser newEntity)
        {
            if (string.IsNullOrWhiteSpace(newEntity.Openid)) return false;

            try
            {
                var entity = this.QueryEntity(m => m.Openid.ToLower().Equals(newEntity.Openid.ToLower()));
                if (entity == null || string.IsNullOrWhiteSpace(entity.Openid)) return true;

                entity.Subscribe = false;
                entity.UnsubscribeTime = DateTime.Now;

                this.Update(entity);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// 检查值是否是正常的日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDateTime( string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value)) return false;

                DateTime time = Convert.ToDateTime(value);
                if (time.Year < 1950) return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
