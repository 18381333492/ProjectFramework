using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;

namespace Framework.BLL
{
    public partial class GoodsCategoriesManager : IGoodsCategoriesManager
    {
        //添加商品分类
        public override EHECD_CategoriesDTO AddGoodsCategories(EHECD_CategoriesDTO c, dynamic p)
        {
            c.ID = Helper.GuidHelper.GetSecuentialGuid();
            c.bIsDeleted = false;
            c.dInsertTime = DateTime.Now;
            c.sImgUri = "";

            var ret = excute.InsertSingle<EHECD_CategoriesDTO>(c);

            InsertSystemLog(
                p.sLoginName.ToString(),
                p.sUserName.ToString(),
                p.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.ADD),
                "系统用户创建商品分类" + c.sCategoryName, c.ID.ToString(),
                ret > 0);

            return ret > 0 ? c : null;
        }

        //删除分类
        public override int DeleteGoodsCategory(EHECD_CategoriesDTO c, dynamic p)
        {
            string sql = @"WITH CTE (ID, PID) AS (
	                        SELECT
		                        ID,
		                        PID
	                        FROM
		                        EHECD_Categories c
	                        WHERE
		                        c.ID = @ID
	                        UNION ALL
		                        SELECT
			                        a.ID,
			                        a.PID
		                        FROM
			                        EHECD_Categories a,
			                        cte c
		                        WHERE
			                        a.PID = c.ID
                        ) SELECT
	                        CTE.ID
                        FROM
	                        CTE;";

            //1.查出要删除的分类和下面的所有子分类
            var delids = query.QueryList<Dictionary<string, object>>(sql, new { ID = c.ID }).Select(m => "'" + m["ID"].ToString() + "'").ToList();

            var delisstr = string.Join(",", delids);

            StringBuilder sb = new StringBuilder();
            //2.删除分类
            sb.AppendLine(Dapper.DBSqlHelper.GetUpdateSQL<EHECD_CategoriesDTO>(new EHECD_CategoriesDTO { bIsDeleted = true }, string.Format("WHERE ID IN ({0})", delisstr)));

            //TODO:3.删除分类对应的商品还有商品对应的规格

            var ret = excute.ExcuteTransaction(sb.ToString());

            InsertSystemLog(
                p.sLoginName.ToString(),
                p.sUserName.ToString(),
                p.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.DELETE),
                "系统用户删除商品分类" + c.ID.ToString(), c.ID.ToString(),
                ret > 0);

            return ret;
        }
    }
}
