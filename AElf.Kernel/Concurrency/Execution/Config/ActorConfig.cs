using System;
using System.Collections.Generic;
using AElf.Configuration;

namespace AElf.Kernel.Concurrency.Execution.Config
{
    [ConfigFile(FileName = "actorconfig.json")]
    public class ActorConfig : ConfigBase<ActorConfig>
    {
        public bool IsCluster { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public int WorkerCount { get; set; }

        public List<SeedNode> Seeds { get; set; }

        public ActorConfig()
        {
            IsCluster = true;
            HostName = "127.0.0.1";
            Port = 0;
            WorkerCount = 1;
            Seeds = new List<SeedNode>();
            Seeds.Add(new SeedNode {HostName = "127.0.0.1", Port = 32551});
//            Seeds.Add(new SeedNode {HostName = "192.168.197.29", Port = 32551});
        }
    }

    public class SeedNode
    {
        public string HostName { get; set; }

        public int Port { get; set; }
    }
}