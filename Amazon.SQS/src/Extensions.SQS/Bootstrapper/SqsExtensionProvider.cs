﻿using Microsoft.Azure.WebJobs;
using Azure.Functions.Extensions.SQS.Collector;

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
            queueTriggerRule.BindToCollector(attr => new SqsQueueAsyncCollector(attr));
        }
    }
}
