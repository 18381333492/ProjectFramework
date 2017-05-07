using Framework.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;
using Framework.DI;
using Framework.MapperConfig;
using Framework.Helper;
using System.Dynamic;

namespace Framework.BLL
{
    public partial class OrderConfirmManager : IOrderConfirmManager
    {
        /// <summary>
        /// 载入优惠券
        /// </summary>
        /// <param name="eHECD_ClientDTO"></param>
        /// <param name="dynamicData"></param>
        /// <returns></returns>
        public override IList<object> LoadCoupons(EHECD_ClientDTO eHECD_ClientDTO, dynamic dynamicData)
        {
            //string stime = dynamicData.data.stime,        // 选择的开始时间字符串
            //           etime = dynamicData.data.etime,    // 选择的结束时间字符串
                      string id = dynamicData.data.gid;        // 商品的id

            string s = DateTime.Now.ToString("yyyy-MM-dd"),// 开始时间
                     e = DateTime.Now.ToString("yyyy-MM-dd");// 结束时间


            // 1.获取商家的id
            var shopID = query.SingleQuery<EHECD_GoodsDTO>("SELECT sStoreId FROM EHECD_Goods WHERE ID = @ID", new { ID = id });

            // 2.获取商家的可用优惠券
            var coupons = query.QueryList<EHECD_CouponDTO>(
                "SELECT * FROM EHECD_Coupon WHERE (sStoreID = @shopID OR sStoreID IS NULL) AND dValidDateStart <= @S AND dValidDateEnd >= @E AND bIsDeleted = 0;",
                new
                {
                    shopID = Guid.Parse(shopID.sStoreId),
                    S = s,
                    E = e
                });

            if (coupons == null || coupons.Count == 0) return new List<object>();

            // 3.获取客户的可用商家优惠券
            var copuonDetails = query.QueryList<EHECD_CouponDetailsDTO>(string.Format("SELECT * FROM EHECD_CouponDetails WHERE sCouponID IN ({0}) AND bIsUsed=0 AND sUserID = @UID",
            string.Join(",", coupons.Select(m => "'" + m.ID + "'"))
            ), new { UID = eHECD_ClientDTO.ID });

            //创建返回的动态生成数据
            var ret = copuonDetails.Select(m =>
            {
                var now_c = coupons.Where(c => c.ID == m.sCouponID).FirstOrDefault();
                dynamic dyc = new ExpandoObject();
                dyc.ts = string.Format("{0}至{1}", now_c.dValidDateStart.ToDateTime().ToString("yyyy/MM/dd"), now_c.dValidDateEnd.ToDateTime().ToString("yyyy/MM/dd"));
                dyc.top = now_c.iUsePrice;
                dyc.p = now_c.iCoiCouponPrice;
                dyc.id = m.sCouponID;                
                dyc.uid = m.sUserID;
                return dyc;
            }).ToList();

            return ret;
        }

