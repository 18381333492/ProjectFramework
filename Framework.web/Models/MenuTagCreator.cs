using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Framework.web.Models
{
    public class MenuTagCreator
    {
        //创建bootstrap菜单的数据
        public static List<dynamic> CreateBootStrapTreeMenu(UserRoleMenuInfo menuInfo)
        {
            return CreateBootStrapTreeMenuString(menuInfo.UserMenu);
        }

        //递归加载菜单
        private static List<dynamic> CreateBootStrapTreeMenuString(IList<UserMenu> userMenu)
        {
            var itemlist = new List<dynamic>();

            //遍历获取到的用户的菜单
            foreach (var item in userMenu)
            {

                //如果有子菜单
                if (item.ChildMenu.Count > 0)
                {
                    itemlist.Add(new
                    {
                        text = item.sMenuName,
                        url = string.IsNullOrWhiteSpace(item.sUrl) ? "root" : item.sUrl,
                        selectable = true,
                        state = new { expanded = false, selected = false, @checked = false, disabled = false },
                        tags = new string[] { item.ChildMenu.Count.ToString() },
                        nodes = CreateBootStrapTreeMenuString(item.ChildMenu)
                    });
                }
                else
                {
                    itemlist.Add(new
                    {
                        text = item.sMenuName,
                        url = string.IsNullOrWhiteSpace(item.sUrl) ? "root" : item.sUrl,
                        selectable = true,
                        state = new { expanded = false, selected = false, @checked = false, disabled = false }
                        //,
                        //tags = new string[] { "badge" }
                    });
                }
            }
            return itemlist;
        }

        [Obsolete("换用bootstrap，该方法弃用")]
        public static HtmlString CreateMenu(UserRoleMenuInfo menuInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetMenuString(menuInfo.UserMenu));
            return new HtmlString(sb.ToString());
        }

        //获取菜单html，这里是递归了的，样式因为没有数据没有调整
        [Obsolete("换用bootstrap，该方法弃用")]
        private static string GetMenuString(IList<UserMenu> menu)
        {
            StringBuilder sb = new StringBuilder();

            //遍历获取到的用户的菜单
            foreach (var item in menu)
            {
                sb.AppendLine("<div title=\"" + item.sMenuName + "\" data-options=\"selected: true\" style=\"overflow: auto; padding: 1px; \">");

                //如果有子菜单
                if (item.ChildMenu.Count > 0)
                {
                    sb.AppendLine("<table class=\"menu_table\">");
                    sb.AppendLine("<tbody>");
                    foreach (var child in item.ChildMenu)
                    {
                        sb.AppendLine("<tr class=\"menu_tr\">");
                        if (child.ChildMenu != null && child.ChildMenu.Count > 0)
                        {
                            sb.AppendLine("<td class=\"menu_td\" data-title=\"" + child.sMenuName + "\" data-link=\"" + child.sUrl + "\">");
                            sb.Append(GetMenuString(child.ChildMenu));
                            sb.AppendLine("</td>");
                        }
                        else
                        {
                            sb.AppendLine("<td class=\"menu_td\" data-title=\"" + child.sMenuName + "\" data-link=\"" + child.sUrl + "\">");
                            sb.AppendLine(child.sMenuName);
                            sb.AppendLine("</td>");
                        }
                        sb.AppendLine("</tr>");
                    }
                    sb.AppendLine("</tbody>");
                    sb.AppendLine("</table>");
                }
                else
                {
                    sb.AppendLine("<table class=\"menu_table\">");
                    sb.AppendLine("<tbody>");
                    sb.AppendLine("<tr class=\"menu_tr\">");
                    sb.AppendLine("<td class=\"menu_td\" data-title=\"" + item.sMenuName + "\" data-link=\"" + item.sUrl + "\">");
                    sb.AppendLine(item.sMenuName);
                    sb.AppendLine("</td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("</tbody>");
                    sb.AppendLine("</table>");
                }
                sb.AppendLine("</div>");
            }
            return sb.ToString();
        }
    }
}