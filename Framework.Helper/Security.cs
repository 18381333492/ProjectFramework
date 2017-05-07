using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helper
{
    /// <summary>
    /// 安全类
    /// 加密码与解密操作(DES)
    /// </summary>
    public class Security
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string input = "123")
        {
            MD5 md5Hash = MD5.Create();
            // 将传入的字符串转为字节数组并计算其哈希值
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            //遍历字节数组将其哈希码转为十六进制字符串
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // 返回计算的十六进制字符串
            return sBuilder.ToString().ToUpper();
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetSHA1Hash(string input)
        {
            SHA1 shaHash = SHA1.Create();
            byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString().ToUpper();
            //return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(input, "SHA1");
        }

        /// <summary>
        /// 获取签名(key)
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string GetSign(string sign, string signKey)
        {
            return Security.GetMD5Hash(sign.ToUpper() + signKey).ToUpper();
        }

        /// <summary>
        /// json数据加密
        /// </summary>
        /// <param name="responseJsonString">要加密的json数据</param>
        /// <returns></returns>
        public static byte[] EncryptionResponse(string responseJsonString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(responseJsonString);

            Parallel.For(0, bytes.Length, index =>
            {
                var temp = bytes[index];
                var b = (Convert.ToString(temp, 2).PadLeft(8, '0')).ToArray();

                char[] tb = new char[7];
                Array.Copy(b, 1, tb, 0, tb.Length);

                Parallel.For(0, tb.Length, tempIndex =>
                {

                    tb[tempIndex] = tb[tempIndex] == '0' ? '1' : '0';
                });

                Array.Copy(tb, 0, b, 1, tb.Length);
                string sb = new string(b);
                bytes[index] = Convert.ToByte(sb, 2);
            });

            return bytes;
        }

        /// <summary>
        /// 请求数据解密
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static string DecryptRequest(byte[] requestData)
        {
            string encode = Encoding.UTF8.GetString(requestData);

            Parallel.For(0, requestData.Length, index =>
            {
                var temp = requestData[index];
                var b = (Convert.ToString(temp, 2).PadLeft(8, '0')).ToArray();

                char[] tb = new char[7];
                Array.Copy(b, 1, tb, 0, tb.Length);

                Parallel.For(0, tb.Length, tempIndex =>
                {
                    tb[tempIndex] = tb[tempIndex] == '1' ? '0' : '1';
                });

                Array.Copy(tb, 0, b, 1, tb.Length);
                string sb = new string(b);
                requestData[index] = Convert.ToByte(sb, 2);
            });

            return Encoding.UTF8.GetString(requestData);
        }

        #region base64算法

        /// <summary> 
        /// Base64加密 
        /// </summary> 
        /// <param name="codeName">加密采用的编码方式</param> 
        /// <param name="source">待加密的明文</param> 
        /// <returns></returns> 
        public static string EncodeBase64(Encoding encode, string source)
        {
            string base64 = "";
            byte[] bytes = encode.GetBytes(source);
            try
            {
                base64 = Convert.ToBase64String(bytes);
            }
            catch
            {
                base64 = source;
            }
            return base64;
        }
        /// <summary> 
        /// Base64解密 
        /// </summary> 
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param> 
        /// <param name="result">待解密的密文</param> 
        /// <returns>解密后的字符串</returns> 
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary> 
        /// Base64加密，采用utf8编码方式加密 
        /// </summary> 
        /// <param name="source">待加密的明文</param> 
        /// <returns>加密后的字符串</returns> 
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }
        /// <summary> 
        /// Base64解密，采用utf8编码方式解密 
        /// </summary> 
        /// <param name="result">待解密的密文</param> 
        /// <returns>解密后的字符串</returns> 
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }

        public static string GetBASE64Hash(string input)
        {
            //return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(input, "BASE64");
            return Encoding.Default.GetString(System.Convert.FromBase64String(input));
        }

        #endregion
    }
}
