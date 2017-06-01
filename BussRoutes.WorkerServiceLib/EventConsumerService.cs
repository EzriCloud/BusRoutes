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

namespace BusRoutes.WorkerServiceLib
{
    public class EventConsumerService : ServiceControl
    {

        private BusTable _myBusTable;
        private Guid? _currentBusId;

        public EventConsumerService (BusTable myBusTable, Guid? currentBusId)
        {
            _myBusTable = myBusTable;
            _currentBusId = currentBusId;
        }

        public bool Start(HostControl hostControl)
        {
            try
            {
               Logger.Debug("Service starting");

                if (_currentBusId.HasValue)
                {
                    _myBusTable.StartBus(_currentBusId.Value);
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
