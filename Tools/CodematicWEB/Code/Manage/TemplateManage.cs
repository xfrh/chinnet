using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodematicWEB.Code.Model;
using System.Web;
using System.Xml;
using System.IO;

namespace CodematicWEB.Code.Manage
{
    /// <summary>
    /// 模版文件的操作类
    /// </summary>
    public class TemplateManage
    {     
        /// <summary>
        /// XML 文件的路径
        /// </summary>
        public static string TemplateXml = "";

        public TemplateManage()
        {
            TemplateManage.TemplateXml =  HttpContext.Current.Server.MapPath( "/File/Config/Template.xml");

            if (string.IsNullOrEmpty(TemplateManage.TemplateXml) || !File.Exists(TemplateManage.TemplateXml))
                throw new Exception("模版的配置文件数据丢失，请检查（/File/Config/Template.xml）");
        }
   

        /// <summary>
        /// 读取XML文件获取模版集合
        /// </summary>
        /// <returns></returns>
        public List<TemplateModel> GetListByXML()
        {
            List<TemplateModel> resultList = new List<TemplateModel>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(TemplateManage.TemplateXml);
                XmlNode root = xmlDoc.SelectSingleNode("Templates");
                XmlNodeList nodeList = root.ChildNodes;

                foreach (XmlNode xn in nodeList)
                {
                    XmlElement xe = (XmlElement)xn;

                    TemplateModel model = new TemplateModel();
                    model.Id = xe["Id"].InnerText;
                    model.CreateDate = DateTime.Parse(xe["CreateDate"].InnerText);
                    model.Name = xe["Name"].InnerText;
                    model.Page = xe["Page"].InnerText;
                    model.Image = xe["Image"].InnerText;
                 
                    if (model != null) resultList.Add(model);
                }

                return resultList;
            }
            catch (Exception)
            {
                throw new Exception("解析 Template.xml 错误，请检查文件是否符合标准！");
            }
        }

        /// <summary>
        /// 根据模版ID获取模版对象
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public TemplateModel GetModelById(string templateId)
        {
            List<TemplateModel> list = this.GetListByXML();
            if (list == null || list.Count <= 0) return null;

            list = list.Where(m => m.Id.Equals(templateId)).ToList();
            if (list == null || list.Count <= 0) return null;

            return list[0];
        }
       
    }
}