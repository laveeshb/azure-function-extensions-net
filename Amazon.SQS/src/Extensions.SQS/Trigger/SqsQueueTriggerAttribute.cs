
namespace Azure.Functions.Extensions.SQS
{
    using System;
    using Microsoft.Azure.WebJobs.Description;

    [Binding]
    public class SqsQueueTriggerAttribute : Attribute
    {
        [AutoResolve]
        public string AWSKeyId { get; set; }

        [AutoResolve]
        public string AWSAccessKey { get; set; }

        [AutoResolve]
        public string QueueUrl { get; set; }
    }
}
