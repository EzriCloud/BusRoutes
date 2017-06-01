using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusRoutes;


namespace BusRoutes.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            
            BusLine.sendCommand("presence", "me!");
        }
    }
}
