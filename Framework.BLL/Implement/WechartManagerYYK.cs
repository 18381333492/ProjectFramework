using Framework.DTO;
using Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
    public partial class WechartManager : IWechartManager
    {
        /// <summary>
        /// 载入关注时的自动回复消息
        /// </summary>
        /// <param name="fromUserName">关注用户的openid</param>
        /// <param name="toUserName">公众平台的原始id</param>
        /// <param name="eventKey">如果是二维码扫码关注的这里是二维码内容</param>
        /// <returns></returns>
        public override Dictionary<string, object> LoadAttentionAutoReply(string fromUserName, string toUserName, string eventKey)
        {
            // 1.判断对应的原始id的公众号对关注回复是否启用
            var state = query.SingleQuery<EHECD_WechartReplyTypeDTO>("SELECT * FROM EHECD_WechartReplyType WHERE sOriginalID = @toUserName AND sReplyType = 0 AND sState = 1 AND sReplyType = 0;", new { toUserName });

            var ret = new Dictionary<string, object>();

            if (null != state)
            {
                ret.Add("set", state);

                // 已开启，获取对应的回复数据
                var reply = query.QueryList<EHECD_WeCharReplyDTO>("SELECT * FROM EHECD_WeCharReply WHERE sReplyType = 0 AND sContentType = @type AND bIsDeleted = 0 AND sShopID = @sShopID", new
                {
                    type = state.sContentType.ToBoolean() ? 1 : 0,
                    sShopID = state.sShopID
                }).ToList();

                ret.Add("ret", reply);

                return ret;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据原始ID判断公众号是否启用自动回复
        /// </summary>
        /// <param name="toUserName">公众号原始ID</param>
        /// <returns>结果</returns>
        /// <Author>杨瑜堃</Author>
        /// <CreateTime>2016-11-11</CreateTime>
        public EHECD_WechartReplyTypeDTO CheckWeChartIsOpenReply(string toUserName, int sReplyType)
        {
            return query.SingleQuery<EHECD_WechartReplyTypeDTO>("SELECT * FROM  EHECD_WechartReplyType WHERE sOriginalID = @sOriginalID AND sReplyType = @sReplyType;", new { sOriginalID = toUserName, sReplyType });
        }

        /// <summary>
        /// 根据关键字和原始id查找回复内容,如果找不到对应的关键字，就回复
        /// </summary>
        /// <param name="toUserName">微信公众号原始ID</param>
        /// <param name="keyword">关键字</param>        
        /// <param name="sContentType">回复类型：0文本 1图文</param>
        /// <returns>结果</returns>
        /// <Author>杨瑜堃</Author>
        /// <CreateTime>2016-11-11</CreateTime>
        public override List<EHECD_WeCharReplyDTO> GetKeyWordMessage(string toUserName, string keyword, out int sContentType)
        {
            // 根据原始id获取这个公众号的消息配置，默认先取关键字回复的配置
            var state = CheckWeChartIsOpenReply(toUserName, 1);

            if (null != state)
            {
                sContentType = state.sContentType.ToBoolean() ? 1 : 0;

                // 判断回复功能是否开启
                if (state.sState.ToBoolean())
                {
                    // 1.根据关键字查询关键字回复的数据
                    var ret = query.QueryList<EHECD_WeCharReplyDTO>(@"SELECT
	                                                                    *
                                                                    FROM
	                                                                    EHECD_WeCharReply
                                                                    WHERE
                                                                    sKeyword = @keyword
                                                                    AND sShopID = @sShopID
                                                                    AND bIsDeleted = 0
                                                                    AND sReplyType = 1;", new {keyword, sShopID = state.sShopID }).ToList();

                    // 2.如果没有根据关键字查找到数据，就返回自动回复信息
                    if (null != ret && ret.Count > 0)
                    {
                        sContentType = ret[0].sContentType.Value;
                        return ret;
                    }
                    else
                    {
                        // 根据原始id获取这个公众号的消息配置，这里就去获取自动回复的配置
                        state = CheckWeChartIsOpenReply(toUserName, 2);
                        sContentType = 0;
                        // 判断配置是否存在和是否开启了自动回复
                        if (null == state || state.sState.ToBoolean() == false) return new List<EHECD_WeCharReplyDTO>();
                        sContentType = state.sContentType.ToBoolean() ? 1 : 0;
                        ret = query.QueryList<EHECD_WeCharReplyDTO>(@"SELECT
	                                                                    *
                                                                    FROM
	                                                                    EHECD_WeCharReply
                                                                    WHERE
	                                                                    sContentType = @sContentType
                                                                    AND sShopID = @sShopID
                                                                    AND bIsDeleted = 0
                                                                    AND sReplyType = 2;", new { sContentType, sShopID = state.sShopID }).ToList();

                        return null != ret && ret.Count > 0 ? ret : new List<EHECD_WeCharReplyDTO>();
                    }
                }
                else
                {
                    //如果未开启关键字则自动回复
                    // 根据原始id获取这个公众号的消息配置，这里就去获取自动回复的配置
                    state = CheckWeChartIsOpenReply(toUserName, 2);
                    sContentType = 0;
                    // 判断配置是否存在和是否开启了自动回复
                    if (null == state || state.sState.ToBoolean() == false) return new List<EHECD_WeCharReplyDTO>();
                    sContentType = state.sContentType.ToBoolean() ? 1 : 0;
                    var ret = query.QueryList<EHECD_WeCharReplyDTO>(@"SELECT
	                                                                    *
                                                                    FROM
	                                                                    EHECD_WeCharReply
                                                                    WHERE
	                                                                    sContentType = @sContentType
                                                                    AND sShopID = @sShopID
                                                                    AND bIsDeleted = 0
                                                                    AND sReplyType = 2;", new { sContentType, sShopID = state.sShopID }).ToList();

                    return null != ret && ret.Count > 0 ? ret : new List<EHECD_WeCharReplyDTO>();
                    //// 未开启的话就返回一个空的消息集合
                    //return new List<EHECD_WeCharReplyDTO>();
                }
            }
            else
            {
                // 根据原始id获取这个公众号的消息配置，这里就去获取自动回复的配置
                state = CheckWeChartIsOpenReply(toUserName, 2);

                sContentType = 0;

                // 判断配置是否存在和是否开启了自动回复
                if (null == state || state.sState.ToBoolean() == false) return new List<EHECD_WeCharReplyDTO>();

                sContentType = state.sContentType.ToBoolean() ? 1 : 0;

                var ret = query.QueryList<EHECD_WeCharReplyDTO>(@"SELECT
	                                                                    *
                                                                    FROM
	                                                                    EHECD_WeCharReply
                                                                    WHERE
	                                                                    sContentType = @sContentType
                                                                    AND sShopID = @sShopID
                                                                    AND bIsDeleted = 0
                                                                    AND sReplyType = 2;", new { sContentType, sShopID = state.sShopID }).ToList();

                return null != ret && ret.Count > 0 ? ret : new List<EHECD_WeCharReplyDTO>();
            }
        }

        /// <summary>
        /// 根据店铺ID获取微信配置
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns>微信配置</returns>
        /// <Author>杨瑜堃</Author>
        /// <CreateTime>2016-11-08</CreateTime>
        public override EHECD_WeChatSetDTO GetSetByShopID(EHECD_SystemUserDTO user)
        {
            var ret = query.SingleQuery<EHECD_WeChatSetDTO>("SELECT * FROM EHECD_WeChatSet WHERE sShopID = @sShopID", new { sShopID = user.ID });
            return ret;
        }

        /// <summary>
        /// 根据店铺ID获取微信菜单
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns>微信菜单配置集合</returns>
        /// <Author>杨瑜堃</Author>
        /// <CreateTime>2016-11-08</CreateTime>
        public override IList<EHECD_WeChartMenuDTO> GetAllMenuByShopID(EHECD_SystemUserDTO user)
        {
            var ret = query.QueryList<EHECD_WeChartMenuDTO>("SELECT * FROM EHECD_WeChartMenu WHERE bIsDeleted = 0 AND sShopID = @sShopID", new { sShopID = user.ID });
            return ret;
        }
    }
}
