using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodematicWEB.Code.Model
{
    /// <summary>
    /// 模版文件的实体类，字段和XML文件对应（/File/Config/Template.xml）
    /// </summary>
    public class TemplateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string Image { get; set; }
        public string Page { get; set; }

    }
}