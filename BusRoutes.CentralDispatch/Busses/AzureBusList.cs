using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusRoutes.CentralDispatch
{
    public class AzureBusList
    {

        private static Dictionary<Guid, AzureBus> _busList = new Dictionary<Guid, AzureBus>();

        public static bool isEmpty
        {
            get
            {
                return (_busList.Count == 0);
            }
        }

        public static AzureBus get (Guid index)
        {
            return (_busList[index]);
        }

        //Returns the first index with no parameter
        public static AzureBus get ()
        {
            return _busList.First().Value;
        }

        public static void add (AzureBus newBus)
        {
            _busList.Add(newBus.BusId, newBus);
        }

         
        
    }
}
