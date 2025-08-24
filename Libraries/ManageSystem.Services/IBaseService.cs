using ManageSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services
{

    /// <summary>
    ///Services中 所有接口的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseService<T> where T : BaseEntity
    {

        /// <summary>
        /// 获取数量行数
        /// </summary>
        /// <returns>返回获取到的数据行数</returns>
        int Count();

        /// <summary>
        /// 根据指定条件获取元素数量
        /// </summary>
        /// <param name="where">查询条件，Lambda表达式，如果没有条件请输入 null</param>
        /// <returns>返回获取到的数据行数</returns>
        int Count(Expression<Func<T, bool>> where);

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity">需要添加的实体对象</param>
        void Insert(T entity);

        /// <summary>
        /// 插入多条数据
        /// </summary>
        /// <param name="entities">需要添加的实体对象</param>
        void Insert(List<T> entities);

        /// <summary>
        ///更新一条数据
        /// </summary>
        /// <param name="entity">需要更新的实体对象</param>
        void Update(T entity);

        /// <summary>
        /// 根据条件修改指定的字段
        /// </summary>
        /// <param name="where">修改条件</param>
        /// <param name="update">需要修改的字段</param>
        void Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> update);

        /// <summary>
        /// 根据主键Id删除单个对象数据
        /// </summary>
        /// <param name="id">需要删除的主键Id</param>
        ///  <returns></returns>
        void Delete(long id);

        /// <summary>
        /// 根据主键Id删除单个或者多个对象数据，如果多个使用英文逗号分割
        /// </summary>
        /// <param name="ids">id集合，使用英文逗号分割</param>
        void Delete(string ids);

        /// <summary>
        /// 根据实体对象删除数据
        /// </summary>
        /// <param name="entity">需要删除的实体对象</param>
        void Delete(T entity);

        /// <summary>
        /// 同时删除多个对象
        /// </summary>
        /// <param name="entities">需要删除的数据集合</param>
        ///  <returns></returns>
        void Delete(List<T> entities);

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="where"> 删除条件，Lambda表达式，</param>
        /// <returns></returns>
        void Delete(Expression<Func<T, bool>> where);

        /// <summary>
        /// 根据id获取一个实体对象
        /// </summary>
        /// <param name="id">需要</param>
        /// <returns></returns>
        T QueryEntity(long id);

        /// <summary>
        /// 根据查询条件获取符合要求的第一个对象
        /// </summary>
        /// <param name="where">查询条件，Lambda表达式，如果没有条件请输入 null</param>
        /// <returns>返回单个实体对象</returns>
        T QueryEntity(Expression<Func<T, bool>> where);

        /// <summary>
        /// 根据查询条件获取符合要求的第一个对象
        /// </summary>
        /// <param name="where">查询条件，Lambda表达式，如果没有条件请输入 null</param>
        /// <param name="isOrderByDesc">是否倒序排列数据，相当于SQL：ORDER BY DESC  </param>
        /// <param name="orderBy">排序字段</param>
        /// <returns>返回单个实体对象</returns>
        T QueryEntity(Expression<Func<T, bool>> where = null, bool isOrderByDesc = false, Expression<Func<T, int?>> orderBy = null);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        IList<T> Query();

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="where">查询条件，Lambda表达式，如果没有条件请输入 null</param>
        /// <param name="topCount">指定查询的条数，小于等于0表示不使用</param>
        /// <param name="isOrderByDesc">是否倒序排列数据，相当于SQL：ORDER BY DESC  </param>
        /// <param name="orderBy">排序字段，Lambda表达式 </param>
        /// <returns>返回对象集合</returns>
        List<T> Query(Expression<Func<T, bool>> where, int topCount = 0, bool isOrderByDesc = false, Expression<Func<T, int?>> orderBy = null);

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="pageSize">查询条件</param>
        /// <returns></returns>
        IPagedList<T> QueryPage(int pageIndex = 0, int pageSize = int.MaxValue, Expression<Func<T, bool>> where = null);


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
        IPagedList<T> QueryPage(Expression<Func<T, bool>> where, int topCount = 0,
            bool isOrderByDesc = false, Expression<Func<T, int?>> orderBy = null,
            int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
