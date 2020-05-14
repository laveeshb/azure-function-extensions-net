using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Azure.Functions.Extensions.SQS.Commons;
using Microsoft.Azure.WebJobs;

namespace Azure.Functions.Extensions.SQS.Collector
{
	public class SqsQueueAsyncCollector : IAsyncCollector<SqsQueueMessage>
	{
		private AmazonSQSClient AmazonSQSClient { get; }
		private SqsQueueOutAttribute TriggerParameters { get; }
		
		public SqsQueueAsyncCollector(SqsQueueOutAttribute triggerParameters)
		{
			this.TriggerParameters = triggerParameters;
			this.AmazonSQSClient = AmazonSQSClientFactory.Build(triggerParameters);

		}
		public async Task AddAsync(SqsQueueMessage item, CancellationToken cancellationToken = new CancellationToken())
		{
			await AmazonSQSClient.SendMessageAsync(item.QueueUrl ?? this.TriggerParameters.QueueUrl, item.Body, cancellationToken);
		}

		public Task FlushAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			// Batching not supported.
			return Task.CompletedTask;
		}
	}
}
