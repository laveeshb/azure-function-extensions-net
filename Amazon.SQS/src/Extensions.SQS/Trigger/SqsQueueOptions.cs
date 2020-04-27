
namespace Azure.Functions.Extensions.SQS
{
    using System;

    public class SqsQueueOptions
    {
        public int? MaxNumberOfMessages { get; set; }

        public TimeSpan? PollingInterval { get; set; }

        public TimeSpan? VisibilityTimeout { get; set; }
    }
}
