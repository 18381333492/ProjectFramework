using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Helper;
using Framework.Dapper;

namespace Framework.BLL
{
    public class BaseSettingManager : IBaseSettingManager
    {
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int DoEdit(Dictionary<string,object> data)
        {
            StringBuilder builder = new StringBuilder();
            //1.删除所有数据
            builder.Append(@"DELETE FROM EHECD_BaseSetting").Append(";");
            builder.Append(@"DELETE FROM EHECD_Images WHERE iState=3").Append(";");
            builder.Append(@"DELETE FROM EHECD_CustomService WHERE sStoreID IS NUll ").Append(";");

            if (excute.ExcuteTransaction(builder.ToString()) > 0)
            {
                builder.Clear();

                //基础设置
                EHECD_BaseSettingDTO baseSettingDto = new EHECD_BaseSettingDTO()
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    sMallPhone = data["sMallPhone"].ToString(),
                    iHighestCommissionPrecent = ConvertHelper.ToInt32(data["iHighestCommissionPrecent"].ToString()),
                    iHours = ConvertHelper.ToInt32(data["iHours"].ToString()),
                    iLevelOneCommissionPrecent = ConvertHelper.ToInt32(data["iLevelOneCommissionPrecent"].ToString()),
                    iLevelTwoCommissionPrecent = ConvertHelper.ToInt32(data["iLevelTwoCommissionPrecent"].ToString()),
                    iLevelThreeCommissionPrecent = ConvertHelper.ToInt32(data["iLevelThreeCommissionPrecent"].ToString()),
                    iPartnerCommissionPrecent = ConvertHelper.ToInt32(data["iPartnerCommissionPrecent"].ToString()),
                    iReturnMoney = ConvertHelper.ToInt32(data["iReturnMoney"].ToString()),
                    iServicePrecent = ConvertHelper.ToInt32(data["iServicePrecent"].ToString()),
                    iUserMoney = ConvertHelper.ToInt32(data["iUserMoney"].ToString()),
                    bIsDeleted = false
                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_BaseSettingDTO>(baseSettingDto)).Append(";");

                // ==========将返还的优惠劵数据插入优惠劵表中=========

                // 先删除这条数据 new Guid() makes an "empty" all-0 guid (00000000-0000-0000-0000-000000000000 is not very useful).
                builder.Append(string.Format(@" DELETE FROM EHECD_Coupon WHERE ID='{0}' ", new Guid())).Append(";");

                // 从新添加数据
                EHECD_CouponDTO coupondto = new EHECD_CouponDTO()
                {

                    ID = GuidHelper.GetSecuentialGuid(),
                    bIsDeleted = false,
                    dInsertTime = DateTime.Now,
                    dValidDateStart = DateTime.Parse("1970-01-01"),
                    dValidDateEnd = DateTime.Parse("3070-01-01"),
                    sStoreID = null,
                    iCoiCouponPrice = ConvertHelper.ToInt32(data["iReturnMoney"].ToString()),
                    iUsePrice = ConvertHelper.ToInt32(data["iUserMoney"].ToString()),
                    iCouponCount = 0,
                    sCouponName = "平台优惠券"

                };
                builder.Append(DBSqlHelper.GetInsertSQL(coupondto)).Append(";");

                // ==========将返还的优惠劵数据插入优惠劵表中=========

                //客服设置
                if (data.ContainsKey("sQQ") && data.ContainsKey("sQQName") && data.ContainsKey("customerCount"))
                {
                    if (ConvertHelper.ToInt32(data["customerCount"]) > 1)
                    {
                        var qqs = JSONHelper.GetModel<System.Collections.Generic.List<string>>(data["sQQ"].ToString());
                        var qqNames = JSONHelper.GetModel<System.Collections.Generic.List<string>>(data["sQQName"].ToString());
                        for (int i = 0; i < qqs.Count(); i++)
                        {
                            EHECD_CustomServiceDTO customerDto = new EHECD_CustomServiceDTO()
                            {
                                ID = GuidHelper.GetSecuentialGuid(),
                                sQQ = qqs[i],
                                sQQName = qqNames[i],
                                bIsDeleted = false
                            };
                            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_CustomServiceDTO>(customerDto)).Append(";");
                        }
                    }
                    else
                    {
                        EHECD_CustomServiceDTO customerDto = new EHECD_CustomServiceDTO()
                        {
                            ID = GuidHelper.GetSecuentialGuid(),
                            sQQ = data["sQQ"].ToString(),
                            sQQName = data["sQQName"].ToString(),
                            bIsDeleted = false
                        };
                        builder.Append(DBSqlHelper.GetInsertSQL<EHECD_CustomServiceDTO>(customerDto)).Append(";");
                    }
                }

                //轮播图片设置sLinks
                var imges = JSONHelper.GetModel<System.Collections.Generic.List<Dictionary<string, object>>>(data["imageList"].ToString());
                if (imges.Count > 1)
                {
                    var sLinks = JSONHelper.GetModel<List<string>>(data["sLinks"].ToString());
                    for (int i = 0; i < imges.Count(); i++)
                    {
                        // 组合限定宽高的图片路径
                        string imgePathTemp = imges[i]["filePath"].ToString();
                        //string path = imgePathTemp.Substring(0, imgePathTemp.LastIndexOf(".")) + "-391-253" + imgePathTemp.Substring(imgePathTemp.LastIndexOf("."));

                        EHECD_ImagesDTO imgDto = new EHECD_ImagesDTO()
                        {
                            ID = GuidHelper.GetSecuentialGuid(),
                            sImagePath = imgePathTemp,
                            sLink = sLinks[i],
                            iOrder = i,
                            iState = 3,
                            bDisplay = true,
                            bIsDeleted = false
                        };
                        builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ImagesDTO>(imgDto)).Append(";");
                    }
                }
                else
                {
                    EHECD_ImagesDTO imgDto = new EHECD_ImagesDTO()
                    {
                        ID = GuidHelper.GetSecuentialGuid(),
                        sImagePath = imges[0]["filePath"].ToString(),
                        sLink = data["sLinks"].ToString(),
                        bDisplay = true,
                        iOrder = 0,
                        iState = 3,
                        bIsDeleted = false
                    };
                    builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ImagesDTO>(imgDto)).Append(";");
                }

                return excute.ExcuteTransaction(builder.ToString());
            }
            else {

                return -1;
            }
            
        }

        /// <summary>
        /// 获取基础设置
        /// </summary>
        /// <returns></returns>
        public override EHECD_BaseSettingDTO GetBaseSetting()
        {
            return query.SingleQuery<EHECD_BaseSettingDTO>(@"SELECT TOP(1) * FROM EHECD_BaseSetting",null);
        }

        /// <summary>
        /// 获取客服列表
        /// </summary>
        /// <returns></returns>
        public override IList<EHECD_CustomServiceDTO> GetCustomers()
        {
            return query.QueryList<EHECD_CustomServiceDTO>(@"SELECT ID,sQQ,sQQName FROM EHECD_CustomService  WHERE bIsDeleted=0 AND sStoreID IS NUll", null);
        }

        /// <summary>
        /// 获取对应的图片数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override IList<EHECD_ImagesDTO> GetImageList()
        {
            return query.QueryList<EHECD_ImagesDTO>(@"SELECT
	                                                        ID,sImagePath,sLink
                                                        FROM
	                                                        EHECD_Images
                                                        WHERE
	                                                        iState = 3
                                                        AND bIsDeleted = 0", null);
        }
    }
}
