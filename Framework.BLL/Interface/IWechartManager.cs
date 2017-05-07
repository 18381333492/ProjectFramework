using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
   public abstract class IWechartManager:BaseBll
    {
        /// <summary>
        /// 载入关注时的自动回复消息
        /// </summary>
        /// <param name="fromUserName">关注用户的openid</param>
        /// <param name="toUserName">公众平台的原始id</param>
        /// <param name="eventKey">如果是二维码扫码关注的这里是二维码内容</param>
        /// <returns></returns>
        public abstract Dictionary<string, object> LoadAttentionAutoReply(string fromUserName, string toUserName, string eventKey);
               
        /// <summary>
        /// 根据关键字和原始id查找回复内容
        /// </summary>
        /// <param name="toUserName">微信公众号原始ID</param>
        /// <param name="keyword">关键字</param>  
        /// <param name="sContentType">回复类型：0文本 1图文</param>
        /// <returns>结果</returns>
        /// <Author>杨瑜堃</Author>
        /// <CreateTime>2016-11-11</CreateTime>
        public abstract List<EHECD_WeCharReplyDTO> GetKeyWordMessage(string toUserName, string keyword,out int sContentType);

        /// <summary>
        /// 根据店铺ID获取微信配置
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns>微信配置</returns>
        /// <Author>杨瑜堃</Author>
        /// <CreateTime>2016-11-08</CreateTime>
        public abstract EHECD_WeChatSetDTO GetSetByShopID(EHECD_SystemUserDTO user);

        /// <summary>
        /// 根据店铺ID获取微信菜单
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns>微信菜单配置集合</returns>
        /// <Author>杨瑜堃</Author>
        /// <CreateTime>2016-11-08</CreateTime>
        public abstract IList<EHECD_WeChartMenuDTO> GetAllMenuByShopID(EHECD_SystemUserDTO user);

        /// <summary>
        /// 获取微信设置
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_WeChatSetDTO GetSet(string ID);
        /// <summary>
        /// 保存微信设置
        /// </summary>
        public abstract int WeChartSet(EHECD_WeChatSetDTO dto,EHECD_SystemUserDTO user);
        /// <summary>
        /// 获取关注回复的信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract IList< EHECD_WeCharReplyDTO> GetFollowReply(string ID,Dictionary<string,object> dic);


        /// <summary>
        /// 设置关注回复
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract int FollowReplySet(EHECD_WeCharReplyDTO dto, EHECD_SystemUserDTO user,Dictionary<string,object> dic);
        /// <summary>
        /// 关键字回复页面绑定
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dic"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string,object>> GetPageList(PageInfo info,Dictionary<string, object> dic,EHECD_SystemUserDTO user);
        /// <summary>
        /// 添加关键字回复
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract int KeyApplyAdd(EHECD_WeCharReplyDTO dto, EHECD_SystemUserDTO user,Dictionary<string,object> dic);
        /// <summary>
        /// 根据ID查看关键字回复的信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_WeCharReplyDTO GetKeyReply(string ID);
        
        /// <summary>
        /// 修改关键字回复
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract int EditKeyReply(EHECD_WeCharReplyDTO dto, EHECD_SystemUserDTO user, Dictionary<string, object> dic);
        /// <summary>
        /// 删除关键字回复
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract int DeleteKeyReply(Dictionary<string, object> dic);
        /// <summary>
        /// 自动回复设置
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract int AutoReplySet(EHECD_WeCharReplyDTO dto, EHECD_SystemUserDTO user);
        /// <summary>
        /// 根据ID获取自动回复设置信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_WeCharReplyDTO GetAutoReply(string ID);
        /// <summary>
        /// 开启/关闭自动回复
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract int ChangeStates(bool state,string ID);
        /// <summary>
        /// 获取关键字回复状态
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_WechartReplyTypeDTO GetStates(string ID);
        /// <summary>
        /// 微信菜单页面绑定
        /// </summary>
        /// <param name="info"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_WeChartMenuDTO> GetMenuList(PageInfo info, EHECD_SystemUserDTO user);
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract int AddMenu(EHECD_WeChartMenuDTO dto, EHECD_SystemUserDTO user);
        /// <summary>
        /// 获取所有的菜单
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract string GetAllMenu(EHECD_SystemUserDTO user);
        /// <summary>
        /// 查看是否有重名的菜单名
        /// </summary>
        /// <param name="menuname"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract EHECD_WeChartMenuDTO SearchMenu(string menuname, EHECD_SystemUserDTO user);
        /// <summary>
        /// 根据ID查看详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_WeChartMenuDTO SearchDetail(string ID);
        /// <summary>
        /// 修改菜单信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int EditMenu(EHECD_WeChartMenuDTO dto);
        /// <summary>
        /// 是否是父菜单
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_WeChartMenuDTO IsFatherMenus(string ID);
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract int DeleteMenus(Dictionary<string, object> dic);
        /// <summary>
        /// 查看关键字回复是否有重名
        /// </summary>
        /// <param name="sKeyword"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract EHECD_WeCharReplyDTO SearchKeyName(Dictionary<string,object> dic, EHECD_SystemUserDTO user);

        /// <summary>
        /// 是否开启以及本文类型
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract EHECD_WechartReplyTypeDTO GetOn(Dictionary<string, object> dic);
        /// <summary>
        /// 根据店铺ID获取原始ID
        /// </summary>
        /// <param name="ShopID"></param>
        /// <returns></returns>
        public abstract EHECD_WeChatSetDTO GetsOriginalID(string ShopID);
        /// <summary>
        /// 更改回复的内容是文本或者是图文
        /// </summary>
        /// <param name="ContentType"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract int ChangeContentType(EHECD_WechartReplyTypeDTO dto, string ID);

        /// <summary>
        /// 关键字信息
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract IList<EHECD_WeCharReplyDTO> GetKeyWordByType(Dictionary<string, object> dic);
        /// <summary>
        /// 父类菜单有多少
        /// </summary>
        /// <param name="ShopID"></param>
        /// <returns></returns>
        public abstract int GetFatherMenuNumber(string ShopID);
    }
}
