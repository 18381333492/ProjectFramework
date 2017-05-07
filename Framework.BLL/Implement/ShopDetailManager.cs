using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework.BLL
{
    public partial class ShopDetailManager : IShopDetailManager
    {

        /// <summary>
        /// 跟新当前用户上级
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="superID"></param>
        /// <returns></returns>
        public override bool UpdateClientSuper(string userID, string superID)
        {
            return excute.UpdateSingle<EHECD_ClientDTO>(new EHECD_ClientDTO() { ID=new Guid(userID),sRegCode=new Guid(superID), dBirthday =DateTime.Now},string.Format(@" where ID='{0}' ",userID))>0;
        }

        /// <summary>
        /// 判断当前用户是否有上级
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override bool IsHaveSuper(string ID)
        {
            var res = query.SingleQuery<Dictionary<string,object>>(@"SELECT
	                                                                        *
                                                                        FROM
	                                                                        EHECD_Client
                                                                        WHERE
	                                                                        ID = @ID
                                                                        AND sRegCode IS NOT NULL", new { ID=ID });
            if (res != null)
            {
                return true;
            }
            else {

                return false;
            }
        }

        /// <summary>
        /// 手机端根据ID查看游记详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override Dictionary<string, object> GetTravelByID(string ID)
        {
            string photo = "";
            var pageList = query.SingleQuery<Dictionary<string, object>>("select * from EHECD_TravelsNotes where ID=@ID", new { ID = ID });
            var image = query.QueryList<EHECD_ImagesDTO>("SELECT  sImagePath from EHECD_Images WHERE bIsDeleted=0 AND sBelongID=@sBelongID and iState=0", new { sBelongID = ID });
            if (image != null)
            {
                for (var i = 0; i < image.Count(); i++)
                {
                    photo += image[i].sImagePath.ToString() + ',';
                }
                pageList["iamge"] = photo.TrimEnd(',');
            }
            else {
                pageList["iamge"] = "";
            }
            return pageList;
        }
        /// <summary>
        ///手机端 店铺所有游记
        /// </summary>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetAllTraveNote(PageInfo page, string ID)
        {
            page.orderType = OrderType.ASC;
            page.OrderBy = "iOrder";

            var pageList = query.PaginationQuery<Dictionary<string, object>>("SELECT * FROM EHECD_TravelsNotes WHERE bIsDeleted=0 AND sStoreID=@sStoreID", page, new { sStoreID = ID });
            for (int i = 0; i < pageList.Result.Count(); i++)
            {
                string image = "";
                var haha = query.QueryList<EHECD_ImagesDTO>("SELECT TOP 3 sImagePath from EHECD_Images WHERE bIsDeleted=0 AND sBelongID=@sBelongID", new { sBelongID = pageList.Result[i]["ID"] });
                if (haha != null) {
                    for (var j = 0; j < haha.Count(); j++)
                    {
                        image += haha[j].sImagePath.ToString() + ',';
                    }
                    pageList.Result[i]["image"] = image.TrimEnd(',');
                }
                pageList.Result[i]["sContent"] = NoHTML(pageList.Result[i]["sContent"].ToString());
               
            }

            return pageList;
        }
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = Regex.Replace(Htmlstring, @"<script.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<style.*?</style>", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"<.+>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])|([\s])+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Htmlstring.Replace("<", "&lt;");
            Htmlstring = Htmlstring.Replace(">", "&gt;");
            //Htmlstring = Htmlstring.Replace("\r\n", "");
            return Htmlstring;
        }


        /// <summary>
        /// 店铺住宿信息绑定
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetShopHome(Dictionary<string, object> dic, PageInfo page)
        {
            page.orderType = OrderType.DESC;
            page.OrderBy = "bSeckill DESC,bSpecialSale";
            //返回的结果
            List<Dictionary<string, object>> returnList = new List<Dictionary<string, object>>();

            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ID,sGoodsName,sGoodsCategory,iHousePerson,sGoodsPictures,bSeckill,bSpecialSale,dSeckillPrices,dSpecialSalePrices,dGoodsFisrtPrice,dGoodsSecPrice,dGoodsThirdPrice,sSeckillTime,sSpecialSaleTime,sActivityUseTime  FROM EHECD_Goods WHERE bIsDeleted=0 AND sGoodsCategory=1 AND bShelves=1 and sStoreId='" + dic["ID"] + "'");

            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))//关键字查询
            {
                builder.AppendFormat("and (sGoodsName like '%{0}%')", dic["sKeyword"].ToString());
            }
            //房间的基本信息页面绑定
            var roomDetail= query.PaginationQuery<Dictionary<string, object>>(builder.ToString(), page, null);

            if (dic["intTime"] != null && !string.IsNullOrEmpty(dic["intTime"].ToString()) && dic["outTime"] != null && !string.IsNullOrEmpty(dic["outTime"].ToString()))
            {
                var intTime = DateTime.Parse(dic["intTime"].ToString());//入住时间

                var outTime = DateTime.Parse(dic["outTime"].ToString());//离店时间

                var dateLength = 1;
                var time= (outTime - intTime).Days;//计算入住天数
                if (time == 0)
                {
                    dateLength = 1;
                }
                else {
                    dateLength = time;
                }
                decimal price = 0.00m;

                for (int i = 0; i < roomDetail.Result.Count(); i++)
                {//循环每个商品
                    //根据ID查出商品价格
                    price = 0.00m;
                    var goodsPrice = query.SingleQuery<Dictionary<string, object>>("SELECT sGoodsId,sFirstTime FROM EHECD_GoodsTimePrice WHERE sGoodsId=@sGoodsId", new { sGoodsId = roomDetail.Result[i]["ID"] });

                    for (int j = 0; j < dateLength; j++)
                    {//根据日期循环
                        //选中日期本店铺的房间入住的总数
                        var orderRoomTotal = query.SingleQuery<Dictionary<string, object>>(@"SELECT
	                                                                                        COUNT (sOrderId) TOTAL
                                                                                        FROM
	                                                                                        EHECD_RoomDetail
                                                                                        WHERE
                                                                                        sGoodsId='" + roomDetail.Result[i]["ID"] + "' AND dStartTime <='" + intTime.AddDays(j) + "'  AND sEndTime >= '" + intTime.AddDays(j) + "'", null);

                        //所有商品的所有房间总数量
                        var roomTotal = query.SingleQuery<EHECD_GoodsDTO>("SELECT ID,iHouseCount FROM EHECD_Goods WHERE sGoodsCategory=1 AND ID=@ID", new { ID = roomDetail.Result[i]["ID"] }).iHouseCount;
                        //当前时间的满房数量
                        var fullTotal = query.SingleQuery<EHECD_FullHouseTimeDTO>("SELECT iFullHouseCount FROM EHECD_FullHouseTime WHERE sGoodsId='" + roomDetail.Result[i]["ID"] + "' AND dStartTime<='" + intTime.AddDays(j) + "' AND dEndTime>='" + intTime.AddDays(j) + "'", null);
                        int? fullRoomTotal = 0;
                        if (fullTotal != null) {
                            fullRoomTotal = fullTotal.iFullHouseCount;
                            
                        }
                       
                        var roomState = roomTotal - fullRoomTotal - int.Parse(orderRoomTotal["TOTAL"].ToString()) > 0 ? "有房" : "满房";//房间状态
                        if (roomState == "满房")
                        {
                            roomDetail.Result[i]["fullState"] = "满房";
                        }
                        roomDetail.Result[i]["orSkill"]  = false;
                        roomDetail.Result[i]["orSpecial"]  = false;
                        if (bool.Parse(roomDetail.Result[i]["bSeckill"].ToString()) == true)
                        {
                            var startTime = DateTime.Parse(roomDetail.Result[i]["sSeckillTime"].ToString().Split(',')[0].ToString() + " 00:00:00");//活动开始时间
                            var endTime = DateTime.Parse(roomDetail.Result[i]["sSeckillTime"].ToString().Split(',')[1].ToString() + " 23:59:59");//活动结束时间
                            var userStart = DateTime.Parse(roomDetail.Result[i]["sActivityUseTime"].ToString().Split(',')[0].ToString() + " 00:00:00");//使用开始时间
                            var userEnd = DateTime.Parse(roomDetail.Result[i]["sActivityUseTime"].ToString().Split(',')[1].ToString() + " 23:59:59");//使用结束时间 
                            if (startTime.CompareTo(DateTime.Now) <= 0 && DateTime.Now.CompareTo(endTime) <= 0) {
                                //今天在活动时间内
                                if (userStart.CompareTo(intTime.AddDays(j)) <= 0 && intTime.AddDays(j).CompareTo(userEnd) <= 0)
                                {
                                    //今天是在可使用的时间
                                    price += Convert.ToDecimal(roomDetail.Result[i]["dSeckillPrices"]);
                                    roomDetail.Result[i]["orSkill"] = true;
                                }
                               
                            }
                        }
                        
                        if (bool.Parse(roomDetail.Result[i]["bSpecialSale"].ToString()) == true)
                        {
                            var startTime = DateTime.Parse(roomDetail.Result[i]["sSpecialSaleTime"].ToString().Split(',')[0].ToString() + " 00:00:00");//活动开始时间
                            var endTime = DateTime.Parse(roomDetail.Result[i]["sSpecialSaleTime"].ToString().Split(',')[1].ToString() + " 23:59:59");//活动结束时间
                            var userStart = DateTime.Parse(roomDetail.Result[i]["sActivityUseTime"].ToString().Split(',')[0].ToString() + " 00:00:00");//使用开始时间
                            var userEnd = DateTime.Parse(roomDetail.Result[i]["sActivityUseTime"].ToString().Split(',')[1].ToString() + " 23:59:59");//使用结束时间 
                            if (startTime.CompareTo(DateTime.Now) <= 0 && DateTime.Now.CompareTo(endTime) <= 0)
                            {
                                //今天在活动时间内
                                if (userStart.CompareTo(intTime.AddDays(j)) <= 0 && intTime.AddDays(j).CompareTo(userEnd) <= 0)
                                {
                                    //今天是在可使用的时间
                                    price += Convert.ToDecimal(roomDetail.Result[i]["dSpecialSalePrices"]);
                                    roomDetail.Result[i]["orSkill"] = true;
                                }

                            }
                           
                        }
                        if (Convert.ToBoolean(roomDetail.Result[i]["orSkill"])==false && Convert.ToBoolean(roomDetail.Result[i]["orSpecial"])== false)
                        {
                            var goodTime = query.SingleQuery<EHECD_GoodsTimePriceDTO>("SELECT sGoodsId,sFirstTime FROM EHECD_GoodsTimePrice WHERE sGoodsId=@sGoodsId", new { sGoodsId = roomDetail.Result[i]["ID"] });
                            if (goodTime == null)
                            {
                                price += Convert.ToDecimal(roomDetail.Result[i]["dGoodsFisrtPrice"]);
                            }
                            else
                            {
                                decimal priceB = 0.00m;
                                string[] tPrice = goodTime.sFirstTime.Split(',');
                                for (int xx = 0; xx < tPrice.Count(); xx++)
                                {

                                    for (int aa = 0; aa < tPrice.Length; aa++)
                                    {
                                        if (intTime.AddDays(j).Month + "-" + intTime.AddDays(j).Day == tPrice[aa].Split(':')[0])
                                        {
                                            var priceA = int.Parse(tPrice[aa].Split(':')[1]);

                                            if (priceA == 1)
                                            {
                                                priceB =Convert.ToDecimal(roomDetail.Result[i]["dGoodsFisrtPrice"]);//第一种价格

                                            }
                                            else if (priceA == 2)
                                            {
                                                priceB = Convert.ToDecimal(roomDetail.Result[i]["dGoodsSecPrice"]);//第二种价格

                                            }
                                            else
                                            {
                                                priceB = Convert.ToDecimal(roomDetail.Result[i]["dGoodsThirdPrice"]);//第三种价格

                                            }
                                        }
                                        if (priceB != 0.00m)
                                        {
                                            break;
                                        }
                                    }
                                    if (priceB == 0.00m)
                                    {
                                        priceB = Convert.ToDecimal(roomDetail.Result[i]["dGoodsFisrtPrice"]);
                                    }

                                }

                                price += priceB;

                            }
                        }

                    }
                    roomDetail.Result[i]["priceTotal"] = price.ToString("#0.00");
                    roomDetail.Result[i]["image"] = roomDetail.Result[i]["sGoodsPictures"].ToString().Split(',')[0];
                }
            }

            // 对新的结果集进行排序 bSeckill DESC,bSpecialSale
            roomDetail.Result = roomDetail.Result.OrderByDescending(a=>a["bSeckill"].ToString()).OrderByDescending(a=>a["bSpecialSale"]).Select(a=>a).ToList();

            return roomDetail;
        }

        /// <summary>
        /// 获取本店的客房
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override PagingRet<EHECD_GoodsDTO> GetRoom(PageInfo page, string ID, Dictionary<string, object> dic)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"SELECT ID,
                                        sGoodsPictures,
                                        sGoodsName,
                                        dGoodsFisrtPrice,
                                        dGoodsSecPrice,
                                        dGoodsThirdPrice,
                                        bSeckill,
                                        bSpecialSale,
                                        dSeckillPrices,
                                        dSpecialSalePrices,
                                        sSeckillTime,
                                        sSpecialSaleTime,sHouseOrTicketDetail
                                        FROM 
                                        EHECD_Goods 
                                        WHERE  
                                        bIsDeleted=0 AND sGoodsCategory=1 AND sStoreId='" + ID + "' and bShelves=0 ORDER BY bSeckill DESC,bSpecialSale DESC");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))
            {
                builder.AppendFormat("and (sGoodsName like '%{0}%')", dic["sKeyword"].ToString());
            }


            return query.PaginationQuery<EHECD_GoodsDTO>(builder.ToString(), page, null);
        }
        /// <summary>
        /// 查看房间的详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override object HomeDetail(string ID, string ClientID)
        {
            //商品的信息
            var detail = query.SingleQuery<Dictionary<string, object>>("select * from EHECD_Goods where ID=@ID", new { ID = ID });
            var price = "";//价格
            bool skillSale = false;//秒杀
            bool sale = false;//特卖
            var timeNow = DateTime.Now;//今天的时间
            if (detail == null)
            {
                detail= query.SingleQuery<Dictionary<String,object>>("select * from EHECD_GoodsPreviewView where ID=@ID", new { ID = ID });
            }

            //评分，总共评分人
            Dictionary<string, object> commentnum = query.SingleQuery<Dictionary<string, object>>("SELECT AVG(iCommentScore) as score ,COUNT(iCommentScore)as comments FROM EHECD_Comment WHERE bIsDeleted=0 and sGoodsID=@sGoodsID", new { sGoodsID = ID });
            if (commentnum["score"] == null)
            {
                commentnum["score"] = 5;
            }
            else {
                commentnum["score"] = Math.Round(commentnum["score"].ToDouble(), 1);
            }
            // commentnum["aveTotal"] = int.Parse(commentnum["score"].ToString());
            //获取一条评论
            var coment = query.SingleQuery<EHECD_CommentDTO>("SELECT TOP 1 sCommentContent FROM EHECD_Comment WHERE bIsDeleted=0 and sGoodsID=@sGoodsID", new { sGoodsID = ID });

            //特卖/秒杀活动时间
            StringBuilder builder = new StringBuilder();
           
            if (Convert.ToBoolean(detail["bSeckill"]) == true ) {//是否是秒杀
                var time = detail["sSeckillTime"].ToString();
                string[] usetime = time.Split(',');
                var starttime = DateTime.Parse(usetime[0]+" 00:00:00");
                var endtime = DateTime.Parse(usetime[1]+" 23:59:59");
                if (starttime.CompareTo(timeNow) <= 0 && timeNow.CompareTo(endtime) <= 0)
                {
                    //如果今日在秒杀时间内
                    skillSale = true;
                    price =detail["dSeckillPrices"].ToDecimal().ToString("#0.00");
                    detail["endtime"] = usetime[1];
                    detail["hour"]=(endtime - timeNow).Hours;
                    detail["Minutes"] = (endtime - timeNow).Minutes;
                    detail["Seconds"] = (endtime - timeNow).Seconds;

                }
            }
            if (Convert.ToBoolean(detail["bSpecialSale"]) == true) {//是否是特卖
                var time = detail["sSpecialSaleTime"].ToString();//查出特卖时间
                string[] usetime = time.Split(',');
                var starttime = DateTime.Parse(usetime[0]+" 00:00:00");//特卖开始时间
                var endtime = DateTime.Parse(usetime[1] + " 23:59:59");//特卖结束时间
                if (starttime.CompareTo(timeNow) <= 0 && timeNow.CompareTo(endtime) <= 0)
                {
                    //如果今日在特卖时间内
                    sale = true;
                    price =detail["dSpecialSalePrices"].ToDecimal().ToString("#0.00");
                    detail["endtime"] = usetime[1];
                    detail["hour"] = (endtime - timeNow).Hours;
                    detail["Minutes"] = (endtime - timeNow).Minutes;
                    detail["Seconds"] = (endtime - timeNow).Seconds;
                }
            }
            if (skillSale == false && sale == false) {//不是特卖也不是秒杀
                var nowdayTime = timeNow.Month.ToString() + '-' + timeNow.Day.ToString();
                //今天是属于哪种价格
                var sFirstTime = query.SingleQuery<EHECD_GoodsDTO>("SELECT sFirstTime from EHECD_GoodsTimePrice WHERE sGoodsId=@sGoodsId", new { sGoodsId = ID });
                if (sFirstTime == null)
                {
                    price = detail["dGoodsFisrtPrice"].ToDecimal().ToString("#0.00");
                }
                else {
                    string[] priceTime = sFirstTime.ToString().Split(',');
                    for (int i = 0; i < priceTime.Count(); i++)
                    {
                        if (nowdayTime == priceTime[i].Split(':')[0])
                        {
                            var pricetype = int.Parse(priceTime[i].Split(':')[1]);
                            if (pricetype == 1)
                            {
                                price = detail["dGoodsFisrtPrice"].ToDecimal().ToString("#0.00");
                            }
                            else if (pricetype == 2)
                            {
                                price = detail["dGoodsSecPrice"].ToDecimal().ToString("#0.00");
                            }
                            else
                            {
                                price = detail["dGoodsThirdPrice"].ToDecimal().ToString("#0.00");
                            }
                        }
                        else
                        {
                            price = detail["dGoodsFisrtPrice"].ToDecimal().ToString("#0.00");
                        }
                    }
                }
            }
          
            
            detail["dGoodsSecPrice"] = price;//记录商品今天的价格
            detail["dGoodsFisrtPrice"]= detail["dGoodsFisrtPrice"].ToDecimal().ToString("#0.00");
            detail["bSeckill"] = skillSale;//今日是否属于秒杀
            detail["bSpecialSale"] = sale;//今日是否属于特价
            //商品图片
            var shopBanner = detail["sGoodsPictures"];
            //店铺电话
            var shopPhone = query.SingleQuery<EHECD_ShopSetDTO>("SELECT sMobileNum FROM EHECD_ShopSet WHERE sShopID=@sShopID", new { sShopID = detail["sStoreId"] }).sMobileNum;
            var collect = new EHECD_CollectDTO();
            //是否收藏
            if (ClientID!= null)
            {
                collect = query.SingleQuery<EHECD_CollectDTO>("SELECT * from EHECD_Collect WHERE iGoodsID='" + ID + "' AND bIsCollect=1 AND bIsDeleted=0 and iClientID='" + ClientID + "'", null);
            }
            else {
                collect = null;
            }
           
            return new Dictionary<string, object>()
            {
                {"commentnum",commentnum },
                {"detail",detail },
                {"coment",coment },
                { "price",price},
                {"shopBanner",shopBanner},
                {"collect",collect},
                {"shopPhone",shopPhone},
            };
        }
        /// <summary>
        /// 获取所有评论
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetAllComment(PageInfo page, string ID)
        {
            PagingRet<Dictionary<string, object>> pageList = query.PaginationQuery<Dictionary<string, object>>("select * from EHECD_Comment where sGoodsID=@sGoodsID and bIsDeleted=0", page, new { sGoodsID = ID });

            for (int i = 0; i < pageList.Result.Count(); i++)
            {
                var image = query.SingleQuery<Dictionary<string, object>>("SELECT sHeadPic,sNickName FROM EHECD_Client WHERE ID=@ID", new { ID = pageList.Result[i]["sCommenterID"] });
                if (image != null) { 
                pageList.Result[i]["image"] = image["sHeadPic"];
                pageList.Result[i]["sNickName"] = image["sNickName"];
                }
            }
            return pageList;
        }

        /// <summary>
        /// 店铺首页
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override object ShopHome(string ID, string clientID)
        {
            //获取店铺设置的信息
            var shopmessage = query.SingleQuery<EHECD_ShopSetDTO>("SELECT * FROM EHECD_ShopSet WHERE bIsDelete=0 AND sShopID=@sShopID", new { sShopID = ID });
            //店铺轮播图
            var shopBanner = query.QueryList<EHECD_ImagesDTO>("SELECT sImagePath FROM EHECD_Images WHERE bIsDeleted=0 AND sBelongID=@sBelongID and iState=6", new { sBelongID = ID });
          
            //游记
            var tralver = query.QueryList<Dictionary<string,object>>("SELECT TOP 3 * FROM EHECD_TravelsNotes WHERE bIsDeleted=0 AND sStoreID=@sStoreID order by iOrder ", new { sStoreID = ID });
            if (tralver != null) {
                for (int i = 0; i < tralver.Count(); i++)
                {
                    var ImageUrl = "";
                    var travelImage = query.QueryList<EHECD_ImagesDTO>("SELECT TOP 3 sImagePath  FROM EHECD_Images WHERE sBelongID='" + tralver[i]["ID"] + "' AND bIsDeleted=0 and iState=0", null);
                    if (travelImage != null)
                    {
                        for (int j = 0; j < travelImage.Count(); j++)
                        {
                            ImageUrl += travelImage[j].sImagePath + ',';
                        }
                        tralver[i]["imageurl"] = ImageUrl.TrimEnd(',');
                    }
                    else
                    {
                        tralver[i]["imageurl"] = "";
                    }
                    tralver[i]["sContent"] = NoHTML(tralver[i]["sContent"].ToString());
                }
            }
            
            IList<EHECD_CouponDTO> coupon ;
            if (clientID != null) {
                //如果用户已经领取的优惠券就不显示
                coupon = query.QueryList<EHECD_CouponDTO>(@"SELECT * FROM EHECD_Coupon as a WHERE bIsDeleted=0 and dValidDateEnd >= convert(varchar(10),getDate(),120)  AND iCouponCount!=0 AND sStoreID='" + ID + "' and  ID NOT IN(SELECT sCouponID FROM EHECD_CouponDetails WHERE bIsDeleted=0  AND sUserID='" + clientID + "') and iCouponCount>(select COUNT(*) from EHECD_CouponDetails where sCouponID=a.ID AND bIsDeleted=0)", null);
            }
            else
            {//用户没有登录的时候就显示全部的店铺优惠券
                coupon = query.QueryList<EHECD_CouponDTO>("SELECT * FROM EHECD_Coupon as a WHERE bIsDeleted=0 AND iCouponCount!=0 AND sStoreID='" + ID + "' and dValidDateEnd >= convert(varchar(10),getDate(),120)  and iCouponCount>(select COUNT(*) from EHECD_CouponDetails where sCouponID=a.ID AND bIsDeleted=0)  ", null);

            }
            //优惠券
            

            //是否收藏
            EHECD_CollectDTO collect = new EHECD_CollectDTO();

            if (clientID != null)
            {
                 collect = query.SingleQuery<EHECD_CollectDTO>("SELECT * from EHECD_Collect WHERE iGoodsID='" + ID + "' AND bIsCollect=1 AND bIsDeleted=0 AND iClientID='" + clientID + "'", null);
            }
            else {
                collect = null;
            }
            return new Dictionary<string, object>() {
                {"shopmessage",shopmessage },
                {"shopBanner",shopBanner },
                {"tralver",tralver },
                {"coupon",coupon },
                {"collect",collect },
            };

        }
        /// <summary>
        /// 票务
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> TicketHome(PageInfo page, Dictionary<string, object> dic)
        {
            //页面排序
            page.orderType = OrderType.DESC;
            page.OrderBy = "bSeckill DESC,bSpecialSale";
            //门票
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT ID,
                                        sGoodsPictures,sHouseOrTicketDetail,
                                        sGoodsName,sGoodsCategory,
                                        dGoodsFisrtPrice,
                                        dGoodsSecPrice,
                                        dGoodsThirdPrice,
                                        bSeckill,
                                        bSpecialSale,
                                        dSeckillPrices,sGoodsIntroduce,
                                        dSpecialSalePrices,
                                        sSeckillTime,
                                        sSpecialSaleTime,
                                        sActivityUseTime
                                        FROM 
                                        EHECD_Goods 
                                        WHERE  
                                        bIsDeleted=0 AND sGoodsCategory=2 AND sStoreId='" + dic["ID"] + "' and bShelves=1");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))//关键字查询
            {
                sb.AppendFormat("and (sGoodsName like '%{0}%')", dic["sKeyword"].ToString());
            }
            var abroundpage = query.PaginationQuery<Dictionary<string, object>>(sb.ToString(), page, null);//分页
            var getTime = new DateTime();
           
            if (dic["selectTime"] != null && !string.IsNullOrEmpty(dic["selectTime"].ToString()))
            {
                getTime = DateTime.Parse(dic["selectTime"].ToString());
            }

            else {
                getTime = DateTime.Now;
                dic["selectTime"] = getTime.Month +"-" + getTime.Day;
            }
            var nowTime = DateTime.Now;

           
            for (int kk = 0; kk < abroundpage.Result.Count(); kk++)
            {
                decimal priceB = 0.00m;//当日价格
                bool special = false;
                if (bool.Parse(abroundpage.Result[kk]["bSeckill"].ToString()) == true)
                {
                    var startTime = DateTime.Parse(abroundpage.Result[kk]["sSeckillTime"].ToString().Split(',')[0].ToString()+" 00:00:00");
                    var endTime = DateTime.Parse(abroundpage.Result[kk]["sSeckillTime"].ToString().Split(',')[1].ToString()+" 23:59:59");
                    var userStart = DateTime.Parse(abroundpage.Result[kk]["sActivityUseTime"].ToString().Split(',')[0].ToString() + " 00:00:00");
                    var userEnd = DateTime.Parse(abroundpage.Result[kk]["sActivityUseTime"].ToString().Split(',')[1].ToString() + " 23:59:59");
                    if ((startTime.CompareTo(nowTime)) <= 0 && nowTime.CompareTo(endTime) <= 0)
                    {
                        //如果今日的时间的秒杀时间内
                        if ((userStart.CompareTo(getTime)) <= 0 && getTime.CompareTo(userEnd) <= 0)
                        {
                            priceB = Convert.ToDecimal(abroundpage.Result[kk]["dSeckillPrices"]);
                            special = true;
                        }
                        else {
                            special = false;
                        }
                    }
                    else
                    {//如果过了秒杀时间
                        abroundpage.Result[kk]["bSeckill"] = false;
                    }
                }
                else if (bool.Parse(abroundpage.Result[kk]["bSpecialSale"].ToString()) == true)
                {
                    var startTime = DateTime.Parse(abroundpage.Result[kk]["sSpecialSaleTime"].ToString().Split(',')[0].ToString() + " 00:00:00");
                    var endTime = DateTime.Parse(abroundpage.Result[kk]["sSpecialSaleTime"].ToString().Split(',')[1].ToString() + " 23:59:59");
                    var userStart = DateTime.Parse(abroundpage.Result[kk]["sActivityUseTime"].ToString().Split(',')[0].ToString() + " 00:00:00");
                    var userEnd = DateTime.Parse(abroundpage.Result[kk]["sActivityUseTime"].ToString().Split(',')[1].ToString() + " 23:59:59");
                    if ((startTime.CompareTo(nowTime)) <= 0 && nowTime.CompareTo(endTime) <= 0)
                    {
                        //如果今日的时间的秒杀时间内
                        if ((userStart.CompareTo(getTime)) <= 0 && getTime.CompareTo(userEnd) <= 0)
                        {
                            //今日是否是使用时间，如果是价格就是秒杀的价格，如果不是则去查询今日的价格
                            priceB = Convert.ToDecimal(abroundpage.Result[kk]["dSpecialSalePrices"]);
                            special = true;
                        }
                        else
                        {
                            special = false;
                        }
                    }
                    else
                    {//如果过了特卖时间
                        abroundpage.Result[kk]["bSpecialSale"] = false;
                    }
                }
                if (special == false)
                {
                    var alltimeprice/*在价格表中查看商品每个时间段的价格*/ = query.SingleQuery<EHECD_GoodsTimePriceDTO>("SELECT ID,sFirstTime FROM EHECD_GoodsTimePrice WHERE sGoodsId=@sGoodsId", new { sGoodsId = abroundpage.Result[kk]["ID"] });
                    if (alltimeprice == null)
                    {//如果价格表中没有价格
                        priceB = Convert.ToDecimal(abroundpage.Result[kk]["dGoodsFisrtPrice"]);
                    }
                    else
                    {
                        string[] tPrice = alltimeprice.sFirstTime.Split(',');
                        for (int xx = 0; xx < tPrice.Count(); xx++)
                        {
                            for (int i = 0; i < tPrice.Length; i++) {
                                if (dic["selectTime"].ToString() == tPrice[i].Split(':')[0])
                                {
                                    var priceA = int.Parse(tPrice[i].Split(':')[1]);

                                    if (priceA == 1)
                                    {
                                        priceB= Convert.ToDecimal(abroundpage.Result[kk]["dGoodsFisrtPrice"]);
                                       
                                    }
                                    else if (priceA == 2)
                                    {
                                        priceB = Convert.ToDecimal(abroundpage.Result[kk]["dGoodsSecPrice"]);
                                       
                                    }
                                    else
                                    {
                                        priceB = Convert.ToDecimal(abroundpage.Result[kk]["dGoodsThirdPrice"]);
                                        
                                    }
                                   

                                }
                                if (priceB != 0.00m) {
                                    break;
                                }
                            }
                            if (priceB == 0.00m) {
                                priceB = Convert.ToDecimal(abroundpage.Result[kk]["dGoodsFisrtPrice"]);
                            }
                           
                        }


                    }
                }
                abroundpage.Result[kk]["price"] = priceB.ToString("#0.00");
                abroundpage.Result[kk]["image"] = abroundpage.Result[kk]["sGoodsPictures"].ToString().Split(',')[0];
            }

            abroundpage.Result = abroundpage.Result.OrderByDescending(a => a["bSeckill"].ToString()).OrderByDescending(a => a["bSpecialSale"]).Select(a => a).ToList();
            return abroundpage;
        }
        /// <summary>
        /// 周边
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> AroundHome(PageInfo page, Dictionary<string, object> dic)
        {

            page.orderType = OrderType.DESC;
            page.OrderBy = "bSeckill DESC,bSpecialSale";
            //查找该周边的地址

            var address = query.SingleQuery<EHECD_ShopSetDTO>("SELECT sAddress FROM EHECD_ShopSet WHERE sShopID=@sShopID", new { sShopID = dic["ID"] }).sAddress;


            //周边
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT ID,
                                        sGoodsPictures,
                                        sGoodsName,sGoodsCategory,
                                        dGoodsFisrtPrice,
                                        dGoodsSecPrice,
                                        dGoodsThirdPrice,
                                        bSeckill,
                                        bSpecialSale,
                                        dSeckillPrices,
                                        dSpecialSalePrices,
                                        sSeckillTime,
                                        sSpecialSaleTime,
                                        sActivityUseTime
                                        FROM 
                                        EHECD_Goods 
                                        WHERE  
                                        bIsDeleted=0 AND sGoodsCategory=3 AND sStoreId='" + dic["ID"] + "' and bShelves=1");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))//关键字查询
            {
                sb.AppendFormat("and (sGoodsName like '%{0}%')", dic["sKeyword"].ToString());
            }
            var abroundpage = query.PaginationQuery<Dictionary<string, object>>(sb.ToString(), page, null);//分页

            decimal priceB = 0.00m;//当日价格

            var time = DateTime.Now;
            string date = time.Month.ToString() + "-" + time.Day.ToString();//将现在的日期转换为mm-dd
            for (int kk = 0; kk < abroundpage.Result.Count(); kk++)
            {
                if (bool.Parse(abroundpage.Result[kk]["bSeckill"].ToString()) == true)
                {
                    var startTime = DateTime.Parse(abroundpage.Result[kk]["sSeckillTime"].ToString().Split(',')[0].ToString());
                    var endTime = DateTime.Parse(abroundpage.Result[kk]["sSeckillTime"].ToString().Split(',')[1].ToString());
                    if ((startTime.CompareTo(time)) <= 0 && time.CompareTo(endTime) <= 0)
                    {
                        //如果今日的时间的秒杀时间内
                        priceB = Convert.ToDecimal(abroundpage.Result[kk]["dSeckillPrices"]);
                    }
                    else
                    {//如果过了秒杀时间
                        abroundpage.Result[kk]["bSeckill"] = false;
                    }
                }
                else if (bool.Parse(abroundpage.Result[kk]["bSpecialSale"].ToString()) == true)
                {
                    var startTime = DateTime.Parse(abroundpage.Result[kk]["sSpecialSaleTime"].ToString().Split(',')[0].ToString());
                    var endTime = DateTime.Parse(abroundpage.Result[kk]["sSpecialSaleTime"].ToString().Split(',')[1].ToString());
                    if ((startTime.CompareTo(time))<=0 && time.CompareTo(endTime) <= 0)
                    {
                        //如果今日的时间的特卖时间内
                        priceB = Convert.ToDecimal(abroundpage.Result[kk]["dSpecialSalePrices"]);

                    }
                    else
                    {//如果过了特卖时间
                        abroundpage.Result[kk]["bSpecialSale"] = false;
                    }
                }
               if(bool.Parse(abroundpage.Result[kk]["bSpecialSale"].ToString()) == false&& bool.Parse(abroundpage.Result[kk]["bSeckill"].ToString()) == false)
                { 
                    var alltimeprice/*在价格表中查看商品每个时间段的价格*/ = query.SingleQuery<EHECD_GoodsTimePriceDTO>("SELECT ID,sFirstTime FROM EHECD_GoodsTimePrice WHERE sGoodsId=@sGoodsId", new { sGoodsId = abroundpage.Result[kk]["ID"] });
                    if (alltimeprice == null)
                    {//如果价格表中没有价格
                        priceB = Convert.ToDecimal(abroundpage.Result[kk]["dGoodsFisrtPrice"]);
                    }
                    else
                    {
                        string[] tPrice = alltimeprice.sFirstTime.Split(',');
                        for (int xx = 0; xx < tPrice.Count(); xx++)
                        {
                            for (int i = 0; i < tPrice.Length; i++)
                            {
                                if (date == tPrice[i].Split(':')[0])
                                {
                                    var priceA = int.Parse(tPrice[i].Split(':')[1]);

                                    if (priceA == 1)
                                    {
                                        priceB = Convert.ToDecimal(abroundpage.Result[kk]["dGoodsFisrtPrice"]);

                                    }
                                    else if (priceA == 2)
                                    {
                                        priceB = Convert.ToDecimal(abroundpage.Result[kk]["dGoodsSecPrice"]);

                                    }
                                    else
                                    {
                                        priceB = Convert.ToDecimal(abroundpage.Result[kk]["dGoodsThirdPrice"]);

                                    }
                                }
                                if (priceB != 0.00m)
                                {
                                    break;
                                }
                            }
                            if (priceB ==0.00m)
                            {
                                priceB = Convert.ToDecimal(abroundpage.Result[kk]["dGoodsFisrtPrice"]);
                            }

                        }


                    }
                }
                abroundpage.Result[kk]["price"] = priceB.ToString("#0.00");
                abroundpage.Result[kk]["image"] = abroundpage.Result[kk]["sGoodsPictures"].ToString().Split(',')[0];
                abroundpage.Result[kk]["address"] = address;
            }
            abroundpage.Result = abroundpage.Result.OrderByDescending(a => a["bSeckill"].ToString()).OrderByDescending(a => a["bSpecialSale"]).Select(a => a).ToList();
            //周边分页
            return abroundpage;
        }
        /// <summary>
        /// 店主故事
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_ShopSetDTO HosterStory(string ID)
        {
            return query.SingleQuery<EHECD_ShopSetDTO>("SELECT sHeadName,sHeadStory FROM EHECD_ShopSet WHERE bIsDelete=0 AND sShopID=@ID", new { ID = ID });
        }
        /// <summary>
        /// 获取店铺评论
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override Dictionary<string, object> GetCommentScore(string ID)
        {
            Dictionary<string, object> commentnum = query.SingleQuery<Dictionary<string, object>>("SELECT SUM (iCommentScore) AS score,COUNT(iCommentScore)as comments FROM EHECD_Comment WHERE bIsDeleted=0 and sGoodsID=@sGoodsID", new { sGoodsID = ID });
            if (commentnum["score"] == null)
            {
                commentnum["average"] = 5;
                commentnum["start"] = 5;
                commentnum["comments"] = 0;
            }
            else {
                commentnum["average"] = Math.Round(commentnum["score"].ToDecimal() / commentnum["comments"].ToDecimal(), 1);
               
                commentnum["start"] = Convert.ToInt32(commentnum["score"].ToInt32() / commentnum["comments"].ToInt32());
            }
           
            
            return commentnum;
        }
        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public override int CollectionIn(EHECD_CollectDTO dto)
        {
            dto.ID = GuidHelper.GetSecuentialGuid();
            return excute.InsertSingle<EHECD_CollectDTO>(dto);
        }
        /// <summary>
        /// 是否收藏
        /// </summary>
        /// <param name="goodID"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public override EHECD_CollectDTO IsCollect(string goodID, string clientID)
        {
            return query.SingleQuery<EHECD_CollectDTO>("SELECT * from EHECD_Collect WHERE iGoodsID='" + goodID + "' AND bIsCollect=1 AND bIsDeleted=0 AND iClientID='" + clientID + "'", null);
        }
        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="goodID"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public override int CancelCollect(string goodID, string clientID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM EHECD_Collect WHERE bIsCollect=1 AND iGoodsID='" + goodID + "' AND iClientID='" + clientID + "'");
            return excute.ExcuteTransaction(sb.ToString());
        }
        /// <summary>
        /// 写游记
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int WriteTravel(EHECD_TravelsNotesDTO dto, Dictionary<string, object> dic)
        {
            StringBuilder builder = new StringBuilder();
            dto.ID = GuidHelper.GetSecuentialGuid();
            dto.dPublishTime = DateTime.Now;
            dto.bIsDeleted = false;
            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_TravelsNotesDTO>(dto)).Append(";");
            string[] image = dic["sImgPath"].ToString().Split(',');
            for (int i = 0; i < image.Length; i++) {
                EHECD_ImagesDTO imageurl = new EHECD_ImagesDTO() {
                    ID = GuidHelper.GetSecuentialGuid(),
                    sBelongID=dto.ID,
                    sImagePath=image[i],
                    iState=0,
                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ImagesDTO>(imageurl)).Append(";");
            }
            return excute.ExcuteTransaction(builder.ToString());
        }
        /// <summary>
        /// 获取店铺优惠券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override string GetShopCoupon(EHECD_CouponDetailsDTO dto)
        {
            string message = "";
            //先判断优惠券的领取数量
            var haveGet = query.SingleQuery<Dictionary<string, object>>("SELECT COUNT(ID) GET FROM EHECD_CouponDetails where sCouponID=@sCouponID", new { sCouponID =dto.sCouponID });
            var have = query.SingleQuery<Dictionary<string, object>>("SELECT iCouponCount FROM EHECD_Coupon WHERE ID=@ID", new { ID = dto.sCouponID });
            if (Convert.ToUInt32(have["iCouponCount"]) - Convert.ToUInt32(haveGet["GET"]) > 0) {
                dto.ID = GuidHelper.GetSecuentialGuid();
                dto.dGetTime = DateTime.Now;
                excute.InsertSingle<EHECD_CouponDetailsDTO>(dto);
                message = "领取成功";
            }
            else {
                message = "已领取完";
            }

            return message;
        }
        /// <summary>
        /// 判断是否已领取了优惠券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override EHECD_CouponDetailsDTO IsGet(EHECD_CouponDetailsDTO dto)
        {
            return query.SingleQuery<EHECD_CouponDetailsDTO>("SELECT ID FROM EHECD_CouponDetails WHERE bIsDeleted=0 AND sCouponID='" + dto.sCouponID + "' AND sUserID='" + dto.sUserID + "'",null);
        }
        /// <summary>
        /// 根据openid查询客户表中是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_ClientDTO SearchOpernID(string ID)
        {
            return query.SingleQuery<EHECD_ClientDTO>("SELECT * FROM EHECD_Client WHERE bIsDeleted=0 AND sOpenId=@ID", new { ID = ID });
        }
        /// <summary>
        /// 向client表中插入新的用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int CreateNewClient(EHECD_ClientDTO dto)
        {
            dto.dBirthday = DateTime.Now;
            return excute.InsertSingle<EHECD_ClientDTO>(dto);
        }

        /// <summary>
        /// 根据用户进入页面后获取的ID后用户信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_ClientDTO GetClient(string ID)
        {
            return query.SingleQuery<EHECD_ClientDTO>("SELECT ID,sRegCode,sOpenId FROM EHECD_Client WHERE bIsDeleted=0 AND ID=@ID", new { ID = ID });
        }
        /// <summary>
        /// 核销订单
        /// </summary>
        /// <param name="sOrderNo"></param>
        /// <returns></returns>
        public override Dictionary<string,object> CancelOrder(string sOrderNo, string shopID)
        {
            Dictionary<string, object> dic1 = new Dictionary<string, object>();//一级分销商的信息
            Dictionary<string, object> dic2 = new Dictionary<string, object>();//二级分销商的信息
            Dictionary<string, object> dic3 = new Dictionary<string, object>();//三级分销商的信息
            dic1["havaOne"] = false;
            dic2["havaOne"] = false;
            dic3["havaOne"] = false;
            var message = "";
            var searchID = query.SingleQuery<EHECD_OrdersDTO>("SELECT ID,iType,sReceiver,iState,sClientID,dBookTime,iTotalPrice,iOriginalTotalPrice,sPartnerID FROM EHECD_Orders WHERE sOrderNo=@sOrderNo AND( iState = 1 OR iState=3) AND bIsDeleted=0 AND sStoreID='" + shopID+"'", new { sOrderNo = sOrderNo });
            
            if (searchID == null)
            {
                
                message = "订单不存在";
            }
            else {
                if (searchID.iState == 3) {
                    //如果订单是维权状态时查找
                    var returnGoods = query.SingleQuery<EHECD_ReturnOrdersDTO>("SELECT * FROM EHECD_ReturnOrders WHERE bIsDeleted=0 AND sOrderID=@ID AND (iState=0 or iState=3)", new { ID = searchID.ID });
                    if (returnGoods == null) {
                        return new Dictionary<string, object>() {
                                            {"message","订单维权中" },
                                            {"dic1",dic1 },
                                            {"dic2",dic2 },
                                            {"dic3",dic3 } };
                    }
                }


                StringBuilder builder = new StringBuilder();
                //根据订单去查询购买人的昵称
                var buyName = query.SingleQuery<EHECD_ClientDTO>("SELECT sNickName FROM EHECD_Client WHERE bIsDeleted=0 AND ID=@ID", new { ID = searchID.sClientID }).sNickName;
                if (searchID.iType == 0)
                {//如果是客房
                    //如果订单的状态是客房查出今天是否在预定的时间内
                    var orderTime = query.SingleQuery<EHECD_RoomDetailDTO>("SELECT dStartTime,sEndTime FROM EHECD_RoomDetail WHERE sOrderId=@sOrderId", new { sOrderId = searchID.ID });
                    DateTime start=Convert.ToDateTime(orderTime.dStartTime.Value.Year + "-" + orderTime.dStartTime.Value.Month + "-" + orderTime.dStartTime.Value.Day + " 00:00:00");

                    DateTime end = Convert.ToDateTime(orderTime.sEndTime.Value.Year + "-" + orderTime.sEndTime.Value.Month + "-" + orderTime.sEndTime.Value.Day + " 23:59:59");
                    //如果在预定的时间内进行核销
                    if (DateTime.Now >= start && DateTime.Now <= end)
                    {
                        builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_OrdersDTO>(new EHECD_OrdersDTO() { iState = 2 , dFinishTime =DateTime.Now}, string.Format("where sOrderNo='{0}' AND sStoreID='" + shopID + "'", sOrderNo))).Append(";");
                        message = "订单已核销";
                    }
                    //不在预定的时间内显示过期信息
                    if (DateTime.Now < start) {
                        message = "预定时间未到";
                    }
                    if (DateTime.Now > end) {
                        message = "订单已过期";
                    }
                }
                if (searchID.iType == 1)
                {
                    var ticket=query.SingleQuery<EHECD_RoomDetailDTO>("SELECT dStartTime FROM EHECD_RoomDetail WHERE sOrderId=@sOrderId", new { sOrderId = searchID.ID });
                   
                    if (DateTime.Now <= Convert.ToDateTime(ticket.dStartTime.Value.Year+"-"+ ticket.dStartTime.Value.Month + "-"+ ticket.dStartTime.Value.Day  + " 23:59:59"))
                    {
                        builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_OrdersDTO>(new EHECD_OrdersDTO() { iState = 2, dFinishTime = DateTime.Now }, string.Format("where sOrderNo='{0}' AND sStoreID='" + shopID + "'", sOrderNo))).Append(";");
                        message = "订单已核销";
                    }
                    else {
                        message = "票务已过期";
                    }
                   
                }
                if (searchID.iType == 2)
                {
                    builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_OrdersDTO>(new EHECD_OrdersDTO() { iState = 2, dFinishTime = DateTime.Now }, string.Format("where sOrderNo='{0}' AND sStoreID='" + shopID + "'", sOrderNo))).Append(";");
                    message = "订单已核销";
                }
                if (message == "订单已核销") {
                    //根据订单查出商品的id信息
                    var goods = query.SingleQuery<EHECD_OrdersGoodsDTO>("SELECT sGoodsPrimaryKey,iCommission FROM EHECD_OrdersGoods WHERE sOrderNo=@sOrderNo AND bIsDeleted=0", new { sOrderNo = sOrderNo });
                    //查找出店铺名
                    var shopName = query.SingleQuery<EHECD_ShopSetDTO>("SELECT sShopName FROM EHECD_ShopSet WHERE sShopID= @shopID", new { shopID = shopID }).sShopName;

                    //根据商品的ID查出佣金以及类型
                    decimal? money = goods.iCommission;//佣金价格

                    //查出总后台设置的提成比例
                    var precent = query.SingleQuery<EHECD_BaseSettingDTO>("SELECT iLevelOneCommissionPrecent,iLevelTwoCommissionPrecent,iLevelThreeCommissionPrecent,iServicePrecent,iPartnerCommissionPrecent FROM EHECD_BaseSetting", null);
                    //一级分销商提成总额
                    decimal? precentOne = 0.00m;
                    decimal? precentTwo = 0.00m;
                    decimal? precentThree = 0.00m;
                    precentOne = money * (Convert.ToDecimal(precent.iLevelOneCommissionPrecent) / 100);
                    //二级分销商提成总额
                     precentTwo = money * (Convert.ToDecimal(precent.iLevelTwoCommissionPrecent) / 100);
                    //三级分销商提成总额
                     precentThree = money * (Convert.ToDecimal(precent.iLevelThreeCommissionPrecent) / 100);

                    //平台提成
                    var adminPrecent = searchID.iOriginalTotalPrice * (Convert.ToDecimal(precent.iServicePrecent) / 100);

                    //合伙人提成
                    var partner = adminPrecent * (Convert.ToDecimal(precent.iPartnerCommissionPrecent) / 100);
                    
                    //查出最近一条的资金变动
                    var priceChange = query.SingleQuery<EHECD_ClientBalanceDetailDTO>("SELECT top 1 iAfterPrice FROM EHECD_ClientBalanceDetail WHERE sClientID=@sClientID ORDER BY dChangeTime DESC", new { sClientID = shopID });
                    decimal? shopMoney;//记录现在商铺变化前的钱
                    if (priceChange == null) {
                        shopMoney = 0;
                    }
                    else
                    {
                        shopMoney = priceChange.iAfterPrice;

                    }
                    //给商家加入订单金额
                    EHECD_ClientBalanceDetailDTO order = new EHECD_ClientBalanceDetailDTO()
                    {
                        ID = GuidHelper.GetSecuentialGuid(),
                        sClientID=new Guid( shopID),
                        iClientType=2,
                        sUserName= shopName,
                        iMethod=2,
                        iType=7,
                        dChangeTime=searchID.dBookTime,
                        iBeforePrice= shopMoney,
                        iPrice=searchID.iTotalPrice,
                        iAfterPrice = shopMoney + searchID.iTotalPrice,
                        sOrderID=searchID.ID,
                        sOrderNo= sOrderNo,
                        iServicePrice= adminPrecent,
                       
                        iShopID= new Guid(shopID),
                    };
                    builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(order)).Append(";");
                    //商家给平台服务费
                    EHECD_ClientBalanceDetailDTO giveService = new EHECD_ClientBalanceDetailDTO() {
                        ID = GuidHelper.GetSecuentialGuid(),
                        sClientID = new Guid(shopID),
                        iClientType = 2,
                        sUserName = shopName,
                        iMethod = 1,
                        iType=9,
                        dChangeTime = searchID.dBookTime,
                        iBeforePrice = order.iAfterPrice,
                        iPrice= adminPrecent,
                        iAfterPrice= order.iAfterPrice- adminPrecent,
                        sOrderID = searchID.ID,
                        sOrderNo = sOrderNo,
                        iServicePrice = adminPrecent,
                        iCommissionPrice = money,
                        iShopID = new Guid(shopID),
                    };
                    builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(giveService)).Append(";");
                    //合伙人信息
                    var partnerMoney = query.SingleQuery<EHECD_SystemUserDTO>("SELECT * FROM EHECD_SystemUser WHERE ID IN( SELECT sPartnerID FROM EHECD_SystemUser WHERE ID=@ID)", new { ID = shopID });

                    
                    if (searchID.sPartnerID != null) {//如果有合伙人

                        //合伙人的钱的信息
                        var pMoney = query.SingleQuery<EHECD_ClientBalanceDetailDTO>("SELECT top 1 iAfterPrice FROM EHECD_ClientBalanceDetail WHERE sClientID=@sClientID ORDER BY dChangeTime DESC", new { sClientID = searchID.sPartnerID });
                        decimal? befrorePartner;
                        if (pMoney != null)//如果没有改合伙人的信息
                        {
                            befrorePartner = pMoney.iAfterPrice;
                        }
                        else
                        {
                            befrorePartner = 0;
                        }
                        //合伙人收入提成【平台支出的】
                        EHECD_ClientBalanceDetailDTO partnerService = new EHECD_ClientBalanceDetailDTO()
                        {
                            ID = GuidHelper.GetSecuentialGuid(),
                            sClientID = searchID.sPartnerID,
                            iClientType=3,
                            sUserName=partnerMoney.sLoginName,
                            iMethod=2,
                            iType=10,
                            dChangeTime = searchID.dBookTime,
                            iBeforePrice=befrorePartner,
                            iPrice= partner,
                            iAfterPrice=befrorePartner+partner,
                            sOrderID = searchID.ID,
                            sOrderNo = sOrderNo,
                            iServicePrice = adminPrecent,
                            iCommissionPrice = money,
                            iShopID = new Guid(shopID),
                        };
                        builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(partnerService)).Append(";");
                        var befrorepartnerIncome = query.SingleQuery<EHECD_SystemUserDTO>("SELECT iAccountNalance FROM EHECD_SystemUser WHERE bIsDeleted=0 AND ID=@ID", new { ID = searchID.sPartnerID });
                        var Income = befrorepartnerIncome.iAccountNalance + partner;
                        EHECD_SystemUserDTO partnerIncome = new EHECD_SystemUserDTO();
                        partnerIncome.iAccountNalance = Income;
                        builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_SystemUserDTO>(partnerIncome, string.Format(" Where ID='{0}'", searchID.sPartnerID)));
                    }
                    bool haveOne = false;
                    bool havaTwo = false;
                    bool havaThree = false;
                    //根据订单号查看佣金
                    var goodiCommission = query.SingleQuery<EHECD_ClientBalanceDetailDTO>("SELECT iCommissionPrice FROM [dbo].[EHECD_ClientBalanceDetail] WHERE bIsDeleted=0 AND iMethod=2 AND sOrderNo=@sOrderNo AND iType=4 ", new { sOrderNo = sOrderNo }).iCommissionPrice;
                    if (goodiCommission == precentOne)
                    {
                        haveOne = true;
                    }
                    else if (goodiCommission == precentOne + precentTwo)
                    {
                        haveOne = true;
                        havaTwo = true;
                    }
                    else if (goodiCommission == precentOne + precentTwo + precentThree) {
                        haveOne = true;
                        havaTwo = true;
                        havaThree = true;
                    }
                    //商品名称
                    var goodsname = query.SingleQuery<EHECD_OrdersGoodsDTO>("SELECT sGoodsName FROM EHECD_OrdersGoods WHERE sOrderNo=@sOrderNo", new { sOrderNo = sOrderNo }).sGoodsName;

                    //查看购买人所属的分销客
                    var share= query.SingleQuery<EHECD_ClientDTO>("SELECT ID,sRegCode FROM EHECD_Client WHERE ID=@ID AND bIsDeleted=0", new { ID = searchID.sClientID });
                    var one = query.SingleQuery<EHECD_ClientDTO>("SELECT ID,sName,sRegCode,sOpenId,sNickName FROM EHECD_Client WHERE ID=@ID AND bIsDeleted=0", new { ID = share.sRegCode });
                    //如果存在一级分销客【share.sRegCode为一级分销客的ID】
                    if (haveOne == true&& one!=null)
                    {
                        dic1["havaOne"] = true;//保存一级分销商的信息
                        dic1["sOpenId"] = one.sOpenId;
                        dic1["stage"]="1级";
                        dic1["money"] ="¥"+precentOne.Value.ToString("#0.00");
                        dic1["goodsname"] = goodsname;
                        dic1["name"] = buyName;
                        //查出一级分销客的信息
                        //给一级分销客提成
                        EHECD_ClientBalanceDetailDTO dto = new EHECD_ClientBalanceDetailDTO() {
                            ID = GuidHelper.GetSecuentialGuid(),
                            sClientID = new Guid(shopID),
                            iClientType = 2,
                            sUserName = shopName,
                            iMethod = 1,
                            iType=8,
                            dChangeTime = searchID.dBookTime,
                            iBeforePrice= giveService.iAfterPrice,
                            iPrice= precentOne,
                            iAfterPrice= giveService.iAfterPrice- precentOne,
                            sOrderID = searchID.ID,
                            sOrderNo = sOrderNo,
                            iServicePrice = adminPrecent,
                            iCommissionPrice = money,
                            iShopID = new Guid(shopID),
                        };
                        builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(dto)).Append(";");
                        //查出一级分销客变更之前的钱
                        var oneBefore=query.SingleQuery<EHECD_ClientBalanceDetailDTO>("SELECT top 1 iAfterPrice FROM EHECD_ClientBalanceDetail WHERE sClientID=@sClientID ORDER BY dChangeTime DESC", new { sClientID = share.sRegCode });
                        decimal? oneBeforePrice;
                        //如果没有数据，则为0
                        if (oneBefore == null)
                        {
                            oneBeforePrice = 0;
                        }
                        else {
                            oneBeforePrice = oneBefore.iAfterPrice;
                        }
                        //一级分销商收入
                        EHECD_ClientBalanceDetailDTO oneIn = new EHECD_ClientBalanceDetailDTO()
                        {
                            ID = GuidHelper.GetSecuentialGuid(),
                            sClientID= share.sRegCode,
                            iClientType=1,
                            sUserName=one.sNickName,
                            iMethod=2,
                            iType = 5,
                            dChangeTime = searchID.dBookTime,
                            iBeforePrice= oneBeforePrice,
                            iPrice= precentOne,
                            iAfterPrice= oneBeforePrice+ precentOne,
                            sOrderID = searchID.ID,
                            sOrderNo = sOrderNo,
                            iServicePrice = adminPrecent,
                            iCommissionPrice = money,
                            iShopID = new Guid(shopID),
                        };
                        builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(oneIn)).Append(";");
                        //查出一级分销商的总收入
                        var OneIncome = query.SingleQuery<EHECD_ClientDTO>("SELECT dIncome FROM EHECD_Client WHERE ID=@ID", new { ID = one.ID });
                        //算出现在一级分销商的总收入
                        var changeOneIn = OneIncome.dIncome + precentOne;
                        //将数据更新到client表中
                        builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ClientDTO>(new EHECD_ClientDTO { dIncome = changeOneIn }, string.Format(" Where ID='{0}'", one.ID)));
                        var two = query.SingleQuery<EHECD_ClientDTO>("SELECT ID,sRegCode,sOpenId,sNickName FROM EHECD_Client WHERE ID=@ID AND bIsDeleted=0", new { ID = one.sRegCode });
                        //如果存在二级分销商
                        if (havaTwo ==true&&two!=null){
                            dic2["havaOne"] = true;//保存二级分销商的信息
                            dic2["sOpenId"] = two.sOpenId;
                            dic2["stage"] = "2级";
                            dic2["money"] = "¥" + precentTwo.Value.ToString("#0.00");
                            dic2["goodsname"] = goodsname;
                            dic2["name"] = buyName;
                            EHECD_ClientBalanceDetailDTO twoShare = new EHECD_ClientBalanceDetailDTO()
                            {
                                ID = GuidHelper.GetSecuentialGuid(),
                                sClientID = new Guid(shopID),
                                iClientType = 2,
                                sUserName = shopName,
                                iMethod = 1,
                                iType = 8,
                                dChangeTime = searchID.dBookTime,
                                iBeforePrice = oneIn.iAfterPrice,
                                iPrice = precentTwo,
                                iAfterPrice = oneIn.iAfterPrice - precentTwo,
                                sOrderID = searchID.ID,
                                sOrderNo = sOrderNo,
                                iServicePrice = adminPrecent,
                                iCommissionPrice = money,
                                iShopID = new Guid(shopID),
                            };
                            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(twoShare)).Append(";");

                            //查出二级分销客变更之前的钱
                            var twoBefore = query.SingleQuery<EHECD_ClientBalanceDetailDTO>("SELECT top 1 iAfterPrice FROM EHECD_ClientBalanceDetail WHERE sClientID=@sClientID ORDER BY dChangeTime DESC", new { sClientID = two.ID });

                            decimal? twoBeforePrice;
                            //如果没有数据，则为0
                            if (twoBefore == null)
                            {
                                twoBeforePrice = 0;
                            }
                            else
                            {
                                twoBeforePrice = twoBefore.iAfterPrice;
                            }

                            EHECD_ClientBalanceDetailDTO twoIn = new EHECD_ClientBalanceDetailDTO()
                            {
                                ID = GuidHelper.GetSecuentialGuid(),
                                sClientID = two.ID,
                                iClientType = 1,
                                sUserName = two.sNickName,
                                iMethod = 2,
                                iType = 5,
                                dChangeTime = searchID.dBookTime,
                                iBeforePrice = twoBeforePrice,
                                iPrice = precentTwo,
                                iAfterPrice = twoBeforePrice + precentTwo,
                                sOrderID = searchID.ID,
                                sOrderNo = sOrderNo,
                                iServicePrice = adminPrecent,
                                iCommissionPrice = money,
                                iShopID = new Guid(shopID),
                            };
                            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(twoIn)).Append(";");

                            //查出二级分销商的总收入
                            var TwoIncome = query.SingleQuery<EHECD_ClientDTO>("SELECT dIncome FROM EHECD_Client WHERE ID=@ID", new { ID = two.ID });
                            //算出现在二级分销商的总收入
                            var changeTwoIn = TwoIncome.dIncome + precentTwo;
                            //将数据更新到client表中
                            builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ClientDTO>(new EHECD_ClientDTO { dIncome = changeTwoIn }, string.Format(" Where ID='{0}'", two.ID)));
                            var three = query.SingleQuery<EHECD_ClientDTO>("SELECT ID,sRegCode,sOpenId,sNickName FROM EHECD_Client WHERE ID=@ID AND bIsDeleted=0", new { ID = two.sRegCode });
                            //如果存在三级分销商查出分销商的信息
                            if (havaThree == true&&three!=null) {
                                dic3["havaOne"] = true;//保存二级分销商的信息
                                dic3["sOpenId"] = three.sOpenId;
                                dic3["stage"] = "3级";
                                dic3["money"] = "¥" + precentThree.Value.ToString("#0.00");
                                dic3["goodsname"] = goodsname;
                                dic3["name"] = buyName;
                                EHECD_ClientBalanceDetailDTO threeShare = new EHECD_ClientBalanceDetailDTO()
                                {
                                    ID = GuidHelper.GetSecuentialGuid(),
                                    sClientID = new Guid(shopID),
                                    iClientType = 2,
                                    sUserName = shopName,
                                    iMethod = 1,
                                    iType = 8,
                                    dChangeTime = searchID.dBookTime,
                                    iBeforePrice = twoIn.iAfterPrice,
                                    iPrice = precentThree,
                                    iAfterPrice = twoIn.iAfterPrice - precentThree,
                                    sOrderID = searchID.ID,
                                    sOrderNo = sOrderNo,
                                    iServicePrice = adminPrecent,
                                    iCommissionPrice = money,
                                    iShopID = new Guid(shopID),
                                };
                                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(threeShare)).Append(";");

                                //查出三级分销客变更之前的钱
                                var threeBefore = query.SingleQuery<EHECD_ClientBalanceDetailDTO>("SELECT top 1 iAfterPrice FROM EHECD_ClientBalanceDetail WHERE sClientID=@sClientID ORDER BY dChangeTime DESC", new { sClientID = three.ID });

                                decimal? threeBeforePrice;
                                //如果没有数据，则为0
                                if (threeBefore == null)
                                {
                                    threeBeforePrice = 0;
                                }
                                else
                                {
                                    threeBeforePrice = twoBefore.iAfterPrice;
                                }

                                EHECD_ClientBalanceDetailDTO threeIn = new EHECD_ClientBalanceDetailDTO()
                                {
                                    ID = GuidHelper.GetSecuentialGuid(),
                                    sClientID = three.ID,
                                    iClientType = 1,
                                    sUserName = three.sNickName,
                                    iMethod = 2,
                                    iType = 5,
                                    dChangeTime = searchID.dBookTime,
                                    iBeforePrice = threeBeforePrice,
                                    iPrice = precentThree,
                                    iAfterPrice = threeBeforePrice + precentThree,
                                    sOrderID = searchID.ID,
                                    sOrderNo = sOrderNo,
                                    iServicePrice = adminPrecent,
                                    iCommissionPrice = money,
                                    iShopID = new Guid(shopID),
                                };
                                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(threeIn)).Append(";");

                                //查出二级分销商的总收入
                                var ThreeIncome = query.SingleQuery<EHECD_ClientDTO>("SELECT dIncome FROM EHECD_Client WHERE ID=@ID", new { ID = three.ID });
                                //算出现在二级分销商的总收入
                                var changeThreeIn = ThreeIncome.dIncome + precentThree;
                                //将数据更新到client表中
                                builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ClientDTO>(new EHECD_ClientDTO { dIncome = changeThreeIn }, string.Format(" Where ID='{0}'", three.ID)));
                            }
                        }
                    }
                    decimal ?shopToShare=0.00m;
                    if (haveOne == true) {
                        shopToShare = precentOne;
;                    }
                    if (havaTwo == true) {
                        shopToShare += precentTwo;
                    }
                    if (havaThree == true) {
                        shopToShare += precentThree;
                    }

                  
                    //查出现在店铺的金额
                    var shopMoneyBefote = query.SingleQuery<EHECD_SystemUserDTO>("SELECT iAccountNalance FROM EHECD_SystemUser WHERE bIsDeleted=0 AND ID=@ID", new { ID = shopID });
                    //平台支付
                    builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_SystemUserDTO>(new EHECD_SystemUserDTO() { iAccountNalance = shopMoneyBefote.iAccountNalance + searchID.iTotalPrice - adminPrecent- shopToShare }, string.Format("where ID='{0}'", shopID)));
                    //根据佣金的总金额修改商家的佣金数据记录
                    builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ClientBalanceDetailDTO>(new EHECD_ClientBalanceDetailDTO() { iCommissionPrice = shopToShare }, string.Format(" Where ID='{0}'", order.ID)));
                }
                excute.ExcuteTransaction(builder.ToString());


            }

            return new Dictionary<string, object>() {
                {"message",message },
                {"dic1",dic1 },
                {"dic2",dic2 },
                {"dic3",dic3 },

            };
        }
        /// <summary>
        /// 是否是本店的分享客
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public override Dictionary<string, object> IsShareBelong(Dictionary<string, object> dic)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (dic["sclient"].ToString() == "")
            {
                result["bIsShopShare"] = 0;
                result["ID"] = "";
            }
            else {
                var isShare = query.SingleQuery<EHECD_SharedClientInfoDTO>("SELECT * FROM [dbo].[EHECD_SharedClientInfo] WHERE sClientID='" + dic["sclient"] + "' and  sShopID='" + dic["shopID"] + "' AND bIsForzen=0 AND bIsDeleted=0", null);
                if (isShare != null)
                {
                    result["bIsShopShare"] = 1;
                    result["ID"] = dic["sclient"];
                }
                else
                {
                    result["bIsShopShare"] = 0;
                    result["ID"] = "";
                }
            }
            
            return result;
        }
        /// <summary>
        /// 是否被冻结
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_ClientDTO IsFrozen(string ID)
        {
            var result = query.SingleQuery<EHECD_ClientDTO>("SELECT * FROM EHECD_Client WHERE bIsDeleted=0 AND ID=@ID", new { ID = ID });
            return result;
        }

        
    }
}
