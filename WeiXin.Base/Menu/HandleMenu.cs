using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeiXin.Tool;

namespace WeiXin.Base.Menu
{
    /// <summary>
    /// 菜单操作类
    /// </summary>
   public class HandleMenu
    {
        /// <summary>
        /// 获取菜单数据
        /// </summary>
        /// <param name="sAccessToken">调用接口凭证</param>
        /// <returns></returns>
        public Menu Get(string sAccessToken)
        {
            Menu button = null;
            var sUrl = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", sAccessToken);
            //处理
            string sJsonData = HttpHelper.Get(sUrl);
            button = JsonConvert.DeserializeObject<Menu>(sJsonData);
            return button;
        }


        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="sAccessToken">调用接口凭证</param>
        /// <returns></returns>
        public bool Delete(string sAccessToken)
        {
            var sUrl = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", sAccessToken);
            var sResult = HttpHelper.Get(sUrl);
            JObject jResult = JObject.Parse(sResult);

            return jResult["errcode"].ToString().Equals("0");
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="sAccessToken">调用接口凭证</param>
        /// <param name="menuJson">菜单对象</param>
        /// <returns></returns>
        public bool Create(string sAccessToken, Menu button)
        {
            var sUrl = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", sAccessToken);
            var sJsonData = JsonConvert.SerializeObject(button);
            var sResult = HttpHelper.Post(sUrl, sJsonData);
            JObject jResult = JObject.Parse(sResult);

            return jResult["errcode"].ToString().Equals("0");
        }


        public bool Create(string sAccessToken, string JosnMenu)
        {
            var sUrl = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", sAccessToken);
            //var sJsonData = JsonConvert.SerializeObject(button);
            var sResult = HttpHelper.Post(sUrl, JosnMenu);
            JObject jResult = JObject.Parse(sResult);

            return jResult["errcode"].ToString().Equals("0");
        }
    }
}
