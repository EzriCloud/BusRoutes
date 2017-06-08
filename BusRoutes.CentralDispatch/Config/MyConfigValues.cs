using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.ServiceBus;
using BusRoutes.CentralDispatch.Logger;

namespace BusRoutes.CentralDispatch.Config
{
    public static class MyConfigValues
    {

        public static Uri GetServiceBusUri(string queueName = "")
        {

            if (queueName == "")
            {
                queueName = "defaultqueue";
            }

            return ServiceBusEnvironment.CreateServiceUri("sb",
                    ServiceBusName,
                    queueName);
            
        }


        public static string ServiceEnvironment {  get { return GetSettingString("ServiceBus.Environment"); } }
        
        public static string ServiceBusKeyName {  get { return GetSettingString("ServiceBus.KeyName"); } }
        public static string ServiceBusKey { get { return GetSettingString("ServiceBus.Key"); } }
        public static string ServiceQueues {  get { return GetSettingString("ServiceBus.Queues"); } }
        
        public static Guid MyIdentifier
        {
            get
            {
                Guid retVal;
                try
                {
                    string MyIDString = GetSettingString("UnitID");
                    retVal = new Guid(MyIDString);
                    Logger.Logger.Info($"Hello!  I am unit: {retVal.ToString()}");
                    return retVal;

                }
                catch (Exception ex)
                {
                    Logger.Logger.Warn(ex.Message);
                    retVal = Guid.NewGuid();
                    Logger.Logger.Warn($"[UnitID] is not in the config file. I chose my temporary identity as {retVal.ToString()} but you should set a permanent GUID in App.Config soon. ");
                    return retVal; ;
                }
            }
        }


        public static string ServiceBusName { get { return GetSettingString("ServiceBus.Name"); } }
        //public static string ServiceQueueName {  get { return GetSettingString("ServiceBus.Queue", "defaultqueue"); } }
        public static string CryptoKey {  get { return GetSettingString("CryptoKey"); } }

        private static bool _isConsumer = false;

        public static int OperationTimeoutSeconds {
            get
            {
                int timeoutValue;
                string timeoutSeconds = GetSettingString("OperationTimeoutSeconds", "5");
                int.TryParse(timeoutSeconds, out timeoutValue);
                return timeoutValue;
            }
        }
        public static bool isConsumer { get { return _isConsumer; } }


        public static string GetSettingString(string settingName, string defaultValue = null)
        {
            try
            {
                string retVal = ConfigurationManager.AppSettings[settingName];
                return (String.IsNullOrEmpty(retVal) ? defaultValue : retVal);
            }
            catch (Exception ex)
            {
                Logger.Logger.Debug(String.Format("An exception occurred fetching Config String {0}", settingName), ex);
                return defaultValue;
            }

        }
    }


}

