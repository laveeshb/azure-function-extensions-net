
using Azure.Functions.Extensions.SQS.Commons;

namespace Azure.Functions.Extensions.SQS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon;
    using Amazon.Runtime;
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
            this.Executor = executor;
            this.SqsQueueOptions = sqsQueueOptions;
            this.TriggerParameters = triggerParameters;

            this.SqsQueueOptions.Value.MaxNumberOfMessages = this.SqsQueueOptions.Value.MaxNumberOfMessages ?? 5;
            this.SqsQueueOptions.Value.PollingInterval = this.SqsQueueOptions.Value.PollingInterval ?? TimeSpan.FromSeconds(5);
            this.SqsQueueOptions.Value.VisibilityTimeout = this.SqsQueueOptions.Value.VisibilityTimeout ?? TimeSpan.FromSeconds(5);

            this.AmazonSQSClient = AmazonSQSClientFactory.Build(triggerParameters);
        }

        public void Cancel()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            this.AmazonSQSClient?.Dispose();
            this.AmazonSQSClient = null;

            this.TriggerTimer?.Dispose();
            this.TriggerTimer = null;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.TriggerTimer = new Timer(
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
                QueueUrl = this.TriggerParameters.QueueUrl,
                MaxNumberOfMessages = this.SqsQueueOptions.Value.MaxNumberOfMessages.Value,
                VisibilityTimeout = (int)this.SqsQueueOptions.Value.VisibilityTimeout.Value.TotalSeconds,
            };

            var result = await this.AmazonSQSClient.ReceiveMessageAsync(getMessageRequest);
            Console.WriteLine($"Invoked the queue trigger at '{DateTime.UtcNow} UTC'. Fetched messages count: '{result.Messages.Count}'.");

            foreach (var message in result.Messages)
            {
                var triggerData = new TriggeredFunctionData
                {
                    ParentId = Guid.NewGuid(),
                    TriggerValue = message.Body,
                    TriggerDetails = new Dictionary<string, string>(),
                };

                var functionExecutionResult = await this.Executor.TryExecuteAsync(triggerData, CancellationToken.None);
                if (functionExecutionResult.Succeeded)
                {
                    var deleteMessageRequest = new DeleteMessageRequest
                    {
                        QueueUrl = this.TriggerParameters.QueueUrl,
                        ReceiptHandle = message.ReceiptHandle,
                    };

                    await this.AmazonSQSClient.DeleteMessageAsync(deleteMessageRequest);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.Dispose();
            return Task.CompletedTask;
        }
    }
}