        /// <summary>
        /// 载入订单需要的数据
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public override room_con_json LoadOrderInfo(dynamic param)
        {

            int gtype = param.data.gtype;                   // 商品的类型：1是客房，2是票务

            var res = new room_con_json();// 返回结果

            DateTime nowtime = DateTime.Now;//当前时间
            if (param.data.nowtime != null)
                nowtime =DateTime.Parse(param.data.nowtime.Value);

            int year = nowtime.Year;
            int month = nowtime.Month;
            int days = DateTime.DaysInMonth(year, month);

            string id = param.data.gid;  // 商品的id



            DateTime s = DateTime.Parse(DateTime.Parse(string.Format("{0}/{1}/1", year, month)).ToString("yyyy-MM-dd ") + "00:00:00"),// 开始时间
                e = DateTime.Parse(DateTime.Parse(string.Format("{0}/{1}/{2}", year, month, days)).ToString("yyyy-MM-dd ") + "23:59:59");// 结束时间


            // 获取商品的信息
            var goodInfo = query.SingleQuery<EHECD_GoodsDTO>("SELECT * FROM EHECD_Goods WHERE ID = @ID;", new { ID = id });

            // 获取设置的房间时段价格
            var timePrice = query.SingleQuery<EHECD_GoodsTimePriceDTO>("SELECT * FROM EHECD_GoodsTimePrice WHERE sGoodsId = @ID;", new { ID = id });

            List<string> alltimePrice =timePrice!=null?timePrice.sFirstTime.Split(',').ToList():new List<string>();
            alltimePrice = alltimePrice.Where(m => m.Substring(0, month.ToString().Length) == month.ToString()).ToList();

            // 1.组装一个月的价格数据
            for (var i = 1; i <= days; i++)
            {
                var temp = alltimePrice.Where(m => m.Contains(string.Format("{0}-{1}:", month, i)));
                if (temp != null && temp.Count() == 1)
                {//设置的价格
                    int priceOrder = temp.ToList()[0].Split(':')[1].ToInt32();
                    switch (priceOrder)
                    {
                        case 1: res.normal_price.Add(goodInfo.dGoodsFisrtPrice.Value); break;
                        case 2: res.normal_price.Add(goodInfo.dGoodsSecPrice.Value); break;
                        case 3: res.normal_price.Add(goodInfo.dGoodsThirdPrice.Value); break;
                    }
                }
                else
                {//默认价格
                    res.normal_price.Add(goodInfo.dGoodsFisrtPrice.Value);
                }
            }

            // 2.组装特价数据
            if (goodInfo.bSeckill.Value || goodInfo.bSpecialSale.Value)
            {//特价商品或者秒杀商品
             // ["2016/9/10-350", "2016/11/21-119"]--组装的数据格式
                var activeTime = goodInfo.bSeckill.Value == true ? goodInfo.sSeckillTime : goodInfo.sSpecialSaleTime;
                DateTime start =DateTime.Parse(DateTime.Parse(activeTime.Split(',')[0]).ToString("yyyy-MM-dd")+" 00:00:00"),//活动的开始时间
                        end = DateTime.Parse(DateTime.Parse(activeTime.Split(',')[1]).ToString("yyyy-MM-dd") + " 23:59:59");//活动的结束时间

                var useTime = goodInfo.sActivityUseTime;//活动商品的使用时间

                DateTime useSta = DateTime.Parse(DateTime.Parse(useTime.Split(',')[0]).ToString("yyyy-MM-dd") + " 00:00:00"),//使用的开始时间
                   useEnd = DateTime.Parse(DateTime.Parse(useTime.Split(',')[1]).ToString("yyyy-MM-dd") + " 23:59:59");//使用的结束时间

                //遍历查找活动日期的价格
                for (var j=1;j<= days; j++)
                {
                    DateTime sta = DateTime.Parse((new DateTime(year, month, j).ToString("yyyy-MM-dd") + " 00:00:00")),
                       ending = DateTime.Parse((new DateTime(year, month, j).ToString("yyyy-MM-dd") + " 23:59:59"));

                    if(sta>= useSta&&ending<= useEnd)
                    {//当天在使用时间范围内
                        //在判断今天是不是在活动时间范围内
                        if(DateTime.Now>= start&& DateTime.Now<= end)
                        {//当天价格为活动价
                            if (goodInfo.bSeckill.Value)
                            {//秒杀价
                                res.special_offer.Add(string.Format("{0}/{1}/{2}-{3}", year, month, j, goodInfo.dSeckillPrices.Value));
                            }
                            else
                            {//特价
                                res.special_offer.Add(string.Format("{0}/{1}/{2}-{3}", year, month, j, goodInfo.dSpecialSalePrices.Value));
                            }
                        }
                    }
                }
            }
            //****************************//
            // 3.组装满房时间段
            if (gtype == 1)
            {//满房时间段 不能选择的时间
             //---- ["2016/11/22", "2016/11/23", "2016/11/24", "2016/11/29"] 数据格式

                // 1.获取客房满房时间段
                var fullTime = LoadAllFullHouseTime(id);

                // 2.获取订房详情
                var roomDetail = LoadRoomDetail(id, s, e, gtype);

                //遍历一个月时候有满房的时间
                for (int k = 1; k <= days; k++)
                {
                    DateTime sta = DateTime.Parse((new DateTime(year, month, k).ToString("yyyy-MM-dd") + " 00:00:00")),
                        end = DateTime.Parse((new DateTime(year, month, k).ToString("yyyy-MM-dd") + " 23:59:59"));

                    //查询当天所属于的时间段.
                    var funtimeDate = fullTime.Where(m => m.dStartTime <= sta && m.dEndTime >= end);
                    if (funtimeDate!=null&&funtimeDate.Count()>0)
                    {
                        // 获取当天的订房总数
                        var todayBookedRoomTotalAmount = roomDetail.Where(de => de.dStartTime <= sta && de.sEndTime >= end).Sum(de => de.iAmount);
                        if (todayBookedRoomTotalAmount + funtimeDate.First().iFullHouseCount >= goodInfo.iHouseCount)
                        {
                            res.no_choice.Add(sta.ToString("yyyy/MM/dd"));//添加满房的时间数据
                        }
                    }
                    else
                    {//
                        var todayBookedRoomTotalAmount = roomDetail.Where(de => de.dStartTime <= sta && de.sEndTime >= end).Sum(de => de.iAmount);
                        if (todayBookedRoomTotalAmount >= goodInfo.iHouseCount)
                        {
                            res.no_choice.Add(sta.ToString("yyyy/MM/dd"));
                        }
                    }
                }
            }
            if (gtype == 2)
            {//商品是票务
             //---- ["2016/11/22", "2016/11/23", "2016/11/24", "2016/11/29"] 数据格式

                // 1.获取票务时间段数量
                var fullTime = LoadAllFullHouseTime(id);

                // 3.获取票务预订详情
                var ticketDetail = LoadRoomDetail(id, s, e, gtype); //票务预订的时间都是只有一天

                for(var h = 1; h <= days; h++)
                {
                    DateTime sta = DateTime.Parse((new DateTime(year, month, h).ToString("yyyy-MM-dd") + " 00:00:00")),
                   end = DateTime.Parse((new DateTime(year, month, h).ToString("yyyy-MM-dd") + " 23:59:59"));

                    //查询当天所属于的票务时间段
                    var time = fullTime.Where(m => m.dStartTime <= sta && m.dEndTime >= end);
                    if (time != null&&time.Count()>0)
                    {
                        var ticketCount = time.ToList()[0].iFullHouseCount;//该票务时间段的票务数量
                        DateTime timeSta = time.ToList()[0].dStartTime.Value,//该票务时间段的开始时间
                            timeEnd = time.ToList()[0].dStartTime.Value;//该票务时间段的结束时间
                        var Count=ticketDetail.Where(m => m.dStartTime >= timeSta && m.sEndTime <= timeEnd).Sum(m=>m.iAmount);//已卖的数量
                        if(Count>= ticketCount)
                        {//已卖完
                            res.no_choice.Add(sta.ToString("yyyy/MM/dd"));
                        }
                    }
                    else
                    {//遗漏的时间
                        res.no_choice.Add(sta.ToString("yyyy/MM/dd"));
                    }
                }
            }
            return res;
        }

        
        /// <summary>
        /// 检查商品库存
        /// </summary>
        /// <param name="param"></param>
        public override bool checkCount(dynamic param)
        {
            string stime = param.data.stime,    // 选择的开始时间字符串
                   etime = param.data.etime,    // 选择的结束时间字符串
                     id = param.data.gid,       // 商品的id
                     gtype = param.data.gtype;  //商品分类 1-客房,2-票务
            int count = param.data.count;//数量

            DateTime s = DateTime.Parse(DateTime.Parse(stime).ToString("yyyy-MM-dd ") + "00:00:00"),// 开始时间
                e = DateTime.Parse(DateTime.Parse(etime).ToString("yyyy-MM-dd ") + "23:59:59");// 结束时间

            //获取相差的时间段
            TimeSpan timeCha = e.Subtract(s);

            // 获取商品的信息
            var goodInfo = query.SingleQuery<EHECD_GoodsDTO>("SELECT * FROM EHECD_Goods WHERE ID = @ID;", new { ID = id });

            // 1.获取客房满房时间段
            var fullTime = LoadAllFullHouseTime(id);

            // 2.获取订房详情
            var roomDetail = LoadRoomDetail(id, s, e, gtype.ToInt32());
            var res = false;
            if (gtype.ToInt32() == 1)
            {//商品为客房的时候

                //相差的天数
                var timespan = e.Subtract(s).Days;
                for (var i = 0; i <= timespan; i++)
                {
                    var RoomSta = s.AddDays(i);//住房当天的开始时间
                    var RoomEnd = DateTime.Parse(s.AddDays(i).ToString("yyyy-MM-dd") + " 23:59:59");//住房当天的结束时间
                                                                                                    //获取当天的订房数量
                    int GoodsCount = roomDetail.Where(de => RoomSta >= de.dStartTime && RoomEnd <= de.sEndTime).Sum(de => de.iAmount).Value;
                    if (count + GoodsCount <= goodInfo.iHouseCount)
                    {//数量满足
                     //还要判断当天是否是在满房时间段内
                        if (fullTime.Where(m => m.dStartTime <= RoomSta && m.dEndTime >= RoomEnd).Count() > 0)
                        {//当天满房
                            var funtimeDate = fullTime.Where(m => m.dStartTime <= RoomSta && m.dEndTime >= RoomEnd).First();
                            if (count + GoodsCount + funtimeDate.iFullHouseCount > goodInfo.iHouseCount)
                            {
                                res = false; break;
                            }
                            else
                            {
                                res = true;
                            }
                        }
                        else
                        {
                            res = true;
                        }
                    }
                    else
                    {
                        res = false; break;
                    }
                }
                return res;
            }
            else
            {//商品为票务

                if (timeCha.Days != 0)
                {//判断票务选择的时间是否是1天
                    return res;
                }
                else
                {
                    //查询当天所属于的票务时间段
                    var time = fullTime.Where(m => m.dStartTime <= s && m.dEndTime >= e);
                    if (time != null&& time.Count()>0)
                    {
                        var ticketCount = time.ToList()[0].iFullHouseCount;//该票务时间段的票务数量
                        DateTime timeSta = time.ToList()[0].dStartTime.Value,//该票务时间段的开始时间
                            timeEnd = time.ToList()[0].dEndTime.Value;//该票务时间段的结束时间
                        var Count = roomDetail.Where(m => m.dStartTime >= timeSta && m.sEndTime <= timeEnd).Sum(m => m.iAmount);//已卖的数量
                        if (Count + count > ticketCount)
                        {//已卖完
                            return res;
                        }
                        else
                        {
                            res = true;
                        }
                    }
                    else
                    {//遗漏的时间
                        return res;//当天没有票
                    }
                }
            }
            return res;
        }


        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name=""></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public override int MakeOrder(EHECD_OrdersDTO order, dynamic param)
        {

            string id = param.data.gid;//商品主键ID

            int count = param.data.count;//数量

            // 获取商品的信息
            var goodInfo = query.SingleQuery<EHECD_GoodsDTO>("SELECT * FROM EHECD_Goods WHERE ID = @ID;", new { ID = id });

            StringBuilder sSql = new StringBuilder();

            if (goodInfo.sGoodsCategory <= 2)
            {
                string stime = param.data.stime,    // 选择的开始时间字符串
                etime = param.data.etime;   // 选择的结束时间字符串

                DateTime s = DateTime.Parse(DateTime.Parse(stime).ToString("yyyy-MM-dd ") + "00:00:00"),// 开始时间
             e = DateTime.Parse(DateTime.Parse(etime).ToString("yyyy-MM-dd ") + "23:59:59");// 结束时间

                if (s< DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00"))
                {
                    return -3;//只能选中今天以后的日期
                }
                

                if (checkCount(param))
                {//检查库存

                    //查询该订单的商家
                    var buss = query.SingleQuery<EHECD_SystemUserDTO>(@"SELECT * FROM  EHECD_SystemUser WHERE ID=@ID", new { ID = goodInfo.sStoreId });

                    // 1.组织订单数据
                    order.ID = GuidHelper.GetSecuentialGuid();
                    order.dBookTime = DateTime.Now;
                    order.sStoreID = goodInfo.sStoreId.ToGuid();//订单说所属的店铺ID
                    order.sOrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");//订单编号
                    order.iState = 0;//待付款
                    order.iType = goodInfo.sGoodsCategory.Value - 1;//订单类别 0客房 1门票 2周边产品
                    order.sPartnerID = buss.sPartnerID;//该订单的合伙人ID

                    sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_OrdersDTO>(order));//添加订单的SQL语句

                    // 2.组织订单商品数据
                    var orderGoods = new EHECD_OrdersGoodsDTO();
                    orderGoods.ID = GuidHelper.GetSecuentialGuid();
                    orderGoods.iAmount = count;
                    if (goodInfo.iCommissionType == 1)
                    {//1--固定金额，2--商品价格比例
                        orderGoods.iCommission = goodInfo.dMoney * count;
                    }
                    else
                    {
                        // 2--商品价格比例
                        orderGoods.iCommission = order.iOriginalTotalPrice * goodInfo.dMoney * (0.01.ToDecimal());
                    }
                    orderGoods.iSinglePrice = order.iOriginalTotalPrice / count;//商品单价
                    orderGoods.iGoodsType = goodInfo.sGoodsCategory.Value - 1;//商品分类
                    orderGoods.iServicePrice =//平台服务费
                        (GetServerLevel().iServicePrecent.ToDecimal()) * (0.01.ToDecimal()) * order.iOriginalTotalPrice;
                    orderGoods.sGoodsName = goodInfo.sGoodsName;
                    orderGoods.sGoodsPrimaryKey = goodInfo.ID;
                    orderGoods.sOrderID = order.ID;
                    orderGoods.sOrderNo = order.sOrderNo;

                    sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_OrdersGoodsDTO>(orderGoods));//添加订单商品的SQL语句

                    //组装住房明细
                    var roomDetail = new EHECD_RoomDetailDTO();
                    roomDetail.ID = GuidHelper.GetSecuentialGuid();
                    roomDetail.iAmount = count;
                    roomDetail.sGoodsId = goodInfo.ID;
                    roomDetail.sOrderId = order.ID;
                    roomDetail.sStoreId = goodInfo.sStoreId.ToGuid();
                    roomDetail.dStartTime = DateTime.Parse(stime);
                    roomDetail.sEndTime = DateTime.Parse(etime);

                    sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_RoomDetailDTO>(roomDetail));//添加住房明细的SQL语句

                    //修改优惠卷的状态
                    if (order.sCouponID != null)
                    {//判断优惠卷是否为空
                        sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_CouponDetailsDTO>(new EHECD_CouponDetailsDTO()
                        {
                            bIsUsed = true,
                        }, string.Format("WHERE sCouponID='{0}'", order.sCouponID)));
                    }

