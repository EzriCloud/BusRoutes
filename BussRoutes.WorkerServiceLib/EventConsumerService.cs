using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using BusRoutes.CentralDispatch.Logger;
using BusRoutes.CentralDispatch.Busses;
using BusRoutes.CentralDispatch.Config;

namespace BusRoutes.WorkerServiceLib
{
    public class EventConsumerService : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            try
            {
                Logger.Debug("Service starting");

                BusTable _myBusTable = new BusTable();
                Guid? newBusId = _myBusTable.addBus(MyConfigValues.ServiceBusUri, MyConfigValues.ServiceBusKeyName, MyConfigValues.ServiceBusKey, true);
                if (newBusId.HasValue)
                {
                    _myBusTable.StartBus(newBusId.Value);
                }


                Logger.Debug("Service started successfully");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Debug("service failed to start successfully", ex);
            }

            return false;

            
        }

        public bool Stop(HostControl hostControl)
        {

            try
            {
                Logger.Debug("Service stopping.");
                Logger.Debug("Service stopped successfully");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Debug("Service failed to stop successfully", ex);
            }

            return false;
        }

    }
}
