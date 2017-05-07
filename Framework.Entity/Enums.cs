using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DTO
{
    public enum SYSTEM_LOG_TYPE
    {
        MENU = 1,
        BUTTON = 2,
        LOGON = 4,
        SYSTEMUSER = 8,
        ROLE = 16,
        ADD = 16384 ,
        DELETE = 8192,
        MODIFY = 4096,
        QUERY = 2048
    }
}
