using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core
{
    /// <summary>
    /// 实体类基类
    /// </summary>
    public abstract partial class BaseEntity
    {

        /// <summary>
        /// 主键 id
        /// </summary>
        public long Id  {get;  set;    }

        private DateTime insertTime = DateTime.Now;
        /// <summary>
        /// 该条记录插入数据库时间
        /// </summary>
        public DateTime InsertTime
        {
            set { insertTime = value; }
            get { return insertTime; }
        }


        private DateTime updateTime = DateTime.Now;
        /// <summary>
        /// 该条记录数据库最后修改时间
        /// </summary>
        public DateTime UpdateTime
        {
            set { updateTime = value; }
            get { return updateTime; }
        }

        private DateTime deleteTime = DateTime.Parse("1900-01-01");
        /// <summary>
        /// 该条记录数据库删除时间
        /// </summary>
        public DateTime DeleteTime
        {
            set { deleteTime = value; }
            get { return deleteTime; }
        }


        private long version = 1;
        /// <summary>
        /// 该条记录的版本
        /// </summary>
        public long Version
        {
            set { version = value; }
            get { return version; }
        }


        private int mark = 1;
        /// <summary>
        /// 改行数据状态，0：表示删除   1：表示插入  2：表示修改，其他值无效，默认是插入
        /// </summary>
        public int Mark
        {
            set { mark = value; }
            get { return mark; }
        }

        private string describe = "";
        /// <summary>
        /// 描述信息
        /// <summary>
        public string Describe
        {
            set { describe = value; }
            get { return describe; }
        }


        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        private static bool IsTransient(BaseEntity obj)
        {
            return obj != null && Equals(obj.Id, default(int));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }
    }
}
