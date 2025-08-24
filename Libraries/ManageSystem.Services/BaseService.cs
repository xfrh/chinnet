using ManageSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EntityFramework.Extensions;
using ManageSystem.Core.Utility;
using System.Linq.Dynamic;

namespace ManageSystem.Services
{
    /// <summary>
    /// 针对所有继承IBaseService的接口Service类中函数进行初步封装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BaseService<T> where T : BaseEntity, new()
    {

        /// <summary>
        /// 操作数据库对象
        /// </summary>
        protected readonly Core.Data.IRepository<T> _repository;

        public BaseService(Core.Data.IRepository<T> repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// 获取数量行数
        /// </summary>
        /// <returns>返回获取到的数据行数</returns>
        public virtual int Count()
        {
            return Count(p => true);
        }

        /// <summary>
        /// 根据指定条件获取元素数量
        /// </summary>
        /// <param name="where">查询条件，Lambda表达式，如果没有条件请输入 null</param>
        /// <returns>返回获取到的数据行数</returns>
        public virtual int Count(Expression<Func<T, bool>> where)
        {
            if (where == null) where = p => true;

            return this._repository.Table.Where(where).Count();
        }

        /// <summary>
        /// 插入数据，返回操作成功的行数
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public virtual void Insert(T entity)
        {
            if (entity.Id <= 0) entity.Id = CommonHelper.GuidToLongID;

            this._repository.Insert(entity);
        }

        /// <summary>
        /// 批量插入多条数据，返回操作成功的行数
        /// </summary>
        /// <param name="entities">实体对象</param>
        /// <returns></returns>
        public virtual void Insert(List<T> entities)
        {
            this._repository.Insert(entities);
        }

        /// <summary>
        /// 修改一个实体对象
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            entity.Mark = 2;
            entity.UpdateTime = DateTime.Now;
            entity.Version++;

            this._repository.Update(entity);
        }

        /// <summary>
        /// 根据条件修改指定的字段
        /// </summary>
        /// <param name="where">修改条件</param>
        /// <param name="update">需要修改的字段</param>
        public virtual void Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> update)
        {
            //调用Entity Framework Extended Library 扩展库 https://github.com/loresoft/EntityFramework.Extended
            //调用示例，where 是更新的条件（等效于：where StatusId=1）,update 是更新数据（等效于：SET StatusId=2） ,Task 为实体对象，StatusId为属性值
            // this.dbContext.Set<T>().Where(t => t.StatusId == 1).Update(  t2 => new Task {StatusId = 2});

            this._repository.Table.Where(where).Update(update);
        }

        /// <summary>
        /// 根据主键Id删除单个对象
        /// </summary>
        /// <param name="id">需要删除的主键Id</param>
        ///  <returns></returns>
        public virtual void Delete(long id)
        {
            T entity = this.QueryEntity(id);
            this.Delete(entity);
        }

        /// <summary>
        /// 根据主键Id删除单个或者多个对象数据，如果多个使用英文逗号分割
        /// </summary>
        /// <param name="ids">id集合，使用英文逗号分割</param>
        public virtual void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return;
            ids = ids.TrimEnd(',');

