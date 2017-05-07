using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
    /// <summary>
    /// 会员中心
    /// </summary>
    public abstract class IClientCenterManager : BaseBll
    {

        #region 查询相关

        /// <summary>
        /// 根据会员主键ClientId获取会员的头像
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public abstract object GetClientNickName(Guid ClientId);


        /// <summary>
        /// 根据会员主键ClientId获取会员个人信息
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public abstract object GetClientInfo(Guid ClientId);


        /// <summary>
        /// 根据会员主键ClientId获取待付款和待付款订单数目
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public abstract object GetPayUseOrderCount(Guid ClientId);


        /// <summary>
        /// 根据会员主键ClientId获取站内信未读条数
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public abstract object GetNoReadMessageCount(Guid ClientId);


        /// <summary>
        /// 分页获会员取站内信数据列表
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public abstract object GetSysMessageList(Guid ClientId, PageInfo info);


        /// <summary>
        /// 根据ClientId获取站内信的详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract Dictionary<string,object> GetSysMessageDetail(Guid ID);


        /// <summary>
        /// 根据状态获取相应的订单的数据列表
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="iState"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract object GetOrderListByState(Guid ClientId, int iState, PageInfo info);

        /// <summary>
        /// 根据订单ID获取订单详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract object GetOrderDetail(Guid ID);


        /// <summary>
        /// 根据会员ClientId获取会员所有订单
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public abstract object GetAllOrder(Guid ClientId, PageInfo info);


        /// <summary>
        /// 根据订单ID获取订单商品评价
        /// </summary>
        /// <param name="sOrderId"></param>
        /// <returns></returns>
        public abstract Dictionary<string,object> GetAppraise(Guid sOrderId);

        /// <summary>
        /// 根据会员ID获取收藏夹
        /// </summary>
        /// <param name="Info"></param>
        /// <param name="ClientId"></param>
        /// <param name="iCollectType">收藏类型:0-店铺，1-客房，2-票务，3-周边</param>
        /// <returns></returns>
        public abstract object GetCollectionList(PageInfo Info, Guid ClientId, int iCollectType);


        /// <summary>
        /// 获取会员优惠卷数据列表
        /// </summary>
        /// <param name="Info"></param>
        /// <param name="ClientId"></param>
        /// <param name="bIsUsed">是否使用(0-否 1-是)</param>
        /// <returns></returns>
        public abstract object GetCouponList(PageInfo Info, Guid ClientId, int bIsUsed);


        /// <summary>
        /// 获取会员的优惠卷
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public abstract object GetCouponList(Guid ClientId);


        /// <summary>
        /// 获取帮助中心数据列表
        /// </summary>
        /// <returns></returns>
        public abstract object GetHelpCenterList();


        /// <summary>
        /// 根据ID获取帮助中心详情
        /// </summary>
        /// <returns></returns>
        public abstract object GetHelpCenterDetail(Guid ID);

        /// <summary>
        /// 根据标题获取协议
        /// </summary>
        /// <param name="sTitle"></param>
        /// <returns></returns>
        public abstract object GetProtocolByTitle(string sTitle);

        /// <summary>
        /// 根据电话判断用户是否是合伙人以及状态
        /// </summary>
        /// <param name="sPhone"></param>
        /// <param name="iState">0-未通过 1-已通过 2-拒绝</param>
        /// <returns></returns>
        public abstract bool IsPartner(string sPhone, out int iState);


        /// <summary>
        /// 根据电话判断用户是否是商家以及申请状态
        /// </summary>
        /// <param name="sPhone"></param>
        /// <param name="iState">0-未通过 1-已通过 2-拒绝</param>
        /// <returns></returns>
        public abstract bool IsBusiness(string sPhone, out int iState);



        /// <summary>
        /// 根据电话号码检查该用户是否是商家，合伙人,或者平台用户
        /// </summary>
        /// <param name="sPhone"></param>
        /// <returns></returns>
        public abstract int Check(string sPhone);
        #endregion




        #region 操作相关

        /// <summary>
        /// 设置站内信为为以读
        /// </summary>
        /// <param name="ID"></param>
        public abstract int SetiRecStatus(Guid ID);

        /// <summary>
        /// 成为分享客
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public abstract int IntoShareClient(Guid ClientId);

        /// <summary>
        /// 成为合伙人
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int IntoPartner(EHECD_ApplyDTO model);


        /// <summary>
        /// 成为商家
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int IntoBusiness(EHECD_ApplyDTO model);


        /// <summary>
        /// 评价订单
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public abstract int OrderAppraise(EHECD_CommentDTO comment);

        /// <summary>
        /// 根据订单ID取消订单
        /// </summary>
        /// <param name="sOrderId"></param>
        /// <returns></returns>
        public abstract int OrderCancel(Guid sOrderId);


        /// <summary>
        /// 申请订单退款
        /// </summary>
        /// <param name="sOrderId"></param>
        /// <returns></returns>
        public abstract int OrderReturn(Guid sOrderId);

        /// <summary>
        /// 根据ID删除收藏夹
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public abstract int CancelCollect(string Ids);



        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="sNickName"></param>
        /// <param name="sHeadPic"></param>
        /// <returns></returns>
        public abstract int AlertInfo(Guid ClientId, string sNickName, string sHeadPic);

        #endregion

    }
}
