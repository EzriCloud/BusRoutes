using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using BusRoutes.CentralDispatch.Logger;
using Microsoft.ServiceBus;

namespace BusRoutes.CentralDispatch.Busses
{
    public class BusTable
    {
        //Private Methods
        private Dictionary<Guid, IBusControl> _myBusses;

        //Public Methods
        public BusTable()
        {
            _myBusses = new Dictionary<Guid, IBusControl>();
        }

        public void StartBus(Guid busId)
        {
            if (_myBusses.ContainsKey(busId))
            {
                _myBusses[busId].Start();
            } else
            {
                Logger.Logger.Debug(String.Format("Cannot start bus {0}. Bus ID does not exist. Did you create it?", busId.ToString()));
            }
        }

        public void StopBus(Guid busId)
        {
            if (_myBusses.ContainsKey(busId))
            {
                _myBusses[busId].Stop(TimeSpan.FromSeconds(30));
            }
            else
            {
                Logger.Logger.Debug(String.Format("Cannot stop bus {0}. Bus ID does not exist. Did you create it?", busId.ToString()));
            }
        }

        public Guid? addBus(Uri ServiceBusUri, String ServiceBusKeyName, String ServiceBusKey, bool isQueueWorker)
        {
            try
            {
                Guid myRouteId = Guid.NewGuid();
                IBusControl serviceBus = Bus.Factory.CreateUsingAzureServiceBus(cfg => {
                    IServiceBusHost serviceBusHost = cfg.Host(ServiceBusUri, host =>
                     {
                         host.OperationTimeout = TimeSpan.FromSeconds(5);
                         host.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(
                                 ServiceBusKeyName,
                                 ServiceBusKey,
                                 TimeSpan.FromDays(1),
                                 TokenScope.Namespace
                                 );

                     });

                    if (isQueueWorker)
                    {
                        cfg.ReceiveEndpoint(serviceBusHost, "happyhour_queue", e =>
                        {
                            //e.Consumer<AnnouncePresenceConsumer>();
                        });

                    }

                });

                _myBusses.Add(myRouteId, serviceBus);
                return myRouteId;
            }
            catch (Exception ex)
            {
                Logger.Logger.Debug("An error occurred while adding a bus.", ex);
            }
            return null;

        }
    }
}
