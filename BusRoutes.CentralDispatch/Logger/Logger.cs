using log4net;
using log4net.Config;
using System;

namespace BusRoutes.CentralDispatch.Logger
{
    public class Logger
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(Logger));

        public Logger()
        {
            BasicConfigurator.Configure();
        }

        public static void Debug(string message)
        {
            log.Debug(message);
        }

        public static void Debug(string message, Exception ex)
        {
            log.Debug(message, ex);
        }


    }
}
