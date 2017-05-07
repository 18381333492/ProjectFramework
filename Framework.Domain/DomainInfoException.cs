using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain
{
    public class DomainInfoException:Exception
    {
        private string _Info;

        public DomainInfoException(string Info)
        {
            this._Info = Info;
        }

        /// <summary>
        /// 是否记录日志
        /// </summary>
        public bool IsLog { get; set; } = false;

        private DomainInfoException() { }

        /// <summary>
        /// 获取提示信息
        /// </summary>
        public override string Message
        {
            get
            {
                return this._Info;
            }
        }
    }
}
