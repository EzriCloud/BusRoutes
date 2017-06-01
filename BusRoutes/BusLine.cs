using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusRoutes.WorkerServiceLib;


namespace BusRoutes
{
    public static class BusLine
    {
        public static int begin()
        {
            return WorkerServiceLib.BusLine.begin();
            
        }

       
    }
}
