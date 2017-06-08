using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusRoutes.CentralDispatch.Busses
{
    public class ServiceBus : IServiceBus
    {
        public string ServiceBusNamespace { get; set; }
    }

    public interface IServiceBus
    {
        string ServiceBusNamespace { get; set; }
    }
}
