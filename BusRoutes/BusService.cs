using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using BusRoutes.CentralDispatch.Busses;
using BusRoutes.CentralDispatch.Config;
using BusRoutes.CentralDispatch.Commands;
using BusRoutes.CentralDispatch.Logger;
using BusRoutes.CentralDispatch.Contracts;
using BusRoutes.CentralDispatch;
using MassTransit;
using BusRoutes.WorkerService;

namespace BusRoutes
{
    public static class BusService
    {

        public static void initializeBus()
        {
            AzureBus myBus = new AzureBus();
       
        }

        public static int startListening()
        {
            //TODO - This should probably somehow be moved into the EventConsumerService.Start function so we don't delay windows service starting...
            initializeBus();
            
            return (int)HostFactory.Run(cfg => cfg.Service(x => new EventConsumerService()));
            
        }

        public static void sendCommand(string commandName, object data, string testQueue)
        {
            initializeBus();
            publishPresence(data.ToString(), testQueue).Wait();
        }

        public static async Task publishPresence(string identifier, string testQueue)
        {
            AnnouncePresence presenceMsg = new AnnouncePresence()
            {
                MyTimestamp = DateTime.UtcNow,
                MyIdentifier = identifier
            };

            Logger.Debug(String.Format("Sending a presence message for my identifier {0}", presenceMsg.MyIdentifier));
            try
            {
                if (AzureBusList.isEmpty) {
                    Logger.Fatal("Your Azure Bus List is empty.  You should try adding a bus first.");
                    throw new Exception("Your BusList table is empty.  Have you tried adding a bus?");
                }


                AzureBus myAzureBus = AzureBusList.get();
                IBusControl myBus = myAzureBus._bus;
                if (myBus == null)
                {
                    Console.WriteLine("The bus is missing.");
                    Logger.Error(String.Format("Woah there Nelly!  This bus went missing {0}", myAzureBus.BusIdString));
                    return;
                }

                Console.WriteLine("Sending presence message ...");



                Uri myServiceUri = MyConfigValues.GetServiceBusUri(testQueue);
                Console.WriteLine("Hello " + myServiceUri.ToString());
                
                
                ISendEndpoint sendEndpoint = await myBus.GetSendEndpoint(myServiceUri);
                Console.WriteLine("I have an endpoint now");
                await sendEndpoint.Send<IAnnouncePresence>(
                         presenceMsg
                         );

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Logger.Error("Yelp! Something went wrong trying to send this message.", ex);
            }




            Console.WriteLine("QED");
        }





    }
}
