
namespace Azure.Functions.Extension.AzureFiles
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.File;

    public class AzureFileManager : IAsyncConverter<AzureFileAttribute, AzureFileResult>
    {
        public async Task<AzureFileResult> ConvertAsync(AzureFileAttribute input, CancellationToken cancellationToken)
        {
            var storageAccount = CloudStorageAccount.Parse(input.StorageConnectionString);
            var fileClient = storageAccount.CreateCloudFileClient();

            var fileReference = this.GetFileReference(
                fileShareReference: fileClient.GetShareReference(input.ShareName),
                filePath: input.FilePath);

            var targetStream = new MemoryStream();
            await fileReference.DownloadToStreamAsync(targetStream).ConfigureAwait(continueOnCapturedContext: false);
            targetStream.Position = 0;

            var streamHash = this.GetHash(targetStream);
            targetStream.Position = 0;

            return new AzureFileResult
            {
                SHA256Hash = streamHash,
                FileContentStream = targetStream,
            };
        }

        private CloudFile GetFileReference(CloudFileShare fileShareReference, string filePath)
        {
            var rootDirectoryReference = fileShareReference.GetRootDirectoryReference();

            var filePathSegments = filePath
                .Split('/')
                .Where(segment => !string.IsNullOrWhiteSpace(segment))
                .ToArray();

            if (filePathSegments.Length == 1)
            {
                return rootDirectoryReference.GetFileReference(filePathSegments.Single());
            }
            else
            {
                var folderPath = string.Join("/", filePathSegments.Take(filePathSegments.Length - 1)).TrimStart('/');
                var directoryReference = rootDirectoryReference.GetDirectoryReference(folderPath);

                return directoryReference.GetFileReference(filePathSegments.Last());
            }
        }

        private string GetHash(Stream stream)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var hashValue = sha256Hash.ComputeHash(stream);
                return BitConverter.ToString(hashValue).Replace("-", string.Empty).ToLowerInvariant();
            }
        }
    }
}
