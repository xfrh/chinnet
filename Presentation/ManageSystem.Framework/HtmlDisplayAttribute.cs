
using ManageSystem.Framework.Mvc;

namespace ManageSystem.Framework
{
    public class HtmlDisplayAttribute : System.ComponentModel.DisplayNameAttribute, IModelAttribute
    {
        /// <summary>
        /// 页面的显示的名称
        /// </summary>
        private string _htmlName = string.Empty;
        public string HtmlName
        {
            get { return _htmlName; }
            set { this._htmlName = value; }
        }

        /// <summary>
        /// 鼠标悬浮的内容
        /// </summary>
        private string _htmlTitle = string.Empty;
        public string HtmlTitle
        {
            get { return _htmlTitle; }
            set { this._htmlTitle = value; }
        }

        /// <summary>
        /// 是否为必填
        /// </summary>
        private bool _required = false;
        public bool Required
        {
            get { return _required; }
            set { this._required = value; }
        }

        public HtmlDisplayAttribute(string htmlName, bool required = false)
            : base(htmlName)
        {
            HtmlName = htmlName;
        }
        public HtmlDisplayAttribute(string htmlName, string htmlTitle, bool required = false)
         : base(htmlName)
        {
            HtmlName = htmlName;
            HtmlTitle = htmlTitle;
            Required = required;
        }

        public override string DisplayName
        {
            get
            {
                return this.HtmlName;
            }
        }


        public string Name
        {
            get { return "HtmlDisplayAttribute"; }
        }
    }
}
