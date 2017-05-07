using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;

namespace Framework.BLL
{
    public partial class GoodsCategoriesManager : IGoodsCategoriesManager
    {
        public override EHECD_CategoriesDTO LoadGoodsCategorie(EHECD_CategoriesDTO c)
        {
            return query.SingleQuery<EHECD_CategoriesDTO>("SELECT * FROM EHECD_Categories WHERE ID = @ID;", new { ID = c.ID });
        }

        public override string LoadGoodsCategories()
        {
            var goodsCatories = query.QueryList<EHECD_CategoriesDTO>(@"SELECT ID,PID,sCategoryName,sCategoryCaption,iOrder,sImgUri,dInsertTime FROM EHECD_Categories WHERE bIsDeleted = 0", null);
            var ret = LoadLevelCategories(goodsCatories, goodsCatories.Where(m => m.PID == null).FirstOrDefault());
            return JSONHelper.GetJsonString(ret);
        }

        private dynamic LoadLevelCategories(IList<EHECD_CategoriesDTO> categories, EHECD_CategoriesDTO parent)
        {
            var ca = new List<dynamic>();
            for (int i = 0; i < categories.Count; i++)
            {
                if (parent.ID == categories[i].PID)
                {
                    ca.Add(new
                    {
                        id = categories[i].ID,
                        text = categories[i].sCategoryName,
                        sCategoryCaption  = categories[i].sCategoryCaption,
                        addDate = categories[i].dInsertTime,
                        iOrder = categories[i].iOrder,
                        sPID = categories[i].PID,
                        sImgUri= categories[i].sImgUri,
                        children = LoadLevelCategories(categories, categories[i])                       
                    });
                }
            }            
            return ca;

        }
    }
}
