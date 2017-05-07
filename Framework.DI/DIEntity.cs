using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Linq;
using System.Configuration;

namespace Framework.DI
{
    /// <summary>
    /// 依赖注入对象
    /// </summary>
    public class DIEntity
    {
        /// <summary>
        /// 注入配置对象如果你要用这个配置，就
        /// 表示对象的配置信息都是从web.config上
        /// 获取的，这里我没有用这种方式，这里我
        /// 直接用的是代码做映射，如果你要用，
        /// 配置文件在这个项目的Web.config里面
        /// </summary>
        private UnityConfigurationSection configuration = ConfigurationManager.GetSection(UnityConfigurationSection.SectionName) as UnityConfigurationSection;

        //注入容器
        private UnityContainer container = new UnityContainer();

        /// <summary>
        /// 获取注入容器
        /// </summary>
        [Obsolete("这个方法是以前用来获取容器对象自己来获取的，现在有了GetImpl方法，这个可以不要了，如果你要用也可以用这种方式来做", true)]
        public UnityContainer Container
        {
            get
            {
                return container;
            }
        }

        /// <summary>
        /// 获取接口的实现对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetImpl<T>()
        {
            try
            {
                return container.Resolve<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 单例
        /// </summary>
        private static DIEntity instance = new DIEntity();

        /// <summary>
        /// 构造函数
        /// </summary>
        private DIEntity()
        {
            //使用代码方式注释，这样有一个坏处，你必须依赖配置的库，这样容易造成循环引用
            //container.RegisterType<QueryHelper, DapperQueryDBHelper>();
            //container.RegisterType<ExcuteHelper, DapperExcuteHelper>();

            //初始化容器
            //这里是用web.config的方式配置的初始化方法。如果你要用
            //配置文件来映射实体和接口，那就把上面那句注释了，用
            //这个就是了
            configuration.Configure(container, "defaultContainer");
        }

        /// <summary>
        /// 获取注入对象实例
        /// </summary>
        /// <returns>注入对象</returns>
        public static DIEntity GetInstance()
        {
            return instance;
        }
    }
}
