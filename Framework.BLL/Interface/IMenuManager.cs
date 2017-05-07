using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;

namespace Framework.BLL
{
    public abstract class IMenuManager : BaseBll
    {
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="dto">菜单对象</param>
        /// <param name="p">插入系统操作日志需要的动态参数</param>
        /// <returns>添加结果</returns>
        public abstract EHECD_FunctionMenuDTO AddMenu(EHECD_FunctionMenuDTO dto,dynamic p);
                        
        /// <summary>
        /// 添加菜单按钮
        /// </summary>
        /// <param name="dto">菜单按钮</param>
        /// <param name="menuID">所属菜单ID</param>
        /// <param name="p">插入系统操作日志需要的动态参数</param>
        /// <returns>添加结果</returns>
        public abstract EHECD_MenuButtonDTO AddButton(EHECD_MenuButtonDTO dto,string menuID, dynamic p);
               
        /// <summary>
        /// 编辑菜单按钮
        /// </summary>
        /// <param name="dto">菜单按钮</param>
        /// <param name="p">插入系统操作日志需要的动态参数</param>
        /// <returns>编辑结果</returns>
        public abstract EHECD_MenuButtonDTO EditButton(EHECD_MenuButtonDTO dto, dynamic p);
             
        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="menu">菜单对象</param>
        /// <param name="p">插入系统操作日志需要的动态参数</param>
        /// <returns>编辑结果</returns>
        public abstract EHECD_FunctionMenuDTO EditMenu(EHECD_FunctionMenuDTO menu, dynamic p);
        
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menu">菜单对象</param>
        /// <param name="p">插入系统操作日志需要的动态参数</param>
        /// <returns>删除结果</returns>
        public abstract int DeleteMenu(EHECD_FunctionMenuDTO menu, dynamic p);
                
        /// <summary>
        /// 删除菜单按钮
        /// </summary>
        /// <param name="btn">菜单按钮</param>
        /// <param name="p">插入系统操作日志需要的动态参数</param>
        /// <returns>删除结果</returns>
        public abstract int DeleteMenuButton(EHECD_MenuButtonDTO btn, dynamic p);
        
        /// <summary>
        /// 物理删除逻辑删除的数据
        /// </summary>
        /// <param name="dynamicData">插入系统操作日志需要的动态参数</param>
        /// <returns>删除结果</returns>
        public abstract bool DelDeletedData(dynamic dynamicData);
    }
}
