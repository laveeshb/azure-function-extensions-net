using System;
using System.Linq;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;

namespace Azure.Functions.Extensions.SQS.Commons
{
	public class AmazonSQSClientFactory
	{
		public static AmazonSQSClient Build(SqsQueueTriggerAttribute triggerParameters)
		{
			return Build(triggerParameters.QueueUrl, triggerParameters.AWSKeyId, triggerParameters.AWSAccessKey);

		}
		public static AmazonSQSClient Build(SqsQueueOutAttribute outParameters)
		{
			return Build(outParameters.QueueUrl, outParameters.AWSKeyId, outParameters.AWSAccessKey);

		}
		public static AmazonSQSClient Build(string queueUrl, string awsKeyId, string awsAccessKey)
		{
			var sqsRegion = new Uri(queueUrl).Host.Split('.').Skip(1).First();
			return new AmazonSQSClient(
				credentials: new BasicAWSCredentials(accessKey: awsKeyId, secretKey: awsAccessKey),
				region: RegionEndpoint.EnumerableAllRegions.Single(region => region.SystemName.Equals(sqsRegion, StringComparison.InvariantCultureIgnoreCase)));
		}

	}
}
