using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
   public abstract class IPasswordManager:BaseBll
    {
        /// <summary>
        /// 绑定扫描员页面数据
        /// </summary>
        /// <param name="info"></param>
        /// <param name="user"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_ScannerDTO> GetPageList(PageInfo info, EHECD_SystemUserDTO user, Dictionary<string, object> dic);
        /// <summary>
        /// 添加扫描员
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int AddScanner(EHECD_ScannerDTO dto, EHECD_SystemUserDTO user);
        /// <summary>
        /// 编辑扫描员
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int EditScanner(EHECD_ScannerDTO dto);
        /// <summary>
        /// 根据ID查询信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_ScannerDTO GetScannerById(string ID);
        /// <summary>
        /// 删除扫描员
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract int DeleteScanner(Dictionary<string, object> dic);
        /// <summary>
        /// 查看是否有相同的登录名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract Dictionary<string, object> SearchName(string name);
    }
}
