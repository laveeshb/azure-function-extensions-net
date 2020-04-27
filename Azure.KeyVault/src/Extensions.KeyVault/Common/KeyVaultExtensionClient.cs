namespace Azure.Functions.Extensions.KeyVault
{
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    public class KeyVaultExtensionClient
    {
        private KeyVaultPropertiesAttribute KeyVaultProperties { get; set; }

        public KeyVaultExtensionClient(KeyVaultPropertiesAttribute keyVaultProperties)
        {
            this.KeyVaultProperties = keyVaultProperties;
        }

        public KeyVaultClient GetKeyVaultClient()
        {
            return new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(this.GetAccessToken));
        }

        private async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority);
            var credential = new ClientCredential(clientId: this.KeyVaultProperties.ClientId, clientSecret: this.KeyVaultProperties.ClientSecret);
            var tokenResult = await context.AcquireTokenAsync(resource, credential);
            return tokenResult.AccessToken;
        }
    }
}
