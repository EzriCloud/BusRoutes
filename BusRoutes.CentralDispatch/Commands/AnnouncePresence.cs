using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusRoutes.CentralDispatch.Contracts;

namespace BusRoutes.CentralDispatch.Commands
{
    public class AnnouncePresence : IAnnouncePresence
    {
        public Guid CommandId { get; set; }
        public String MyIdentifier { get; set; }
        public DateTime MyTimestamp { get; set; }
    }
}
