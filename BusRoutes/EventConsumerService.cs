using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using BusRoutes.CentralDispatch.Logger;
using BusRoutes.CentralDispatch.Busses;
using BusRoutes.CentralDispatch.Config;
using BusRoutes.CentralDispatch.Consumers;
using MassTransit;
using BusRoutes.CentralDispatch;

namespace BusRoutes.WorkerService
{
    public class EventConsumerService : ServiceControl
    {


        public EventConsumerService ()
        {
        }

        public bool Start(HostControl hostControl)
        {
            try
            {
               Logger.Debug("Service starting");

                if (!AzureBusList.isEmpty)
                {
                    AzureBusList.get().StartBus();
                }


                Logger.Info("Service started successfully");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Fatal("service failed to start successfully", ex);
            }

            return false;

            
        }

        public bool Stop(HostControl hostControl)
        {

            try
            {
                Logger.Debug("Service stopping.");
                Logger.Info("Service stopped successfully");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Fatal("Service failed to stop successfully", ex);
            }

            return false;
        }

    }
}
