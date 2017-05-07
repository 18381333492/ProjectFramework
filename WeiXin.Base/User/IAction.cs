
namespace WeiXin.Base.User
{
    /// <summary>
    /// 用户管理操作接口
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// 保存微信用户信息
        /// </summary>
        /// <param name="userinfo">用户</param>
        /// <param name="sWeixinId">公众号id</param>
        /// <param name="iInviteid">邀请人ID</param>
        /// <returns></returns>
        bool Add(UserInfo.UserInfo userinfo, string sWeixinId, string iInviteid="");

        /// <summary>
        /// 保存微信分组信息
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        bool Create(UserGroup.Group group);

        /// <summary>
        /// 修改微信分组信息
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        bool Update(UserGroup.Group group);

        /// <summary>
        /// 移动用户分组
        /// </summary>
        /// <param name="sOpenId">微信用户标识</param>
        /// <param name="iGroupId">分组ID</param>
        /// <returns></returns>
        bool Move(string sOpenId, int iGroupId);
    }
}
