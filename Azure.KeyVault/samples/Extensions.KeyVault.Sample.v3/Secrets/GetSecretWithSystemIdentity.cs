
namespace Azure.Functions.Extensions.KeyVault.Sample.V3
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;

    public static class GetSecretWithSystemIdentity
    {
        [FunctionName("GetSecretWithSystemIdentity")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request,
            [KeyVaultSecret(
                AuthenticationType = AuthenticationType.ManagedIdentity,
                VaultName = "%VaultName%",
                SecretName = "%TestSecretName%")] string secretValue,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var message = $"Secret value retrieved from the vault: '{secretValue}'";
            log.LogInformation(message);

            return (ActionResult)new OkObjectResult(message);
        }
    }
}
