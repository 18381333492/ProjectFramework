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
    /// 评论管理
    /// Author 王其
    /// </summary>
    public class CommentController : SuperController
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
        /// 编辑视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            var id = Request.QueryString["ID"];
            ICommentManager manager = LoadInterface<ICommentManager>();
            EHECD_CommentDTO dto = manager.GetSingle(new Guid(id));
            ////获取对应的图片数据
            //if (dto != null) {
            //    ViewBag.ImageList=manager.GetImageList(dto.ID); 
            //}
            return View(dto);
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
            ICommentManager manager = LoadInterface<ICommentManager>();
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
        /// 编辑
        /// </summary>
        public void DoEdit()
        {
            var data = LoadParam<EHECD_CommentDTO>();
            ICommentManager manager = LoadInterface<ICommentManager>();
            data.bIsReplay = true;//设置为已回复状态
            bool flag=manager.DoEdit(data)>0;
            result.Data = "";
            result.Msg = flag ? "回复成功" : "回复失败";
            result.Succeeded = flag;
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
            ICommentManager manager = LoadInterface<ICommentManager>();
            bool flag = manager.DoDelete(ids) > 0;
            result.Data = "";
            result.Msg = flag ? "删除成功" : "删除失败";
            result.Succeeded = flag;
        }

        #endregion
    }
}