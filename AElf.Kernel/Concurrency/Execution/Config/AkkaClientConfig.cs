using AElf.Configuration;

namespace AElf.Kernel.Concurrency.Execution.Config
{
    public class AkkaClientConfig:ConfigBase<AkkaClientConfig>
    {
        public string Content { get; set; }

        public AkkaClientConfig()
        {
            Content = "";
        }
    }
}