                    return excute.ExcuteTransaction(sSql.ToString());

                }
                else
                {//库存不足
                    return goodInfo.sGoodsCategory.Value * -1;
                }
            }
            else
            {//商品为周边

                //查询该订单的商家
                var buss = query.SingleQuery<EHECD_SystemUserDTO>(@"SELECT * FROM  EHECD_SystemUser WHERE ID=@ID", new { ID = goodInfo.sStoreId });


                // 1.组织订单数据
                order.ID = GuidHelper.GetSecuentialGuid();
                order.dBookTime = DateTime.Now;
                order.sStoreID = goodInfo.sStoreId.ToGuid();//订单说所属的店铺ID
                order.sOrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");//订单编号
                order.iState = 0;//待付款
                order.iType = goodInfo.sGoodsCategory.Value - 1;//订单类别 0客房 1门票 2周边产品
                order.sPartnerID = buss.sPartnerID;//该订单的合伙人ID


                sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_OrdersDTO>(order));//添加订单的SQL语句

                // 2.组织订单商品数据
                var orderGoods = new EHECD_OrdersGoodsDTO();
                orderGoods.ID = GuidHelper.GetSecuentialGuid();
                orderGoods.iAmount = count;
                if (goodInfo.iCommissionType == 1)
                {//1--固定金额，2--商品价格比例
                    orderGoods.iCommission = goodInfo.dMoney * count;
                }
                else
                {
                    // 2--商品价格比例
                    orderGoods.iCommission = order.iOriginalTotalPrice * goodInfo.dMoney * (0.01.ToDecimal());
                }
                orderGoods.iSinglePrice = order.iOriginalTotalPrice / count;//商品单价
                orderGoods.iGoodsType = goodInfo.sGoodsCategory.Value - 1;//商品分类
                orderGoods.iServicePrice =//平台服务费
                     (GetServerLevel().iServicePrecent.ToDecimal()) * (0.01.ToDecimal()) * order.iOriginalTotalPrice;                                                         //orderGoods.iServicePrice//服务费
                orderGoods.sGoodsName = goodInfo.sGoodsName;
                orderGoods.sGoodsPrimaryKey = goodInfo.ID;
                orderGoods.sOrderID = order.ID;
                orderGoods.sOrderNo = order.sOrderNo;

                sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_OrdersGoodsDTO>(orderGoods));//添加订单商品的SQL语句

                //修改优惠卷的状态
                if (order.sCouponID != null)
                {//判断优惠卷是否为空
                    sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_CouponDetailsDTO>(new EHECD_CouponDetailsDTO()
                    {
                        bIsUsed = true,
                    }, string.Format("WHERE sCouponID='{0}'", order.sCouponID)));
                }
                return excute.ExcuteTransaction(sSql.ToString());
            }
        }



        /// <summary>
        /// 获取平台设置的服务费比例
        /// </summary>
        /// <returns></returns>
        private EHECD_BaseSettingDTO GetServerLevel()
        {
            return query.SingleQuery<EHECD_BaseSettingDTO>(@"SELECT * FROM EHECD_BaseSetting",null);
        }


        /// <summary>
        /// 根据商品ID获取所有的满房时间段
        /// </summary>
        private List<EHECD_FullHouseTimeDTO> LoadAllFullHouseTime(string id)
        {
         
            //  满房时间段
            var fullTime = query.QueryList<EHECD_FullHouseTimeDTO>(@"SELECT * FROM 
                                                                    EHECD_FullHouseTime WHERE sGoodsId = @ID",
                new
                {
                    ID = id,
                }).Select(m =>
                {
                    m.dStartTime = DateTime.Parse(m.dStartTime.ToDateTime().ToString("yyyy-MM-dd ") + "00:00:00");
                    m.dEndTime = DateTime.Parse(m.dEndTime.ToDateTime().ToString("yyyy-MM-dd ") + "23:59:59");
                    return m;
                }).ToList();
            return fullTime;
        }

        /// <summary>
        /// 根据客房ID、开始时间、结束时间获取满房时间段
        /// </summary>
        private List<EHECD_FullHouseTimeDTO> LoadFullHouseTime(string id, DateTime stime, DateTime etime)
        {
            DateTime s = stime,// 开始时间
                        e = etime;// 结束时间

            // 满房时间段
            var fullTime = query.QueryList<EHECD_FullHouseTimeDTO>(@"SELECT * FROM 
                                                                    EHECD_FullHouseTime WHERE sGoodsId = @ID 
                                                                    AND 
                                                                    ((dStartTime >= @dStartTime AND dEndTime <= @dEndTime)   --第一种情况包含关系
                                                                      OR (dStartTime<@dStartTime AND dEndTime>=@dStartTime)  --第二种情况跨月的开始时间情况
                                                                      OR (dStartTime<=@dEndTime AND dEndTime>@dEndTime)      --第三种情况跨月的结束时间
                                                                      OR (dStartTime<=@dStartTime AND dEndTime>=@dEndTime)   --第四种情况被包含关系
                                                                     );", 
                new
                {
                    ID = id,
                    dStartTime = s,
                    dEndTime = e
                }).Select(m =>
                {
                    m.dStartTime = DateTime.Parse(m.dStartTime.ToDateTime().ToString("yyyy-MM-dd ") + "00:00:00");
                    m.dEndTime = DateTime.Parse(m.dEndTime.ToDateTime().ToString("yyyy-MM-dd ") + "23:59:59");
                    return m;
                }).ToList();

            return fullTime;
        }

        /// <summary>
        /// 根据客房ID、开始时间、结束时间获取订房详情
        /// </summary>
        private List<EHECD_RoomDetailDTO> LoadRoomDetail(string id, DateTime stime, DateTime etime,int gtype)
        {
            DateTime s = stime,// 开始时间
                       e = etime;// 结束时间

            if (gtype == 1)
            {
                var roomDetail = query.QueryList<EHECD_RoomDetailDTO>(@"SELECT * FROM EHECD_RoomDetail 
                                                                             WHERE sGoodsId = @ID 
                                                                             AND 
                                                                      ((dStartTime >= @dStartTime AND sEndTime <= @dEndTime)   --第一种情况包含关系
                                                                      OR (dStartTime<@dStartTime AND sEndTime>=@dStartTime)  --第二种情况跨月的开始时间情况
                                                                      OR (dStartTime<=@dEndTime AND sEndTime>@dEndTime)      --第三种情况跨月的结束时间
                                                                      OR (dStartTime<=@dStartTime AND sEndTime>=@dEndTime)   --第四种情况被包含关系
                                                                     );",
                        new
                        {
                            ID = id,
                            dStartTime = s,
                            dEndTime = e
                        }).Select(m =>
                        {
                            m.dStartTime = DateTime.Parse(m.dStartTime.ToDateTime().ToString("yyyy-MM-dd ") + "00:00:00");
                            m.sEndTime = DateTime.Parse(m.sEndTime.ToDateTime().ToString("yyyy-MM-dd ") + "23:59:59");
                            return m;
                        }).ToList();

                return roomDetail;
            }
            else
            {//获取票务的时间段数量
                var roomDetail = query.QueryList<EHECD_RoomDetailDTO>(@"SELECT * FROM EHECD_RoomDetail 
                                                                             WHERE sGoodsId = @ID 
                                                                             AND dStartTime>=@dStartTime AND sEndTime<=@dEndTime",
                       new
                       {
                           ID = id,
                           dStartTime = s,
                           dEndTime = e
                       }).Select(m =>
                       {
                           m.dStartTime = DateTime.Parse(m.dStartTime.ToDateTime().ToString("yyyy-MM-dd ") + "00:00:00");
                           m.sEndTime = DateTime.Parse(m.sEndTime.ToDateTime().ToString("yyyy-MM-dd ") + "23:59:59");
                           return m;
                       }).ToList();

                return roomDetail;
            }
        }

        /// <summary>
        /// 解析不同时间的价格（设置的特价）
        /// </summary>
        /// <param name="sFirstTime"></param>
        /// <param name="goodInfo"></param>
        /// <returns></returns>
        private List<TimePrice> LoadTimePriceInfo(string sFirstTime, EHECD_GoodsDTO goodInfo)
        {
            // 解析不同时间的价格
            var timePriceStringArray = sFirstTime.Split(new char[] { ',' });
            var timePriceArray = new List<TimePrice>();

            foreach (var item in timePriceStringArray)
            {
                var time_price = item.Split(new char[] { ':' });
                var priceType = time_price[1].ToInt32();

                // 确定设定时间段的客房单价
                var price = priceType == 0 ?
                            goodInfo.dGoodsFisrtPrice :
                                priceType == 1 ?
                                goodInfo.dGoodsSecPrice :
                                goodInfo.dGoodsThirdPrice;

                var month_day = time_price[0].Split(new char[] { '-' });

                var month = month_day[0].ToInt32();
                var day = month_day[1].ToInt32();

                // 封装设定的特价价格
                timePriceArray.Add(new
                TimePrice
                {
                    Price = price.ToDecimal(),
                    Month = month,
                    Day = day
                });
            }
            return timePriceArray;
        }
    }

    /// <summary>
    /// 这个类用来封装设置的特价房间信息
    /// </summary>
    internal class TimePrice
    {
        /// <summary>
        /// 当日价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 设置的月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 设置的日期
        /// </summary>
        public int Day { get; set; }
    }
    

    /// <summary>
    /// 数据结果集
    /// </summary>
    public class room_con_json
    {
        /// <summary>
        /// 房价价格数据
        /// </summary>
        public List<decimal> normal_price=new List<decimal>();

        /// <summary>
        /// 特殊价格数据
        /// </summary>
        public List<string> special_offer = new List<string>();

        /// <summary>
        /// 满房数据
        /// </summary>
        public List<string> no_choice = new List<string>();
    }

}
