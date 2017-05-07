using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helper
{
    public class RandomHelper : IRandomHelper
    {
        private Random random = new Random();

        public string[] _chchar = null;
        private string[] _jpchar = null;
        public  string[] _enchar = null;
        public  string[] _numberchar = null;

        public string[] CHSChars
        {
            set
            {
                _chchar = value;
            }
        }

        public string[] ENChars
        {
            set
            {
                _enchar = value;
            }
        }

        public string[] JPChars
        {
            set
            {
                _jpchar = value;
            }
        }

        public string[] NumberChars
        {
            set
            {
                _numberchar = value;
            }
        }

        public string GetRandomCHSString(int bit = 4)
        {
            var ret = new StringBuilder();

            for (int i = 0; i < bit; i++)
            {
                ret.Append(_chchar[random.Next(0, _chchar.Length)]);
            }

            return ret.ToString();
        }

        public string GetRandomENString(int bit = 4)
        {
            var ret = new StringBuilder();

            for (int i = 0; i < bit; i++)
            {
                ret.Append(_enchar[random.Next(0, _enchar.Length)]);
            }

            return ret.ToString();
        }

        public string GetRandomIntString(int s = 0, int e = int.MaxValue, int bit = 4)
        {
            var ret = random.Next(s, e);
            var retStr = ret.ToString();
            if (retStr.Length > bit)
            {
                return retStr.Substring(retStr.Length - bit, bit);
            }
            else
            {
                return ret.ToString().PadLeft(bit, '0');
            }
        }

        public string GetRandomJPString(int bit = 4)
        {
            var ret = new StringBuilder();

            for (int i = 0; i < bit; i++)
            {
                ret.Append(_jpchar[random.Next(0, _jpchar.Length)]);
            }

            return ret.ToString();
        }

        /// <summary>
        /// 获取字母数字混合随字符串
        /// </summary>
        /// <param name="bit">位数，默认8</param>
        /// <returns></returns>
        public string GetMixNumberLetter(int bit = 8)
        {
            var ret = new StringBuilder();

            for (int i = 0; i < bit; i++)
            {
                var c = random.Next(2);
                switch (c)
                {
                    case 0:
                        ret.Append(_numberchar[random.Next(0, _numberchar.Length)]);
                        break;
                    case 1:
                        ret.Append(_chchar[random.Next(0, _chchar.Length)]);
                        break;                    
                    default:
                        break;
                }
            }

            return ret.ToString();
        }

        public string GetMixRandomString(int bit = 8)
        {
            var ret = new StringBuilder();

            for (int i = 0; i < bit; i++)
            {
                var c = random.Next(4);
                switch (c)
                {
                    case 0:
                        ret.Append(_numberchar[random.Next(0, _numberchar.Length)]);
                        break;
                    case 1:
                        ret.Append(_chchar[random.Next(0, _chchar.Length)]);
                        break;
                    case 2:
                        ret.Append(_jpchar[random.Next(0, _jpchar.Length)]);
                        break;
                    case 3:
                        ret.Append(_enchar[random.Next(0, _enchar.Length)]);
                        break;
                    default:
                        break;
                }
            }

            return ret.ToString();
        }

        public string GetRandomNumberString(int bit = 4)
        {
            var ret = new StringBuilder();

            for (int i = 0; i < bit; i++)
            {
                ret.Append(_numberchar[random.Next(0, _numberchar.Length)]);
            }

            return ret.ToString();
        }
    }
}
