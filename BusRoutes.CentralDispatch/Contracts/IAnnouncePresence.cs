using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace BusRoutes.CentralDispatch.Contracts
{
    public interface IAnnouncePresence
    {
        Guid CommandId { get; }
        String MyIdentifier { get; }
        DateTime MyTimestamp { get; }
    }

   
}
