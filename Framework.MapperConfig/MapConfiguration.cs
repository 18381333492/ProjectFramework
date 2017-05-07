using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Framework.DTO;

namespace Framework.MapperConfig
{
    public class MapConfiguration
    {
        /// <summary>
        /// 初始化automapper配置
        /// </summary>
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                //TODO:配置对象的映射，如下
                cfg.CreateMap<EHECD_RoleDTO, UserRole>();
                cfg.CreateMap<EHECD_FunctionMenuDTO, UserMenu>();
                cfg.CreateMap<EHECD_MenuButtonDTO, UserMenuButton>();

                cfg.CreateMap<UserRole, EHECD_RoleDTO>();
                cfg.CreateMap<UserMenu, EHECD_FunctionMenuDTO>();
                cfg.CreateMap<UserMenuButton, EHECD_MenuButtonDTO>();
                cfg.CreateMap<EHECD_ArticleDTO, Article>();
                cfg.CreateMap<Article, EHECD_ArticleDTO>();
                                
            });
        }
    }
}
