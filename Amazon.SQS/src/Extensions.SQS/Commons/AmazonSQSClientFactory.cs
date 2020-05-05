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
			var sqsRegion = new Uri(triggerParameters.QueueUrl).Host.Split('.').Skip(1).First();
			return new AmazonSQSClient(
				credentials: new BasicAWSCredentials(accessKey: triggerParameters.AWSKeyId, secretKey: triggerParameters.AWSAccessKey),
				region: RegionEndpoint.EnumerableAllRegions.Single(region => region.SystemName.Equals(sqsRegion, StringComparison.InvariantCultureIgnoreCase)));

		}
	}
}
