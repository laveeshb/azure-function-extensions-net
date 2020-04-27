
namespace Azure.Functions.Extensions.SQS
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs.Host.Bindings;

    public class SqsQueueMessageValueProvider : IValueProvider
    {
        private object Value { get; set; }

        public Type Type => typeof(string);

        public SqsQueueMessageValueProvider(object value)
        {
            this.Value = value;
        }

        public Task<object> GetValueAsync()
        {
            return Task.FromResult(this.Value);
        }

        public string ToInvokeString()
        {
            return this.Value.ToString();
        }
    }
}
