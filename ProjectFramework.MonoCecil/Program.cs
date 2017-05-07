using Mono.Cecil;
using System;
using System.Linq;

namespace ProjectFramework.MonoCecil
{
    class Program
    {
        static void Main(string[] args)
        {
            var asmFile = args[0];
            Console.WriteLine("====================将程序集[{0}]中匿名类型访问权限修改为public。====================", asmFile);

            //读取编译文件信息
            var asmDef = AssemblyDefinition.ReadAssembly(asmFile, new ReaderParameters
            {
                ReadSymbols = true
            });

            //找出程序集所有匿名类型
            var anonymousTypes = asmDef.Modules
                .SelectMany(m => m.Types)
                .Where(t => t.Name.Contains("<>f__AnonymousType"));

            //重新设置访问权限
            foreach (var type in anonymousTypes)
            {
                type.IsPublic = true;
            }

            //重写文件
            asmDef.Write(asmFile, new WriterParameters
            {
                WriteSymbols = true
            });
        }
    }
}
