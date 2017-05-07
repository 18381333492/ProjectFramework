using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
    /// <summary>
    /// 提现 【实现】
    /// author 王其
    /// </summary>
    public class CommentManager : ICommentManager
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetList(Dapper.PageInfo pageInfo, Guid? guid, dynamic where)
        {
            pageInfo.orderType = OrderType.DESC;
            pageInfo.OrderBy = "dCommentTime";

            StringBuilder builder = new StringBuilder();

            string sql = @"SELECT
			                    ID,
			                    (
				                    SELECT
					                    TOP (1) g.sGoodsName
				                    FROM
					                    EHECD_Goods AS g
				                    WHERE
					                    ID = sGoodsID
			                    ) AS sGoodsName,
			                    sOrderNo,
			                    (
				                    SELECT
					                    TOP (1) sShopName
				                    FROM
					                    EHECD_ShopSet
				                    WHERE
					                    sShopID = sStoreID
			                    ) AS sStoreName,
			                    sCommentContent,
			                    dCommentTime,
                                sStoreID,
			                    (
		                            SELECT
			                            TOP (1) b.sNickName
		                            FROM
			                            EHECD_Client AS b
		                            WHERE
			                            b.ID = sCommenterID
	                            ) AS sCommenterName,
			                    bIsReplay
		                    FROM
			                    EHECD_Comment
		                    WHERE
			                    bIsDeleted = 0";

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

            //状态查询
            if (!string.IsNullOrEmpty(where.bIsReplay.Value.ToString()) && where.bIsReplay.Value.ToString() != "-1")
            {
                //设置状态
                builder.Append(string.Format(@" AND a.bIsReplay={0} ", where.bIsReplay.Value.ToString()));
            }

            //如果有查询条件
            if (!string.IsNullOrEmpty(where.sWhere.Value.ToString()))
            {
                //设置开头
                builder.Append(" AND ( ");

                builder.Append(string.Format(@"a.sGoodsName LIKE '%{0}%'
	                                            OR a.sOrderNo LIKE '%{0}%'
	                                            OR a.sCommenterName LIKE '%{0}%'
	                                            OR a.sCommentContent LIKE '%{0}%'", where.sWhere.Value.ToString()));

                //设置结尾
                builder.Append(" ) ");
            }

            //查询数据
            return query.PaginationQuery<Dictionary<string, object>>(builder.ToString(), pageInfo, null);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public override int DoEdit(EHECD_CommentDTO dto)
        {
            dto.dReplayTime = DateTime.Now;
            return excute.UpdateSingle<EHECD_CommentDTO>(dto, string.Format(" where ID='{0}' ", dto.ID));
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override EHECD_CommentDTO GetSingle(Guid? id)
        {
            return query.SingleQuery<EHECD_CommentDTO>(@"SELECT
	                                                        ID,
	                                                        (
		                                                        SELECT
			                                                        TOP (1) g.sGoodsName
		                                                        FROM
			                                                        EHECD_Goods AS g
		                                                        WHERE
			                                                        ID = sGoodsID
	                                                        ) AS sGoodsName,
	                                                        sCommenterName,
	                                                        dCommentTime,
	                                                        sCommentContent,
	                                                        sReplayContent,
	                                                        bIsReplay,
                                                            sCommentImgPath
                                                        FROM
	                                                        EHECD_Comment AS A
                                                        WHERE
	                                                        ID = @ID", new { ID = id });
        }

        /// <summary>
        /// 获取对应的图片数据
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

            //删除评论
            builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_CommentDTO>(new EHECD_CommentDTO() { bIsDeleted = true }, string.Format(@" WHERE ID IN ({0}) ", string.Join(",", idArr))));
            builder.Append(";");
            //删除评论对应的图片
            builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ImagesDTO>(new EHECD_ImagesDTO() { bIsDeleted = true }, string.Format(@" WHERE sBelongID IN ({0}) ", string.Join(",", idArr))));
            return excute.ExcuteTransaction(builder.ToString());
        }
    }
}
