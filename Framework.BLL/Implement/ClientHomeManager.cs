using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;
using Framework.Helper;

namespace Framework.BLL
{
    /// <summary>
    /// 首页业务类
    /// </summary>
    public class ClientHomeManager : IClientHomeManager
    {
        /// <summary>
        /// 获取首页基础展示数据
        /// </summary>
        /// <returns></returns>
        public override object QueryClietHomeData(Dictionary<string, object> param)
        {
            //1.Banner首页轮播图
            var homeCarouselImages = query.QueryList<Dictionary<string, object>>(@"SELECT
                                                                                        ID,
	                                                                                    sImagePath,
	                                                                                    sLink,
	                                                                                    iOrder
                                                                                    FROM
	                                                                                    EHECD_Images
                                                                                    WHERE
	                                                                                    bIsDeleted = 0
                                                                                    AND bDisplay = 1
                                                                                    AND iState = 3
                                                                                    ORDER BY
	                                                                                    iOrder ASC", null);

            //2.秒杀专区（首页显示5个）
            var homeSeckillListTemp = query.QueryList<Dictionary<string, object>>(@"SELECT
	                                                                                TOP (5) A.ID,
	                                                                                A.sGoodsName,
                                                                                    A.sGoodsCategory,--1--客房，2-票务，3--周边
	                                                                                A.sGoodsPictures,
	                                                                                A.dSeckillPrices,
	                                                                                A.sSeckillTime,
                                                                                    A.sActivityUseTime
                                                                                FROM
	                                                                                EHECD_Goods AS A
	                                                                                LEFT JOIN  EHECD_SystemUser AS B
	                                                                                ON B.ID=A.sStoreId
                                                                                WHERE
                                                                                B.tUserState=0--过滤掉被冻结的商家
	                                                                            AND A.bShelves = 1
                                                                                AND A.bSeckill = 1
                                                                                AND A.bIsDeleted = 0", null);

           
            var homeSeckillList = from a in homeSeckillListTemp
                                  where DateTime.Parse(a["sSeckillTime"].ToString().Split(',')[0]) <= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))
                                     && DateTime.Parse(a["sSeckillTime"].ToString().Split(',')[1]) >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))
                                  select a;

