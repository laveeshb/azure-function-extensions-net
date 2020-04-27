
namespace Azure.Functions.Extensions.SQS
{
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs.Host.Triggers;
    using Microsoft.Extensions.Options;

    public class SqsQueueTriggerBindingProvider : ITriggerBindingProvider
    {
        private IOptions<SqsQueueOptions> SqsQueueOptions { get; set; }

        public SqsQueueTriggerBindingProvider(IOptions<SqsQueueOptions> sqsQueueOptions)
        {
            this.SqsQueueOptions = sqsQueueOptions;
        }

        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            var triggerAttribute = context.Parameter.GetCustomAttribute<SqsQueueTriggerAttribute>(inherit: false);
            return triggerAttribute is null
                ? Task.FromResult<ITriggerBinding>(null)
                : Task.FromResult<ITriggerBinding>(new SqsQueueTriggerBinding(parameterInfo: context.Parameter, triggerParameters: triggerAttribute, sqsQueueOptions: this.SqsQueueOptions));
        }
    }
}
