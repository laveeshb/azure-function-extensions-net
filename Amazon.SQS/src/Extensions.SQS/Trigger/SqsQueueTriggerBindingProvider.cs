
namespace Azure.Functions.Extensions.SQS
{
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.Azure.WebJobs.Host.Triggers;
    using Microsoft.Extensions.Options;

    public class SqsQueueTriggerBindingProvider : ITriggerBindingProvider
    {
        private IOptions<SqsQueueOptions> SqsQueueOptions { get; set; }

        private INameResolver NameResolver { get; set; }

        public SqsQueueTriggerBindingProvider(IOptions<SqsQueueOptions> sqsQueueOptions, INameResolver nameResolver)
        {
            this.SqsQueueOptions = sqsQueueOptions;
            this.NameResolver = nameResolver;
        }

        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            var triggerAttribute = context.Parameter.GetCustomAttribute<SqsQueueTriggerAttribute>(inherit: false);
            return triggerAttribute is null
                ? Task.FromResult<ITriggerBinding>(null)
                : Task.FromResult<ITriggerBinding>(new SqsQueueTriggerBinding(parameterInfo: context.Parameter, triggerParameters: this.ResolveTriggerParameters(triggerAttribute), sqsQueueOptions: this.SqsQueueOptions));
        }

        private SqsQueueTriggerAttribute ResolveTriggerParameters(SqsQueueTriggerAttribute triggerAttribute)
        {
            return new SqsQueueTriggerAttribute
            {
                AWSKeyId = this.Resolve(triggerAttribute.AWSKeyId),
                AWSAccessKey = this.Resolve(triggerAttribute.AWSAccessKey),
                QueueUrl = this.Resolve(triggerAttribute.QueueUrl)
            };
        }

        private string Resolve(string property)
        {
            return this.NameResolver.Resolve(property) ?? this.NameResolver.ResolveWholeString(property) ?? property;
        }
    }
}
