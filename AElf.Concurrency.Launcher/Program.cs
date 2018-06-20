using System;
using AElf.Database;
using AElf.Kernel.Concurrency.Execution.Config;
using AElf.Kernel.Miner;
using AElf.Kernel.Modules.AutofacModule;
using AElf.Kernel.Node.Config;
using AElf.Kernel.TxMemPool;
using AElf.Launcher;
using AElf.Network.Config;
using Akka.Actor;
using Akka.Configuration;
using Autofac;

namespace AElf.Concurrency.Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigParser confParser = new ConfigParser();
            bool parsed = confParser.Parse(args);

            if (!parsed)
                return;
            
            var txPoolConf = confParser.TxPoolConfig;
            var netConf = confParser.NetConfig;
            var minerConfig = confParser.MinerConfig;
            var nodeConfig = confParser.NodeConfig;
            var isMiner = confParser.IsMiner;
            
            // Setup ioc 
            IContainer container = SetupIocContainer(isMiner, netConf, txPoolConf, minerConfig, nodeConfig);

            if (container == null)
            {
                Console.WriteLine("IoC setup failed");
                return;
            }

            if (!CheckDBConnect(container))
            {
                Console.WriteLine("Database connection failed");
                return;
            }
            
            using(var scope = container.BeginLifetimeScope())
            {
                InitActor();
                Console.ReadLine();
            }
        }
        
        private static IContainer SetupIocContainer(bool isMiner, IAElfNetworkConfig netConf, ITxPoolConfig txPoolConf, IMinerConfig minerConf, INodeConfig nodeConfig)
        {
            var builder = new ContainerBuilder();
            
            // Register everything
            builder.RegisterModule(new MainModule()); // todo : eventually we won't need this
            
            // Module registrations
            
            builder.RegisterModule(new TransactionManagerModule());
            builder.RegisterModule(new LoggerModule());
            builder.RegisterModule(new DatabaseModule());
            builder.RegisterModule(new NetworkModule(netConf));
            builder.RegisterModule(new RpcServerModule());

            IContainer container = null;
            
            try
            {
                container = builder.Build();
            }
            catch (Exception e)
            {
                return null;
            }

            return container;
        }

        private static bool CheckDBConnect(IContainer container)
        {
            var db = container.Resolve<IKeyValueDatabase>();
            return db.IsConnected();
        }

        private static void InitActor()
        {
            var config =
                ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.port=" + 2551)
                    //.WithFallback(ConfigurationFactory.ParseString("akka.cluster.seed-nodes = [\"akka.tcp://ClusterSystem@127.0.0.1:"+ ports[0]+"\"]"))
                    .WithFallback(AkkaConfig.Instance.Content);
            ActorSystem.Create("ClusterSystem",config);
        }
    }
}