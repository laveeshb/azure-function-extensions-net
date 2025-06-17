
namespace Azure.Functions.Extensions.SQS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using Microsoft.Azure.WebJobs.Host.Executors;
    using Microsoft.Azure.WebJobs.Host.Listeners;
    using Microsoft.Extensions.Options;

    public class SqsQueueTriggerListener : IListener
    {
        private Timer TriggerTimer { get; set; }

        private IOptions<SqsQueueOptions> SqsQueueOptions { get; set; }

        private SqsQueueTriggerAttribute TriggerParameters { get; set; }

        private ITriggeredFunctionExecutor Executor { get; set; }

        private AmazonSQSClient AmazonSQSClient { get; set; }

        public SqsQueueTriggerListener(SqsQueueTriggerAttribute triggerParameters, IOptions<SqsQueueOptions> sqsQueueOptions, ITriggeredFunctionExecutor executor)
        {
            Executor = executor;
            SqsQueueOptions = sqsQueueOptions;
            TriggerParameters = triggerParameters;

            SqsQueueOptions.Value.MaxNumberOfMessages = this.SqsQueueOptions.Value.MaxNumberOfMessages ?? 5;
            SqsQueueOptions.Value.PollingInterval = this.SqsQueueOptions.Value.PollingInterval ?? TimeSpan.FromSeconds(5);
            SqsQueueOptions.Value.VisibilityTimeout = this.SqsQueueOptions.Value.VisibilityTimeout ?? TimeSpan.FromSeconds(5);

            AmazonSQSClient = AmazonSQSClientFactory.Build(triggerParameters);
        }

        public void Cancel()
        {
            Dispose();
        }

        public void Dispose()
        {
            AmazonSQSClient?.Dispose();
            AmazonSQSClient = null;

            TriggerTimer?.Dispose();
            TriggerTimer = null;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            TriggerTimer = new Timer(
                callback: async (state) => await this.OnTriggerCallback(),
                state: null,
                dueTime: TimeSpan.FromSeconds(0),
                period: this.SqsQueueOptions.Value.PollingInterval.Value);

            return Task.CompletedTask;
        }

        public async Task OnTriggerCallback()
        {
            var getMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = TriggerParameters.QueueUrl,
                MaxNumberOfMessages = SqsQueueOptions.Value.MaxNumberOfMessages.Value,
                VisibilityTimeout = (int)SqsQueueOptions.Value.VisibilityTimeout.Value.TotalSeconds,
                AttributeNames = { "All" }
            };

            var result = await this.AmazonSQSClient.ReceiveMessageAsync(getMessageRequest);
            Console.WriteLine($"Invoked the queue trigger at '{DateTime.UtcNow} UTC'. Fetched messages count: '{result.Messages.Count}'.");
            await Task.WhenAll(result.Messages.Select(message => this.ProcessMessage(message)));
        }

        private async Task ProcessMessage(Message message)
        {
            var triggerData = new TriggeredFunctionData
            {
                ParentId = Guid.NewGuid(),
                TriggerValue = message,
                TriggerDetails = new Dictionary<string, string>(),
            };

            var functionExecutionResult = await Executor.TryExecuteAsync(triggerData, CancellationToken.None);
            if (functionExecutionResult.Succeeded)
            {
                var deleteMessageRequest = new DeleteMessageRequest
                {
                    QueueUrl = TriggerParameters.QueueUrl,
                    ReceiptHandle = message.ReceiptHandle,
                };

                await AmazonSQSClient.DeleteMessageAsync(deleteMessageRequest);
            }
            else if (bool.TryParse(TriggerParameters.ExponentialRetry, out var exponentialRetry) && exponentialRetry)
            {
                int.TryParse(message.Attributes["ApproximateReceiveCount"], out int approximateReceiveCount);
                await AmazonSQSClient.ChangeMessageVisibilityAsync(TriggerParameters.QueueUrl, message.ReceiptHandle, approximateReceiveCount * 60);
            } 
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
