using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusRoutes.WorkerServiceLib;
using Topshelf;
using BusRoutes.CentralDispatch.Busses;
using BusRoutes.CentralDispatch.Config;
using BusRoutes.CentralDispatch.Commands;
using BusRoutes.CentralDispatch.Logger;
using BusRoutes.CentralDispatch.Contracts;
using MassTransit;

namespace BusRoutes
{
    public static class BusLine
    {

        public static BusTable busTable = new BusTable();
        private static Guid? _currentBusId;


        public static void initializeBus()
        {
            busTable = new BusTable();
            _currentBusId = busTable.addBus();
        }

        public static int startListening()
        {
            //TODO - This should probably somehow be moved into the EventConsumerService.Start function so we don't delay windows service starting...
            initializeBus();
            
            return (int)HostFactory.Run(cfg => cfg.Service(x => new EventConsumerService(busTable, _currentBusId)));
            
        }

        public static void sendCommand(string commandName, object data)
        {
            initializeBus();
            publishPresence(data.ToString());
        }

       

        public static void publishPresence(string identifier)
        {



            AnnouncePresence presenceMessage = new AnnouncePresence()
            {
                MyTimestamp = DateTime.UtcNow,
                MyIdentifier = identifier
            };

            Logger.Debug(String.Format("Sending a presence message for my identifier {0}", presenceMessage.MyIdentifier));
            try
            {
                if (!_currentBusId.HasValue) { Console.WriteLine("No current busid");  Logger.Error("There is no currently selected bus."); return; }
                IBusControl myBus = busTable.GetBus(_currentBusId);
                if (myBus == null)
                {
                    Logger.Error(String.Format("Woah there Nelly!  This bus went missing {0}", _currentBusId.Value.ToString()));
                    return;
                }


               
                Task publishTask = busTable.GetBus(_currentBusId).Publish<IAnnouncePresence>(
                    presenceMessage,
                        context =>
                        {

                        context.Headers.Set("CryptoKey", MyConfigValues.CryptoKey);
                        context.Headers.Set("Environment", MyConfigValues.ServiceBusEnvironmentName);
                        }
                    );
                TaskStatus myStatus = publishTask.Status;
                publishTask.Wait();
                Console.WriteLine("Message has been sent. Thanks.");
                Logger.Debug("Submission of a presence message seems to be done now.");
                //if (busPublishTask.Exception != null) { throw busPublishTask.Exception; }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Logger.Error("Yelp! Something went wrong trying to send this message.", ex);
            }
        }


    }
}
