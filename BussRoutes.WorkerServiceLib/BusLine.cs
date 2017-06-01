using Topshelf;


namespace BusRoutes.WorkerServiceLib
{
    public static class BusLine
    {

        public static int begin()
        {
            return (int)HostFactory.Run(cfg => cfg.Service(x => new EventConsumerService()));
        }
    }

    
}
