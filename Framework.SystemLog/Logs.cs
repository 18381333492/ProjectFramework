using System;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Framework.SystemLog
{
    public class Logs
    {

        private ILog log = null;        
        private static Logs ins = new Logs();

        /// <summary>
        /// 获取日志对象
        /// </summary>
        /// <returns>日志对象</returns>
        public static Logs GetLog()
        {
            return ins;
        }

        /// <summary>
        /// 写入异常日志
        /// </summary>
        /// <param name="err">异常对象</param>
        public void WriteErrorLog(Exception err)
        {
            StringBuilder sb = new StringBuilder();
            ILog log = LogManager.GetLogger("log");
            sb.AppendLine(string.Format("引发类型：{0}", err.TargetSite.DeclaringType));
            sb.AppendLine(string.Format("所在方法：{0}", err.TargetSite.Name));
            sb.AppendLine(string.Format("异常简述：{0}", err.Message));
            sb.AppendLine("详细信息：");
            sb.AppendLine(string.Format(err.StackTrace));
            log.Error(sb.ToString());            
        }

        /// <summary>
        /// 构造方法，将初始化异常日志信息
        /// </summary>
        private Logs()
        {
            try
            {
                SetLevel("log", "ERROR");
                AddAppender("log", CreateFileAppender("rfa", web.config.WebConfig.LoadElement("logPath")));
                log = log4net.LogManager.GetLogger("log");
                log4net.Config.BasicConfigurator.Configure();
            }
            catch (Exception)
            {
                SetLevel("log", "ERROR");
                AddAppender("log", CreateFileAppender("rfa", "../SystemLogs/Log\\"));
                log = log4net.LogManager.GetLogger("log");
                log4net.Config.BasicConfigurator.Configure();
            }
        }

        //设置log名称和记录等级
        private void SetLevel(string loggerName, string levelName)
        {
            ILog log = LogManager.GetLogger(loggerName);
            Logger l = (Logger)log.Logger;
            l.Level = l.Hierarchy.LevelMap[levelName];
        }

        /// <summary>
        /// 给日志对象添加一个附加器(默认的为滚动日期日志)
        /// </summary>
        /// <param name="loggerName"></param>
        /// <param name="appender"></param>
        public void AddAppender(string loggerName, IAppender appender)
        {
            ILog log = LogManager.GetLogger(loggerName);
            Logger l = (Logger)log.Logger;
            l.AddAppender(appender);
        }

        /// <summary>
        /// 创建一个附加器
        /// </summary>
        /// <param name="name">附加器名称</param>
        /// <param name="filePath">日志文件路径</param>
        /// <returns>附加器对象</returns>
        private IAppender CreateFileAppender(string name, string filePath)
        {
            RollingFileAppender appender = new RollingFileAppender();
            appender.Encoding = Encoding.UTF8;
            appender.File = filePath;
            appender.Name = name;
            appender.AppendToFile = true;
            appender.MaxSizeRollBackups = 10;
            appender.MaxFileSize = 1024 * 1024 * 10;
            appender.RollingStyle = RollingFileAppender.RollingMode.Date;
            appender.DatePattern = "异常日志_yyyyMMdd'.txt'";
            appender.StaticLogFileName = false;

            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = "%n===================================================================================================%n异常时间：%d %n异常级别：%-5p %n%m";
            layout.ActivateOptions();

            appender.Layout = layout;
            appender.ActivateOptions();

            return appender;
        }
    }
}
