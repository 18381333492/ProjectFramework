using System.Web.Configuration;
using System.Configuration;
using System.Web;
namespace Onlinepay
{

    public class SDKConfig
    {

        //private static string signCertPath = ConfigurationManager.AppSettings["sdk.signCert.path"];  //功能：读取配置文件获取签名证书路径

        private static string validateCertDir = System.Web.HttpContext.Current.Server.MapPath("~/Lib/");//功能：读取配置文件获取验签目录
        private static string signCertPath = validateCertDir + ConfigurationManager.AppSettings["sdk.signCert.name"];


        private static string signCertPwd = ConfigurationManager.AppSettings["sdk.signCert.pwd"];//功能：读取配置文件获取签名证书密码
       // private static string validateCertDir = ConfigurationManager.AppSettings["sdk.validateCert.dir"];//功能：读取配置文件获取验签目录
    
        public static string encryptCert = ConfigurationManager.AppSettings["sdk.encryptCert.path"];  //功能：加密公钥证书路径
        private static string cardRequestUrl = ConfigurationManager.AppSettings["sdk.cardRequestUrl"];  //功能：有卡交易路径;
        private static string appRequestUrl = ConfigurationManager.AppSettings["sdk.appRequestUrl"];  //功能：appj交易路径;

        private static string singleQueryUrl = ConfigurationManager.AppSettings["sdk.singleQueryUrl"]; //功能：读取配置文件获取交易查询地址
        private static string fileTransUrl = ConfigurationManager.AppSettings["sdk.fileTransUrl"];  //功能：读取配置文件获取文件传输类交易地址
        private static string frontTransUrl = ConfigurationManager.AppSettings["sdk.frontTransUrl"]; //功能：读取配置文件获取前台交易地址
        private static string backTransUrl = ConfigurationManager.AppSettings["sdk.backTransUrl"];//功能：读取配置文件获取后台交易地址
        private static string batTransUrl = ConfigurationManager.AppSettings["sdk.batTransUrl"];//功能：读取配批量交易地址


        public static string CardRequestUrl
        {
            get { return SDKConfig.cardRequestUrl; }
            set { SDKConfig.cardRequestUrl = value; }
        }
        public static string AppRequestUrl
        {
            get { return SDKConfig.appRequestUrl; }
            set { SDKConfig.appRequestUrl = value; }
        }

        public static string FrontTransUrl
        {
            get { return SDKConfig.frontTransUrl; }
            set { SDKConfig.frontTransUrl = value; }
        }
        public static string EncryptCert
        {
            get { return SDKConfig.encryptCert; }
            set { SDKConfig.encryptCert = value; }
        }


        public static string BackTransUrl
        {
            get { return SDKConfig.backTransUrl; }
            set { SDKConfig.backTransUrl = value; }
        }

        public static string SingleQueryUrl
        {
            get { return SDKConfig.singleQueryUrl; }
            set { SDKConfig.singleQueryUrl = value; }
        }

        public static string FileTransUrl
        {
            get { return SDKConfig.fileTransUrl; }
            set { SDKConfig.fileTransUrl = value; }
        }

        public static string SignCertPath
        {
            get { return SDKConfig.signCertPath; }
            set { SDKConfig.signCertPath = value; }
        }

        public static string SignCertPwd
        {
            get { return SDKConfig.signCertPwd; }
            set { SDKConfig.signCertPwd = value; }
        }

        public static string ValidateCertDir
        {
            get { return SDKConfig.validateCertDir; }
            set { SDKConfig.validateCertDir = value; }
        }
        public static string BatTransUrl
        {
            get { return SDKConfig.batTransUrl; }
            set { SDKConfig.batTransUrl = value; }
        }

    }
}