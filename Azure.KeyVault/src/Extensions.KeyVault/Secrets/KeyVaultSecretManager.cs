
namespace Azure.Functions.Extensions.KeyVault
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.WebJobs;

    public class KeyVaultSecretManager : IAsyncConverter<KeyVaultSecretAttribute, string>
    {
        public async Task<string> ConvertAsync(KeyVaultSecretAttribute input, CancellationToken cancellationToken)
        {
            var client = new KeyVaultExtensionClient(keyVaultProperties: input).GetKeyVaultClient();

            var secret = string.IsNullOrWhiteSpace(input.SecretVersion)
                ? await client.GetSecretAsync(
                    vaultBaseUrl: KeyVaultHelpers.GetVaultBaseUrl(input.VaultName),
                    secretName: input.SecretName)
                : await client.GetSecretAsync(
                    vaultBaseUrl: KeyVaultHelpers.GetVaultBaseUrl(input.VaultName),
                    secretName: input.SecretName,
                    secretVersion: input.SecretVersion);

            return secret.Value;
        }
    }
}
