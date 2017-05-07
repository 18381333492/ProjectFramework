using System;
using System.ComponentModel;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WeiXin.Base.Pay.Lib
{
    using System.Xml.Linq;

    /// <summary>
    /// 配置微信支付账号信息
    /// </summary>
    public class WxPayConfig
    {

        //=======【微信公众号基本信息设置】=====================================       
        /// <summary>绑定支付的APPID（必须配置）</summary>
        public static string APPID = "";

        //=======【微信公众号基本信息设置】=====================================       
        /// <summary>绑定支付的APPID（必须配置）</summary>
        //public static string APPID = "";

        /// <summary>商户号（必须配置）</summary>
        public static string MCHID = "";

        /// <summary>商户支付密钥(API密钥)，参考开户邮件设置（必须配置）</summary>
        public static string KEY = "";

        /// <summary>公众帐号secert（仅JSAPI支付的时候需要配置）</summary>
        public static string APPSECRET = "";


        //=======【证书路径设置】===================================== 
        /// <summary>证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）</summary>
        public static string SSLCERT_PATH = "Certs/WeChat/apiclient_cert.p12";

        /// <summary>商户证书调用或安装都需要使用到密码，该密码的值为微信商户号（mch_id）</summary>
        public static string SSLCERT_PASSWORD = "";


        //=======【支付结果通知url】===================================== 
        /// <summary>支付结果通知回调url，用于商户接收支付结果</summary>
        public static string NOTIFY_URL = "";

        /// <summary>
        /// 发起支付的连接地址，用于code为空时的回调地址
        /// </summary>
        public static string REDIRECT_URI = "";


        //=======【商户系统后台机器IP】===================================== 
        /// <summary>此参数可手动配置也可在程序中自动获取</summary>
        public static string IP = "8.8.8.8";


        //=======【代理服务器设置】===================================
        /// <summary>默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）</summary>
        public static string PROXY_URL = "http://0.0.0.0:0";


        //=======【上报信息配置】===================================
        /// <summary>测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报</summary>
        public static int REPORT_LEVENL = 0;


        //=======【日志级别】===================================
        /// <summary>日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息</summary>
        public static int LOG_LEVENL = 0;

        /// <summary>
        /// 初始化微信支付信息
        /// 从xml配置文件中读取信息，根节点为settings
        /// </summary>
        /// <exception cref="WxPayException"></exception>
        public static void Initialize(string filePath)
        {
            #region 初始化微信支付信息

            XDocument xDoc = XDocument.Load(filePath);
            var rootNode = xDoc.Element("settings");
            if (null == rootNode)
            {
                throw new WxPayException("微信支付信息配置文件的根节点为settings");
            }

            #region 微信公众号基本信息设置

            //绑定支付的APPID
            var elAppId = rootNode.Element("wx.appid");
            if (null == elAppId)
            {
                throw new WxPayException("必须配置绑定支付的APPID");
            }
            //公众帐号secert
            var elAppSecret = rootNode.Element("wx.appsecret");

            //商户号
            var elMchId = rootNode.Element("wx.mchid");
            if (null == elMchId)
            {
                throw new WxPayException("必须配置微信支付关联的商户号");
            }
            //商户支付密钥
            var elMchKey = rootNode.Element("wx.mchkey");
            if (null == elMchKey)
            {
                throw new WxPayException("必须配置商户支付密钥");
            }

            #endregion

            //支付结果通知url
            var elNotifyUrl = rootNode.Element("wx.notify.url");
            if (null == elNotifyUrl)
            {
                throw new WxPayException("必须配置支付结果通知回调url");
            }

            #region 设置所有配置信息

            APPID = elAppId.Value;
            APPSECRET = null == elAppSecret ? string.Empty : elAppSecret.Value;
            KEY = elMchKey.Value;
            MCHID = elMchId.Value;
            NOTIFY_URL = elNotifyUrl.Value;

            GetParameter(filePath);

            #endregion

            #endregion
        }

        /// <summary>
        /// 初始化微信支付信息
        /// 从前端传递json数据进行解析，解析以 WxOpenModel 字段为准
        /// </summary>
        /// <param name="sJsonData"></param>
        public static void LoadParameter(string sJsonData)
        {
            #region 初始化微信支付信息

            try
            {
                WxOpenModel item = JsonConvert.DeserializeObject<WxOpenModel>(sJsonData);
                APPID = item.sAppId;
                APPSECRET = item.sAppSecret;
                MCHID = item.sMchId;
                KEY = item.sPaySignKey;
                NOTIFY_URL = item.sTenpayNotify;
                REDIRECT_URI = item.sSendUrl;
            }
            catch
            {
                throw new WxPayException("获取微信支付参数错误");
            }

            string sFilePath = HttpContext.Current.Server.MapPath(@"~\App_Data\WeChatPay.xml");
            if (File.Exists(sFilePath))
                GetParameter(sFilePath);

            #endregion
        }

        /// <summary>
        /// 获取一些微信的配置信息
        /// </summary>
        /// <param name="sFilePath"></param>
        static void GetParameter(string sFilePath)
        {
            #region 获取一些微信的配置信息

            XDocument xDoc = XDocument.Load(sFilePath);
            var rootNode = xDoc.Element("settings");
            if (null == rootNode)
            {
                throw new WxPayException("微信支付信息配置文件的根节点为settings");
            }

            #region 证书路径设置

            //证书路径,注意应该填写绝对路径
            var elCertPath = rootNode.Element("wx.ssl.cert.path");
            //商户证书调用或安装都需要使用到密码，该密码的值为微信商户号
            var elCertPwd = rootNode.Element("wx.ssl.cert.password");

            #endregion

            //商户系统后台机器IP
            var elIP = rootNode.Element("wx.system.ip");
            if (null == elIP)
            {
                throw new WxPayException("必须配置商城IP地址");
            }
            //代理服务器IP地址
            var elProxyIP = rootNode.Element("wx.system.proxy.ip");
            //代理服务器端口
            var elProxyPort = rootNode.Element("wx.system.proxy.port");
            //日志输出级别
            var elLogLevel = rootNode.Element("wx.log.level");

            SSLCERT_PATH = null == elCertPath ? string.Empty : elCertPath.Value;
            SSLCERT_PASSWORD = null == elCertPwd ? string.Empty : elCertPwd.Value;
            IP = elIP.Value;
            if (null != elProxyIP && null != elProxyPort)
            {
                PROXY_URL = string.Concat("http://", elProxyIP.Value, ":", elProxyPort.Value);
            }
            LOG_LEVENL = null == elLogLevel || string.IsNullOrEmpty(elLogLevel.Value) ? 0 : int.Parse(elLogLevel.Value);

            #endregion
        }
    }
}
