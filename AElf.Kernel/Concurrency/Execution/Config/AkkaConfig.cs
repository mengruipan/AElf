using AElf.Configuration;

namespace AElf.Kernel.Concurrency.Execution.Config
{
    public class AkkaConfig : ConfigBase<AkkaConfig>
    {
        public bool IsCluster { get; set; }

        public string Content { get; set; }

        public AkkaConfig()
        {
            IsCluster = true;
            Content = @"akka {
                    actor {
                        provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
                        deployment {
                            ""/*"" {
                                router = round-robin-pool # routing strategy
                                nr-of-instances = 10 # max number of total routees
                                cluster {
                                    enabled = on
                                    allow-local-routees = off
                                    max-nr-of-instances-per-node = 1
                                }
                            }
                        }
                    }
                    remote {
                        dot-netty.tcp {
                            hostname = ""127.0.0.1""
                            port = 0
                        }
                    }
                    cluster {
                        seed-nodes = [
                            ""akka.tcp://ClusterSystem@127.0.0.1:2551"",
                        ]
                    }
                }";
        }
    }
}