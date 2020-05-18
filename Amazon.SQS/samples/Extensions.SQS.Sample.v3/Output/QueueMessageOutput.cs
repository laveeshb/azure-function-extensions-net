using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Azure.Functions.Extensions.SQS;
using Azure.Functions.Extensions.SQS.Collector;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Extensions.SQS.Sample.v3.Output
{
	public class QueueMessageOutput
	{
		[FunctionName("QueueMessageOutput")]
		public static void Run(
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
		public static void RunFull(
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
		public static async Task RunMulti(
			[HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
			ILogger log,
			[SqsQueueOut(QueueUrl = "%AWS_QUEUE_URL%", AWSAccessKey = "%AWS_ACCESS_KEY%", AWSKeyId = "%AWS_KEY_ID%")] IAsyncCollector<SqsQueueMessage>  messageWriter)
		{
			var message = req.Query["message"];
			var outMessages = new [] { 1, 2, 3 }.Select(n => new SqsQueueMessage
			{
				Body = $"Hello {message} n°{n}"
			});

			foreach (var outMessage in outMessages)
			{
				await messageWriter.AddAsync(outMessage);
			}

			log.LogInformation($"Function triggered with message '{message}'");
		}
	}
}
