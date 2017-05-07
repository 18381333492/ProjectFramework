using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.web.App_Start
{
    public class EHECD_WeiXinKeyWord
    {
        public long ID { get; set; }
        public int iReplyType { get; set; }
        public string sKeywords { get; set; }
        public int iTextType { get; set; }
        public string sTitle { get; set; }
        public string sDescription { get; set; }
        public string sUrl { get; set; }
        public string sPicUrl { get; set; }
        public System.DateTime dInsertTime { get; set; }
        public System.DateTime dEditTime { get; set; }
        public bool bIsDeleted { get; set; }
    }
}