

namespace ManageSystem.Core.Domain.Configuration
{
    /// <summary>
    ///系统设置
    /// </summary>
    public partial class Setting : BaseEntity
    {
        public Setting() { }

        /// <summary>
        /// 网页上显示的名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 配置的关键字，只能是因为，且不能重复
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 配置的具体值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 是否只能通过数据库操作
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 是否保存到缓存
        /// </summary>
        public bool IsCache { get; set; }


        /// <summary>
        /// 配置所属类型
        /// </summary>
        public string Type { get; set; }


    }
}
