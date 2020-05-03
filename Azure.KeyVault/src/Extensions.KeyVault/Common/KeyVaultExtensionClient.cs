namespace Azure.Functions.Extensions.KeyVault
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    public class KeyVaultExtensionClient
    {
        private KeyVaultPropertiesAttribute KeyVaultProperties { get; set; }

        public KeyVaultExtensionClient(KeyVaultPropertiesAttribute keyVaultProperties)
        {
            if (!string.IsNullOrEmpty(keyVaultProperties.ClientSecret) && !string.IsNullOrEmpty(keyVaultProperties.EncodedClientCertificate))
            {
                throw new InvalidOperationException("Binding has both client secret and certificate set. Please use exactly one type of secret.");
            }

            this.KeyVaultProperties = keyVaultProperties;
        }

        public KeyVaultClient GetKeyVaultClient()
        {
            if (!string.IsNullOrEmpty(this.KeyVaultProperties.ClientSecret))
            {
                return new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(this.GetAccessTokenWithClientSecret));
            }
            else if (!string.IsNullOrEmpty(this.KeyVaultProperties.EncodedClientCertificate))
            {
                return new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(this.GetAccessTokenWithClientCertificate));
            }

            throw new InvalidOperationException("Unexpected secret type encountered");
        }

        private async Task<string> GetAccessTokenWithClientSecret(string authority, string resource, string scope)
        {
            var credential = new ClientCredential(
                clientId: this.KeyVaultProperties.ClientId,
                clientSecret: this.KeyVaultProperties.ClientSecret);

            var context = new AuthenticationContext(authority);
            var tokenResult = await context.AcquireTokenAsync(resource, credential);
            return tokenResult.AccessToken;
        }

        private async Task<string> GetAccessTokenWithClientCertificate(string authority, string resource, string scope)
        {
            var credential = new ClientAssertionCertificate(
                clientId: this.KeyVaultProperties.ClientId,
                certificate: new X509Certificate2(rawData: Convert.FromBase64String(this.KeyVaultProperties.EncodedClientCertificate)));

            var context = new AuthenticationContext(authority);
            var tokenResult = await context.AcquireTokenAsync(resource, credential);
            return tokenResult.AccessToken;
        }
    }
}
