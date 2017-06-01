using log4net;
using log4net.Config;
using System;

namespace BusRoutes.CentralDispatch.Logger
{
    public class Logger
    {
        protected static ILog log;

        private static bool _configured = false;

        private static void ConfigureLogger()
        {
            //BasicConfigurator.Configure();
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("Log4Net.config"));
            log = LogManager.GetLogger(typeof(Logger));
            _configured = true;
        }

        public static void Fatal(object message)
        {
            if (!_configured) { ConfigureLogger(); }
            log.Fatal(message);
        }

        public static void Fatal(object message, Exception ex)
        {
            if (!_configured) { ConfigureLogger(); }
            log.Fatal(message, ex);
        }

        public static void Error(object message)
        {
            if (!_configured) { ConfigureLogger(); }
            log.Error(message);
        }

        public static void Error(object message, Exception ex)
        {
            if (!_configured) { ConfigureLogger(); }
            log.Error(message, ex);
        }

        public static void Debug(string message)
        {
            if (!_configured) { ConfigureLogger(); }
            log.Debug(message);
        }

        public static void Debug(string message, Exception ex)
        {
            if (!_configured) { ConfigureLogger(); }
            log.Debug(message, ex);
        }


        public static void Info(string message)
        {
            if (!_configured) { ConfigureLogger(); }
            log.Info(message);
        }

        public static void Warn(string message)
        {
            if (!_configured) { ConfigureLogger(); }
            log.Warn(message);

        }

        public static void Warn(string message, Exception ex)
        {
            if (!_configured) { ConfigureLogger(); }
            log.Warn(message, ex);
        }
    }
}
