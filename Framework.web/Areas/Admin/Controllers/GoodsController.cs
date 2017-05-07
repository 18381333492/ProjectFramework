using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.Helper;
using Newtonsoft.Json.Linq;


namespace Framework.web.Areas.Admin.Controllers
{
    public class GoodsController : SuperController
    {
        // GET: Admin/GoodsController

        private IGoodsManager domin;

        public GoodsController()
        {
            //商品业务对象
            domin = DI.DIEntity.GetInstance().GetImpl<IGoodsManager>();
        }


        #region 商品视图
        public ActionResult Index()
        {   //用户类型 0：平台用户，1：店铺，2：合伙人
            ViewBag.tUserType = SessionUser.User.tUserType.ToInt32();
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit(Guid ID)
        {
            List<EHECD_FullHouseTimeDTO> list;
            var goods = domin.GetGoods(ID,out list);
            ViewBag.List = list;
            return View(goods);
        }

        /// <summary>
        /// 商品详情查看
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Detail(Guid ID)
        {
            List<EHECD_FullHouseTimeDTO> list;
            var goods = domin.GetGoods(ID, out list);
            ViewBag.List = list;
            return View(goods);
        }

        //秒杀视图
        public ActionResult SeckillView(string sGoodsName, string dGoodsPrice,int sGoodsCategory)
        {
            ViewBag.sGoodsName = sGoodsName;
            ViewBag.dGoodsPrice = dGoodsPrice;
            ViewBag.sGoodsCategory = sGoodsCategory;
            return View();
        }
        //特价视图
        public ActionResult SpecialSaleView(string sGoodsName, string dGoodsPrice, int sGoodsCategory)
        {
            ViewBag.sGoodsName = sGoodsName;
            ViewBag.dGoodsPrice = dGoodsPrice;
            ViewBag.sGoodsCategory = sGoodsCategory;
            return View();
        }

        //统一分销佣金视图
        public ActionResult SetAllMoneyView()
        {
            return View();
        }

        //设置商品的时间段的价格视图
        public ActionResult SetPricesView(string dGoodsFisrtPrice, string dGoodsSecPrice,string dGoodsThirdPrice)
        {
            ViewBag.dGoodsFisrtPrice = dGoodsFisrtPrice;
            ViewBag.dGoodsSecPrice = dGoodsSecPrice;
            ViewBag.dGoodsThirdPrice = dGoodsThirdPrice;
            return View();
        }

        #endregion


        /// <summary>
        /// 分页获取商品列表
        /// </summary>
        public void List()
        {
            PageInfo info = LoadParam<PageInfo>();
            info.orderType = OrderType.DESC;
            info.OrderBy = "ID";
            var param = LoadParam<Dictionary<string, object>>();
            param.Add("sStoreId", SessionUser.User.ID);//所属店铺
            result.Data = domin.GetList(info, param);
            result.Succeeded = true;
        }


        /// <summary>
        /// 添加商品
        /// </summary>
        public void Insert()
        {
            EHECD_GoodsDTO goods = LoadParam<EHECD_GoodsDTO>();
            goods.sStoreId = SessionUser.User.ID.ToString();

            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();

            List<EHECD_FullHouseTimeDTO> List =dic.ContainsKey("time") ?
                JSONHelper.GetModel<List<EHECD_FullHouseTimeDTO>>(dic["time"].ToString()) :
                new List<EHECD_FullHouseTimeDTO>();
            var res = domin.Insert(goods, List);
            if (res > 0)
            {
                result.Succeeded = true;
                result.Msg = "商品添加成功!";
            }
            else
            {
                if (res == -10)
                {//佣金设置超出限制
                    string number= domin.GetRate().ToString();
                    result.Msg = "佣金设置超过平台佣金设置的最高限制(商品金额"+number+"%)";
                }
                else
                {
                    result.Msg = "商品添加失败!";
                }
            }
        }


        /// <summary>
        /// 编辑商品
        /// </summary>
        public void Update()
        {
            EHECD_GoodsDTO goods = LoadParam<EHECD_GoodsDTO>();
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();

            List<EHECD_FullHouseTimeDTO> List = dic.ContainsKey("time") ?
                JSONHelper.GetModel<List<EHECD_FullHouseTimeDTO>>(dic["time"].ToString()) :
                new List<EHECD_FullHouseTimeDTO>();
            var res = domin.Update(goods, List);
            if (res > 0)
            {
                result.Succeeded = true;
                result.Msg = "商品编辑成功!";
            }
            else
            {
                if (res == -10)
                {//佣金设置超出限制
                    string number = domin.GetRate().ToString();
                    result.Msg = "佣金设置超过平台佣金设置的最高限制(商品金额" + number + "%)";
                }
                else
                {
                    result.Msg = "商品编辑失败!";
                }
            }
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        public void Cancel()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var res = domin.Delete(dic);
            if (res > 0)
            {
                result.Succeeded = true;
            }
            else result.Msg = "删除商品失败!";
        }

        /// <summary>
        /// 商品的上下架
        /// </summary>
        public void Shelves()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();

            var ID = new Guid(dic["ID"].ToString());
            int type = dic["type"].ToInt32();
            var res = domin.bShelves(ID, type, SessionUser.User);
            if (res > 0)
            {
                result.Succeeded = true;
            }
            else
            {
                if (res == -10)
                {
                    result.Msg = "该商品已被平台下架,不能上架!";
                }
                else
                {
                    result.Msg = "操作失败!";
                }

            }
        }

