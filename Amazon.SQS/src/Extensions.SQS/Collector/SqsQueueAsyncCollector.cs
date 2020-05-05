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
	public class SqsQueueAsyncCollector : IAsyncCollector<SqsMessage>
	{
		private AmazonSQSClient AmazonSQSClient { get; }

		public SqsQueueAsyncCollector(SqsQueueTriggerAttribute triggerParameters)
		{
			this.AmazonSQSClient = AmazonSQSClientFactory.Build(triggerParameters);

		}
		public Task AddAsync(SqsMessage item, CancellationToken cancellationToken = new CancellationToken())
		{
			throw new NotImplementedException();
		}

		public Task FlushAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			throw new NotImplementedException();
		}
	}

	public class SqsMessage
	{

	}
}
