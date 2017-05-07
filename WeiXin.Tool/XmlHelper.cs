using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;
namespace WeiXin.Tool
{
    public class XmlHelper
    {
        /// <summary>
        /// 带有CDATA数据的反序列化时，对应的字段必须为CDATA类型，否则出错。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sXmlContent"></param>
        /// <returns></returns>
        public static T XmlToObject<T>(string sXmlContent)
        {
            StringReader stringReader = new StringReader(sXmlContent);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(xmlReader);
        }


        public static string ObjectToXml(Object oObject)
        {
            StringBuilder sResult = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;　//是否缩进
            settings.NewLineOnAttributes = true;
            using (XmlWriter writer = XmlWriter.Create(sResult, settings))
            {
                XmlSerializer serializer = new XmlSerializer(oObject.GetType());
                serializer.Serialize(writer, oObject);
            }
            return sResult.ToString();
        }

        /// <summary>
        /// 生成xml文档内容，包含属性的属性
        /// </summary>
        /// <param name="oObject"></param>
        /// <param name="sRootNodeName">根结点名字</param>
        /// <returns></returns>
        public static string ObjectToXml(Object oObject, string sRootNodeName)
        {
            StringBuilder sResult = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            using (XmlWriter writer = XmlWriter.Create(sResult, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(sRootNodeName);
                ObjectToXml(writer, oObject);
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            return sResult.ToString();
        }

        /// <summary>
        /// 反射对象属性为xml格式
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="oObject"></param>
        private static void ObjectToXml(XmlWriter writer, Object oObject)
        {
            Type type = oObject.GetType();
            var fieldInfos = type.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                Console.WriteLine("{0}\t\t{1}", fieldInfo.FieldType.Name, fieldInfo.Name);
                if (!fieldInfo.IsPublic) continue;

                switch (fieldInfo.FieldType.Name)
                {
                    case "String":
                        writer.WriteStartElement(fieldInfo.Name);
                        writer.WriteCData(fieldInfo.GetValue(oObject).ToString());
                        writer.WriteEndElement();
                        break;
                    case "Int32":
                    case "Int64":
                        writer.WriteElementString(fieldInfo.Name, fieldInfo.GetValue(oObject).ToString());
                        break;
                    case "CImage[]":
                    case "CMusic[]":
                    case "CVideo[]":
                    case "CVoice[]":
                    case "Article[]":
                        var objs = (Object[])(fieldInfo.GetValue(oObject));
                        if (null != objs && objs.Length > 0)
                        {
                            writer.WriteStartElement(fieldInfo.Name);
                            foreach (Object objItem in objs)
                            {
                                ObjectToXml(writer, objItem);
                            }
                            writer.WriteEndElement();
                        }
                        break;
                    case "Item":
                        var item = (fieldInfo.GetValue(oObject));
                        writer.WriteStartElement(fieldInfo.Name);
                        ObjectToXml(writer, item);
                        writer.WriteEndElement();
                        break;
                }
            }
        }


        /// <summary>
        /// 根据名字获取对应的值
        /// </summary>
        /// <param name="sXmlContent"></param>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static string getTextByNode(string sXmlContent, string sName)
        {
            XElement xElement = XElement.Parse(sXmlContent);
            var xResultElement = xElement.Element(sName);
            return xResultElement == null ? string.Empty : xResultElement.Value;
        }
    }
}
