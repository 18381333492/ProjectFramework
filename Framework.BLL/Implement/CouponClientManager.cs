using Framework.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;

namespace Framework.BLL
{
    /// <summary>
    /// 优惠劵静态管理类（针对管理用户领取优惠劵）
    /// </summary>
    public static class CouponClientManager
    {

        private static Dapper.QueryHelper query = DIEntity.GetInstance().GetImpl<Dapper.QueryHelper>();
        private static Dapper.ExcuteHelper excute = DIEntity.GetInstance().GetImpl<Dapper.ExcuteHelper>();

        private static Object lockObj = new object();

        /// <summary>
        /// 领取优惠券
        /// </summary>
        /// <returns>操作结果</returns>
        public static int GetCoupon(Dictionary<string, object> param)
        {
            lock (lockObj)
            {
                var receiver = Helper.CommonHelper.GetDictionaryValue("sUserID", param, typeof(string));//领取人ID
                var sCouponID = Helper.CommonHelper.GetDictionaryValue("sCouponID", param, typeof(string));//领取的优惠劵ID
                var userName = Helper.CommonHelper.GetDictionaryValue("sUserName",param,typeof(string));//领取人登录名
                // 1.查询剩余优惠券
                var count = 0;
                var shengyuCoupon = query.SingleQuery<Dictionary<string,object>>(@"SELECT
			                                                                    sCouponID,
			                                                                    COUNT (sCouponID) couponCount
		                                                                    FROM
			                                                                    EHECD_CouponDetails WHERE sCouponID=@sCouponID
		                                                                    GROUP BY
			                                                                    sCouponID
	                                                                    ) a,", new { sCouponID = sCouponID });
                if (shengyuCoupon == null)
                {
                    count = 0;
                }
                else {
                    count = Convert.ToInt32(shengyuCoupon["couponCount"]);
                }
                var totalCoupons = query.SingleQuery<Dictionary<string, object>>(@"SELECT
	                                                                                iCouponCount 
                                                                                FROM
	                                                                                EHECD_Coupon 
                                                                                WHERE
                                                                                 ID =@sCouponID", new { @sCouponID = @sCouponID });
                Dictionary<string, object> surplusCoupons = new Dictionary<string, object>();
                surplusCoupons["surplusCoupons"] = Convert.ToInt32(totalCoupons["iCouponCount"]) - count;

                //var surplusCoupons = query.SingleQuery<Dictionary<string, object>>(@"SELECT
	               //                                                                     b.iCouponCount-a.couponCount surplusCoupons---剩余优惠劵 
                //                                                                    FROM
	               //                                                                     (
		              //                                                                      SELECT
			             //                                                                       sCouponID,
			             //                                                                       COUNT (sCouponID) couponCount
		              //                                                                      FROM
			             //                                                                       EHECD_CouponDetails
		              //                                                                      GROUP BY
			             //                                                                       sCouponID
	               //                                                                     ) a,
	               //                                                                     EHECD_Coupon b
                //                                                                    WHERE
	               //                                                                     a.sCouponID = b.ID and a.sCouponID=@sCouponID
                //                                                                    AND a.couponCount < b.iCouponCount", new { sCouponID = sCouponID });

                // 2.判断优惠券够不够
                var surplusCouponsVar=Helper.CommonHelper.GetDictionaryValue("surplusCoupons", surplusCoupons,typeof(string));
                if (Helper.ConvertHelper.ToInt32(surplusCouponsVar) > 0) {
                    ////查询出用户名
                    //var userName = query.SingleQuery<Dictionary<string, object>>(@"SELECT
	                   //                                                                 sLoginName
                    //                                                                FROM
	                   //                                                                 EHECD_Client
                    //                                                                WHERE
	                   //                                                                 ID = @ID
                    //                                                                AND bIsDeleted = 0",new { ID= surplusCouponsVar})["sLoginName"];
                    //3.插入发放的优惠券给用户
                    return excute.InsertSingle<EHECD_CouponDetailsDTO>(new EHECD_CouponDetailsDTO() {
                        ID=Helper.GuidHelper.GetSecuentialGuid(),
                        sCouponID=new Guid(sCouponID.ToString()),
                        sUserID=new Guid(receiver.ToString()),
                        sUserName=userName.ToString(),
                        bIsDeleted=false,
                        bIsUsed=false,
                        dGetTime=DateTime.Now

                    });
                }

            }
            return 0;
        }
    }
}
