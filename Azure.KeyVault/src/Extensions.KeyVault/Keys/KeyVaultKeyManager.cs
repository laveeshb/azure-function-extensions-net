
namespace Azure.Functions.Extensions.KeyVault
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.WebJobs;

    public class KeyVaultKeyManager : IAsyncConverter<KeyVaultKeyAttribute, KeyResult>
    {
        public async Task<KeyResult> ConvertAsync(KeyVaultKeyAttribute input, CancellationToken cancellationToken)
        {
            var client = new KeyVaultExtensionClient(keyVaultProperties: input).GetKeyVaultClient();

            var key = string.IsNullOrWhiteSpace(input.KeyVersion)
                ? await client.GetKeyAsync(
                    vaultBaseUrl: KeyVaultHelpers.GetVaultBaseUrl(input.VaultName),
                    keyName: input.KeyName)
                : await client.GetKeyAsync(
                    vaultBaseUrl: KeyVaultHelpers.GetVaultBaseUrl(input.VaultName),
                    keyName: input.KeyName,
                    keyVersion: input.KeyVersion);

            return new KeyResult { Key = key.Key };
        }
    }
}