            //3.特卖专区（首页显示5个）
            var homeSpecialSaleListTemp = query.QueryList<Dictionary<string, object>>(@"SELECT
                                                                                        TOP (5) A.ID,
	                                                                                    A.sGoodsName,
	                                                                                    A.sGoodsPictures,
                                                                                        A.sGoodsCategory,--1--客房，2-票务，3--周边
                                                                                        A.dGoodsFisrtPrice,
	                                                                                    A.dSpecialSalePrices,
	                                                                                    A.sSpecialSaleTime,
                                                                                        A.sActivityUseTime
                                                                                    FROM
	                                                                                    EHECD_Goods AS A 
	                                                                                    LEFT JOIN  EHECD_SystemUser AS B
																						ON B.ID=A.sStoreId
                                                                                    WHERE
                                                                                        B.tUserState=0--过滤掉被冻结的商家
	                                                                                AND A.bShelves = 1
                                                                                    AND A.bSpecialSale = 1
                                                                                    AND A.bIsDeleted = 0", null);

            var homeSpecialSaleList = from a in homeSpecialSaleListTemp
                                      where DateTime.Parse(a["sSpecialSaleTime"].ToString().Split(',')[0]) <=DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))
                                     && DateTime.Parse(a["sSpecialSaleTime"].ToString().Split(',')[1]) >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))
                                      select a;

            //4.首页展示Banner图
            var homeShowBannerList = query.QueryList<Dictionary<string, object>>(@"SELECT
                                                                                        TOP (10) ID,
                                                                                        sBelongID,
	                                                                                    sImagePath,
	                                                                                    sLink,
	                                                                                    iOrder
                                                                                    FROM
	                                                                                    EHECD_Images
                                                                                    WHERE
	                                                                                    bIsDeleted = 0
                                                                                    AND bDisplay = 1
                                                                                    AND iState = 1
                                                                                    ORDER BY
	                                                                                    iOrder ASC", null);

            //5.推荐名宿(销量排名前十的名宿)
            var homeRecommentStore = query.QueryList<Dictionary<string, object>>(@"SELECT TOP 10 * FROM (
                                                                                        SELECT  S.sShopID AS sStoreID,--店铺ID
	                                                                                    
                                                                                            (
                                                                                                SELECT
                                                                                                    TOP (1) imageT.sImagePath
                                                                                                FROM
                                                                                                    EHECD_Images imageT
                                                                                                WHERE
                                                                                                    imageT.sBelongID = S.sShopID and imageT.iState=6 and imageT.bIsDeleted=0
                                                                                            ) sImagePath, --店铺图片
                                                                                            --民宿客房总销量
                                                                                               ISNULL((SELECT
                                                                                                    SUM (B.iAmount)  --销量
                                                                                                FROM
                                                                                                    EHECD_Orders A,
                                                                                                    --订单表
                                                                                                    EHECD_OrdersGoods B --订单商品表
                                                                                                WHERE
                                                                                                    A.ID = B.sOrderID
                                                                                                AND A.iType = 0 --客房订单
                                                                                                AND A.iState = 2 --已核销
                                                                                                AND A.sStoreID=S.sShopID),0) AS saleCount ,
                                                                                            D.sShopName,--店铺名称			                                                                                    
                                                                                            D.sAddress, --店铺具体地址			                                                                                   
                                                                                            D.sCity, --店铺所在城市			                                                                                   
                                                                                            D.sCounty,--店铺所在区			                                                                                    
                                                                                            ISNULL(dbo.FN_CALCULATE_AVG_SCORE (S.sShopID,1),5) iCommentScore, --店铺所有客房评论平均分			                                                                                   
                                                                                            dbo.FN_CALCULATE_MIN_PRICE (S.sShopID,1) dGoodsPrice --店铺所有客房最低价
		
        
                                                                                                FROM EHECD_ShopSet S,EHECD_SystemUser Q,EHECD_ShopSet D 
        
		                                                                                        WHERE S.sShopID=q.ID AND S.bIsDelete=0 AND Q.tUserState=0 AND Q.bIsDeleted=0
			                                                                                        AND  S.sShopID = D.sShopID AND CHARINDEX(@sCity,D.sCity,0)>0
                                                                                        )TEMP
                                                                                        ORDER BY TEMP.saleCount DESC
                                                                                    ", new { sCity = param["sCity"] });
            return new Dictionary<string, object>() {
                {"homeCarouselImages",homeCarouselImages },
                {"homeSeckillList",homeSeckillList.ToList() },
                {"homeSpecialSaleList",homeSpecialSaleList.ToList() },
                {"homeShowBannerList",homeShowBannerList },
                {"homeRecommentStore",homeRecommentStore }
            };
        }

        /// <summary>
        /// 获取优惠劵列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override PagingRet<EHECD_CouponDTO> QueryCouponList(PageInfo pageInfo, Dictionary<string, object> param)
        {
            //SQL
            StringBuilder builder = new StringBuilder();

            // 当前用户
            var sUserID = Helper.CommonHelper.GetDictionaryValue("sUserID", param, typeof(string));

            // 查询出平台的所有优惠劵
            builder.Append(string.Format(@"SELECT
	                                *
                                FROM
	                                (
		                                SELECT
			                                b.*
		                                FROM
			                                (
				                                SELECT
					                                sCouponID,
					                                COUNT (sCouponID) couponCount
				                                FROM
					                                EHECD_CouponDetails
				                                GROUP BY
					                                sCouponID
			                                ) a,
			                                EHECD_Coupon b
		                                WHERE
			                                a.sCouponID = b.ID
		                                AND a.couponCount < b.iCouponCount
		                                UNION
			                                SELECT
				                                *
			                                FROM
				                                EHECD_Coupon
			                                WHERE
				                                ID NOT IN (
					                                SELECT
						                                sCouponID
					                                FROM
						                                EHECD_CouponDetails
					                                WHERE
						                                bIsDeleted = 0
				                                )
	                                ) g
                                WHERE
	                                dValidDateEnd >= convert(varchar(10),getDate(),120)
                                AND bIsDeleted=0
                                AND sStoreID IS NULL AND ID!='{0}' AND sCouponName IS NULL ", new Guid()));// 排除商家入驻平台返还的给分享客的优惠劵

            // 排除当前登录人所领取的优惠劵
            if (!string.IsNullOrEmpty(sUserID.ToString())) builder.Append(string.Format(@" AND ID NOT IN (
	                                                                                        SELECT
		                                                                                        sCouponID
	                                                                                        FROM
		                                                                                        EHECD_CouponDetails
	                                                                                        WHERE
		                                                                                        sUserID = '{0}' ) ",sUserID.ToString()));

            // 查询结果
            return query.PaginationQuery<EHECD_CouponDTO>(builder.ToString(), pageInfo,null); ;
        }

        /// <summary>
        /// 获取秒杀专区列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> QuerySecKillList(PageInfo pageInfo)
        {
            var homeSeckillListTemp = query.PaginationQuery<Dictionary<string, object>>(@"SELECT
	                                                                                        A.ID,
	                                                                                        A.sGoodsName,
                                                                                            A.sGoodsCategory,--1--客房，2-票务，3--周边
	                                                                                        A.dGoodsFisrtPrice,
	                                                                                        A.sGoodsPictures,
	                                                                                        A.dSeckillPrices,
	                                                                                        A.sSeckillTime,
                                                                                            A.sActivityUseTime
                                                                                        FROM
	                                                                                        EHECD_Goods AS A
                                                                                            LEFT JOIN  EHECD_SystemUser AS B
																						    ON B.ID=A.sStoreId
                                                                                        WHERE
                                                                                            B.tUserState=0--过滤掉被冻结的商家
	                                                                                    AND A.bShelves = 1
                                                                                        AND A.bSeckill = 1
                                                                                        AND A.bIsDeleted = 0", pageInfo, null);
            var homeSeckillList = from a in homeSeckillListTemp.Result
                                  where DateTime.Parse(a["sSeckillTime"].ToString().Split(',')[0]) <= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))
                                     && DateTime.Parse(a["sSeckillTime"].ToString().Split(',')[1]) >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))
                                  select a;

            return new PagingRet<Dictionary<string, object>>() { MaxCount = homeSeckillListTemp.MaxCount, Result = homeSeckillList.ToList() };
        }

        /// <summary>
        /// 查询定位城市
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override IList<Dictionary<string, object>> QuerySelectCity(Dictionary<string, object> param)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"SELECT
	                                *
                                FROM
	                                (
		                                SELECT
			                                --首字母
			                                A.sFristLetter,
			                                --城市
			                                A.sCity
		                                FROM
			                                EHECD_ShopSet A,
			                                EHECD_SystemUser B
		                                WHERE
			                                A.sShopID = B.ID
		                                AND B.tUserType = 1 --店铺用户
		                                AND B.tUserState = 0 --正常（未冻结）
		                                AND A.bIsDelete = 0 --未删除
		                                GROUP BY
			                                --首字母分组
			                                A.sFristLetter,
			                                --城市
			                                A.sCity
	                                ) C
                                WHERE
	                                1 = 1 ");

            var sCity = Helper.CommonHelper.GetDictionaryValue("sCity", param, typeof(string));

            if (!string.IsNullOrEmpty(sCity.ToString())) builder.Append(string.Format(@" AND sCity LIKE '%{0}%'", sCity.ToString()));

            //添加排序
            builder.Append(@" ORDER BY sFristLetter ASC ");

            //查询出的结果
            var data = query.QueryList<Dictionary<string, object>>(builder.ToString(), null).ToList();

            //获取所有的首字母
            var allFristLatter = data.Select(a => a["sFristLetter"].ToString().ToUpper()).Distinct();

            //组装数据
            List<Dictionary<string, object>> resultArr = new List<Dictionary<string, object>>();

            //循环首字母
            foreach (var latter in allFristLatter)
            {

                var tempDic = new Dictionary<string, object>();
                //添加首字母
                tempDic["latter"] = latter;

                //装载城市数据
                List<string> sCityBuilder = new List<string>();
                //循环结果集合
                Parallel.ForEach<Dictionary<string, object>>(data, delegate (Dictionary<string, object> item)
                {
                    //如果首字母相同
                    if (latter.ToUpper() == item["sFristLetter"].ToString().ToUpper())
                    {
                        sCityBuilder.Add(item["sCity"].ToString());
                    }
                });

                //添加城市
                tempDic["sCity"] = string.Join(",", sCityBuilder);

                //将筛选结果添加进resultArr
                resultArr.Add(tempDic);
            };

            return resultArr;
        }

        /// <summary>
        /// 查询特价专区列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> QuerySpecialSaleList(PageInfo pageInfo)
        {
            var homeSpecialSaleListTemp = query.PaginationQuery<Dictionary<string, object>>(@"SELECT
                                                                                                A.ID,
	                                                                                            A.sGoodsName,
	                                                                                            A.sGoodsPictures,
                                                                                                A.dGoodsFisrtPrice,
	                                                                                            A.dSpecialSalePrices,
	                                                                                            A.sSpecialSaleTime,
                                                                                                A.sActivityUseTime
                                                                                            FROM
	                                                                                             EHECD_Goods AS A
                                                                                                 LEFT JOIN  EHECD_SystemUser AS B
																						         ON B.ID=A.sStoreId
                                                                                            WHERE
                                                                                                B.tUserState=0--过滤掉被冻结的商家
	                                                                                        AND A.bShelves = 1
                                                                                            AND A.bSpecialSale = 1
                                                                                            AND A.bIsDeleted = 0", pageInfo, null);
            var homeSpecialSaleList = from a in homeSpecialSaleListTemp.Result
                                      where DateTime.Parse(a["sSpecialSaleTime"].ToString().Split(',')[0]) <= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))
                                     && DateTime.Parse(a["sSpecialSaleTime"].ToString().Split(',')[1]) >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))
                                      select a;

            return new PagingRet<Dictionary<string, object>>() { MaxCount = homeSpecialSaleListTemp.MaxCount, Result = homeSpecialSaleList.ToList() }; ;
        }

        /// <summary>
        /// 领取优惠劵
        /// </summary>
        /// <returns></returns>
        public override int ExcuteGetCoupon(Dictionary<string, object> param)
        {
            return CouponClientManager.GetCoupon(param);
        }

        /// <summary>
        /// 查询【名宿】
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> QueryByStore(PageInfo pageInfo, Dictionary<string, object> param)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"SELECT
	                                *
                                FROM
	                                (
		                                SELECT
			                                C.sShopID,
			                                --店铺ID
                                            (
				                                SELECT
					                                TOP (1) imageT.sImagePath
				                                FROM
					                                EHECD_Images imageT
				                                WHERE
					                                imageT.sBelongID = C.sShopID  AND iState=6 AND bIsDelete=0
			                                ) sImagePath,
			                                --店铺图片
			                                C.sShopName,
			                                --店铺名称
			                                C.sAddress,
			                                --店铺具体地址
			                                C.sCity,
			                                --店铺所在城市
			                                C.sCounty,C.sIntroduce,
			                                --店铺所在区
			                                {0}
			                                dbo.FN_CALCULATE_AVG_SCORE (C.sShopID,1) iCommentScore,
			                                --店铺所有客房评论平均分
			                                dbo.FN_CALCULATE_MIN_PRICE (C.sShopID,1) dGoodsPrice --店铺所有客房最低价
		                                FROM
			                                EHECD_ShopSet C,EHECD_SystemUser F WHERE C.sShopID=F.ID AND F.tUserState=0 AND F.bIsDeleted=0--过滤已冻结的店铺
	                                ) D
                                WHERE
                                    -- 筛选拥有【名宿商品】的店铺
	                                dGoodsPrice IS NOT NULL AND 1=1 ");

            var name = Helper.CommonHelper.GetDictionaryValue("name", param, typeof(string));//搜索框条件
            var sCounty = Helper.CommonHelper.GetDictionaryValue("sCounty", param, typeof(string));//区
            var sCity = Helper.CommonHelper.GetDictionaryValue("sCity", param, typeof(string));//市
            var iPriceStart = Helper.CommonHelper.GetDictionaryValue("iPriceStart", param, typeof(string));//价格筛选
            var iPriceEnd = Helper.CommonHelper.GetDictionaryValue("iPriceEnd", param, typeof(string));//价格筛选
            var orderType = Helper.CommonHelper.GetDictionaryValue("orderType", param, typeof(string));//排序类型（desc asc）
            var roomType = Helper.CommonHelper.GetDictionaryValue("iRoomType", param, typeof(string));//房间类型
            var longitude=Helper.CommonHelper.GetDictionaryValue("longitude",param,typeof(string));//精度
            var latitude= Helper.CommonHelper.GetDictionaryValue("latitude", param, typeof(string));//纬度

            //搜索框
            if (!string.IsNullOrEmpty(name.ToString())) builder.Append(string.Format(" AND sShopName LIKE '%{0}%' ", name.ToString()));

            //地区选择
            if (!string.IsNullOrEmpty(sCounty.ToString()) && sCounty.ToString() != "-1") builder.Append(string.Format(" AND sCounty = '{0}' ", sCounty.ToString()));

            //城市 sCity
            if (!string.IsNullOrEmpty(sCity.ToString())) builder.Append(string.Format(" AND sCity = '{0}' ", sCity.ToString()));

            //开始价格
            if (!string.IsNullOrEmpty(iPriceStart.ToString()) && iPriceStart.ToString() != "-1") builder.Append(string.Format(" AND dGoodsPrice>={0} ", iPriceStart.ToString()));
            //结束价格
            if (!string.IsNullOrEmpty(iPriceEnd.ToString()) && iPriceStart.ToString() != "2000" && iPriceEnd.ToString() != "-1") builder.Append(string.Format(" AND dGoodsPrice<={0} ", iPriceEnd.ToString()));

            //房型选择
            if (!string.IsNullOrEmpty(roomType.ToString()) && roomType.ToString() != "-1")
                builder.Append(string.Format(@" AND sShopID IN (
	                                                SELECT
		                                                sStoreId
	                                                FROM
		                                                EHECD_Goods --商品表
	                                                WHERE
		                                                sGoodsCategory = 1
	                                                AND bSeckill = 0 --非秒杀
	                                                AND bSpecialSale = 0 --非特卖
	                                                AND bShelves = 1 --已上架
	                                                AND sHouseSize = '{0}' --房型ID
                                                ) ", roomType.ToString()));

            //执行语句Sql
            string doSql = builder.ToString();

            //排序字段
            if (string.IsNullOrEmpty(orderType.ToString()))
            {
                //(默认)距离优先(调用存储过程)
                orderType = "0";
            }

            switch (orderType.ToString())
            {
                case "0"://距离优先
                    if (!string.IsNullOrEmpty(longitude.ToString()) && !string.IsNullOrEmpty(latitude.ToString()))
                    {

                        // 计算最短距离的商店
                        doSql = string.Format(doSql,
                            string.Format(@"dbo.Func_GetDistance ({0}, {1}, {2}, {3}) iDistance,--店铺距离当前定位的距离", longitude, latitude, "C.sLONG", "C.sLat"));

                        //排序规则
                        pageInfo.OrderBy = "iDistance";
                        pageInfo.orderType = OrderType.ASC;
                    }
                    else
                    {
                        //去除初始占位符
                        doSql = string.Format(doSql, "");

                    }

                    break;
                case "1"://好评优先
                    doSql = string.Format(doSql, "");
                    pageInfo.OrderBy = "iCommentScore";
                    pageInfo.orderType = OrderType.DESC;
                    var good= query.PaginationQuery<Dictionary<string, object>>(doSql, pageInfo, null);//获取所有名宿
                    for (int i = 0; i < good.Result.Count(); i++) {
                        if (good.Result[i]["iCommentScore"] == null) {
                            good.Result[i]["iCommentScore"] = 5;
                        }
                    }
                    //去除初始占位符
                    good.Result = good.Result.OrderByDescending(a => a["iCommentScore"].ToInt32()).ToList();
                    return good;
                case "2"://价格从高到底

                    //去除初始占位符
                    doSql = string.Format(doSql, "");

                    pageInfo.OrderBy = "dGoodsPrice";
                    pageInfo.orderType = OrderType.DESC;
                    break;
                case "3"://价格从低到高

                    //去除初始占位符
                    doSql = string.Format(doSql, "");

                    pageInfo.OrderBy = "dGoodsPrice";
                    pageInfo.orderType = OrderType.ASC;
                    break;
            }

            var homeRecommentStore = query.PaginationQuery<Dictionary<string, object>>(doSql, pageInfo, null);//获取所有名宿

            return homeRecommentStore;
        }

        /// <summary>
        /// 查询【票务】
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> QueryByTiket(PageInfo pageInfo, Dictionary<string, object> param)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"SELECT
	                                *
                                FROM
	                                (
		                                --每张票务的评论平均分
		                                SELECT
			                                A.ID,
			                                A.sStoreId,
			                                --店铺ID
                                            A.dGoodsFisrtPrice dGoodsPrice,
			                                --商品价格
			                                A.sGoodsName,
			                                --商品名称
											A.sGoodsIntroduce,A.sHouseOrTicketDetail,
											--商品简介
                                            A.sGoodsPictures sImagePath,
			                                --商品图片
			                                (
				                                SELECT
					                                CASE WHEN AVG (temp.iCommentScore) IS NULL THEN 5 ELSE AVG (temp.iCommentScore) END --没有评价就默认5分
				                                FROM
					                                EHECD_Comment temp
				                                WHERE
					                                temp.sGoodsID = A.ID
			                                ) iCommentScore,
			                                --评分 平均分（某些商品可能没有）
			                                C.sShopName,
			                                --店铺名称
			                                C.sAddress,
			                                --店铺具体地址
			                                C.sCounty,
                                            {0}
			                                --店铺所在区
			                                C.sCity --店铺所在市
		                                FROM
			                                EHECD_Goods A
		                                INNER JOIN EHECD_ShopSet C ON A.sStoreId = C.sShopID --店铺表
                                        INNER JOIN EHECD_SystemUser F ON C.sShopID=F.ID AND F.tUserState=0 AND F.bIsDeleted=0
		                                WHERE
			                                A.sGoodsCategory = 2 --票务商品
		                                AND A.bIsDeleted = 0 --未删除
		                                AND A.bSeckill = 0 --非秒杀
		                                AND A.bSpecialSale = 0 --非特卖
		                                AND A.bShelves = 1 --已上架
	                                ) G
                                WHERE
	                                1 = 1 ");

            var name = Helper.CommonHelper.GetDictionaryValue("name", param, typeof(string));//搜索框条件
            var sCounty = Helper.CommonHelper.GetDictionaryValue("sCounty", param, typeof(string));//区
            var sCity = Helper.CommonHelper.GetDictionaryValue("sCity", param, typeof(string));//市
            var iPriceStart = Helper.CommonHelper.GetDictionaryValue("iPriceStart", param, typeof(string));//价格筛选
            var iPriceEnd = Helper.CommonHelper.GetDictionaryValue("iPriceEnd", param, typeof(string));//价格筛选
            var orderType = Helper.CommonHelper.GetDictionaryValue("orderType", param, typeof(string));//排序类型（desc asc）
            var longitude=Helper.CommonHelper.GetDictionaryValue("longitude",param,typeof(string));//精度
            var latitude= Helper.CommonHelper.GetDictionaryValue("latitude", param, typeof(string));//纬度

            //搜索框
            if (!string.IsNullOrEmpty(name.ToString())) builder.Append(string.Format(" AND sGoodsName LIKE '%{0}%' ", name.ToString()));

            //地区选择
            if (!string.IsNullOrEmpty(sCounty.ToString()) && sCounty.ToString() != "-1") builder.Append(string.Format(" AND sCounty = '{0}' ", sCounty.ToString()));

            //城市 sCity
            if (!string.IsNullOrEmpty(sCity.ToString())) builder.Append(string.Format(" AND sCity = '{0}' ", sCity.ToString()));

            //开始价格
            if (!string.IsNullOrEmpty(iPriceStart.ToString()) && iPriceStart.ToString() != "-1") builder.Append(string.Format(" AND dGoodsPrice>={0} ", iPriceStart.ToString()));
            if (!string.IsNullOrEmpty(iPriceEnd.ToString()) && iPriceStart.ToString() != "2000" && iPriceEnd.ToString() != "-1") builder.Append(string.Format(" AND dGoodsPrice<={0} ", iPriceEnd.ToString()));

            //执行语句Sql
            string doSql = builder.ToString();

            //排序字段
            if (string.IsNullOrEmpty(orderType.ToString()))
            {
                //(默认)距离优先(调用存储过程)
                orderType = "0";
            }

            switch (orderType.ToString())
            {
                case "0"://距离优先
                    if (!string.IsNullOrEmpty(longitude.ToString()) && !string.IsNullOrEmpty(latitude.ToString()))
                    {

                        // 计算最短距离的商店
                        doSql = string.Format(doSql,
                            string.Format(@"dbo.Func_GetDistance ({0}, {1}, {2}, {3}) iDistance,--店铺距离当前定位的距离", longitude, latitude, "C.sLONG", "C.sLat"));

                        //排序规则
                        pageInfo.OrderBy = "iDistance";
                        pageInfo.orderType = OrderType.ASC;
                    }
                    else
                    {
                        //去除初始占位符
                        doSql = string.Format(doSql, "");

                    }

                    break;
                case "1"://好评优先

                    //去除初始占位符
                    doSql = string.Format(doSql, "");

                    pageInfo.OrderBy = "iCommentScore";
                    pageInfo.orderType = OrderType.DESC;
                    break;
                case "2"://价格从高到底

                    //去除初始占位符
                    doSql = string.Format(doSql, "");

                    pageInfo.OrderBy = "dGoodsPrice";
                    pageInfo.orderType = OrderType.DESC;
                    break;
                case "3"://价格从低到高

                    //去除初始占位符
                    doSql = string.Format(doSql, "");

                    pageInfo.OrderBy = "dGoodsPrice";
                    pageInfo.orderType = OrderType.ASC;
                    break;
            }

            return query.PaginationQuery<Dictionary<string, object>>(doSql, pageInfo, null);
        }

        /// <summary>
        /// 查询【周边】
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> QueryByAround(PageInfo pageInfo, Dictionary<string, object> param)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"SELECT
	                                *
                                FROM
	                                (
		                                --每张票务的评论平均分
		                                SELECT
			                                A.ID,
			                                A.sStoreId,
			                                --店铺ID
                                            A.dGoodsFisrtPrice dGoodsPrice,
			                                --商品价格
			                                A.sGoodsName,
			                                --商品名称
                                             A.sGoodsPictures sImagePath,
			                                --商品图片
			                                (
				                                SELECT
					                                CASE WHEN AVG (temp.iCommentScore) IS NULL THEN 5 ELSE AVG (temp.iCommentScore) END --没有评价就默认5分
				                                FROM
					                                EHECD_Comment temp
				                                WHERE
					                                temp.sGoodsID = A.ID
			                                ) iCommentScore,
			                                --评分 平均分（某些商品可能没有）
			                                C.sShopName,
			                                --店铺名称
			                                C.sAddress,
			                                --店铺具体地址
			                                C.sCounty,
			                                --店铺所在区
                                            {0}
			                                C.sCity --店铺所在市
		                                FROM
			                                EHECD_Goods A
		                                INNER JOIN EHECD_ShopSet C ON A.sStoreId = C.sShopID --店铺表
                                        INNER JOIN EHECD_SystemUser F ON C.sShopID = F.ID 
	                                        AND F.tUserState = 0 AND F.bIsDeleted=0
		                                WHERE
			                                A.sGoodsCategory = 3 --周边商品
		                                AND A.bIsDeleted = 0 --未删除
		                                AND A.bSeckill = 0 --非秒杀
		                                AND A.bSpecialSale = 0 --非特卖
		                                AND A.bShelves = 1 --已上架
	                                ) G
                                WHERE
	                                1 = 1 ");

            var name = Helper.CommonHelper.GetDictionaryValue("name", param, typeof(string));//搜索框条件
            var sCounty = Helper.CommonHelper.GetDictionaryValue("sCounty", param, typeof(string));//区
            var sCity = Helper.CommonHelper.GetDictionaryValue("sCity", param, typeof(string));//市
            var iPriceStart = Helper.CommonHelper.GetDictionaryValue("iPriceStart", param, typeof(string));//价格筛选
            var iPriceEnd = Helper.CommonHelper.GetDictionaryValue("iPriceEnd", param, typeof(string));//价格筛选
            var orderType = Helper.CommonHelper.GetDictionaryValue("orderType", param, typeof(string));//排序类型（desc asc）
            var longitude = Helper.CommonHelper.GetDictionaryValue("longitude", param, typeof(string));//精度
            var latitude = Helper.CommonHelper.GetDictionaryValue("latitude", param, typeof(string));//纬度

            //搜索框
            if (!string.IsNullOrEmpty(name.ToString())) builder.Append(string.Format(" AND sGoodsName LIKE '%{0}%' ", name.ToString()));

            //地区选择 sCounty
            if (!string.IsNullOrEmpty(sCounty.ToString()) && sCounty.ToString() != "-1") builder.Append(string.Format(" AND sCounty = '{0}' ", sCounty.ToString()));

            //城市 sCity
            if (!string.IsNullOrEmpty(sCity.ToString())) builder.Append(string.Format(" AND sCity = '{0}' ", sCity.ToString()));

            //开始价格
            if (!string.IsNullOrEmpty(iPriceStart.ToString()) && iPriceStart.ToString() != "-1") builder.Append(string.Format(" AND dGoodsPrice>={0} ", iPriceStart.ToString()));
            if (!string.IsNullOrEmpty(iPriceEnd.ToString()) && iPriceStart.ToString() != "2000" && iPriceEnd.ToString() != "-1") builder.Append(string.Format(" AND dGoodsPrice<={0} ", iPriceEnd.ToString()));

            //执行语句Sql
            string doSql = builder.ToString();

            //排序字段
            if (string.IsNullOrEmpty(orderType.ToString()))
            {
                //(默认)距离优先(调用存储过程)
                orderType = "0";
            }

            switch (orderType.ToString())
            {
                case "0"://距离优先
                    if (!string.IsNullOrEmpty(longitude.ToString()) && !string.IsNullOrEmpty(latitude.ToString()))
                    {

                        // 计算最短距离的商店
                        doSql = string.Format(doSql,
                            string.Format(@"dbo.Func_GetDistance ({0}, {1}, {2}, {3}) iDistance,--店铺距离当前定位的距离", longitude, latitude, "C.sLONG", "C.sLat"));

                        //排序规则
                        pageInfo.OrderBy = "iDistance";
                        pageInfo.orderType = OrderType.ASC;
                    }
                    else
                    {
                        //去除初始占位符
                        doSql = string.Format(doSql, "");

                    }

                    break;
                case "1"://好评优先

                    //去除初始占位符
                    doSql = string.Format(doSql, "");

                    pageInfo.OrderBy = "iCommentScore";
                    pageInfo.orderType = OrderType.DESC;
                    break;
                case "2"://价格从高到底

                    //去除初始占位符
                    doSql = string.Format(doSql, "");

                    pageInfo.OrderBy = "dGoodsPrice";
                    pageInfo.orderType = OrderType.DESC;
                    break;
                case "3"://价格从低到高

                    //去除初始占位符
                    doSql = string.Format(doSql, "");

                    pageInfo.OrderBy = "dGoodsPrice";
                    pageInfo.orderType = OrderType.ASC;
                    break;
            }

            return query.PaginationQuery<Dictionary<string, object>>(doSql, pageInfo, null);
        }

        /// <summary>
        /// 根据当前定位获取该城市下面的所有区
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override IList<Dictionary<string, object>> QueryCountryByCity(Dictionary<string, object> param)
        {
            var sCity = Helper.CommonHelper.GetDictionaryValue("sCity", param, typeof(string));
            var result = query.QueryList<Dictionary<string, object>>(@"SELECT DISTINCT
	                                                                        (sCounty)
                                                                        FROM
	                                                                        EHECD_ShopSet
                                                                        WHERE
	                                                                        sCity = @sCity
                                                                        AND sCounty IS NOT NULL
                                                                        AND bIsDelete = 0", new { sCity = sCity });
            return result;
        }

        /// <summary>
        /// 获取所有房型
        /// </summary>
        /// <returns></returns>
        public override IList<Dictionary<string, object>> QueryRoomTypeList()
        {
            return query.QueryList<Dictionary<string, object>>(@"SELECT * FROM EHECD_GuestRoomType WHERE bIsDeleted=0", null);
        }

        /// <summary>
        /// 获取展示Banner详情
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override EHECD_ImagesDTO QueryShowBannerDetail(Dictionary<string, object> dic)
        {
            var bannerModel = query.SingleQuery<EHECD_ImagesDTO>(@"SELECT
	                                                                            ID,
	                                                                            sTitle,
	                                                                            dPublishTime,
	                                                                            sContent
                                                                            FROM
	                                                                            EHECD_Images
                                                                            WHERE
	                                                                            bIsDeleted = 0
                                                                            AND bDisplay = 1
                                                                            AND iState = 1
                                                                            AND ID=@ID ", new { ID = dic["ID"] });
            return bannerModel;
        }

        /// <summary>
        /// 根据搜索框的输入实时查询商品列表
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override IList<Dictionary<string, object>> QueryGoodsByWhere(Dictionary<string, object> dic)
        {
            //商品名称
            var sGoodsName = Helper.CommonHelper.GetDictionaryValue("sGoodsName", dic, typeof(string));

            //当前定位城市
            var sCity = Helper.CommonHelper.GetDictionaryValue("sCity", dic, typeof(string));

            //查询结果
            var resultTemp = query.QueryList<Dictionary<string, object>>(@"SELECT
	                                                                            A.ID,
	                                                                            A.sGoodsName,
	                                                                            --商品名称
	                                                                            A.sGoodsCategory,
	                                                                            --商品类型 1：民宿  2：票务  3：周边
	                                                                            B.sShopName,
	                                                                            --所属店铺
	                                                                            B.sCity --所在城市
                                                                            FROM
	                                                                            EHECD_Goods A,
	                                                                            EHECD_ShopSet B
                                                                            WHERE
	                                                                            A.sStoreId = B.sShopID AND A.sGoodsName LIKE @sGoodsName
                                                                            ORDER BY
	                                                                            B.sCity --店铺所在城市分组", new { sGoodsName = "%"+sGoodsName+"%" });

            if (resultTemp != null && resultTemp.Count() > 0) {

                //根据当前定位所在城市的筛选结果，商品类型分类(当前城市的数据排列在最前面)
                var resultByCity = resultTemp.Where(a => a["sCity"].ToString().Trim() == sCity.ToString());

                //排除当前定位所在城市的结果，按照商品类型分类
                var resultByCategroy = resultTemp.Select(a => a).Except(resultByCity).GroupBy(a => a["sGoodsCategory"].ToString());

                //组合结果
                var compontResult = resultByCity.GroupBy(a => a["sGoodsCategory"].ToString()).Union(resultByCategroy).SelectMany(group => group).ToList();

                //返回结果
                return compontResult;

            }
            else
            {

                return new List<Dictionary<string, object>>();
            }
        }

        public override EHECD_ClientDTO IsShareClient(string ClientID)
        {
            var type = query.SingleQuery<EHECD_ClientDTO>("SELECT iClientType,ID FROM EHECD_Client WHERE bIsDeleted=0 AND iState=1 AND sOpenId=@sOpenId", new { sOpenId = ClientID });
           return type;
        }
    }
}
