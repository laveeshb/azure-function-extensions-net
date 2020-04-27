using Azure.Functions.Extensions.KeyVault;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(KeyVaultExtensionStartup))]

namespace Azure.Functions.Extensions.KeyVault
{
    public class KeyVaultExtensionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<KeyVaultExtensionProvider>();
        }
    }
}