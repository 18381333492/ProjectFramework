using Framework.DTO;
using System.Collections.Generic;
using System;

namespace Framework.BLL
{
    //自定义的用户按钮对比器
    internal class ButtonsCompare : IEqualityComparer<UserMenuButton>
    {
        public bool Equals(UserMenuButton x, UserMenuButton y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(UserMenuButton obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ButtonMenuRoleCompare : IEqualityComparer<object>
    {
        public new bool Equals(object x, object y)
        {
            return ((dynamic)x).id == ((dynamic)y).id && ((dynamic)x).menuid == ((dynamic)y).menuid;
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}