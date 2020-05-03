
namespace Azure.Functions.Extensions.KeyVault
{
    using System;
    using Microsoft.Azure.WebJobs.Description;

    public abstract class KeyVaultPropertiesAttribute : Attribute
    {
        public AuthenticationType AuthenticationType { get; set; }

        [AutoResolve]
        public string VaultName { get; set; }

        [AutoResolve]
        public string ClientId { get; set; }

        [AutoResolve]
        public string ClientSecret { get; set; }

        [AutoResolve]
        public string EncodedClientCertificate { get; set; }
    }
}
