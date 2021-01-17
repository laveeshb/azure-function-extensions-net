
namespace Azure.Functions.Extensions.SQS.Sample.V3
{
    using System.Linq;
    using System.Threading.Tasks;
    using Amazon.SQS.Model;
    using Azure.Functions.Extensions.SQS;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;

    public class QueueMessageOutput
    {
        [FunctionName("QueueSingleMessageOutput")]
        public static void QueueSingleMessageOutput(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
            ILogger log,
            [SqsQueueOut(QueueUrl = "%AWS_OUT_QUEUE_URL%", AWSAccessKey = "%AWS_ACCESS_KEY%", AWSKeyId = "%AWS_KEY_ID%")] out SqsQueueMessage outMessage)
        {
            var message = req.Query["message"];
            outMessage = new SqsQueueMessage
            {
                Body = message
            };

            log.LogInformation($"Function triggered with message '{message}'");
        }

        [FunctionName("QueueFullMessageOutput")]
        public static void QueueFullMessageOutput(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
            ILogger log,
            [SqsQueueOut(QueueUrl = "%AWS_OUT_QUEUE_URL%", AWSAccessKey = "%AWS_ACCESS_KEY%", AWSKeyId = "%AWS_KEY_ID%")] out SendMessageRequest outMessage)
        {
            var message = req.Query["message"];
            outMessage = new SendMessageRequest
            {
                MessageBody = message,
                DelaySeconds = 2,
                /*MessageAttributes =  ... any supported message property*/
            };

            log.LogInformation($"Function triggered with message '{message}'");
        }

        [FunctionName("QueueMultiMessageOutput")]
        public static async Task QueueMultiMessageOutput(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
            ILogger log,
            [SqsQueueOut(QueueUrl = "%AWS_QUEUE_URL%", AWSAccessKey = "%AWS_ACCESS_KEY%", AWSKeyId = "%AWS_KEY_ID%")] IAsyncCollector<SqsQueueMessage> messageWriter)
        {
            var message = req.Query["message"];
            var outMessages = new [] { 1, 2, 3 }.Select(index => new SqsQueueMessage
            {
                Body = $"Hello {message} n°{index}"
            });

            await Task.WhenAll(outMessages.Select(message => messageWriter.AddAsync(message)));
            log.LogInformation($"Function triggered with message '{message}'");
        }
    }
}
