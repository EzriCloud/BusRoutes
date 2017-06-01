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

        

        public static Uri ServiceBusUri
        {
            get
            {
                return ServiceBusEnvironment.CreateServiceUri("sb",
                    ServiceBusName,
                    ServiceBusEnvironmentName);
            }
        }
        public static string ServiceBusKeyName {  get { return GetSettingString("ServiceBus.KeyName"); } }
        public static string ServiceBusKey { get { return GetSettingString("ServiceBus.Key"); } }

        public static string ServiceBusName {  get { return GetSettingString("ServiceBus.Name"); } }
        public static string ServiceBusEnvironmentName {  get { return GetSettingString("ServiceBus.Environment"); } }
        public static string CryptoKey {  get { return GetSettingString("CryptoKey"); } }
        
        public static string SubEnvironment {  get { return GetSettingString("ServiceBus.SubEnvironment").ToLower(); } }
        public static bool isConsumer {  get {
                string consumerString = GetSettingString("IAmAConsumer");
                if (consumerString == null) { return false; }

                return consumerString.ToLower().Equals("yes"); } }


        public static string GetSettingString(string settingName)
        {
            try
            {
                return (ConfigurationManager.AppSettings[settingName]);
            }
            catch (Exception ex)
            {
                Logger.Logger.Debug(String.Format("An exception occurred fetching Config String {0}", settingName), ex);
                return null;
            }

        }
    }


}

