
namespace Azure.Functions.Extensions.KeyVault.Sample.V3
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public static class GetKey
    {
        [FunctionName("GetKey")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request,
            [KeyVaultKey(
                AuthenticationType = AuthenticationType.ClientSecret,
                VaultName = "%VaultName%",
                ClientId = "%ClientId%",
                ClientSecret = "%ClientSecret%",
                KeyName = "%TestKeyName%")] KeyResult keyResult,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var serializedKeyResult = JsonConvert.SerializeObject(keyResult);
            var message = $"Key value retrieved from the vault: '{serializedKeyResult}'";
            log.LogInformation(message);

            return (ActionResult)new OkObjectResult(message);
        }
    }
}
