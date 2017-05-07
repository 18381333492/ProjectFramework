using Framework.web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.BLL;
using Framework.DTO;
using Framework.Helper;

namespace Framework.web.Areas.Admin.Controllers
{
    /// <summary>
    /// 基本设置
    /// author 王其
    /// </summary>
    public class BaseSettingController : SuperController
    {
        #region 视图
        /// <summary>
        /// 展示
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            IBaseSettingManager manager = LoadInterface<IBaseSettingManager>();
            //商城设置
            EHECD_BaseSettingDTO baseSettingDto = manager.GetBaseSetting();
            if(baseSettingDto!=null)baseSettingDto.iUserMoney = baseSettingDto.iUserMoney.ToInt32();
            //ViewBag.BaseSettingDto = baseSettingDto;
            //客服
            IList<EHECD_CustomServiceDTO> customerList = manager.GetCustomers();
            ViewBag.CustomerList = customerList;
            return View(baseSettingDto);
        }
        #endregion

        #region 操作
        /// <summary>
        /// 编辑
        /// </summary>
        public void DoEdit()
        {
            var data = LoadParam<Dictionary<string, object>>();
            IBaseSettingManager manager = LoadInterface<IBaseSettingManager>();
            bool flag=manager.DoEdit(data)>0;
            result.Data = "";
            result.Msg = flag ? "编辑失败" : "编辑成功";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 获取轮播图
        /// </summary>
        public void GetImages()
        {
            IBaseSettingManager manager = LoadInterface<IBaseSettingManager>();
            IList<EHECD_ImagesDTO> imageList = manager.GetImageList();
            bool flag = imageList != null;
            result.Data = imageList;
            result.Msg = flag ? "获取失败" : "获取成功";
            result.Succeeded = flag;
        }

        #endregion
    }

}