        /// <summary>
        /// 取消/设置秒杀活动
        /// </summary>
        public void SetOrCancelSeckill()
        {
            var Data = LoadParam<Dictionary<string,object>>();
            Guid sGoodId = new Guid(Data["sGoodsId"].ToString());
            int type = Data["type"].ToInt32();// 1 - 设置秒杀,2 - 取消秒杀
            if (type == 1)
            {//设置秒杀活动
                string sActivityUseTime = string.Empty;
                string sSeckillTime = Data["sSeckillTime"].ToString();
                decimal dSeckillPrices = Data["dSeckillPrices"].ToDecimal();
                if(Data.ContainsKey("sActivityUseTime") &&Data["sActivityUseTime"]!=null)
                sActivityUseTime = Data["sActivityUseTime"].ToString();
                int res=domin.SetOrCancelSeckill(sGoodId, type, sSeckillTime, dSeckillPrices, sActivityUseTime);
                if (res > 0)
                {
                    result.Succeeded = true;
                }
                else result.Msg = "操作失败!";
            }
            else
            {//取消秒杀活动
                int res=domin.SetOrCancelSeckill(sGoodId, type);
                if (res > 0)
                {
                    result.Succeeded = true;
                }
                else result.Msg = "操作失败!";
            }
        }


        /// <summary>
        /// 取消/设置特价活动
        /// </summary>
        public void SetOrCancelSpecialSale()
        {
            var Data = LoadParam<Dictionary<string, object>>();
            Guid sGoodId = new Guid(Data["sGoodsId"].ToString());
            int type = Data["type"].ToInt32();// 1 - 设置特价,2 - 取消特价
            if (type == 1)
            {//设置特价活动
                string sActivityUseTime = string.Empty;
                string sSpecialSaleTime = Data["sSpecialSaleTime"].ToString();
                decimal dSpecialSalePrices = Data["dSpecialSalePrices"].ToDecimal();
                if (Data.ContainsKey("sActivityUseTime")&&Data["sActivityUseTime"] != null)
                     sActivityUseTime = Data["sActivityUseTime"].ToString();
                int res = domin.SetOrCancelSpecialSale(sGoodId, type, sSpecialSaleTime, dSpecialSalePrices, sActivityUseTime);
                if (res > 0)
                {
                    result.Succeeded = true;
                }
                else result.Msg = "操作失败!";
            }
            else
            {//取消特价活动
                int res = domin.SetOrCancelSpecialSale(sGoodId, type);
                if (res > 0)
                {
                    result.Succeeded = true;
                }
                else result.Msg = "操作失败!";
            }
        }


        /// <summary>
        /// 统一设置佣金
        /// </summary>
        public void SetAllMoney()
        {
            var Data = LoadParam<Dictionary<string, object>>();
            var sStoreId =SessionUser.User.ID;
            int iCommissionType = Data["iCommissionType"].ToInt32();
            decimal dMoney = Data["dMoney"].ToDecimal();
            int res =domin.SetAllMoney(sStoreId, iCommissionType, dMoney);
            if (res > 0)
            {
                result.Succeeded = true;
            }
            else
            {
                if (res == -10)
                {//佣金设置超出限制
                    string number = domin.GetRate().ToString();
                    result.Msg = "有商品的佣金设置超过平台佣金设置的最高限制(商品金额" + number + " %)";
                }
                else
                {
                    result.Msg = "操作失败!";
                }
            }
        }


        /// <summary>
        /// 商品价格管理
        /// </summary>
        public void SetPrices()
        {
            var Data = LoadParam<Dictionary<string, object>>();
            var sGoodsId =new Guid(Data["sGoodsId"].ToString());
            var sTimePrices = Data["sTimePrices"].ToString();
            int res = domin.SetPrices(sGoodsId, sTimePrices);
            if (res > 0)
            {
                result.Succeeded = true;
            }
            else result.Msg = "设置失败!";
        }



        /// <summary>
        /// 获取所有房型数据
        /// </summary>
        public void GetHouseSize()
        {
           var  interfaces = DI.DIEntity.GetInstance().GetImpl<IGuestRoomTypeManager>();

           var list=interfaces.GetAllUsedRoomType();
            if (list != null)
            {
                result.Succeeded = true;
                result.Data = JSONHelper.GetJsonString(list);
            }
        }


        /// <summary>
        /// 获取树型时间数据列表
        /// </summary>
        public void GetTime()
        {
            var nowDate = DateTime.Now; 

            JArray Mian = new JArray();
            for(int i = 1; i <= 12; i++)
            {
                JObject job = new JObject();
                job.Add(new JProperty("ID", Guid.NewGuid().ToString()));
                job.Add(new JProperty("Date", i.ToString()+"月"));
                var days = DateTime.DaysInMonth(nowDate.Year,i);
                JArray Child = new JArray();
                for(var j=1;j<= days; j++)
                {
                    JObject Mon = new JObject();
                    Mon.Add(new JProperty("ID", Guid.NewGuid().ToString()));
                    Mon.Add(new JProperty("Date", j.ToString()+"号"));
                    Mon.Add(new JProperty("first_prices", 1));
                    Mon.Add(new JProperty("sec_prices", 2));
                    Mon.Add(new JProperty("thrid_prices", 3));
                    Mon.Add(new JProperty("Month", i.ToString() + "月"));
                    Child.Add(Mon);
                }
                job.Add(new JProperty("children", Child));
                Mian.Add(job);
            }

            result.Succeeded = true;
            result.Data = Mian.ToString();
        }

        /// <summary>
        /// 根据商品ID获取该商品的选择的时间段的价格
        /// </summary>
        public void GetSelectTimePricesByGoodsId()
        {
            var Data = LoadParam<Dictionary<string, object>>();
            var sGoodsId = new Guid(Data["sGoodsId"].ToString());
            result.Data=domin.GetSelectTimePricesByGoodsId(sGoodsId);
            result.Succeeded = true;
        }
    }
}