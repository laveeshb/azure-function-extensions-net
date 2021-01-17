
namespace Azure.Functions.Extensions.SQS
{
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using Microsoft.Azure.WebJobs;

    public class SqsQueueAsyncCollector : IAsyncCollector<SendMessageRequest>
    {
        private AmazonSQSClient AmazonSQSClient { get; set; }

        private SqsQueueOutAttribute SqsQueueOut { get; set; }

        public SqsQueueAsyncCollector(SqsQueueOutAttribute sqsQueueOut)
        {
            this.SqsQueueOut = sqsQueueOut;
            this.AmazonSQSClient = AmazonSQSClientFactory.Build(sqsQueueOut);
        }

        public async Task AddAsync(SendMessageRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            request.QueueUrl = request.QueueUrl ?? SqsQueueOut.QueueUrl; 
            await AmazonSQSClient.SendMessageAsync(request, cancellationToken);
        }

        public Task FlushAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Batching not supported.
            return Task.CompletedTask;
        }
    }
}
