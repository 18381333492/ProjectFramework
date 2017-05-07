using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Helper;
using Framework.Dapper;

namespace Framework.BLL
{
    public partial class WechartManager : IWechartManager
    {        
        /// <summary>
        /// 获取微信设置
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_WeChatSetDTO GetSet(string ID)
        {
            return query.SingleQuery<EHECD_WeChatSetDTO>("select * from EHECD_WeChatSet where sShopID=@sShopID and bIsDeleted=0", new { sShopID = ID });
        }
        /// <summary>
        /// 保存微信设置
        /// </summary>
        public override int WeChartSet(EHECD_WeChatSetDTO dto, EHECD_SystemUserDTO user)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"DELETE FROM EHECD_WeChatSet where sShopID='" + user.ID + "'").Append(";");
            EHECD_WeChatSetDTO wechat = new EHECD_WeChatSetDTO()
            {
                ID = GuidHelper.GetSecuentialGuid(),
                sShopID = user.ID,
                sWeChatName = dto.sWeChatName,
                sWeChatNo = dto.sWeChatNo,
                sToken = dto.sToken,
                sUrl = dto.sUrl,
                sAppId = dto.sAppId,
                sAppSecret = dto.sAppSecret,
                bIsDeleted = false,
                sOriginalID = dto.sOriginalID,
                sPaySignKey = dto.sPaySignKey
            };
            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_WeChatSetDTO>(wechat)).Append(";");
            return excute.ExcuteTransaction(builder.ToString());
        }
        /// <summary>
        /// 获取关注回复的信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override IList<EHECD_WeCharReplyDTO> GetFollowReply(string ID, Dictionary<string, object> dic)
        {
            return query.QueryList<EHECD_WeCharReplyDTO>("select * from EHECD_WeCharReply where bIsDeleted=0 and sReplyType='"+dic["sReplyType"] +"' and sShopID=@sShopID and sContentType='"+dic["sContentType"] +"'", new { sShopID = ID });
        }
        /// <summary>
        /// 关注回复设置
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override int FollowReplySet(EHECD_WeCharReplyDTO dto, EHECD_SystemUserDTO user, Dictionary<string, object> dic)
        {
            StringBuilder builder = new StringBuilder();
            //删除表中数据
            builder.Append("DELETE FROM EHECD_WeCharReply where sShopID='" + user.ID + "' AND sContentType='" + dic["sContentType"] + "'  AND sReplyType='" + dic["sReplyType"] + "'").Append(";");
            if (dto.sContentType == 1)
            {
                string[] shopUrl = dic["sShopUrl"].ToString().Split(',');
                string[] Title = dic["sTitle"].ToString().Split(',');
                string[] Content = dic["sContent"].ToString().Split(',');
                string[] PictureUrl = dic["sPictureUrl"].ToString().Split(',');
                if (dto.sReplyType == 0) //关键字回复
                {
                    for (int i = 0; i < shopUrl.Length; i++)
                    {
                        EHECD_WeCharReplyDTO reply = new EHECD_WeCharReplyDTO()
                        {
                            ID = GuidHelper.GetSecuentialGuid(),
                            sShopID = user.ID,
                            sTitle = Title[i],
                            sShopUrl = shopUrl[i],
                            sPictureUrl = PictureUrl[i],
                            sReplyType = dto.sReplyType,
                            sContentType = dto.sContentType,
                            sState = dto.sState
                        };
                        builder.Append(DBSqlHelper.GetInsertSQL<EHECD_WeCharReplyDTO>(reply)).Append(";");
                    }
                }
                if (dto.sReplyType == 2) //自动回复
                {
                    for (int i = 0; i < shopUrl.Length; i++)
                    {
                        EHECD_WeCharReplyDTO reply = new EHECD_WeCharReplyDTO()
                        {
                            ID = GuidHelper.GetSecuentialGuid(),
                            sShopID = user.ID,
                            sTitle = Title[i],
                            sShopUrl = shopUrl[i],
                            sContent = Content[i],
                            sPictureUrl = PictureUrl[i],
                            sReplyType = dto.sReplyType,
                            sContentType = dto.sContentType,
                            sState = dto.sState
                        };
                        builder.Append(DBSqlHelper.GetInsertSQL<EHECD_WeCharReplyDTO>(reply)).Append(";");
                    }
                }
               

            }
            else {
                dto.ID = GuidHelper.GetSecuentialGuid();
                dto.sShopID = user.ID;
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_WeCharReplyDTO>(dto)).Append(";");
            }
            
            builder.Append("DELETE FROM EHECD_WechartReplyType where sShopID='" + user.ID + "' AND sReplyType='" + dic["sReplyType"] + "'").Append(";");//删除表中基本设置的数据
            EHECD_WechartReplyTypeDTO wechart = new EHECD_WechartReplyTypeDTO();
            wechart.ID = GuidHelper.GetSecuentialGuid();
            wechart.sShopID = user.ID;
            wechart.sContentType = Convert.ToBoolean(dic["sContentType"]);
            wechart.sOriginalID = dic["sOriginalID"].ToString();
            wechart.sState =dto.sState;
            wechart.sReplyType =Convert.ToInt32(dic["sReplyType"]);
            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_WechartReplyTypeDTO>(wechart)).Append(";");//向表中插入基本设置数据
            return excute.ExcuteTransaction(builder.ToString());
        }
        /// <summary>
        /// 关键字回复页面绑定
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dic"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetPageList(PageInfo info, Dictionary<string, object> dic, EHECD_SystemUserDTO user)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT
	                    sKeyword,
	                    sContentType,
	                    MAX (sContent) content
                    FROM
	                    EHECD_WeCharReply
                    WHERE
	                    bIsDeleted = 0
                    AND sReplyType = 1
                    AND sShopID = '" + user.ID+ "'");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))
            {

                sb.AppendFormat("and (sKeyword like '%{0}%')", dic["sKeyword"].ToString());
            }
            if (dic["sContentType"] != null && !string.IsNullOrEmpty(dic["sContentType"].ToString()))
            {
                sb.AppendFormat("and (sContentType like '%{0}%')", dic["sContentType"].ToString());
            }
            sb.Append(" GROUP BY sKeyword, sContentType");
            return query.PaginationQuery<Dictionary<string, object>>(sb.ToString(), info, null);
        }
        /// <summary>
        /// 添加关键字回复
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int KeyApplyAdd(EHECD_WeCharReplyDTO dto, EHECD_SystemUserDTO user, Dictionary<string, object> dic)
        {
            StringBuilder builder = new StringBuilder();
           
            if (dto.sContentType == 1)
            {
                string[] shopUrl = dic["sShopUrl"].ToString().Split(',');
                string[] Title = dic["sTitle"].ToString().Split(',');
                string[] Content = dic["sContent"].ToString().Split(',');
                string[] PictureUrl = dic["sPictureUrl"].ToString().Split(',');
                for (int i = 0; i < shopUrl.Length; i++)
                {
                    EHECD_WeCharReplyDTO reply = new EHECD_WeCharReplyDTO()
                    {
                        ID = GuidHelper.GetSecuentialGuid(),
                        sShopID = user.ID,
                        sTitle = Title[i],
                        sShopUrl = shopUrl[i],
                        sContent = Content[i],
                        sPictureUrl = PictureUrl[i],
                        sReplyType = 1,
                        sContentType = dto.sContentType,
                        sState = dto.sState,
                        sKeyword=dto.sKeyword
                    };
                    builder.Append(DBSqlHelper.GetInsertSQL<EHECD_WeCharReplyDTO>(reply)).Append(";");
                }
            }
            else {
                dto.ID = GuidHelper.GetSecuentialGuid();
                dto.sShopID = user.ID;
                dto.sReplyType = 1; 
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_WeCharReplyDTO>(dto)).Append(";");
            }
            return excute.ExcuteTransaction(builder.ToString());
        }
        /// <summary>
        /// 根据ID查看关键字回复的信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_WeCharReplyDTO GetKeyReply(string ID)
        {
            return query.SingleQuery<EHECD_WeCharReplyDTO>(@"SELECT ID,
                                                             sReplyType,
                                                             sContentType,
                                                             sKeyword,
                                                             sTitle,
                                                             sPictureUrl,
                                                             sShopUrl,
                                                             sContent,
                                                             sState,
                                                            sShopID 
                                                            FROM EHECD_WeCharReply WHERE bIsDeleted=0 AND sReplyType=1 AND ID=@ID", new { ID = ID });
        }
        /// <summary>
        /// 修改关键字回复
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override int EditKeyReply(EHECD_WeCharReplyDTO dto, EHECD_SystemUserDTO user, Dictionary<string, object> dic)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("DELETE FROM EHECD_WeCharReply where sShopID='" + user.ID + "' AND sContentType='" + dic["exType"] + "'and sKeyword='" + dic["exKeyword"] + "'  AND sReplyType=1").Append(";");
            if (dto.sContentType == 1)
            {
                string[] shopUrl = dic["sShopUrl"].ToString().Split(',');
                string[] Title = dic["sTitle"].ToString().Split(',');
                string[] Content = dic["sContent"].ToString().Split(',');
                string[] PictureUrl = dic["sPictureUrl"].ToString().Split(',');
                for (int i = 0; i < shopUrl.Length; i++)
                {
                    EHECD_WeCharReplyDTO reply = new EHECD_WeCharReplyDTO()
                    {
                        ID = GuidHelper.GetSecuentialGuid(),
                        sShopID = user.ID,
                        sTitle = Title[i],
                        sShopUrl = shopUrl[i],
                        sContent = Content[i],
                        sPictureUrl = PictureUrl[i],
                        sReplyType = 1,
                        sContentType = dto.sContentType,
                        sState = dto.sState,
                        sKeyword = dto.sKeyword
                    };
                    builder.Append(DBSqlHelper.GetInsertSQL<EHECD_WeCharReplyDTO>(reply)).Append(";");
                }
            }
            else
            {
                dto.ID = GuidHelper.GetSecuentialGuid();
                dto.sShopID = user.ID;
                dto.sReplyType = 1;
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_WeCharReplyDTO>(dto)).Append(";");
            }
            return excute.ExcuteTransaction(builder.ToString());
        }
        /// <summary>
        /// 删除关键字回复
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int DeleteKeyReply(Dictionary<string, object> dic)
        {
            StringBuilder builer = new StringBuilder();
            var sKeyword = dic["sKeyword"].ToString().Split(',');
            var sContentType = dic["sContentType"].ToString().Split(',');
            for (int ABC = 0; ABC < sKeyword.Count(); ABC++)
            {
                builer.Append(DBSqlHelper.GetUpdateSQL<EHECD_WeCharReplyDTO>(new EHECD_WeCharReplyDTO() { bIsDeleted = true }, string.Format("where sKeyword = ('{0}') AND sContentType =('{1}') AND sShopID='"+dic["shopID"] +"'", sKeyword[ABC], sContentType[ABC])));
            }
            return excute.ExcuteTransaction(builer.ToString());
        }
        /// <summary>
        /// 自动回复设置
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override int AutoReplySet(EHECD_WeCharReplyDTO dto, EHECD_SystemUserDTO user)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"DELETE FROM EHECD_WeCharReply where sShopID='" + user.ID + "' and sReplyType=2 and bIsDeleted=0").Append(";");
            dto.ID = GuidHelper.GetSecuentialGuid();
            dto.sShopID = user.ID;
            dto.sReplyType = 2;
            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_WeCharReplyDTO>(dto)).Append(";");
            return excute.ExcuteTransaction(builder.ToString());
        }

        public override EHECD_WeCharReplyDTO GetAutoReply(string ID)
        {
            return query.SingleQuery<EHECD_WeCharReplyDTO>("select * from EHECD_WeCharReply where sShopID=@sShopID and sReplyType=2 and bIsDeleted=0", new { sShopID = ID });
        }
        /// <summary>
        /// 开启/关闭自动回复
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override int ChangeStates(bool state, string ID)
        {
            //return excute.Update(string.Format(@"UPDATE EHECD_WeCharReply SET sState='{0}'  WHERE sReplyType=1 and sShopID = '{1}'",state,ID), null);
            return excute.Update(@"UPDATE EHECD_WechartReplyType SET sState='" + state + "'  WHERE sReplyType=1 and sShopID =@sShopID", new { sShopID = ID });

        }
        /// <summary>
        /// 获取关键字回复状态
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_WechartReplyTypeDTO GetStates(string ID)
        {
            return query.SingleQuery<EHECD_WechartReplyTypeDTO>("SELECT  sState,sContentType from EHECD_WechartReplyType where sReplyType=1 and sShopID=@ID", new { ID = ID });
        }
        /// <summary>
        /// 微信菜单页面绑定
        /// </summary>
        /// <param name="info"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override PagingRet<EHECD_WeChartMenuDTO> GetMenuList(PageInfo info, EHECD_SystemUserDTO user)
        {
            return query.PaginationQuery<EHECD_WeChartMenuDTO>("SELECT * FROM EHECD_WeChartMenu WHERE bIsDeleted=0 and sShopID=@sShopID", info, new { sShopID = user.ID });
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override int AddMenu(EHECD_WeChartMenuDTO dto, EHECD_SystemUserDTO user)
        {
            dto.ID = GuidHelper.GetSecuentialGuid();
            dto.sShopID = user.ID;
            dto.bIsDeleted = false;
            return excute.InsertSingle<EHECD_WeChartMenuDTO>(dto);
        }

        public override string GetAllMenu(EHECD_SystemUserDTO user)
        {
            var menu = query.QueryList<EHECD_WeChartMenuDTO>("select ID,sMenuName from EHECD_WeChartMenu where  bIsDeleted=0 AND sSubmenuID IS NULL and sShopID=@sShopID ", new { sShopID = user.ID });
            var data = from m in menu
                       select new
                       {
                           id = m.ID,
                           text = m.sMenuName
                       };
            return JSONHelper.GetJsonString(data);

        }
        /// <summary>
        /// 查看是否有重名的菜单名
        /// </summary>
        /// <param name="menuname"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override EHECD_WeChartMenuDTO SearchMenu(string menuname, EHECD_SystemUserDTO user)
        {
            return query.SingleQuery<EHECD_WeChartMenuDTO>("select * from EHECD_WeChartMenu where bIsDeleted=0 and sMenuName='" + menuname + "' and sShopID=@sShopID", new { sShopID = user.ID });
        }
        /// <summary>
        /// 根据ID查看详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_WeChartMenuDTO SearchDetail(string ID)
        {
            return query.SingleQuery<EHECD_WeChartMenuDTO>("select * from EHECD_WeChartMenu where ID=@ID", new { ID = ID });
        }
        /// <summary>
        /// 修改菜单信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int EditMenu(EHECD_WeChartMenuDTO dto)
        {
            return excute.UpdateSingle<EHECD_WeChartMenuDTO>(dto, string.Format("where ID='{0}'", dto.ID));
        }
        /// <summary>
        /// 是否是父菜单
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_WeChartMenuDTO IsFatherMenus(string ID)
        {
            return query.SingleQuery<EHECD_WeChartMenuDTO>("select * from EHECD_WeChartMenu where bIsDeleted=0 and ID=@ID", new { ID = ID });
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int DeleteMenus(Dictionary<string, object> dic)
        {
            var di = DI.DIEntity.GetInstance().GetImpl<IWechartManager>();
            StringBuilder builder = new StringBuilder();
            string[] ID = dic["ID"].ToString().Split(',');
            for (int i = 0; i < ID.Count(); i++)
            {
                var father = di.IsFatherMenus(ID[i]);
                if (father != null)
                {
                    builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_WeChartMenuDTO>(new EHECD_WeChartMenuDTO { bIsDeleted = true }, string.Format("where sSubmenuID='{0}'", ID[i])));
                }

                builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_WeChartMenuDTO>(new EHECD_WeChartMenuDTO { bIsDeleted = true }, string.Format("where ID='{0}'", ID[i])));

            }
            return excute.ExcuteTransaction(builder.ToString());
        }
        /// <summary>
        /// 查看关键字回复是否有重名
        /// </summary>
        /// <param name="sKeyword"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override EHECD_WeCharReplyDTO SearchKeyName(Dictionary<string, object> dic, EHECD_SystemUserDTO user)
        {
            return query.SingleQuery<EHECD_WeCharReplyDTO>("select * from EHECD_WeCharReply where bIsDeleted=0 and sKeyword='" + dic["sKeyword"] + "' and sShopID=@sShopID and sReplyType=1 AND sContentType='"+dic["sContentType"] +"'", new { sShopID = user.ID });
        }
        /// <summary>
        /// 是否开启以及本文类型
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override EHECD_WechartReplyTypeDTO GetOn(Dictionary<string, object> dic)
        {
            return query.SingleQuery<EHECD_WechartReplyTypeDTO>("SELECT sState,sContentType FROM EHECD_WechartReplyType WHERE sShopID='" + dic["ID"] + "' AND sReplyType='" + dic["type"] + "'", null);
        }
        /// <summary>
        /// 获取原始ID
        /// </summary>
        /// <param name="ShopID"></param>
        /// <returns></returns>
        public override EHECD_WeChatSetDTO GetsOriginalID(string ShopID)
        {
            return query.SingleQuery<EHECD_WeChatSetDTO>("SELECT sOriginalID FROM EHECD_WeChatSet WHERE sShopID=@sShopID", new { sShopID = ShopID });
        }

        /// <summary>
        /// 更改回复的内容是文本或者是图文
        /// </summary>
        /// <param name="ContentType"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override int ChangeContentType(EHECD_WechartReplyTypeDTO dto, string ID)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("DELETE FROM EHECD_WechartReplyType where sShopID='" + ID+ "' AND sReplyType=1").Append(";");//删除表中基本设置的数据
            dto.ID = GuidHelper.GetSecuentialGuid();
            dto.sShopID =new Guid(ID);
            dto.sReplyType = 1;
            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_WechartReplyTypeDTO>(dto)).Append(";");
            return excute.ExcuteTransaction(builder.ToString());

        }
        /// <summary>
        /// 关键字信息
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override IList<EHECD_WeCharReplyDTO> GetKeyWordByType(Dictionary<string, object> dic)
        {
            return query.QueryList<EHECD_WeCharReplyDTO>("SELECT * FROM EHECD_WeCharReply WHERE bIsDeleted=0 AND sReplyType=1 AND sContentType='"+dic["sContentType"] +"' AND sShopID='"+dic["sShopID"] +"' and sKeyword='"+dic["sKeyword"] +"'", null);
        }

        public override int GetFatherMenuNumber(string ShopID)
        {
            var menu= query.QueryList<EHECD_WeChartMenuDTO>("SELECT ID FROM EHECD_WeChartMenu WHERE bIsDeleted=0 AND sShopID=@ShopID AND sSubmenuID IS NULL", new { ShopID = ShopID });
            int count = menu.Count();
            return count;
        }
    }
}
