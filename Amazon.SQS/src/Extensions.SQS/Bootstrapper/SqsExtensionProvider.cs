
namespace Azure.Functions.Extensions.SQS
{
    using System;
    using Microsoft.Azure.WebJobs.Description;
    using Microsoft.Azure.WebJobs.Host.Config;
    using Microsoft.Extensions.Options;

    [Extension(name: "sqsQueue", configurationSection: "sqsQueue")]
    public class SqsExtensionProvider : IExtensionConfigProvider
    {
        private IOptions<SqsQueueOptions> SqsQueueOptions { get; set; }

        public SqsExtensionProvider(IOptions<SqsQueueOptions> sqsQueueOptions)
        {
            this.SqsQueueOptions = sqsQueueOptions;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var queueTriggerRule = context.AddBindingRule<SqsQueueTriggerAttribute>();
            queueTriggerRule.BindToTrigger(new SqsQueueTriggerBindingProvider(this.SqsQueueOptions));
        }
    }
}
