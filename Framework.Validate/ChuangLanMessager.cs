using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Framework.web.config;
using System.Web;
using Framework.Helper;

namespace Framework.Validate
{
    public class ChuangLanMessager : IMessager
    {
        private string sMsgAccount = WebConfig.LoadDynamicJson("MessageInfo_Chuanglan").GetValue("account").ToString();
        private string sMsgPassword = WebConfig.LoadDynamicJson("MessageInfo_Chuanglan").GetValue("password").ToString();
        private string sMsgMD5key = WebConfig.LoadDynamicJson("MessageInfo_Chuanglan").GetValue("MD5key").ToString();
        private string sRegisteMSG = WebConfig.LoadDynamicJson("MessageInfo_Chuanglan").GetValue("Msg_Registe").ToString();
        private string sChangePWDMSG = WebConfig.LoadDynamicJson("MessageInfo_Chuanglan").GetValue("Msg_ResetPassword").ToString();
        private string postUrl = WebConfig.LoadDynamicJson("MessageInfo_Chuanglan").GetValue("postUrl").ToString();
        private IRandomHelper radomTool= new RandomHelper();

        public ChuangLanMessager()
        {
            try
            {                
                var spchar = new char[] { ',' };
                radomTool.CHSChars = WebConfig.LoadDynamicJson("RandomMessagerCharArray").GetValue("chsChars").ToString().Split(spchar);
                radomTool.ENChars = WebConfig.LoadDynamicJson("RandomMessagerCharArray").GetValue("enChars").ToString().Split(spchar);
                radomTool.JPChars = WebConfig.LoadDynamicJson("RandomMessagerCharArray").GetValue("jpChars").ToString().Split(spchar);
                radomTool.NumberChars = WebConfig.LoadDynamicJson("RandomMessagerCharArray").GetValue("numbers").ToString().Split(spchar);
            }
            catch (Exception)
            {                
            }
        }

        public string RegisteMessage
        {
            get
            {
                return sRegisteMSG;
            }
        }

        public string ChangePWDMessage
        {
            get
            {
                return sChangePWDMSG;
            }
        }

        public IRandomHelper RandomTool
        {
            get
            {
                return radomTool;
            }            
        }

        public bool SendMessage(string mobileNumber, string MsgContent)
        {
            bool bResult = false;
            string postStrTpl = "un={0}&pw={1}&phone={2}&msg={3}&rd=1";
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(string.Format(postStrTpl, sMsgAccount, sMsgPassword, mobileNumber, MsgContent));
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(postUrl);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = postData.Length;
            Stream newStream = myRequest.GetRequestStream();

            newStream.Write(postData, 0, postData.Length);
            newStream.Flush();
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            if (myResponse.StatusCode == HttpStatusCode.OK)
            {
                bResult = true;
            }
            return bResult;
        }
    }
}
