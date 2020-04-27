
namespace Azure.Functions.Extensions.KeyVault
{
    using System;
    using Microsoft.Azure.WebJobs.Host.Config;

    public class KeyVaultExtensionProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var secretRule = context.AddBindingRule<KeyVaultSecretAttribute>();
            secretRule.BindToInput(new KeyVaultSecretManager());

            var certificateRule = context.AddBindingRule<KeyVaultCertificateAttribute>();
            certificateRule.BindToInput(new KeyVaultCertificateManager());

            var keyRule = context.AddBindingRule<KeyVaultKeyAttribute>();
            keyRule.BindToInput(new KeyVaultKeyManager());
        }
    }
}
