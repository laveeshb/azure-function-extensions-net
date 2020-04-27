
namespace Azure.Functions.Extension.AzureFiles
{
    using System.IO;

    public class AzureFileResult
    {
        public Stream FileContentStream { get; set; }

        public string SHA256Hash { get; set; }
    }
}
