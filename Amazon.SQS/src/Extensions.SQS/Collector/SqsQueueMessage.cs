namespace Azure.Functions.Extensions.SQS.Collector
{
	public class SqsQueueMessage
	{
		public string Body { get; set; }
		public string QueueUrl { get; set; }
	}
}