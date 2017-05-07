
namespace WeiXin.Base.Send.PassiveResponse.Message
{
    public class NewsMessage : BaseMessage
    {
        public NewsMessage()
        {
            MsgType =  MessageType.NEWS.ToString().ToLower();
        }

        /// <summary>
        /// 图文消息个数，限制为10条以内
        /// </summary>
        public int ArticleCount;

        /// <summary>
        /// 多条图文消息信息，默认第一个item为大图,注意，如果图文数超过10，则将会无响应
        /// </summary>
        public Article[] Articles;

        public class Article
        {

            public Item item;

            public class Item
            {
                /// <summary>
                /// 图文消息标题
                /// </summary>
                public string Title;

                /// <summary>
                /// 图文消息描述
                /// </summary>
                public string Description;

                /// <summary>
                ///  图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
                /// </summary>
                public string PicUrl;

                /// <summary>
                /// 点击图文消息跳转链接
                /// </summary>
                public string Url;
            }
        }
    }
}
