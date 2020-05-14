using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Azure.Functions.Extensions.SQS.Commons;
using Microsoft.Azure.WebJobs;

namespace Azure.Functions.Extensions.SQS.Collector
{
	public class SqsQueueAsyncCollector : IAsyncCollector<SendMessageRequest>
	{
		private AmazonSQSClient AmazonSQSClient { get; }
		private SqsQueueOutAttribute SqsQueueOut { get; }
		public SqsQueueAsyncCollector(SqsQueueOutAttribute sqsQueueOut)
		{
			this.SqsQueueOut = sqsQueueOut;
			this.AmazonSQSClient = AmazonSQSClientFactory.Build(sqsQueueOut);

		}
		public async Task AddAsync(SendMessageRequest item, CancellationToken cancellationToken = new CancellationToken())
		{
			item.QueueUrl = item.QueueUrl ?? SqsQueueOut.QueueUrl; 
			await AmazonSQSClient.SendMessageAsync(item, cancellationToken);
		}

		public Task FlushAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			// Batching not supported.
			return Task.CompletedTask;
		}
	}
}
