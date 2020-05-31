
namespace Azure.Functions.Extensions.SQS
{
    using System;
    using System.Text;  
    using Amazon.SQS.Model;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Description;
    using Microsoft.Azure.WebJobs.Host.Config;
    using Microsoft.Extensions.Options;

    [Extension(name: "sqsQueue", configurationSection: "sqsQueue")]
    public class SqsExtensionProvider : IExtensionConfigProvider
    {
	    private IOptions<SqsQueueOptions> SqsQueueOptions { get; set; }

	    private INameResolver NameResolver { get; set; }

        public SqsExtensionProvider(IOptions<SqsQueueOptions> sqsQueueOptions, INameResolver nameResolver)
        {
	        this.SqsQueueOptions = sqsQueueOptions;
	        this.NameResolver = nameResolver;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var queueTriggerRule = context.AddBindingRule<SqsQueueTriggerAttribute>();
            queueTriggerRule.BindToTrigger(new SqsQueueTriggerBindingProvider(this.SqsQueueOptions, this.NameResolver));

            var queueCollectorRule = context.AddBindingRule<SqsQueueOutAttribute>();
            queueCollectorRule.BindToCollector(attribute => new SqsQueueAsyncCollector(attribute));
            queueCollectorRule.AddConverter<SqsQueueMessage, SendMessageRequest>(SqsExtensionProvider.ConvertSqsQueueMessageToSendMessageRequest);
            queueCollectorRule.AddConverter<string, SendMessageRequest>(SqsExtensionProvider.ConvertStringToSendMessageRequest);
            queueCollectorRule.AddConverter<byte[], SendMessageRequest>(SqsExtensionProvider.ConvertByteArrayToSendMessageRequest);
        }

        private static SendMessageRequest ConvertByteArrayToSendMessageRequest(byte[] body) 
        {
            var utfString = Encoding.UTF8.GetString(body, 0, body.Length);
            return ConvertStringToSendMessageRequest(utfString);
        }

        private static SendMessageRequest ConvertStringToSendMessageRequest(string body)
        {
	        return new SendMessageRequest
	        {
                MessageBody = body
	        };
        }

        private static SendMessageRequest ConvertSqsQueueMessageToSendMessageRequest(SqsQueueMessage sqsQueueMessage)
        {
	        return new SendMessageRequest
	        {
                QueueUrl =  sqsQueueMessage.QueueUrl,
                MessageBody = sqsQueueMessage.Body
	        };
        }
    }
}
