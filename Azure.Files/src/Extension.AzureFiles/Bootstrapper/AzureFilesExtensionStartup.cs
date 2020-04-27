using Azure.Functions.Extension.AzureFiles;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(AzureFilesExtensionStartup))]

namespace Azure.Functions.Extension.AzureFiles
{
    public class AzureFilesExtensionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<AzureFilesExtensionProvider>();
        }
    }
}