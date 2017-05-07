using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.Helper;


namespace Framework.web.Areas.Admin.Controllers
{
    /// <summary>
    /// 商品草稿箱控制器
    /// </summary>
    public class GoodsPreviewViewController : SuperController
    {
        // GET: Admin/GoodsHistoryController

        private IGoodsPreviewViewManager domin;

        public GoodsPreviewViewController()
        {
            //商品业务对象
            domin = DI.DIEntity.GetInstance().GetImpl<IGoodsPreviewViewManager>();
        }


        #region 商品草稿视图
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(Guid ID)
        {
            List<EHECD_FullHouseTimeDTO> list;
            var goods = domin.GetGoodsPreviewView(ID, out list);
            ViewBag.List = list;
            return View(goods);
        }


        /// <summary>
        /// 获取商品草稿的预览二维码视图
        /// </summary>
        /// <returns></returns>
        public ActionResult GoodsPreview()
        {
            return View();
        }


        #endregion


        /// <summary>
        /// 分页获取商品草稿列表
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
        /// 添加商品草稿
        /// </summary>
        public void Insert()
        {
            EHECD_GoodsPreviewViewDTO goods = LoadParam<EHECD_GoodsPreviewViewDTO>();
            goods.sStoreId = SessionUser.User.ID;

            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();

            List<EHECD_FullHouseTimeDTO> List = dic.ContainsKey("time") ?
                JSONHelper.GetModel<List<EHECD_FullHouseTimeDTO>>(dic["time"].ToString()) :
                new List<EHECD_FullHouseTimeDTO>();
            goods.ID = Helper.GuidHelper.GetSecuentialGuid();
            var res = domin.Insert(goods, List);
            if (res > 0)
            {
                result.Succeeded = true;
                result.Msg = "添加草稿箱成功!";
                result.Data = goods.ID;
            }
            else
            {
                if (res == -10)
                {//佣金设置超出限制
                    var goodsinter = LoadInterface<IGoodsManager>();
                    string number=goodsinter.GetRate().ToString();
                    result.Msg = "佣金设置超过平台佣金设置的最高限制(商品金额的"+ number + "%)";
                }
                else
                {
                    result.Msg = "商品草稿添加失败!";
                }
            }

        }

        /// <summary>
        /// 编辑商品草稿
        /// </summary>
        public void Update()
        {
            EHECD_GoodsPreviewViewDTO goods = LoadParam<EHECD_GoodsPreviewViewDTO>();
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();

            List<EHECD_FullHouseTimeDTO> List = dic.ContainsKey("time") ?
                JSONHelper.GetModel<List<EHECD_FullHouseTimeDTO>>(dic["time"].ToString()) :
                new List<EHECD_FullHouseTimeDTO>();
            var res = domin.Update(goods, List);
            if (res > 0)
            {
                result.Succeeded = true;
                result.Data = goods.ID;
            }
            else
            {
                if (res == -10)
                {//佣金设置超出限制
                    var goodsinter = LoadInterface<IGoodsManager>();
                    string number = goodsinter.GetRate().ToString();
                    result.Msg = "佣金设置超过平台佣金设置的最高限制(商品金额的" + number + "%)";
                }
                else
                {
                    result.Msg = "商品草稿编辑失败!";
                }
            }
                
        }

        /// <summary>
        /// 删除商品草稿
        /// </summary>
        public void Cancel()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var res = domin.Delete(dic);
            if (res > 0)
            {
                result.Succeeded = true;
            }
            else result.Msg = "删除商品草稿失败!";
        }


        /// <summary>
        /// 删除草稿箱
        /// </summary>
        public void DeleteAndIndert()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            Guid historyGoodsID = dic["ID"].ToGuid();
            var res = domin.DeleteAndIndert(historyGoodsID);
            if (res > 0)
            {
                result.Succeeded = true;
            }
            else
            {
                if (res == -10)
                {//佣金设置超出限制
                    var goodsinter = LoadInterface<IGoodsManager>();
                    string number = goodsinter.GetRate().ToString();
                    result.Msg = "佣金设置超过平台佣金设置的最高限制(商品金额的" + number + "%)";
                }
                else
                {
                    result.Msg = "操作失败!";
                }
            }
        }



    }
}