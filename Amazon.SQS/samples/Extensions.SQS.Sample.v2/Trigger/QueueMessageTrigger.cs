
namespace Azure.Functions.Extensions.SQS.Sample.V2
{
    using Amazon.SQS.Model;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    public static class QueueMessageTrigger
    {
        [FunctionName("QueueMessageTrigger")]
        public static void Run(
            [SqsQueueTrigger(
                AWSKeyId = "",
                AWSAccessKey = "",
                QueueUrl = "")] Message message,
            ILogger log)
        {
            log.LogInformation($"Function triggered with message Id '{message.MessageId}' body '{message.Body}' receipt handle '{message.ReceiptHandle}' MD5 '{message.MD5OfBody}' attributes count '{message.MessageAttributes.Count}'");
        }
    }
}
