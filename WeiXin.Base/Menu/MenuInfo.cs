using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeiXin.Base.Menu
{
    /// <summary>
    /// 菜单基本信息
    /// </summary>
    public class MenuInfo
    {

        /// <summary>
        /// 按钮描述，既按钮名字，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 按钮类型（click或view）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }

        /// <summary>
        /// 按钮KEY值，用于消息接口(event类型)推送，不超过128字节
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string key { get; set; }

        /// <summary>
        /// 网页链接，用户点击按钮可打开链接，不超过256字节
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string url { get; set; }

        /// <summary>
        /// 构造函数， View 或 Click
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public MenuInfo(ButtonType type, string name, string value)
        {

            this.type = type.ToString();
            this.name = name;
            if (type == ButtonType.click)
            {
                this.key = value;
            }
            else if (type == ButtonType.view)
            {
                this.url = value;
            }
        }

        /// <summary>
        /// View菜单
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static MenuInfo View(string name, string url)
        {
            return new MenuInfo(ButtonType.view, name, url);
        }

        /// <summary>
        /// Click菜单
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static MenuInfo Click(string name, string key)
        {
            return new MenuInfo(ButtonType.click, name, key);
        }
    }
}