            List<long> idList = new List<long>();
            var cities = ids.Split(',').Select(x => { return long.Parse(x); }).ToList();
            this.Update(m => cities.Contains(m.Id), p => new T { Mark = 0, DeleteTime = DateTime.Now });
        }

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            entity.Mark = 0;
            entity.DeleteTime = DateTime.Now;

            this._repository.Update(entity);
        }

        /// <summary>
        /// 同时删除多个对象
        /// </summary>
        /// <param name="entities">需要删除的数据集合</param>
        ///  <returns></returns>
        public virtual void Delete(List<T> entities)
        {
            foreach (var item in entities)
            {
                item.Mark = 0;
                item.DeleteTime = DateTime.Now;
            }

            this._repository.Delete(entities);
        }

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="where"> 删除条件，Lambda表达式，</param>
        /// <returns></returns>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            List<T> list = this.Query(where);
            this.Delete(list);
        }

        /// <summary>
        /// 根据Id获取一个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T QueryEntity(long id)
        {
            T t = this._repository.GetById(id);
            if (t == null) return null;

            return t;
        }

        /// <summary>
        /// 根据查询条件获取符合要求的第一个对象
        /// </summary>
        /// <param name="where">查询条件，Lambda表达式，如果没有条件请输入 null</param>
        /// <returns>返回单个实体对象</returns>
        public virtual T QueryEntity(Expression<Func<T, bool>> where)
        {
            if (where == null) where = p => true;
            return this._repository.Table.Where(where).FirstOrDefault();
        }

        /// <summary>
        /// 根据查询条件获取符合要求的第一个对象
        /// </summary>
        /// <param name="where">查询条件，Lambda表达式，如果没有条件请输入 null</param>
        /// <param name="isOrderByDesc">是否倒序排列数据，相当于SQL：ORDER BY DESC  </param>
        /// <param name="orderBy">排序字段</param>
        /// <returns>返回单个实体对象</returns>
        public virtual T QueryEntity(Expression<Func<T, bool>> where, bool isOrderByDesc = false, Expression<Func<T, int?>> orderBy = null)
        {
            List<T> query = this.Query(where, 0, isOrderByDesc, orderBy);
            if (query == null || query.Count <= 0) return null;

            return query[0];
        }

        /// <summary>
        /// 查询全部数据
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> Query()
        {
            return this.Query(m => true);
        }

        /// <summary>
        /// 查询数据-会失败
        /// </summary>
        /// <param name="where">查询条件，Lambda表达式，如果没有条件请输入 null</param>
        /// <param name="topCount">指定查询的条数，小于等于0表示不使用</param>
        /// <param name="isOrderByDesc">是否倒序排列数据，相当于SQL：ORDER BY DESC  </param>
        /// <param name="orderBy">排序字段，Lambda表达式 </param>
        /// <returns>返回对象集合</returns>
        public virtual List<T> Query(Expression<Func<T, bool>> where, int topCount = 0, bool isOrderByDesc = false, Expression<Func<T, int?>> orderBy = null)
        {
            //try
            //{
                if (where == null) where = p => true;
                IQueryable<T> query = this._repository.Table.Where(where).Where(m => m.Mark > 0);

                if (orderBy == null)
                {
                    //不启用排序
                    query = query.OrderByDescending(m => m.InsertTime);
                }
                else
                {
                    //启用排序
                    if (!isOrderByDesc && orderBy != null) query = query.OrderBy(orderBy);
                    else if (isOrderByDesc && orderBy != null) query = query.OrderByDescending(orderBy);
                }

                if (topCount > 0) query = query.Take<T>(topCount);

                if (query == null || query.Count() <= 0)
                {
                    return new List<T>();
                }
                return query.ToList<T>();
            //}
            //catch (Exception)
            //{

            //    throw new Exception("数据库用户登录失败"); ;
            //}

        }

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="pageSize">查询条件</param>
        /// <returns></returns>
        public virtual IPagedList<T> QueryPage(int pageIndex = 0, int pageSize = int.MaxValue, Expression<Func<T, bool>> where = null)
        {
            List<T> query = this.Query(where);
            var list = new PagedList<T>(query, pageIndex, pageSize);

            return list;
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="where">查询条件，Lambda表达式，如果没有条件请输入 null</param>
        /// <param name="topCount">指定查询的条数，小于等于0表示不使用</param>
        /// <param name="isOrderByDesc">是否倒序排列数据，相当于SQL：ORDER BY DESC  </param>
        /// <param name="orderBy">排序字段，Lambda表达式 </param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IPagedList<T> QueryPage(Expression<Func<T, bool>> where, int topCount = 0,
            bool isOrderByDesc = false, Expression<Func<T, int?>> orderBy = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            List<T> query = this.Query(where, topCount, isOrderByDesc, orderBy);
            var list = new PagedList<T>(query, pageIndex, pageSize);

            return list;
        }
    }
}
