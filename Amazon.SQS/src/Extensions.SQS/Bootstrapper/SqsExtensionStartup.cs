using Azure.Functions.Extensions.SQS;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(SqsExtensionStartup))]

namespace Azure.Functions.Extensions.SQS
{
    public class SqsExtensionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<SqsExtensionProvider>().BindOptions<SqsQueueOptions>();
        }
    }
}