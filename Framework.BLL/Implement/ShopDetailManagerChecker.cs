using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using System.Runtime.Remoting.Messaging;

namespace Framework.BLL
{
    /// <summary>
    /// Author:王其
    /// Description:部分类
    /// </summary>
    public partial class ShopDetailManager : IShopDetailManager
    {

        /// <summary>
        /// 插入分销的商品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override int InsertShareGoods(Dictionary<string, object> param)
        {
          

            // 获取参数
            EHECD_SharedGoodsDTO dto = new EHECD_SharedGoodsDTO() {

                ID = Helper.GuidHelper.GetSecuentialGuid(),
                sClientID = new Guid(Helper.CommonHelper.GetDictionaryValue("clientID", param, typeof(string)).ToString()),
                sGoodsID= new Guid(Helper.CommonHelper.GetDictionaryValue("goodsID", param, typeof(string)).ToString()),
                sShopID= new Guid(Helper.CommonHelper.GetDictionaryValue("shopID", param, typeof(string)).ToString()),
                iPirce= decimal.Parse(Helper.CommonHelper.GetDictionaryValue("price", param, typeof(string)).ToString()),
                bIsDeleted=false,
                dInsertTime=DateTime.Now

            };
            
            // 返回结果
            return excute.InsertSingle<EHECD_SharedGoodsDTO>(dto);
        }

        /// <summary>
        /// 检查当前分享的人员是否为分销商品所在店铺的分享客
        /// 1.是否是这家店的分销客 bIsShopShare 0否 1是
        /// 2.分销客ID
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> CheckUser(Dictionary<string, object> param)
        {
            // 返回结果
            var resulltDic = new Dictionary<string, object>();

            // 获取用户ID
            var sOpenId = Helper.CommonHelper.GetDictionaryValue("sOpenId", param, typeof(string));

            // 获取商品所在店铺ID
            var sShopID = Helper.CommonHelper.GetDictionaryValue("shopID",param,typeof(string));

            // 查询出当前用户是否是本商品所在店的分销客
            var result = query.SingleQuery<EHECD_ClientDTO>(@"SELECT
	                                                                A.ID --用户ID
                                                                FROM
	                                                                EHECD_Client A
                                                                WHERE
	                                                                sOpenId = @sOpenId
                                                                AND A.bIsDeleted = 0 --未删除
                                                                AND A.iState = 1 --正常（未冻结）
                                                                AND A.iClientType = 1 --分享客
                                                                AND A.sLoginName != '' --登录名不为空
                                                                AND A.sPassWord != '' --登录密码不为空
                                                                AND A.ID IN (
	                                                                SELECT
		                                                                B.sClientID --分享客ID
	                                                                FROM
		                                                                EHECD_SharedClientInfo B --分享客商品表
	                                                                WHERE
		                                                                B.bIsForzen = 0 --未冻结
	                                                                AND B.bIsDeleted = 0 --未删除
	                                                                AND B.sShopID = @sShopID --店铺ID
                                                                )", new { sOpenId = sOpenId.ToString(), sShopID= sShopID.ToString() });

            // 检查当前分享的人员是否为分销商品所在店铺的分享客
            if (result == null)
            {
                // 不是这家店的分销客
                resulltDic["bIsShopShare"] = 0;

                // 当前用户ID
                resulltDic["ID"] = "";

            } 
            else {

                // 是这家店的分销客
                resulltDic["bIsShopShare"] = 1;

                // 当前用户ID
                resulltDic["ID"] = result.ID;

            }

            // 返回结果
            return resulltDic;
        }
        /// <summary>
        /// 是否是分享客
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int IsShare(Dictionary<string, object> dic)
        {
            var i = 0;
            //先查询是否分销了此商品
            var good = query.SingleQuery<EHECD_SharedGoodsDTO>("SELECT * FROM EHECD_SharedGoods WHERE bIsDeleted=0 AND sClientID='" + dic["clientID"] + "' AND sGoodsID='" + dic["goodsID"] + "'", null);
            if (good != null)
            {
                i = 1;//i=1则表示分享过此商品
            }
            else {
                i = 2;
            }
            return i;
        }
        /// <summary>
        /// 查出分享客上级的信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_ClientDTO SearchUpShare(string ID)
        {
            return query.SingleQuery<EHECD_ClientDTO>("SELECT * FROM EHECD_Client WHERE sRegCode=@ID AND bIsDeleted=0 AND iState=1", new { ID = ID });
        }
        /// <summary>
        /// 所有下级的集合
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override IList<EHECD_ClientDTO> AllLower(string ID)
        {
            return query.QueryList<EHECD_ClientDTO>("SELECT * FROM EHECD_Client WHERE bIsDeleted=0 AND iState=1 AND sRegCode=@sRegCode", new { sRegCode = ID });
        }
    }
}
