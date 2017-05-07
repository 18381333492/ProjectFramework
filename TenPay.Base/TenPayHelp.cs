using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace TenPay.Base
{
    public class TenPayHelp
    {
       
        /// <summary>
        /// 从微信请求的参数中获取字典集合
        /// </summary>
        /// <param name="sXmlContent"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionaryFromXml(string sXmlContent)
        {
            if (!string.IsNullOrEmpty(sXmlContent))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sXmlContent);
                //得到XML文档根节点
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodeList = root.ChildNodes;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (XmlNode node in nodeList)
                {
                    dic.Add(node.Name, node.InnerText);
                }
                return dic;
            }
            return null;
        }


        /// <summary>
        /// 将POST请求的返回的数据转化为字典集合
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionaryFromCDATAXml(string xmlData)
        {
            if (!string.IsNullOrEmpty(xmlData))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlData);
                //得到XML文档根节点
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodeList = root.ChildNodes;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (XmlNode node in nodeList)
                {
                    dic.Add(node.Name, node.InnerText.Replace("<![CDATA[", string.Empty).Replace("]]>", string.Empty));
                }
                return dic;
            }
            return null;
        }

        /// <summary>
        /// 拼接的[CDATA]XML数据
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static string InstallCDATAXml(Dictionary<string, string> ht)
        {
            string xml = "<xml>";
            foreach (string key in ht.Keys)
            {
                xml += "<" + key + "><![CDATA[" + ht[key].ToString() + "]]></" + key + ">";
            }
            xml += "</xml>";
            return xml;
        }

        /// <summary>
        /// 拼接统一下单支付请求的XML数据
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static string InstallXml(Dictionary<string, string> ht)
        {
            string xml = "<xml>";
            foreach (string key in ht.Keys)
            {
                xml += "<" + key + ">" + ht[key].ToString() + "</" + key + ">";
            }
            xml += "</xml>";
            return xml;
        }

        /// <summary>
        /// 对url进行urlencode处理
        /// </summary>
        /// <param name="sUrl">链接</param>
        /// <param name="sEncoding">编码格式</param>
        /// <returns></returns>
        public static string UrlEncode(string sUrl, string sEncoding)
        {
            sUrl = sUrl.Trim();
            if (!string.IsNullOrEmpty(sUrl))
            {
                try
                {
                    sUrl = HttpUtility.UrlEncode(sUrl, Encoding.GetEncoding(sEncoding));
                }
                catch (Exception e)
                {
                    sUrl = HttpUtility.UrlEncode(sUrl, Encoding.GetEncoding("GB2312"));
                }
            }
            return sUrl;
        }

        /// <summary>
        /// Post请求微信系统
        /// </summary>
        /// <param name="sUrl">请求的链接</param>
        /// <param name="PostData">请求的参数</param>
        /// <returns></returns>
        public static string HttpPost(string sUrl, string PostData)
        {
            byte[] bPostData = System.Text.Encoding.UTF8.GetBytes(PostData);
            string sResult = string.Empty;
            try
            {
                HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sUrl);
                webRequest.ProtocolVersion = HttpVersion.Version10;
                webRequest.Timeout = 30000;
                webRequest.Method = "POST";
                webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                if (bPostData != null)
                {
                    Stream postDataStream = webRequest.GetRequestStream();
                    postDataStream.Write(bPostData, 0, bPostData.Length);
                }
                HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                if (webResponse.ContentEncoding.ToLower() == "gzip")//如果使用了GZip则先解压
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (var zipStream =
                            new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.GetEncoding(webResponse.CharacterSet)))
                            {
                                sResult = sr.ReadToEnd();
                            }
                        }
                    }
                }
                else if (webResponse.ContentEncoding.ToLower() == "deflate")//如果使用了deflate则先解压
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (var zipStream =
                            new System.IO.Compression.DeflateStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.GetEncoding(webResponse.CharacterSet)))
                            {
                                sResult = sr.ReadToEnd();
                            }
                        }
                    }
                }
                else
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.GetEncoding(0)))
                        {
                            sResult = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sResult;
        }

    }
}
