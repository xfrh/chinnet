using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ManageSystem.Core;
using ManageSystem.Core.Caching;
using ManageSystem.Core.Configuration;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Configuration;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Caching;

namespace ManageSystem.Services.Configuration
{
    /// <summary>
    /// Setting manager
    /// </summary>
    public partial class SettingService : BaseService<Setting>, ISettingService
    {
        private readonly ICacheManager CacheManager;

        public SettingService(ICacheManager _cacheManager,
            IRepository<Setting> repository) :
            base(repository)
        {
            CacheManager = _cacheManager;
        }



        /// <summary>
        ///缓存的 Key 
        /// </summary>
        private const string SETTINGS_ALL_KEY = "manage.system.setting";

        /// <summary>
        /// 设置所有的配置到缓存中
        /// </summary>
        public List<Setting> SetAllSettingsCached()
        {
            var list = base.Query(m => m.Mark > 0 && m.IsCache);
            if (list == null) list = new List<Setting>();

            foreach (var item in list)
            {
                this.CacheManager.Set(item.Name, item, 3600);

            }

            return list;
        }

        /// <summary>
        /// 获取缓存的中的所有配置
        /// </summary>
        /// <returns>Settings</returns>
        //public List<Setting> GetAllSettingsCached()
        //{
        //    string key = string.Format(SETTINGS_ALL_KEY);

        //    List<Setting> settingList = this.CacheManager.Get<List<Setting>>(key);

        //    if (settingList == null || !settingList.Any())
        //    {
        //        //缓存中没有数据，这里重新设置缓存
        //        settingList = this.SetAllSettingsCached();
        //    }

        //    return settingList;
        //}

        /// <summary>
        /// 新增缓存中的配置
        /// </summary>
        /// <param name="setting">对象</param>
        public void InsetCached(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            //不保存到缓存中
            if (!setting.IsCache) return;

            this.CacheManager.Set(setting.Name, setting, 3600);
        }

        /// <summary>
        /// 更新缓存中的配置
        /// </summary>
        /// <param name="setting">对象</param>
        public void UpdateCached(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            //不保存到缓存中
            if (!setting.IsCache) return;

            this.CacheManager.Set(setting.Name, setting, 3600);

        }

        /// <summary>
        /// 删除缓存中的配置
        /// </summary>
        /// <param name="setting">对象</param>
        public void DeleteCached(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            //不保存到缓存中
            if (!setting.IsCache) return;

            this.CacheManager.Remove(setting.Name);

        }

        /// <summary>
        /// 新增一个配置
        /// </summary>
        /// <param name="setting">保存对象</param>
        public override void Insert(Setting entity)
        {
            if (entity == null)
                throw new ArgumentNullException("setting");

            base.Insert(entity);

            //插入到缓存
            if (entity.IsCache)
            {
                //更新本网站缓存
                this.InsetCached(entity);

                //更新其他网站的缓存
                this.RefreshWebCache("all");
            }
        }

        /// <summary>
        /// 修改配置
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(Setting entity)
        {
            //如果是从缓存中读取的数据无法关联到EF，所以重新查询并更新数据库和缓存
            var model = this.QueryEntity(entity.Id);
            model.Value = entity.Value;
            model.Describe = entity.Describe;
            model.IsAdmin = entity.IsAdmin;
            model.IsCache = entity.IsCache;
            model.Title = entity.Title;
            model.Type = entity.Type;

            base.Update(model);

            //修改缓存
            if (entity.IsCache)
            {
                //更新本网站缓存
                this.UpdateCached(model);

                //更新其他网站的缓存
                this.RefreshWebCache("all");
            }

        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(Setting entity)
        {
            base.Delete(entity);

            //删除缓存
            if (entity.IsCache)
            {
                //更新本网站缓存
                this.DeleteCached(entity);

                //更新其他网站的缓存
                this.RefreshWebCache("all");
            }
        }

        /// <summary>
        /// 根据Key获取配置的值，根据泛型反回值，注意后面是要更新到数据库，请不要使用缓存获取
        /// </summary>
        /// <typeparam name="T">配置中的值类型</typeparam>
        ///<typeparam name = "cache" > 从缓存读取 </ typeparam >
        /// <returns>Setting value</returns>
        public Setting QueryEntity(string key, bool cache = true)
        {
            if (String.IsNullOrWhiteSpace(key)) return null;

            key = key.Trim().ToLowerInvariant();
            Setting entity = null;

            if (cache)
            {
                //查询缓存
                entity = this.CacheManager.Get<Setting>(key);
                if (entity != null && entity.Id > 0 )
                    return entity;
            }

            //数据库查询
            entity = this.QueryEntity(m => m.Name.ToLower().Equals(key.ToLower()));
            if (entity != null)
            {
                this.InsetCached(entity);

                return entity;
            }

            return null;
        }

        /// <summary>
        /// 根据Key获取配置的值，根据泛型反回值
        /// </summary>
        /// <typeparam name="T">配置中的值类型</typeparam>
        /// <param name="key">配置的名称</param>
        /// <returns></returns>
        public string QueryValue(string key)
        {
            return this.QueryValue<string>(key);
        }

        /// <summary>
        /// 根据Key获取配置的值，根据泛型反回值
        /// </summary>
        /// <typeparam name="T">配置中的值类型</typeparam>
        /// <param name="key">配置的名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>Setting value</returns>
        public T QueryValue<T>(string key, T defaultValue = default(T))
        {
            if (String.IsNullOrEmpty(key))
                return defaultValue;

            Setting entity = this.QueryEntity(key);
            if (entity == null) return defaultValue;

            return CommonHelper.To<T>(entity.Value);
        }

        /// <summary>
        /// 确定设置是否存在
        /// </summary>
        /// <typeparam name="name">设置的名称</typeparam>
        public bool Exists(string name)
        {
            return base.Count(m => m.Name.ToLower().Equals(name.ToLower()) && m.Mark > 0) > 0;
        }


        public IPagedList<Setting> QueryPage(string title, string name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(m => m.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));


            query = query.OrderBy(m => m.Id);

            return new PagedList<Setting>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// 根据key更新数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="updateCache">是否更新缓存</param>
        public bool UpdateValue(string key, string value, bool updateCache = true)
        {
            try
            {
                var entity = this.QueryEntity(key, false);
                if (entity == null) return false;

                entity.Value = value;

                base.Update(entity);

                if (updateCache && entity.IsCache)
                {
                    //更新本网站缓存
                    this.UpdateCached(entity);
                    //更新其他网站的缓存
                    this.RefreshWebCache("all");
                }

                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }

    }
}