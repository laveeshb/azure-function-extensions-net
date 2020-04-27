namespace Azure.Functions.Extensions.KeyVault.Sample.V3
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using System.Security.Cryptography.X509Certificates;

    public static class GetCertificatePublicKey
    {
        [FunctionName("GetCertificatePublicKey")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request,
            [KeyVaultCertificate(
                AuthenticationType = AuthenticationType.ClientSecret,
                VaultName = "%VaultName%",
                ClientId = "%ClientId%",
                ClientSecret = "%ClientSecret%",
                CertificateName = "%TestCertificateName%",
                FetchPrivateKey = false)] X509Certificate2 certificate,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var message = $"Certificate retrieved from vault. Thumbprint: '{certificate.Thumbprint}'. Has private key: '{certificate.HasPrivateKey}'";
            log.LogInformation(message);

            return (ActionResult)new OkObjectResult(message);
        }
    }
}
