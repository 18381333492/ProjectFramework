using Framework.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;
using Framework.DI;
using Framework.MapperConfig;
using Framework.Helper;

namespace Framework.BLL
{
    public partial class ClientRegisterManager : IClientRegisterManager
    {
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="eHECD_ClientDTO"></param>
        /// <returns></returns>
        public override EHECD_ClientDTO RegisteClient(EHECD_ClientDTO eHECD_ClientDTO)
        {
            var isHas = query.SingleQuery<EHECD_ClientDTO>("SELECT * FROM EHECD_Client WHERE sOpenId = @sOpenId;", new { sOpenId = eHECD_ClientDTO.sOpenId });
            if(null != isHas)
            {
                return excute.UpdateSingle<EHECD_ClientDTO>(eHECD_ClientDTO, string.Format(" where  sOpenId = '{0}' ",eHECD_ClientDTO.sOpenId.ToString())) > 0 ? eHECD_ClientDTO : null;
            }
            else
            {
                //没有通过分享链接注册 直接是一级会员
                eHECD_ClientDTO.sIDType = "1";
                return excute.InsertSingle<EHECD_ClientDTO>(eHECD_ClientDTO) > 0 ? eHECD_ClientDTO : null;
            }            
        }
    }
}
