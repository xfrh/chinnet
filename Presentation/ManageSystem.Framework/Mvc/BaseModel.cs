using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ManageSystem.Framework.Mvc
{
    /// <summary>
    /// Base nopCommerce model
    /// </summary>
    [ModelBinder(typeof(ModelBinder))]
    public partial class BaseModel
    {
        public BaseModel()
        {
            this.CustomProperties = new Dictionary<string, object>();
            PostInitialize();
        }

        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }

        /// <summary>
        /// 开发人员可以在定制部分类覆盖这个方法
        /// 为了添加一些自定义构造函数初始化代码
        /// </summary>
        protected virtual void PostInitialize()
        {

        }

        /// <summary>
        ///使用这个属性来存储任何自定义值为您的模型。
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }
    }

    /// <summary>
    /// 基础实体模型
    /// </summary>
    public partial class BaseEntityModel : BaseModel
    {
        public virtual long Id { get; set; }


 
        private DateTime insertTime = DateTime.Now;
        /// <summary>
        /// 该条记录插入数据库时间
        /// </summary>
        [HtmlDisplayAttribute("添加时间", "添加的时间")]
        public DateTime InsertTime
        {
            set { insertTime = value; }
            get { return insertTime; }
        }


        private DateTime updateTime = DateTime.Now;
        /// <summary>
        /// 该条记录数据库最后修改时间
        /// </summary>
        [HtmlDisplayAttribute("修改时间", "最后一次修改的时间")]
        public DateTime UpdateTime
        {
            set { updateTime = value; }
            get { return updateTime; }
        }

        private DateTime deleteTime = DateTime.Parse("1900-01-01");
        /// <summary>
        /// 该条记录数据库删除时间
        /// </summary>
        [HtmlDisplayAttribute("删除时间", "修改的时间")]
        public DateTime DeleteTime
        {
            set { deleteTime = value; }
            get { return deleteTime; }
        }


        private long version = 1;
        /// <summary>
        /// 该条记录的版本
        /// </summary>
        [HtmlDisplayAttribute("数据版本", "该条数据的版本")]
        public long Version
        {
            set { version = value; }
            get { return version; }
        }


        private int mark = 1;
        /// <summary>
        /// 改行数据状态，0：表示删除   1：表示插入  2：表示修改，其他值无效，默认是插入
        /// </summary>
        [HtmlDisplayAttribute("数据状态", "该条数据的状态，0：表示删除   1：表示插入  2：表示修改")]
        public int Mark
        {
            set { mark = value; }
            get { return mark; }
        }

        private string describe = "";
        /// <summary>
        /// 描述信息
        /// <summary>
        [HtmlDisplayAttribute("描述信息", "描述信息，一般用于后台管理员查看")]
        public string Describe
        {
            set { describe = value; }
            get { return describe; }
        }

        /// <summary>
        /// 是否是编辑页面，控制页面的显示
        /// <summary>
        public bool IsEditPage { get; set; }

    }

}
