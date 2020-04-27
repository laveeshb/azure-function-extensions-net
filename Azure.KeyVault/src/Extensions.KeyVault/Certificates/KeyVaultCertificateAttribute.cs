
namespace Azure.Functions.Extensions.KeyVault
{
    using System;
    using Microsoft.Azure.WebJobs.Description;

    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class KeyVaultCertificateAttribute : KeyVaultPropertiesAttribute
    {
        [AutoResolve]
        public string CertificateName { get; set; }

        [AutoResolve]
        public string CertificateVersion { get; set; }

        public bool FetchPrivateKey { get; set; }
    }
}
