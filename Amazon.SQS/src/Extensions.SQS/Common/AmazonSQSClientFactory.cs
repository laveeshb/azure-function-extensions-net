
namespace Azure.Functions.Extensions.SQS
{
    using System;
    using System.Linq;
    using Amazon;
    using Amazon.Runtime;
    using Amazon.SQS;

    public class AmazonSQSClientFactory
    {
        public static AmazonSQSClient Build(SqsQueueTriggerAttribute triggerParameters)
        {
            return AmazonSQSClientFactory.Build(
                queueUrl: triggerParameters.QueueUrl,
                awsKeyId: triggerParameters.AWSKeyId,
                awsAccessKey: triggerParameters.AWSAccessKey);
        }

        public static AmazonSQSClient Build(SqsQueueOutAttribute outParameters)
        {
            return AmazonSQSClientFactory.Build(
                queueUrl: outParameters.QueueUrl,
                awsKeyId: outParameters.AWSKeyId,
                awsAccessKey: outParameters.AWSAccessKey);
        }

        private static AmazonSQSClient Build(string queueUrl, string awsKeyId, string awsAccessKey)
        {
            var sqsRegion = new Uri(queueUrl).Host.Split('.').Skip(1).First();
            return new AmazonSQSClient(
                credentials: new BasicAWSCredentials(accessKey: awsKeyId, secretKey: awsAccessKey),
                region: RegionEndpoint.EnumerableAllRegions.Single(region => region.SystemName.Equals(sqsRegion, StringComparison.InvariantCultureIgnoreCase)));
        }

    }
}
