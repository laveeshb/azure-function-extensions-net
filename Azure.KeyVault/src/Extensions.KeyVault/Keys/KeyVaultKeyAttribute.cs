
namespace Azure.Functions.Extensions.KeyVault
{
    using System;
    using Microsoft.Azure.WebJobs.Description;

    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class KeyVaultKeyAttribute : KeyVaultPropertiesAttribute
    {
        [AutoResolve]
        public string KeyName { get; set; }

        [AutoResolve]
        public string KeyVersion { get; set; }
    }
}
