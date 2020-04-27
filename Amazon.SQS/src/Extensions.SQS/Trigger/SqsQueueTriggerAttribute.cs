
namespace Azure.Functions.Extensions.SQS
{
    using System;
    using Microsoft.Azure.WebJobs.Description;

    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class SqsQueueTriggerAttribute : Attribute
    {
        public string AWSKeyId { get; set; }

        public string AWSAccessKey { get; set; }

        public string QueueUrl { get; set; }
    }
}
