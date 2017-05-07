using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiXin.Tool;

namespace WeiXin.Base.User.UserGroup
{
    /// <summary>
    /// 分组列表类
    /// </summary>
    public class GroupList
    {
        /// <summary>
        /// 分组列表
        /// </summary>
        public List<Group> groups;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sJsonData">所有分组json字符串</param>
        /// <returns></returns>
        public static GroupList Init(string sJsonData) {
            return JsonConvert.DeserializeObject<GroupList>(sJsonData);
        }

        /// <summary>
        /// 调用接口凭证
        /// </summary>
        /// <param name="sACCESS_TOKEN"></param>
        /// <returns></returns>
        public static GroupList Get(string sACCESS_TOKEN)
        {
            string sUrl = string.Format("https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}",sACCESS_TOKEN);
            var sResult = HttpHelper.Get(sUrl);
            JObject jResult = JObject.Parse(sResult);
            JToken jErrcode = jResult["errcode"];
            if (null != jErrcode && !jErrcode.ToString().Equals("0"))
            {
                return null;
            }
            else
            {
                return Init(sResult);
            }
        }
    }
}
