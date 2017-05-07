using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
    /// <summary>
    /// 游记 【实现】
    /// author 王其
    /// </summary>
    public class TravelNoteManager : ITravelNoteManager
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetList(Dapper.PageInfo pageInfo, Guid? guid, dynamic where)
        {
            pageInfo.orderType = OrderType.DESC;
            pageInfo.OrderBy = "dPublishTime";
            StringBuilder builder = new StringBuilder();

            string sql = @"SELECT
	                        ID,
	                        sStoreID,
	                        sTitle,
	                        sAuthor,
	                        dPublishTime,
	                        (
		                        SELECT
			                        TOP (1) sShopName
		                        FROM
			                        EHECD_ShopSet
		                        WHERE
			                        sShopID = sStoreID
	                        ) AS sStoreName,
	                        iOrder
                        FROM
	                        EHECD_TravelsNotes
                        WHERE
	                        bIsDeleted = 0 ";
            //guid为null则加载全部数据，否则加载对应数据
            if (guid == null)
            {
                builder.Append(string.Format(@"SELECT
	                                                *, IsStore = 'false'
                                                FROM
	                                                (
		                                              {0}  
	                                                ) AS a WHERE 1=1 ", sql));
            }
            else
            {
                builder.Append(string.Format(@"SELECT
	                                                *, IsStore = 'true'
                                                FROM
	                                                (
		                                              {0}
	                                                ) AS a WHERE 1=1 ", sql));

                builder.Append(string.Format(" AND a.sStoreID='{0}' ", guid));
            }
            //如果有查询条件
            if (!string.IsNullOrEmpty(where.sWhere.Value.ToString()))
            {
                //设置开头
                builder.Append(" AND ( ");

                builder.Append(string.Format(@"a.sTitle LIKE '%{0}%'
	                                            OR a.sAuthor LIKE '%{0}%'
	                                            OR a.sStoreName LIKE '%{0}%'", where.sWhere.Value.ToString()));

                //设置结尾
                builder.Append(" ) ");
            }
            //查询数据
            return query.PaginationQuery<Dictionary<string, object>>(builder.ToString(), pageInfo, null);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public override int DoAdd(Dictionary<string, object> dir)
        {
            StringBuilder builder = new StringBuilder();
            //添加游记
            EHECD_TravelsNotesDTO dto = new EHECD_TravelsNotesDTO()
            {
                ID = GuidHelper.GetSecuentialGuid(),
                sTitle = dir["sTitle"].ToString(),
                sAuthor = dir["sAuthor"].ToString(),
                sHeadImges = dir["sHeadImges"].ToString(),
                sContent = dir["sContent"].ToString(),
                bIsDeleted = false
            };
            if (dir.ContainsKey("sStoreID") && dir["sStoreID"] != null)
            {
                dto.sStoreID = new Guid(dir["sStoreID"].ToString());
            }
            string travelSql = DBSqlHelper.GetInsertSQL(dto);
            builder.Append(travelSql).Append(";");
            //添加游记的图片
            foreach (string item in dir["sTravelImges"].ToString().Split(','))
            {
                EHECD_ImagesDTO img = new EHECD_ImagesDTO()
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    sBelongID = dto.ID,
                    sImagePath = item,
                    bIsDeleted = false
                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ImagesDTO>(img)).Append(";");
            }

            //执行事务
            return excute.ExcuteTransaction(builder.ToString());
        }

        /// <summary>
        /// 编辑游记
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="traveImg"></param>
        /// <returns></returns>
        public override int DoEdit(EHECD_TravelsNotesDTO dto,string traveImg)
        {
            StringBuilder builder = new StringBuilder();
            //修改游记
            builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_TravelsNotesDTO>(dto, string.Format(" where ID='{0}'", dto.ID.ToString())));
            //先删除原来的游记图片
            builder.AppendFormat(" DELETE EHECD_Images WHERE sBelongID='{0}'; ", dto.ID.ToString());
            //在添加新的游记图片
            foreach (string item in traveImg.Split(','))
            {
                EHECD_ImagesDTO img = new EHECD_ImagesDTO()
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    sBelongID = dto.ID,
                    sImagePath = item,
                    bIsDeleted = false
                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ImagesDTO>(img));
            }
            return excute.ExcuteTransaction(builder.ToString());

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override int DoDelete(string ids)
        {
            List<string> idArr = new List<string>();
            foreach (var item in ids.Split(','))
            {
                idArr.Add("'" + item + "'");
            }

            StringBuilder builder = new StringBuilder();

            //删除
            builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_TravelsNotesDTO>(new EHECD_TravelsNotesDTO() { bIsDeleted = true }, string.Format(@" WHERE ID IN ({0}) ", string.Join(",", idArr))));
            builder.Append(";");
            //删除对应的图片
            builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ImagesDTO>(new EHECD_ImagesDTO() { bIsDeleted = true }, string.Format(@" WHERE sBelongID IN ({0}) ", string.Join(",", idArr))));
            return excute.ExcuteTransaction(builder.ToString());
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public override int DoEditOrder(EHECD_TravelsNotesDTO dto)
        {
            return excute.UpdateSingle<EHECD_TravelsNotesDTO>(dto, string.Format(" where ID='{0}' ", dto.ID));
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override EHECD_TravelsNotesDTO GetSingle(Guid? id)
        {
            return query.SingleQuery<EHECD_TravelsNotesDTO>(@" SELECT
	                                                        TOP (1) *
                                                        FROM
	                                                        EHECD_TravelsNotes
                                                        WHERE
	                                                        ID =@ID", new { ID = id });
        }

        /// <summary>
        /// 获取图片数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override IList<EHECD_ImagesDTO> GetImageList(Guid? id)
        {
            return query.QueryList<EHECD_ImagesDTO>(@"SELECT
			                                                    sImagePath
		                                                    FROM
			                                                    EHECD_Images
		                                                    WHERE
			                                                    sBelongID = @sBelongID
                                                            AND bIsDeleted = 0", new { sBelongID = id });
        }

        /// <summary>
        /// 获取店铺列表
        /// </summary>
        /// <returns></returns>
        public override IList<EHECD_ShopSetDTO> GetShopList(string shopName, Guid id)
        {
            string sql = @"SELECT ID,sShopID,sShopName FROM EHECD_ShopSet WHERE bIsDelete=0 AND sShopID!='" + id + "' AND {0}";
            if (!string.IsNullOrWhiteSpace(shopName) && !string.IsNullOrEmpty(shopName))
            {
                sql = string.Format(sql, "sShopName LIKE '%" + shopName + "%'");
            }
            else
            {
                sql = string.Format(sql, "1=1");
            }
            return query.QueryList<EHECD_ShopSetDTO>(sql, null);
        }

        /// <summary>
        /// 将游记布置到
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override int DoSetting(List<Dictionary<string, object>> data)
        {
            //获取要布置到其他店铺的游记信息
            string travelID = data[0]["TravelID"].ToString();
            EHECD_TravelsNotesDTO dto = query.SingleQuery<EHECD_TravelsNotesDTO>(@"SELECT TOP(1) * FROM EHECD_TravelsNotes WHERE ID=@ID", new { ID = new Guid(travelID) });
            //获取游记对应的图片
            IList<EHECD_ImagesDTO> imageList = query.QueryList<EHECD_ImagesDTO>(@"SELECT * FROM EHECD_Images WHERE sBelongID=@sBelongID", new { sBelongID = new Guid(travelID) });

            StringBuilder builder = new StringBuilder();
            foreach (var item in data)
            {
                //布置游记内容
                dto.ID = GuidHelper.GetSecuentialGuid();
                dto.sStoreID = new Guid(item["sShopID"].ToString());
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_TravelsNotesDTO>(dto)).Append(";");
                //布置游记图片
                if (imageList != null && imageList.Count() > 0)
                {
                    foreach (var img in imageList)
                    {
                        img.ID = GuidHelper.GetSecuentialGuid();
                        img.sBelongID = dto.ID;
                        img.iState = 0;
                        builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ImagesDTO>(img)).Append(";");
                    }
                }

            }
            return excute.ExcuteTransaction(builder.ToString());
        }
    }
}
