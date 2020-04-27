namespace Azure.Functions.Extensions.KeyVault
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.WebJobs;

    public class KeyVaultCertificateManager : IAsyncConverter<KeyVaultCertificateAttribute, X509Certificate2>
    {
        public async Task<X509Certificate2> ConvertAsync(KeyVaultCertificateAttribute input, CancellationToken cancellationToken)
        {
            var client = new KeyVaultExtensionClient(keyVaultProperties: input).GetKeyVaultClient();
            var certificateData = await this.GetCertificateData(client, input);
            return new X509Certificate2(certificateData);
        }

        private async Task<byte[]> GetCertificateData(KeyVaultClient client, KeyVaultCertificateAttribute input)
        {
            if (input.FetchPrivateKey)
            {
                var certificateSecret = string.IsNullOrWhiteSpace(input.CertificateVersion)
                    ? await client.GetSecretAsync(
                        vaultBaseUrl: KeyVaultHelpers.GetVaultBaseUrl(input.VaultName),
                        secretName: input.CertificateName)
                    : await client.GetSecretAsync(
                        vaultBaseUrl: KeyVaultHelpers.GetVaultBaseUrl(input.VaultName),
                        secretName: input.CertificateName,
                        secretVersion: input.CertificateVersion);

                return Convert.FromBase64String(certificateSecret.Value);
            }
            else
            {
                var certificateObject = string.IsNullOrWhiteSpace(input.CertificateVersion)
                    ? await client.GetCertificateAsync(
                        vaultBaseUrl: KeyVaultHelpers.GetVaultBaseUrl(input.VaultName),
                        certificateName: input.CertificateName)
                    : await client.GetCertificateAsync(
                        vaultBaseUrl: KeyVaultHelpers.GetVaultBaseUrl(input.VaultName),
                        certificateName: input.CertificateName,
                        certificateVersion: input.CertificateVersion);

                return certificateObject.Cer;
            }
        }
    }
}
