using System;
using System.Threading.Tasks;
using BusRoutes.CentralDispatch.Logger;
using BusRoutes.CentralDispatch.Consumers;
using BusRoutes.CentralDispatch.Contracts;
using MassTransit;
using System.Collections.Generic;
using BusRoutes.CentralDispatch.Config;


namespace BusRoutes.CentralDispatch.Consumers
{
    public class AnnouncePresenceConsumer : IConsumer<IAnnouncePresence>
    {
            public async Task Consume(ConsumeContext<IAnnouncePresence> context)
            {

            bool AllowAnonymous = true;
            HostInfo hostInfo = context.Host;
            String hostString = String.Format("{0}/{1} v{2} @{3}", hostInfo.Assembly, hostInfo.ProcessName, hostInfo.AssemblyVersion, hostInfo.MachineName);
            String MessageAuthKey = context.Headers.Get<string>("CryptoKey", "Anonymous");

            if ((MessageAuthKey == MyConfigValues.CryptoKey) || (AllowAnonymous && MessageAuthKey == "Anonymous"))
            {
                String presenceMessage = $"{context.Message.MyIdentifier} announced their presence at {context.Message.MyTimestamp} UTC (Auth: {MessageAuthKey}) {hostString}";
                Logger.Logger.Debug(presenceMessage);
                await Console.Out.WriteLineAsync(presenceMessage);
            }
            else
            {
                Logger.Logger.Warn($"A message with an invalid CryptoKey was received from {hostString}");
            }

        }

       
    }
}
