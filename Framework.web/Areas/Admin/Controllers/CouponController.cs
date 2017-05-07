using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.DTO;
using Framework.Validate;
using Framework.Helper;
using Framework.BLL;
using Framework.Dapper;

namespace Framework.web.Areas.Admin.Controllers
{
    /// <summary>
    /// 优惠劵管理  
    /// Author 王其
    /// </summary>
    public class CouponController : SuperController
    {
        #region 视图
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            string id = Request.QueryString["ID"];
            ICouponManager manager=LoadInterface<ICouponManager>();
            EHECD_CouponDTO modal = manager.GetSingle(new Guid(id));
            modal.dValidDateEnd =DateTime.Parse(string.Format("{0:d}", modal.dValidDateEnd) + " 23:59:59");
            return View(modal);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            string id = Request.QueryString["ID"];
            ICouponManager manager = LoadInterface<ICouponManager>();
            EHECD_CouponDTO modal = manager.GetSingle(new Guid(id));
            modal.dValidDateEnd= DateTime.Parse(string.Format("{0:d}", modal.dValidDateEnd) + " 23:59:59");
            return View(modal);
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
            EHECD_SystemUserDTO user = SessionUser.User;
            byte? type = user.tUserType;
            ICouponManager manager = LoadInterface<ICouponManager>();
            PagingRet<Dictionary<string, object>> resultList = null;
            if (type == 0)
            {
                //平台用户
                resultList = manager.GetList(pageInfo, null,RequestParameters.dynamicData.data);
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
            if(resultList==null)
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
        /// 添加操作
        /// </summary>
        public void DoAdd()
        {
            var data = LoadParam<EHECD_CouponDTO>();
            //判断当前登录的用户类型
            EHECD_SystemUserDTO user = (GetSessionInfo(SessionInfo.USER_SESSION_NAME) as SessionInfo).SessionUser.User;
            byte? type = user.tUserType;
            if (type == 0)
            {
                //平台用户
                data.sStoreID = null;
            }
            else if (type == 1)
            {
                //店铺用户
                data.sStoreID = user.ID;
            }
            else if (type == 2)
            {
                //合伙人用户（合伙人暂时没有优惠劵功能）
                result.Data = "";
                result.Msg = "该类型用户没有此功能";
                result.Succeeded = false;
                return;
            }
            else {
                //没有这种类型
                result.Data = "";
                result.Msg = "数据库中没有这种类型";
                result.Succeeded = false;
                return;
            }
            //处理业务，添加数据
            ICouponManager manager=LoadInterface<ICouponManager>();
            data.ID = GuidHelper.GetSecuentialGuid();
            data.bIsDeleted = false;
            bool flag=manager.DoAdd(data)>0;
            result.Data = "";
            result.Succeeded = flag;
            result.Msg = flag ? "操作成功" : "操作失败";
        }

        /// <summary>
        /// 编辑操作
        /// </summary>
        public void DoEdit()
        {
            //获取ID 和 投放数量
            var data = LoadParam<EHECD_CouponDTO>();
            ICouponManager manager = LoadInterface<ICouponManager>();
            bool flag=manager.DoEdit(data)>0;
            result.Data = "";
            result.Msg = flag ? "操作成功" : "操作失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            var data = LoadParam<Dictionary<string,object>>();
            //获取要删除的ID集合
            if (!data.ContainsKey("rowIDs")) {
                result.Data = "";
                result.Msg = "键值错误";
                result.Succeeded = false;
                return;
            }
            string ids = data["rowIDs"].ToString();
            ICouponManager manager = LoadInterface<ICouponManager>();
            bool flag = manager.DoDelete(ids)>0;
            result.Data = "";
            result.Msg = flag ? "删除成功" : "删除失败";
            result.Succeeded = flag;
        }
        #endregion
    }
}