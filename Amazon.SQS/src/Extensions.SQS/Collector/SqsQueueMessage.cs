namespace Azure.Functions.Extensions.SQS
{
	public class SqsQueueMessage
	{
		public string Body { get; set; }
		public string QueueUrl { get; set; }
	}
}