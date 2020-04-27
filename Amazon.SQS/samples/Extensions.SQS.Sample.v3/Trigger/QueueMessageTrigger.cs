
namespace Azure.Functions.Extensions.SQS.Sample.V3
{
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    public static class QueueMessageTrigger
    {
        [FunctionName("QueueMessageTrigger")]
        public static void Run(
            [SqsQueueTrigger(
                AWSKeyId = "",
                AWSAccessKey = "",
                QueueUrl = "")] string message,
            ILogger log)
        {
            log.LogInformation($"Function triggered with message '{message}'");
        }
    }
}
