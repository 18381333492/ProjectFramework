using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helper
{
    public class CommonHelper
    {
        /// <summary>
        /// 创建系统日志信息
        /// </summary>
        /// <param name="dyP">动态参数</param>
        /// <param name="Ip">ip地址</param>
        /// <param name="sLoginName">登录名</param>
        /// <param name="sUserName">用户名</param>
        public static void CreateSyslogInfo(ref dynamic dyP, string Ip, string sLoginName, string sUserName)
        {
            if (dyP == null) dyP = new { };
            dyP.dynamicData.sLoginName = sLoginName;
            dyP.dynamicData.sUserName = sUserName;
            dyP.dynamicData.IP = Ip == "::1" ? "127.0.0.1" : Ip;
        }

        /// <summary>
        /// 获取字典的值
        /// </summary>        
        /// <param name="key">key</param>
        /// <param name="dic">字典</param>
        /// <param name="outType">输出的类型</param>
        /// <returns>值</returns>
        public static object GetDictionaryValue(string key, IDictionary<string, object> dic,Type outType)
        {
            if (dic.Keys.Contains(key))
            {
                object value = new object();
                var ret = dic.TryGetValue(key, out value);
                if (ret)
                {
                    try
                    {
                        string typeName = outType.Name.ToLower();

                        string valueType = value.GetType().Name.ToLower();

                        if (typeName.IndexOf("string") >= 0 || typeName.IndexOf("char") >= 0)
                        {
                            return value.ToString().Replace("'","''");

                        }else if (typeName.IndexOf("int") >= 0)
                        {
                            if(valueType == "int16") return Convert.ToInt16(value);
                            
                            if(valueType == "int32") return Convert.ToInt32(value);
                            
                            if(valueType == "int64") return Convert.ToInt64(value);

                            if (valueType == "uint16") return Convert.ToUInt16(value);

                            if (valueType == "uint32") return Convert.ToUInt32(value);

                            if (valueType == "uint64") return Convert.ToUInt64(value);
                        }
                        else if(typeName.IndexOf("guid")>=0)
                        {
                            return Guid.Parse(value.ToString());

                        }else if(typeName.IndexOf("double") >= 0 || typeName.IndexOf("float") >= 0)
                        {
                            return Convert.ToDouble(value);
                            
                        }
                        else if (typeName.IndexOf("decimal")>=0)
                        {
                            return Convert.ToDecimal(value);

                        }
                        else if (typeName.IndexOf("datetime") >= 0)
                        {
                            return DateTime.Parse(value.ToString());

                        }
                        else if (typeName.IndexOf("sbyte") >= 0)
                        {
                            return Convert.ToSByte(value);

                        }
                        else if (typeName.IndexOf("byte") >= 0)
                        {
                            return Convert.ToByte(value);

                        }
                        else if (typeName.IndexOf("boolean") >= 0)
                        {
                            return Convert.ToBoolean(value);

                        }
                        return null;
                    }
                    catch 
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
