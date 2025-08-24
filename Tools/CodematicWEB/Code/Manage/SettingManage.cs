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
    public class SettingManage
    {
          /// <summary>
        /// XML 文件的路径
        /// </summary>
        public static string SettingXml = "";

        public SettingManage()
        {
            SettingManage.SettingXml = HttpContext.Current.Server.MapPath("/File/Config/Setting.xml");

            if (string.IsNullOrEmpty(SettingManage.SettingXml) || !File.Exists(SettingManage.SettingXml))
                throw new Exception("模版的配置文件数据丢失，请检查（/File/Config/Setting.xml）");
        }


        /// <summary>
        /// 通过解析设置的XML文件获取对象
        /// /File/Config/Setting.xml
        /// </summary>
        /// <returns></returns>
        public SettingModel GetSetting()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(SettingManage.SettingXml);
                XmlNode root = xmlDoc.SelectSingleNode("Setting");
                XmlNodeList nodeList = root.ChildNodes;

                XmlElement xe = (XmlElement)root;
                SettingModel model = new SettingModel();
                model.Connection = xe["Connection"].InnerText;
                model.DatabaseName = xe["DatabaseName"].InnerText;
                model.DatabaseVersion = xe["DatabaseVersion"].InnerText;
                model.State = xe["State"].InnerText;
                model.TemplateId = xe["TemplateId"].InnerText;
                model.SolutionName = xe["SolutionName"].InnerText;
                model.FormId = xe["FormId"].InnerText;
                model.ControlId = xe["ControlId"].InnerText;

                return model;
            }
            catch (Exception)
            {
                throw new Exception("解析 Setting.xml 错误，请检查文件是否符合标准！");
            }
        }


        /// <summary>
        /// 保存基本设置，数据保存到XML文件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveSetting(SettingModel model)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(SettingManage.SettingXml);
                XmlNode root = xmlDoc.SelectSingleNode("Setting");
                XmlNodeList nodeList = root.ChildNodes;

                XmlElement xe = (XmlElement)root;
                xe["Connection"].InnerText=model.Connection.Trim();
                xe["DatabaseName"].InnerText=model.DatabaseName ;
                xe["DatabaseVersion"].InnerText = model.DatabaseVersion;
                xe["State"].InnerText=model.State;
                xe["TemplateId"].InnerText = model.TemplateId;
                xe["SolutionName"].InnerText = model.SolutionName;
                xe["FormId"].InnerText=model.FormId;
                xe["ControlId"].InnerText=model.ControlId ;

                 xmlDoc.Save(SettingManage.SettingXml);

                 return true;
            }
            catch (Exception)
            {
            }

            return false;
        }


        /// <summary>
        /// 清除所有的设置，把XML内的数据清空
        /// </summary>
        /// <returns></returns>
        public bool ClearSetting()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(SettingManage.SettingXml);
                XmlNode root = xmlDoc.SelectSingleNode("Setting");
                XmlNodeList nodeList = root.ChildNodes;

                XmlElement xe = (XmlElement)root;
                xe["Connection"].InnerText = "";
                xe["DatabaseName"].InnerText = "";
                xe["DatabaseVersion"].InnerText = "";
                xe["State"].InnerText = "false";
                xe["TemplateId"].InnerText = "";
                xe["FormId"].InnerText = "";
                xe["ControlId"].InnerText = "";
                xe["SolutionName"].InnerText = "";
                
                xmlDoc.Save(SettingManage.SettingXml);

                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }


    }
}