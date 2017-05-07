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
    public partial class ClientManager : IClientManager
    {
        //冻结/解冻用户【用户管理】
        public override bool ForzenClients(List<EHECD_ClientDTO> param, EHECD_SystemUserDTO user)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in param)
            {
                sb.AppendLine(DBSqlHelper.GetUpdateSQL<EHECD_ClientDTO>(item, string.Format("where ID = '{0}'", item.ID)));
                sb.AppendLine(DBSqlHelper.GetInsertSQL<EHECD_OperatDTO>(new EHECD_OperatDTO
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    iState = 2,
                    sOperator = user.sUserName,
                    sContent = string.Format("{0}{1}客户{2}", user.sUserName, item.iState == 0 ? "冻结" : "解冻", item.sPhone),
                    sOperatorID= user.ID
                }));
            }

            var ret = excute.ExcuteTransaction(sb.ToString()) > 0;

            return ret;
        }

        //冻结/解冻分享客【分享客管理】
        public override bool ForzenShardClients(List<EHECD_SharedClientInfoDTO> param, EHECD_SystemUserDTO user)
        {
            //查询该店铺
            var shop = query.SingleQuery<EHECD_ShopSetDTO>(@"SELECT * FROM EHECD_ShopSet WHERE sShopID=@sShopID", new { sShopID = user.ID });

            StringBuilder sb = new StringBuilder();

            foreach (var item in param)
            {

                sb.AppendLine(DBSqlHelper.GetUpdateSQL<EHECD_SharedClientInfoDTO>(item, string.Format("where ID = '{0}'", item.ID)));
                sb.AppendLine(DBSqlHelper.GetInsertSQL<EHECD_OperatDTO>(new EHECD_OperatDTO
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    iState = 2,
                    sOperator = user.sUserName,
                    sContent = string.Format("{0}{1}分享客{2}", user.sUserName, item.bIsForzen == true ? "冻结" : "解冻", item.sClientID)
                }));
                if (item.bIsForzen == true)
                {//冻结该分享客

                    var client = query.SingleQuery<EHECD_ClientDTO>(@"SELECT A.* FROM EHECD_Client AS A 
                                                                                  LEFT JOIN  EHECD_SharedClientInfo AS B 
                                                                                  ON A.ID=B.sClientID 
                                                                                  WHERE B.ID=@ID",new { ID= item.ID });
                    //发送站内信
                    var message = new EHECD_SysMessageDTO()
                    {
                        ID = GuidHelper.GetSecuentialGuid(),
                        bIsDeleted = false,
                        iRecevierType = 1,
                        dInsertTime = DateTime.Now,
                        sMsgTitle = "分享客冻结通知",
                        sMsgContent = string.Format("你已经被商家{0}冻结.", shop.sShopName),
                        sSender = user.sUserName
                    };

                    var messageDetail = new EHECD_SysMessageDetailDTO()
                    {
                        ID = GuidHelper.GetSecuentialGuid(),
                        bIsDeleted = false,
                        iRecStatus = 0,
                        dInsertTime = DateTime.Now,
                        sMailID = message.ID,
                        sReceiver = client.sNickName,
                        sReceiverID = client.ID
                    };
                    sb.AppendLine(DBSqlHelper.GetInsertSQL<EHECD_SysMessageDTO>(message));
                    sb.AppendLine(DBSqlHelper.GetInsertSQL<EHECD_SysMessageDetailDTO>(messageDetail));
                }
                
            }
            var ret = excute.ExcuteTransaction(sb.ToString()) > 0;

            return ret;
        }
    }
}
