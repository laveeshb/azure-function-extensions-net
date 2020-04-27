namespace Azure.Functions.Extension.AzureFiles
{
    using System;
    using Microsoft.Azure.WebJobs.Description;

    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class AzureFileAttribute : Attribute
    {
        [AutoResolve]
        public string StorageConnectionString { get; set; }

        [AutoResolve]
        public string ShareName { get; set; }

        [AutoResolve]
        public string FilePath { get; set; }
    }
}
