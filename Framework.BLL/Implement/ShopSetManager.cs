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
    public class ShopSetManager : IShopSetManager
    {
        /// <summary>
        /// 获取本店的客服
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override IList<EHECD_CustomServiceDTO> GetCustomers(EHECD_SystemUserDTO user)
        {
            return query.QueryList<EHECD_CustomServiceDTO>(@"SELECT ID,sQQ,sQQName FROM EHECD_CustomService WHERE bIsDeleted=0 and sStoreID=@sStoreID", new { sStoreID =user.ID});
        }
       
        /// <summary>
        /// 设置店铺信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override int SetShopMessage(EHECD_SystemUserDTO user, Dictionary<string, object> data)
        {
            var shopSet= query.SingleQuery<EHECD_ShopSetDTO>("SELECT ID,sHeadName,sHeadPicture,sHeadStory,sAutograph FROM EHECD_ShopSet WHERE sShopID=@sShopID", new { sShopID = user.ID });

            StringBuilder builder = new StringBuilder();
            builder.Append(@"DELETE FROM EHECD_ShopSet where sShopID='" + user.ID + "'").Append(";");
            builder.Append(@"DELETE FROM EHECD_Images WHERE iState=6 and sBelongID='" + user.ID + "'").Append(";");
            builder.Append(@"DELETE FROM EHECD_CustomService where sStoreID='"+user.ID+"' ").Append(";");
            if (data["sCity"].ToString()=="市辖区" || data["sCity"].ToString()== "市辖县") {
                data["sCity"]= data["sProvice"];
            }
            EHECD_ShopSetDTO shop = new EHECD_ShopSetDTO()
            {
                ID = GuidHelper.GetSecuentialGuid(),
                sShopID = user.ID,
                sHeadName= shopSet.sHeadName,
                sHeadPicture= shopSet.sHeadPicture,
                sHeadStory=shopSet.sHeadStory,
                sAutograph=shopSet.sAutograph,
                sShopName = data["sShopName"].ToString(),
                sIntroduce = data["sIntroduce"].ToString(),
                bIsDelete = false,
                sMobileNum = data["sMobileNum"].ToString(),
                sAddress = data["sAddress"].ToString(),
                sProvice = data["sProvice"].ToString(),
                sCity = data["sCity"].ToString(),
                sCounty = data["sCounty"].ToString(),
                sLONG = data["sLONG"].ToString(),
                sLat = data["sLat"].ToString(),
                sFristLetter = data["sFristLetter"].ToString()
            };
            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ShopSetDTO>(shop)).Append(";");
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
                            bIsDeleted = false,
                            sStoreID = user.ID
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
                        bIsDeleted = false,
                        sStoreID= user.ID
                    };
                    builder.Append(DBSqlHelper.GetInsertSQL<EHECD_CustomServiceDTO>(customerDto)).Append(";");
                }
            }

            //轮播图片设置
            var imges = JSONHelper.GetModel<System.Collections.Generic.List<Dictionary<string, object>>>(data["imageList"].ToString());
            if (imges.Count > 1)
            {
                
                for (int i = 0; i < imges.Count(); i++)
                {
                    EHECD_ImagesDTO imgDto = new EHECD_ImagesDTO()
                    {
                        ID = GuidHelper.GetSecuentialGuid(),
                        sImagePath = imges[i]["filePath"].ToString(),
                        iState = 6,
                        bIsDeleted = false,
                        sBelongID= user.ID
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
                    iState = 6,
                    bIsDeleted = false,
                    sBelongID = user.ID
                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ImagesDTO>(imgDto)).Append(";");
            }
            return excute.ExcuteTransaction(builder.ToString());
        }

        public override IList<EHECD_ImagesDTO> GetImageList(EHECD_SystemUserDTO user)
        {
            return query.QueryList<EHECD_ImagesDTO>(@"SELECT
	                                                        ID,sImagePath,sLink
                                                        FROM
	                                                        EHECD_Images
                                                        WHERE
	                                                        iState = 6
                                                        AND bIsDeleted = 0 and sBelongID=@sBelongID", new { sBelongID =user.ID});
        }
        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override EHECD_ShopSetDTO GetShopSet(EHECD_SystemUserDTO user)
        {
            return query.SingleQuery<EHECD_ShopSetDTO>(@"select 
                                                            sShopID,sShopName,sIntroduce,sHeadPicture,
                                                            sHeadName,sAutograph,sHeadStory,sMobileNum,
                                                            sProvice,sCity,sCounty,sAddress,sLONG,sLat  
                                                         from EHECD_ShopSet
                                                         where bIsDelete = 0 and sShopID=@sShopID", new { sShopID =user.ID});
        }
       
        /// <summary>
        /// 更改店铺信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override int HosterMessage(EHECD_ShopSetDTO dto, EHECD_SystemUserDTO user)
        {
            return excute.UpdateSingle<EHECD_ShopSetDTO>(dto, string.Format(" Where sShopID='{0}'", user.ID));
        }
    }
}
