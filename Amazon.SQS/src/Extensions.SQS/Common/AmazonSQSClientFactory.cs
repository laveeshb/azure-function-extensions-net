
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
                awsAccessKey: triggerParameters.AWSAccessKey,
                awsSessionToken: triggerParameters.AWSSessionToken);
        }

        public static AmazonSQSClient Build(SqsQueueOutAttribute outParameters)
        {
            return AmazonSQSClientFactory.Build(
                queueUrl: outParameters.QueueUrl,
                awsKeyId: outParameters.AWSKeyId,
                awsAccessKey: outParameters.AWSAccessKey,
                awsSessionToken: outParameters.AWSSessionToken);
        }

        private static AmazonSQSClient Build(string queueUrl, string awsKeyId, string awsAccessKey, string awsSessionToken)
        {
            var sqsRegion = new Uri(queueUrl).Host.Split('.').Skip(1).First();

            var sqsConfig = new AmazonSQSConfig
            {
                RegionEndpoint = RegionEndpoint.EnumerableAllRegions.Single(region =>
                    region.SystemName.Equals(sqsRegion, StringComparison.InvariantCultureIgnoreCase))
            };

            if (awsSessionToken == null)
            {
                return new AmazonSQSClient(awsKeyId, awsAccessKey, sqsConfig);
            }

            return new AmazonSQSClient(awsKeyId, awsAccessKey, awsSessionToken, sqsConfig);
        }

    }
}
