
namespace Azure.Functions.Extensions.SQS
{
    using System;
    using System.Collections.Generic;
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

        [AutoResolve]
        public string MessageAttributeNames { get; set; }

        [AutoResolve]
        public string AttributeNames { get; set; }
    }
}
