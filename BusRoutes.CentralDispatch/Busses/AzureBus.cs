using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using MassTransit.AzureServiceBusTransport;
using MassTransit;
using BusRoutes.CentralDispatch.Consumers;
using BusRoutes.CentralDispatch.Config;
using BusRoutes.CentralDispatch.Commands;
using BusRoutes.CentralDispatch.Contracts;

namespace BusRoutes.CentralDispatch
{

    public class AzureBus : IAzureBus
    {
        private AnnouncePresence _presenceMsg;


        public string NameSpace { get; set; }
        public string EnvironmentName { get; set; }
        public string KeyName { get; set; }
        public string KeyValue { get; set; }
        public TimeSpan OperationTimeout { get; set; }
        public Guid BusId { get; set; }
        public String MyQueues { get; set; }
        public String BusIdString { get
            {
                return (this.BusId.ToString().Replace("-", ""));
            }
        }
        public List<String> QueuesIListenTo = new List<string>();

        //public ServiceBusHostSettings Settings { get; set; }

        public IBusControl _bus { get;set;}

        public void StartBus()
        {
            _bus.Start();
        }

        public void StopBus()
        {
            _bus.Stop();
        }


        public IBusControl ConfigureBus()
        {
            SensibleDefaults(); //Just in case we haven't been called yet.
            IBusControl myBusController = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                IServiceBusHost serviceBusHost = cfg.Host(this.ServiceUri, host =>
                {

                    host.OperationTimeout = this.OperationTimeout;
                    host.TokenProvider = this.ServiceToken;

                });

                //Create a listening queue, scoped to my bus ID GUID.  
                cfg.ReceiveEndpoint(serviceBusHost, this.BusIdString, e =>
                {
                    Logger.Logger.Info("Registering to receive messages at " + this.QueueUri(this.BusIdString).ToString());
                    //Things I should always handle, no matter what my role.
                    e.Consumer<AnnouncePresenceConsumer>();
                });

                //Add all other listening queues, where applicable
                foreach (String q in QueuesIListenTo)
                {
                    Logger.Logger.Info("Registering to receive messages at " + this.QueueUri(q).ToString());
                    cfg.ReceiveEndpoint(serviceBusHost, q, e =>
                    {
                        //e.PrefetchCount = 16;
                        e.Consumer<AnnouncePresenceConsumer>();
                    });
                }

            });

            _bus = myBusController;

            

            return myBusController;

        }

        public async Task AnnouncePresence()
        {
            //Announce my presence
            _presenceMsg = new AnnouncePresence()
            {
                MyTimestamp = DateTime.UtcNow,
                MyIdentifier = this.BusIdString
            };

            
            ISendEndpoint sendEndpoint = await _bus.GetSendEndpoint(QueueUri("attendance"));
            await sendEndpoint.Send<IAnnouncePresence>(_presenceMsg);

        }

        public void SensibleDefaults()
        {
            if (BusId == new Guid()) { BusId = MyConfigValues.MyIdentifier;  }

            if (String.IsNullOrEmpty(this.NameSpace)) { NameSpace = MyConfigValues.ServiceBusName; }
            if (String.IsNullOrEmpty(this.EnvironmentName)) { EnvironmentName = MyConfigValues.ServiceEnvironment; }

            if (String.IsNullOrEmpty(this.KeyName)) {
                Logger.Logger.Debug("Sensible Defaults: Reading [ServiceBus.KeyName] from config file.");
                KeyName = MyConfigValues.ServiceBusKeyName;
            }

            if (String.IsNullOrEmpty(this.KeyValue)) {
                Logger.Logger.Debug("Sensible Defaults: Reading [ServiceBus.Key] from config file.");
                KeyValue = MyConfigValues.ServiceBusKey;
            }

            
            //#PenaltyBox: We have a 3 second minimum timeout... You violated that, you get five!
            if (OperationTimeout.TotalMilliseconds == 0)
            {
                OperationTimeout = TimeSpan.FromSeconds(MyConfigValues.OperationTimeoutSeconds);
                Logger.Logger.Debug($"Sensible Defaults [OperationTimeoutSeconds] {MyConfigValues.OperationTimeoutSeconds} will be used.");
            }

            if (OperationTimeout.TotalMilliseconds < 3) {
                Logger.Logger.Debug("Sensible Defaults [OperationTimeout] Setting value to 5 seconds. Must be 3 or higher");
                this.OperationTimeout = TimeSpan.FromSeconds(5);
            }
            if (String.IsNullOrEmpty(MyQueues)) { MyQueues = MyConfigValues.ServiceQueues; }
        }

        public AzureBus()
        {
            SensibleDefaults();

            
            //Fast way to get queues I listen to into the configuration table
            if (! String.IsNullOrEmpty(MyQueues))
            {
               this.QueuesIListenTo = MyQueues.Split(',').ToList();
            }

            ConfigureBus();

            //Self Register our new bus. Wouldn't it be nice if the DMV did this too?
            AzureBusList.add(this);

            bool announceMyself = true;
            
            if (announceMyself)
            {
                AnnouncePresence().Wait();
            }


        }



        public Uri QueueUri(string index)
        {
                return (ServiceBusEnvironment.CreateServiceUri("sb", this.NameSpace, $"{this.EnvironmentName}/{index}"));
                     
        }

        
        //Return the URI based on NameSpace and EnvironmentName
        public Uri ServiceUri { get {
                return (ServiceBusEnvironment.CreateServiceUri("sb",
                    this.NameSpace,
                    this.EnvironmentName));
            }

        }

        public TokenProvider ServiceToken
        {
            get
            {

                return (
                                 TokenProvider.CreateSharedAccessSignatureTokenProvider(
                                 KeyName,
                                 KeyValue,
                                 TimeSpan.FromDays(1),
                                 TokenScope.Namespace)
                                 );
            }
        }


    }

    public interface IAzureBus
    {
        string NameSpace { get; set; }
        string EnvironmentName { get; set; }
        string KeyName { get; set; }
        string KeyValue { get; set; }
        TimeSpan OperationTimeout { get; set; }
        string MyQueues { get; set; }

    }
}
