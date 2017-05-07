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
    /// 分享中心
    /// </summary>
    public abstract class IShareCenterManager : BaseBll
    {

        /// <summary>
        /// 根据会员ID获取分享客的相关信息
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public abstract object GetShareInfo(Guid ClientId);



        /// <summary>
        /// 获取分享客会员
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="type">0-全部，1：一级，2：二级，3：三级</param>
        ///  <param name="info"></param>
        /// <returns></returns>
        public abstract object GetMember(Guid ClientId, int type, PageInfo info);



        /// <summary>
        /// 获取本月新增会员数据列表
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract object GetNewlyMember(Guid ClientId, PageInfo info);



        /// <summary>
        /// 获取分享客的收支明细数据列表
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract object PaymentDetail(Guid ClientId, PageInfo info);



        /// <summary>
        /// 获取提现记录
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract object GetCashRecord(Guid ClientId, PageInfo info);


        /// <summary>
        /// 获取结算中心的相关信息
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public abstract object GetAccountCenterInfo(Guid ClientId);


        /// <summary>
        /// 获取分享客店铺的数据列表
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="Info"></param>
        /// <param name="bIsShare">是否分享 0-未分享，1-已分享</param>
        /// <param name="sKeyword">店铺名字</param>
        /// <returns></returns>
        public abstract object GetShopManagement(Guid ClientId, PageInfo Info, int bIsShare, string sKeyword);


        /// <summary>
        /// 获取店铺下面的商品列表
        /// </summary>
        /// <param name="sShopId"></param>
        /// <param name="Info"></param>
        /// <param name="sGoodsName"></param>
        /// <returns></returns>
        public abstract object GetShopGoodsList(Guid sShopId, PageInfo Info, string sGoodsName);


        /// <summary>
        /// 获取分享海报管理图片
        /// </summary>
        /// <returns></returns>
        public abstract string GetSharePosterImg();



        /// <summary>
        /// 获取分享客的可提现金额
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public abstract decimal GetClientMoney(Guid ClientId);


        #region 操作接口

        /// <summary>
        /// 分享店铺
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int ShareShop(EHECD_SharedClientInfoDTO model);


        /// <summary>
        /// 取消店铺的分享
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="sShopId"></param>
        /// <returns></returns>
        public abstract int CancelShare(Guid ClientId, Guid sShopId);


        /// <summary>
        /// 扫码成为下级
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public abstract int IntoMember(EHECD_ClientDTO client, out string sNickName, out string sPhone);


        /// <summary>
        /// 分享客的提现
        /// </summary>
        /// <param name="cash"></param>
        /// <param name="sResult">返回的提示信息</param>
        /// <returns></returns>
        public abstract int WithdrawCashHandle(EHECD_WithdrawCashDTO cash, out string sResult);


        /// <summary>
        /// 分享客分享商品
        /// </summary>
        /// <param name="sClientID"></param>
        /// <param name="sGoodsId"></param>
        /// <returns></returns>
        public abstract int ShareGoods(Guid sClientID, Guid sGoodsId);



        /// <summary>
        /// 获取会员的上三级会员
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public abstract List<clientData> Gethigher(Guid ClientID);

        #endregion

    }
}
