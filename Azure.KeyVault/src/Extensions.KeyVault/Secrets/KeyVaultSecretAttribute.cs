
namespace Azure.Functions.Extensions.KeyVault
{
    using System;
    using Microsoft.Azure.WebJobs.Description;

    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class KeyVaultSecretAttribute : KeyVaultPropertiesAttribute
    {
        [AutoResolve]
        public string SecretName { get; set; }

        [AutoResolve]
        public string SecretVersion { get; set; }
    }
}
