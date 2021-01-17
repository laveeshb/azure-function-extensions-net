
namespace Azure.Functions.Extensions.SQS
{
    using System;
    using System.Threading.Tasks;
    using Amazon.SQS.Model;
    using Microsoft.Azure.WebJobs.Host.Bindings;

    public class SqsQueueMessageValueProvider : IValueProvider
    {
        private object Value { get; set; }

        public Type Type => typeof(Message);

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
