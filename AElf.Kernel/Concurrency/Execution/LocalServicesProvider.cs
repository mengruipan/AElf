using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AElf.Kernel.Concurrency.Execution.Config;
using Akka.Actor;
using AElf.Kernel.Services;
using AElf.Kernel.Concurrency.Execution.Messages;
using Akka.Routing;

namespace AElf.Kernel.Concurrency.Execution
{
    public class LocalServicesProvider : UntypedActor
    {
        private ServicePack _servicePack;
        public LocalServicesProvider(ServicePack servicePack)
        {
            // TODO: Where to consider pooling?
            _servicePack = servicePack;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RequestLocalSerivcePack req:
                    Sender.Tell(new RespondLocalSerivcePack(req.RequestId, _servicePack));
                    break;
            }
        }

        public static Props Props(ServicePack servicePack)
        {
            if (AkkaConfig.Instance.IsCluster)
            {
                return Akka.Actor.Props.Create(() => new LocalServicesProvider(servicePack)).WithRouter(FromConfig.Instance);
            }
            else
            {
                return Akka.Actor.Props.Create(() => new LocalServicesProvider(servicePack));
            }
        }
    }
}
