
namespace Azure.Functions.Extensions.SQS
{
    using System;
    using Microsoft.Azure.WebJobs.Description;

    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class SqsQueueOutAttribute : Attribute
    {
        [AutoResolve]
        public string AWSKeyId { get; set; }

        [AutoResolve]
        public string AWSAccessKey { get; set; }

        [AutoResolve]
        public string QueueUrl { get; set; }
    }
}
