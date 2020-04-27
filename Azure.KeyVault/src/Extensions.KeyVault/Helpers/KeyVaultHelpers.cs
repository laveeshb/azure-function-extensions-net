namespace Azure.Functions.Extensions.KeyVault
{
    public static class KeyVaultHelpers
    {
        public static string GetVaultBaseUrl(string vaultName)
        {
            return $"https://{vaultName}.vault.azure.net/";
        }
    }
}
