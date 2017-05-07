using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.Validate;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    /// <summary>
    /// 游记
    /// author 王其
    /// </summary>
    public class TravelNoteController : SuperController
    {
        #region 视图

        /// <summary>
        /// 列表视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 详细
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            var id = Request.QueryString["ID"];
            ITravelNoteManager manager = LoadInterface<ITravelNoteManager>();
            EHECD_TravelsNotesDTO dto = manager.GetSingle(new Guid(id));
            //获取对应的图片数据
            if (dto != null)
            {
                ViewBag.ImageList = manager.GetImageList(dto.ID);
            }
            return View(dto);
        }

        /// <summary>
        /// 编辑排序【店铺用户】
        /// </summary>
        /// <returns></returns>
        public ActionResult EditOrder() {
            var id = Request.QueryString["ID"];
            ITravelNoteManager manager = LoadInterface<ITravelNoteManager>();
            EHECD_TravelsNotesDTO dto = manager.GetSingle(new Guid(id));
            return View(dto);
        }

        /// <summary>
        /// 添加视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(Guid ID)
        {
            ITravelNoteManager manager = LoadInterface<ITravelNoteManager>();
            List<EHECD_ImagesDTO> list = manager.GetImageList(ID).ToList();
            string imgSpath = string.Empty;
            foreach(var m in list)
            {
                imgSpath = imgSpath +m.sImagePath+ ",";
            }
            ViewBag.Img = imgSpath.TrimEnd(',');
            return View(manager.GetSingle(ID));
        }
        /// <summary>
        /// 将游记布置到
        /// </summary>
        /// <returns></returns>
        public ActionResult Setting()
        {
            return View();
        }

        #endregion

        #region 操作
        /// <summary>
        /// 查询数据
        /// </summary>
        public void LoadData()
        {
            //获取分页数据
            var pageInfo = LoadParam<Dapper.PageInfo>();
            //判断当前的用户类型，加载对应的数据
            EHECD_SystemUserDTO user = (GetSessionInfo(SessionInfo.USER_SESSION_NAME) as SessionInfo).SessionUser.User;
            byte? type = user.tUserType;
            ITravelNoteManager manager = LoadInterface<ITravelNoteManager>();
            PagingRet<Dictionary<string, object>> resultList = null;
            if (type == 0)
            {
                //平台用户
                resultList = manager.GetList(pageInfo, null, RequestParameters.dynamicData.data);
            }
            else if (type == 1)
            {
                //店铺用户
                resultList = manager.GetList(pageInfo, user.ID, RequestParameters.dynamicData.data);
            }
            else if (type == 2)
            {
                //合伙人用户（合伙人暂时没有优惠劵功能）
                result.Data = "";
                result.Msg = "该类型用户没有此功能";
                result.Succeeded = false;
                return;
            }
            else
            {
                //没有这种类型
                result.Data = "";
                result.Msg = "数据库中没有这种类型";
                result.Succeeded = false;
                return;
            }

            //返回数据
            if (resultList == null)
            {
                result.Data = "";
                result.Msg = "未获取到数据";
                result.Succeeded = false;
                return;
            }

            result.Data = resultList;
            result.Msg = "获取成功";
            result.Succeeded = true;

        }

        /// <summary>
        /// 获取所有店铺
        /// </summary>
        public void GetShops()
        {
            var data = LoadParam<Dictionary<string, object>>();
            string shopName = data["keyword"].ToString();
            string id = data["sStoreID"].ToString();
            ITravelNoteManager manager = LoadInterface<ITravelNoteManager>();
            IList<EHECD_ShopSetDTO> shops = manager.GetShopList(shopName,new Guid(id));
            bool flag = shops != null && shops.Count() >= 0;
            result.Data = shops;
            result.Succeeded = flag;
            result.Msg = flag ? "获取成功" : "获取失败";
        }

        /// <summary>
        /// 添加操作
        /// </summary>
        public void DoAdd()
        {
            var data = LoadParam<Dictionary<string,object>>();
            //判断当前登录的用户类型
            EHECD_SystemUserDTO user = (GetSessionInfo(SessionInfo.USER_SESSION_NAME) as SessionInfo).SessionUser.User;
            byte? type = user.tUserType;
            if (type == 0)
            {
                //平台用户
                data["sStoreID"] = null;
            }
            else if (type == 1)
            {
                //店铺用户
                data["sStoreID"] = user.ID;
            }
            else if (type == 2)
            {
                //合伙人用户（合伙人暂时没有优惠劵功能）
                result.Data = "";
                result.Msg = "该类型用户没有此功能";
                result.Succeeded = false;
                return;
            }
            else
            {
                //没有这种类型
                result.Data = "";
                result.Msg = "数据库中没有这种类型";
                result.Succeeded = false;
                return;
            }
            ITravelNoteManager manager = LoadInterface<ITravelNoteManager>();
            bool flag=manager.DoAdd(data)>0;
            result.Data = "";
            result.Msg = flag ? "添加成功" : "添加失败";
            result.Succeeded = flag;
        }


        /// <summary>
        /// 编辑游记
        /// </summary>
        public void DoEdit()
        {
            var trave = LoadParam<EHECD_TravelsNotesDTO>();
            var traveImg = LoadParam<Dictionary<string, object>>()["sTravelImges"].ToString();
            ITravelNoteManager manager = LoadInterface<ITravelNoteManager>();
            int res = manager.DoEdit(trave, traveImg);
            if (res > 0)
            {
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "编辑失败!";
            }
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            var data = LoadParam<Dictionary<string, object>>();
            //获取要删除的ID集合
            if (!data.ContainsKey("rowIDs"))
            {
                result.Data = "";
                result.Msg = "键值错误";
                result.Succeeded = false;
                return;
            }
            string ids = data["rowIDs"].ToString();
            ITravelNoteManager manager = LoadInterface<ITravelNoteManager>();
            bool flag = manager.DoDelete(ids) > 0;
            result.Data = "";
            result.Msg = flag ? "删除成功" : "删除失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 编辑排序
        /// </summary>
        public void DoEditOrder()
        {
            var data = LoadParam<EHECD_TravelsNotesDTO>();
            ITravelNoteManager manager = LoadInterface<ITravelNoteManager>();
            bool flag=manager.DoEditOrder(data)>0;
            result.Data = "";
            result.Msg = flag ? "编辑失败" : "编辑成功";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 将游记也布置到
        /// </summary>
        public void DoSetting()
        {
            var data = LoadParam<List<Dictionary<string, object>>>();
            ITravelNoteManager manager = LoadInterface<ITravelNoteManager>();
            bool flag = manager.DoSetting(data) > 0;
            result.Data = "";
            result.Msg = flag ? "设置失败" : "设置成功";
            result.Succeeded = flag;
        }

        #endregion
    }
}