using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using WeiXin.Tool;

namespace WeiXin.Base.User.UserGroup
{
    /// <summary>
    /// <para>微信用户分组类，包括分组id，分组名字name，分组用户数量count</para>
    /// <para>注意：</para>
    /// <para>   1.微信默认有三个分组,例：</para>
    /// <code>
    ///  {
    ///     "id": 0, 
    ///     "name": "未分组", 
    ///     "count": 0
    ///   }, 
    ///  {
    ///      "id": 1, 
    ///      "name": "黑名单", 
    ///      "count": 0
    ///   }, 
    ///   {
    ///      "id": 2, 
    ///      "name": "星标组", 
    ///      "count": 0
    ///   }
    /// </code>
    /// <para>  2.微信自定义的分组id以100开始。</para>  
    /// </summary>
    public class Group
    {
        /// <summary>
        /// 用户操作分组信息实例
        /// </summary>
        public IAction action { get; set; }
        [JsonIgnore]
        private string _name;

        /// <summary>
        /// 分组id，由微信分配
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 分组名字，UTF8编码
        /// </summary>
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value.Length > 30)
                {
                    throw new Exception("分组名字（30个字符以内）");
                }
                _name = value;
            }
        }

        /// <summary>
        /// 分组内用户数量
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 创建分组信息
        /// </summary>
        /// <param name="sACCESS_TOKEN"></param>
        /// <returns></returns>
        public  bool Create(string sACCESS_TOKEN,string sName) {
            //此处业务逻辑是调用公众平台创建公众号用户分组接口
            string sUrl = string.Format("https://api.weixin.qq.com/cgi-bin/groups/create?access_token={0}", sACCESS_TOKEN);
            string sJsonData = "{\"group\":{\"name\":\"" + sName + "\"}}";
            var sResult = HttpHelper.Post(sUrl, sJsonData);
            JObject jResult = JObject.Parse(sResult);
            JToken jErrcode = jResult["errcode"];

            //若返回的字符串中不包含错误代码，则创建成功，否则失败
            bool bResult = null == jErrcode;
            //bool bResult = jErrcode.ToString().Equals("0");            
            //此处逻辑是 判断是否创建用户分组成功，并且保存到数据库中的业务处理实例是否为空。若不是，则调用
            if (bResult && null!=action)
            {
                Group group = JsonConvert.DeserializeObject<Group>(JObject.Parse(sResult)["group"].ToString());
                action.Create(group);
            }
            return bResult;
        }

        /// <summary>
        /// 修改分组名字
        /// </summary>
        /// <param name="sACCESS_TOKEN">调用接口凭证</param>
        /// <param name="group">需要修改的分组信息</param>
        /// <returns></returns>
        public  bool Update(string sACCESS_TOKEN, Group group)
        {
            //此处业务逻辑是调用公众平台修改公众号用户分组名接口
            string sUrl = string.Format("https://api.weixin.qq.com/cgi-bin/groups/update?access_token={0}", sACCESS_TOKEN);
            string sJsonData = "{\"group\":{\"id\":" + group.id + ",\"name\":\"" + group.name + "\"}}";
            //向修改分组名接口post一串json数据
            var sResult = HttpHelper.Post(sUrl, sJsonData);
            JObject jResult = JObject.Parse(sResult);
            JToken jErrcode = jResult["errcode"];
            //若返回的错误代码为0，则修改成功，否则修改失败
            bool bResult = jErrcode.ToString().Equals("0");
            //此处逻辑是 判断是否修改用户分组名成功，并且保存到数据库中的业务处理实例是否为空，若不是，则调用
            if (bResult && null != action)
            {
                action.Update(group);
            }
            return bResult;
        }

        /// <summary>
        /// 查询用户所在分组
        /// </summary>
        /// <param name="sACCESS_TOKEN">调用接口凭证</param>
        /// <param name="sOpenId">用户的标识，对当前公众号唯一</param>
        /// <param name="isSuccess">是否得到分组ID。true-成功，false-失败；</param>
        /// <returns>返回错误代码或分组id</returns>
        public int Query(string sACCESS_TOKEN, string sOpenId, out bool isSuccess)
        {
            isSuccess = false;
            //微信公众平台获取用户所在分组post地址
            string sUrl = string.Format("https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}", sACCESS_TOKEN);
            //构建查询分组id的json字符串
            string sJsonData = "{\"openid\":\"" + sOpenId + "\"}";
            //此处业务逻辑为post数据到微信公众平台进行查询，直接返回分组信息json或错误代码json字符串
            var sResult = HttpHelper.Post(sUrl, sJsonData);
            JObject jResult = JObject.Parse(sResult);
            JToken jErrcode = jResult["errcode"];
            //判断是否获取分组ID成功
            isSuccess = null == jErrcode;
            if (isSuccess)
            {//返回分组id
                return Convert.ToInt32(jResult["groupid"].ToString());
            }
            else
            {//返回错误代码
                return Convert.ToInt32(jErrcode.ToString());
            }
        }

        /// <summary>
        /// 移动用户到分组中
        /// </summary>
        /// <param name="sACCESS_TOKEN">调用接口凭证</param>
        /// <param name="sOpenId">用户的标识</param>
        /// <param name="iGroupId">分组id</param>
        /// <returns></returns>
        public  bool Move(string sACCESS_TOKEN, string sOpenId, int iGroupId)
        {
            //微信公众平台移动用户分组post地址
            string sUrl = string.Format("https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token={0}", sACCESS_TOKEN);
            //构建移动用户到分组的json字符串
            string sJsonData = "{\"openid\":\"" + sOpenId + "\",\"to_groupid\":" + iGroupId + "}";
            //此处业务逻辑为，post数据到移动用户分组的post地址，取得返回结果，根据错误代码判断是否成功移动
            var sResult = HttpHelper.Post(sUrl, sJsonData);
            JObject jResult = JObject.Parse(sResult);
            JToken jErrcode = jResult["errcode"];
            bool bResult =jErrcode.ToString().Equals("0");

            //若操作成功，并且处理移动实例对象不为空
            if (bResult && null != action) {
                //调用用户自定义的移动方法
                action.Move(sOpenId, iGroupId);
            }
            return bResult;
        }
    }
}
