using System.Collections.Generic;
using Newtonsoft.Json;
namespace WeiXin.Base.Send.CustomerService.Message
{
    /// <summary>
    /// 图文消息
    /// </summary>
    public class NewsMessage : BaseMessage
    {
        [JsonIgnore]
        private NewsInfo _news;

        public NewsMessage() {
            msgtype = MessageType.NEWS.ToString().ToLower();
        }
        /// <summary>
        /// 图文信息
        /// </summary>
        public NewsInfo news {
            get {
                if (_news == null) {
                    _news = new NewsInfo();
                }
                return _news;
            }
            set {
                _news = value;
            }
        }
    }

    /// <summary>
    /// 图文
    /// </summary>
   [JsonObject]
    public class NewsInfo {
        private List<Content> _articles;
        /// <summary>
        /// 图文列表
        /// </summary>
        public List<Content> articles {
            get {
                if (null == _articles) {
                    _articles = new List<Content>();
                }
                return _articles;
            }
            set {
                _articles = value;
            }
        }
    }
}
