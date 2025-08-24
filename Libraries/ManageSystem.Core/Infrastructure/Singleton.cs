using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Infrastructure
{
    /// <summary>
    /// 静态编译的“单”用于存储对象在应用程序的生命周期域。
    /// 与其说单例模式的意义上的作为一个标准化的方式来存储单一实例。
    /// </summary>
    /// <typeparam name="T">对象存储的类型。</typeparam>
    /// <remarks>不是synchrnoized访问实例。</remarks>
    public class Singleton<T> : Singleton
    {
        static T instance;

        /// <summary>单例实例指定类型T .只有一个实例(在当时)为每种类型的T的对象.</summary>
        public static T Instance
        {
            get { return instance; }
            set
            {
                instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }

    /// <summary>
    /// 提供了一个独立为一个特定的类型列表。
    /// </summary>
    /// <typeparam name="T">列表来存储的类型。</typeparam>
    public class SingletonList<T> : Singleton<IList<T>>
    {
        static SingletonList()
        {
            Singleton<IList<T>>.Instance = new List<T>();
        }

        /// <summary>单例实例指定类型T .只有一个实例(在当时)这个列表为每个类型的T。</summary>
        public new static IList<T> Instance
        {
            get { return Singleton<IList<T>>.Instance; }
        }
    }

    /// <summary>
    /// 提供了一个单例为某些关键和vlaue类型字典。
    /// </summary>
    /// <typeparam name="TKey">键的类型。</typeparam>
    /// <typeparam name="TValue">值的类型。</typeparam>
    public class SingletonDictionary<TKey, TValue> : Singleton<IDictionary<TKey, TValue>>
    {
        static SingletonDictionary()
        {
            Singleton<Dictionary<TKey, TValue>>.Instance = new Dictionary<TKey, TValue>();
        }

        /// <summary>单例实例指定类型T .只有一个实例(在当时)这本词典为每个类型的T。</summary>
        public new static IDictionary<TKey, TValue> Instance
        {
            get { return Singleton<Dictionary<TKey, TValue>>.Instance; }
        }
    }

    /// <summary>
    /// 提供对所有“独生子”存储的访问 <see cref="Singleton{T}"/>.
    /// </summary>
    public class Singleton
    {
        static Singleton()
        {
            allSingletons = new Dictionary<Type, object>();
        }

        static readonly IDictionary<Type, object> allSingletons;

        /// <summary>字典类型的单例实例。</summary>
        public static IDictionary<Type, object> AllSingletons
        {
            get { return allSingletons; }
        }
    }
}
