using System;
using System.Runtime.InteropServices;

namespace Framework.Helper
{
    public class GuidHelper
    {

        [DllImport("rpcrt4.dll", SetLastError = true)]
        //引用外部dll的生成序列方法
        public static extern int UuidCreateSequential(out Guid guid);
        private const int RPC_S_OK = 0;

        /// <summary>
        /// 引用远程程序调用(RPC)应用程序接口API库生成GUID
        /// </summary>
        /// <returns></returns>
        public static Guid CreateRpcrt4Guid()
        {
            Guid guid;
            int result = UuidCreateSequential(out guid);
            if (result == RPC_S_OK)
            {
                byte[] guidBytes = guid.ToByteArray();
                Array.Reverse(guidBytes, 0, 4);
                Array.Reverse(guidBytes, 4, 2);
                Array.Reverse(guidBytes, 6, 2);

                return new Guid(guidBytes);
            }
            else
                return Guid.NewGuid();
        }

        /// <summary>
        /// 生成序列GUID
        /// </summary>
        /// <returns></returns>
        public static Guid GetSecuentialGuid()
        {
            //获取guid
            byte[] uid = Guid.NewGuid().ToByteArray();
            //获取日期
            byte[] binDate = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

            byte[] secuentialGuid = new byte[uid.Length];

            secuentialGuid[0] = uid[0];
            secuentialGuid[1] = uid[1];
            secuentialGuid[2] = uid[2];
            secuentialGuid[3] = uid[3];
            secuentialGuid[4] = uid[4];
            secuentialGuid[5] = uid[5];
            secuentialGuid[6] = uid[6];

            /*
             * 将第个八字节的第一部分设置为“1100”，以便将来能够验证它是由我们生成的
             * 0xc0转换为二进制是11000000，后面的操作是对0xf（二进制：00001111）与guid
             * 字节数组的第七位进行按位与运算
             */
            secuentialGuid[7] = (Byte)(0xc0 | (0xf & uid[7]));

            // 后8字节是序列,    
            // it minimizes index fragmentation   
            // to a degree as long as there are not a large    
            // number of Secuential-Guids generated per millisecond 

            secuentialGuid[9] = binDate[0];
            secuentialGuid[8] = binDate[1];
            secuentialGuid[15] = binDate[2];
            secuentialGuid[14] = binDate[3];
            secuentialGuid[13] = binDate[4];
            secuentialGuid[12] = binDate[5];
            secuentialGuid[11] = binDate[6];
            secuentialGuid[10] = binDate[7];

            return new Guid(secuentialGuid);
        }
    }
}
