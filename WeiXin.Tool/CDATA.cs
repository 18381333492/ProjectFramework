using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WeiXin.Tool
{
    /// <summary>
    /// CDATA文本元素
    /// </summary>
    public class CDATA : IXmlSerializable
    {
        public CDATA()
        {
        }
        public CDATA(string sValue)
        {
            this.Text = sValue;
        }
        public string Text;

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            string sResult = reader.ReadInnerXml();
            Regex regex = new Regex("^[<][!][[]CDATA[[]|[]][]][>]$");
            char[] trims = new char[] { '\r', '\n', '\t', ' ' };
            sResult = regex.Replace(sResult, string.Empty);
            this.Text = sResult.Trim(trims);
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteCData(this.Text);
        }
    }
}


/**
  [Serializable]
        public class User
        {
            public int ID { get; set; }

            [XmlElement("name", typeof(CDATA))]
            public CDATA name
            {
                get;
                set;
            }
        }
    Main(){
            User user = new User() { name = new CDATA("ddsdfdf") };
            var s = XmlHelper.ObjectToXml(user);
            Console.WriteLine(s);
            var t = XmlHelper.XmlToObject<User>(s);
        }
 
 */