
namespace Azure.Functions.Extensions.KeyVault
{
    public enum AuthenticationType
    {
        /// <summary>
        /// This type uses clientId and clientSecret to get the auth token.
        /// </summary>
        ClientSecret,

        /// <summary>
        /// This type uses clientId and clientCertificate to get the auth token.
        /// </summary>
        ClientCertificate,
    }
}
