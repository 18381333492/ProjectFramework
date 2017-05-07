using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeiXin.Base.Menu
{
    /// <summary>
    /// 子菜单
    /// </summary>
    public class ChildMenu
    {
        private List<MenuInfo> _sub_button;


        public ChildMenu() { }

        public ChildMenu(string name) {
            this.name = name;
        }

        /// <summary>
        /// 按钮描述，既按钮名字，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        /// <summary>
        /// 子菜单列表
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<MenuInfo> sub_button
        {
            get
            {
                if (null == _sub_button)
                {
                    _sub_button = new List<MenuInfo>();
                }
                return _sub_button;
            }
            set
            {
                _sub_button = value;
            }
        }

    }
}
