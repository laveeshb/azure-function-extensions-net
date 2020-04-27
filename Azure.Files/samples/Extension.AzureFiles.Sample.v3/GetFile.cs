
namespace Azure.Functions.Extension.AzureFiles.Sample.V3
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Azure.Functions.Extension.AzureFiles;

    public static class GetFile
    {
        [FunctionName("GetFile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request,
            [AzureFile(
                StorageConnectionString = "%storageConnectionString%",
                ShareName = "%fileShareName%",
                FilePath = "%filePath%")] AzureFileResult result,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var reader = new StreamReader(result.FileContentStream);
            var fileContent = await reader.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false);

            var message = $"File SHA256 hash: '{result.SHA256Hash}' content: '{fileContent}'";
            log.LogInformation(message);

            return (ActionResult)new OkObjectResult(message);
        }
    }
}
