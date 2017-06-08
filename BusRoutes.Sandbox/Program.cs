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
           
            if (args.Count() != 1)
            {
                Console.WriteLine("Usage... ");
                Console.WriteLine("BusRoutes.Sandbox.exe {EnviroName}/{QueueName}");
                Console.WriteLine("example: BusRoutes.Sandbox.Exe keonasandbox/basicqueue");
                return;
            }

            BusService.sendCommand("presence", "me!", args[0]);
        }
    }
}
