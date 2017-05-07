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
    /// 操作日志
    /// author 王其
    /// </summary>
    public class OperateLogController : SuperController
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
            IOperateLogManager manager = LoadInterface<IOperateLogManager>();
            PagingRet<Dictionary<string,object>> resultList = manager.GetList(pageInfo, RequestParameters.dynamicData.data);
            bool flag = resultList != null && resultList.MaxCount >=0;
            result.Data = resultList;
            result.Msg = flag? "获取成功": "获取失败";
            result.Succeeded = flag;
        }
        #endregion
    }
}