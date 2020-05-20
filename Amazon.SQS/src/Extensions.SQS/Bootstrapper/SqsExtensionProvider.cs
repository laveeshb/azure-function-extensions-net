using Amazon.SQS.Model;
using Microsoft.Azure.WebJobs;
using Azure.Functions.Extensions.SQS.Collector;
using System.Text;  

namespace Azure.Functions.Extensions.SQS
{
    using System;
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
            queueCollectorRule.BindToCollector(attr => new SqsQueueAsyncCollector(attr));
            queueCollectorRule.AddConverter<SqsQueueMessage, SendMessageRequest>(ConvertSqsQueueMessageToSendMessageRequest);
            queueCollectorRule.AddConverter<string, SendMessageRequest>(ConvertStringToSendMessageRequest);
            queueCollectorRule.AddConverter<byte[], SendMessageRequest>(ConvertByteArrayToSendMessageRequest);
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
